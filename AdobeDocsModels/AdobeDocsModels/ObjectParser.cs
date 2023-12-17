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
    public IEnumerable<(LineItem Header, List<LineItem> Items)> Parse(XElement root)
    {
        var rootSection = root.XPathSelectElement("./section");
        var sections = ReadSections(rootSection);

        return GroupItems(ProcessItems(sections.Items));
    }

    private IEnumerable<(LineItem Header, List<LineItem> Items)> GroupItems(IEnumerable<LineItem> items)
    {
        LineItem header = null;
        List<LineItem> subItems = [];
        foreach (var item in items)
        {
            if (IsHeader(item))
            {
                if (header != null || subItems.Any())
                    yield return new(header, subItems);

                header = item;
                subItems = [];
            }
            else
                subItems.Add(item);
        }
    }

    private bool IsHeader(LineItem item)
    {
        return item.InteriorTagName == "strong";
    }

    private IEnumerable<LineItem> ProcessItems(IEnumerable<XElement> items)
    {
        foreach (var item in items)
        {
            var tagName = item.Name.LocalName;
            var innerName = item.Elements().FirstOrDefault()?.Name.LocalName;
            var content = GetFullText(item);

            yield return new(tagName, innerName, content);
        }
    }

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

public record LineItem(string TagName, string InteriorTagName, string Content);