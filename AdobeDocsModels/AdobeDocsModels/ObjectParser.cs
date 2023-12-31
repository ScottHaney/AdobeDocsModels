using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using static System.Collections.Specialized.BitVector32;

namespace AdobeDocsModels
{

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
}

// This needs to be added because this project is a dependency of a source generator so it must be .net standard (see https://github.com/dotnet/roslyn/discussions/47517)
// and apparently to fix the resulting error this
// hack is needed (see https://stackoverflow.com/questions/64749385/predefined-type-system-runtime-compilerservices-isexternalinit-is-not-defined)
namespace System.Runtime.CompilerServices
{
    internal static class IsExternalInit { }
}