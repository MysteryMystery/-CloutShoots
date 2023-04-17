
using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using CloutShoots.Scraper;

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false);

IConfiguration config = builder.Build();

string[] urlsToScrape = config.GetSection("urlsToParse").Get<string[]>();

Scraper scraper = new Scraper
{
    SaveLocation = config.GetValue<string>("saveFolder")
};

foreach (var url in urlsToScrape)
{
    Console.WriteLine("Scraping: " + url);
    string html = await scraper.Get(url);
    scraper.ParseShoots(html);
}
scraper.Save();
