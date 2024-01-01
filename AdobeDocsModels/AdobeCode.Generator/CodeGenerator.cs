using AdobeDocsModels;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.IO;
using System.Xml.Linq;

namespace AdobeCode.Generator
{
    [Generator]
    public class CodeGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var applicationXmlPath = Path.Combine("..", "AdobeDocsModels", "XmlDocs", "general", "application.xml");

            var root = XElement.Load(applicationXmlPath);
            var parser = new SectionsParser();

            var result = parser.ReadAllSections(root);

            context.RegisterPostInitializationOutput(ctx =>
            {
                ctx.AddSource("Test.g.cs", $@"namespace Testing
{{
    public class GeneratedClassTest
    {{
    }}
}}
");
            });
        }
    }
}
