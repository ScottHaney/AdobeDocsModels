using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace AdobeDocsModels;

public class SectionsParser
{
    public Section ReadAllSections(XElement root)
    {
        var rootSection = root.XPathSelectElement("./section");
        return ReadSection(rootSection);
    }

    private Section ReadSection(XElement section)
    {
        var children = section.Elements().ToList();

        List<DetailGroup> groups = [];
        List<XElement> sections = [];

        XElement header = null;
        List<XElement> details = [];
        foreach (var child in children)
        {
            if (child.Name == "section")
                sections.Add(child);
            else
            {
                if (IsHeader(child))
                {
                    if (header != null)
                    {
                        groups.Add(new(GetFullText(header), details.Select(x => GetFullText(x)).ToArray()));
                        details = [];
                    }

                    header = child;
                }
                else
                    details.Add(child);
            }
        }

        return new(section.Attribute("names").Value, groups, sections.Select(x => ReadSection(x)).ToList());
    }

    private bool IsHeader(XElement element)
    {
        return element.Name == "title" || element.Elements().FirstOrDefault()?.Name.LocalName == "strong";
    }

    private string GetFullText(XElement element)
    {
        var innerText = element.Value;

        var regex = new Regex("<.*?>");
        return regex.Replace(innerText, x => string.Empty);
    }
}

public record Section(string Name, ICollection<DetailGroup> DetailGroups, ICollection<Section> SubSections);

public record DetailGroup(string Title, ICollection<string> Details);