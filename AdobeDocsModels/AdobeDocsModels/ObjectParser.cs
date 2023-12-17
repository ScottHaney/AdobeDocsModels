using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace AdobeDocsModels;

public class ObjectParser
{


    private string GetFullText(XElement element)
    {
        var innerText = element.Value;

        var regex = new Regex("<.*?>");
        return regex.Replace(innerText, x => string.Empty);
    }

    private SectionItem ReadSections(XElement section)
    {
        var children = section.Elements().ToList();

        List<XElement> items = [];
        List<XElement> sections = [];
        foreach (var child in children)
        {
            if (child.Name == "section")
                sections.Add(child);
            else
                items.Add(child);
        }

        return new(items, sections.Select(x => ReadSections(x)).ToList());
    }
}

public record SectionItem(ICollection<XElement> Items, ICollection<SectionItem> Sections);
