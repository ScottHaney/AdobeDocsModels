using AdobeDocsModels;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Hosting;
using System.Xml.Linq;
using System.Xml.XPath;

namespace AdobeDocsModels;

public class Program()
{
    static void Main(string[] args)
    {

        System.Console.WriteLine("Here again...");
    }
}

//System.Console.WriteLine("Here again...");

//var configuration = new ConfigurationBuilder()
//    .AddJsonFile("config.json", true)
//    .Build();

/*var applicationXmlPath = Path.Combine("XmlDocs", "general", "application.xml");

var root = XElement.Load(applicationXmlPath);
var parser = new SectionsParser();

var result = parser.ReadAllSections(root);*/
//var builder = Host.CreateDefaultBuilder(args);

//await builder.RunConsoleAsync();