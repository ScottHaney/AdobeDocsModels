using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Xml.Linq;
using System.Xml.XPath;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("config.json", true)
    .Build();

var applicationXmlPath = Path.Combine("XmlDocs", "general", "application.xml");

var root = XElement.Load(applicationXmlPath);

var objectSection = root.XPathSelectElement("./section").Value;
var attributesSection = root.XPathSelectElement("./section/section[@ids='attributes']");
var methodsSection = root.XPathSelectElement("./section/section[@ids='methods']");

var builder = Host.CreateDefaultBuilder(args);

await builder.RunConsoleAsync();