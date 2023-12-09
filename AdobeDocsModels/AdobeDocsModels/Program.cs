using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("config.json", true)
    .Build();

var builder = Host.CreateDefaultBuilder(args);

await builder.RunConsoleAsync();