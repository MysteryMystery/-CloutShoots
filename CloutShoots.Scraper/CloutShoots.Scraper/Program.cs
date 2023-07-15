
using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using CloutShoots.Scraper;
using CloutShoots.Common.Models;

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false);

IConfiguration config = builder.Build();

AppConfig appConfig = config.Get<AppConfig>();

Scraper scraper = new Scraper
{
    SaveLocation = appConfig.SaveFolder
};

foreach (KeyValuePair<String, ParseSpec> kv in appConfig.PagesToParse)
{
    string year = kv.Key;
    ParseSpec parseSpec = kv.Value;

    scraper.SaveFile = year + ".json";

    Console.WriteLine("Scraping: " + parseSpec.Url);
    string html = await scraper.Get(parseSpec.Url);
    
    scraper.ParseShoots(html, parseSpec);

    scraper.Save();
    scraper.Reset();
}
