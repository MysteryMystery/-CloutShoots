using CloutShoots.Common.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CloutShoots.Scraper
{
    internal class Scraper
    {
        private List<CloutShoot> _cloutShoots = new();

        public string SaveLocation { get; set; } = "/shoots";
        public string SaveFile { get; set; } = "CloutShoots.json";

        public Scraper() { 
        }

        public async Task<string> Get(string url)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public void ParseShoots(string html)
        {
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var trs = htmlDocument.DocumentNode.Descendants("tr")
                .Where(e => e.GetAttributeValue("class", "").Equals("dsil2C wixui-table__row"))
                .ToList();

            foreach (var tr in trs)
            {
                // there are 5 of these 
                // 0 - date
                // 1 - shoot name
                // 2 - parking (what 3 words)
                // 3 - entry form link 
                // 4 - results url

                var tds = tr.Descendants("td");
                DateTime dateTime = DateTime.Parse(tds.First().Descendants("div").First().InnerHtml);
                
                var venueTd = tds.Skip(1).First();
                string? name = venueTd.Descendants("div")
                    .FirstOrDefault()
                    ?.InnerHtml;
                string? mapUrl = venueTd.Descendants("a")
                    .FirstOrDefault()
                    ?.GetAttributeValue("href", "");

                string? whatThreeWords = tds.Skip(2)
                    .First()
                    .Descendants("a")
                    .FirstOrDefault()
                    ?.GetAttributeValue("href", "");

                string? formUrl = tds.Skip(3)
                    .First()
                    .Descendants("a")
                    .FirstOrDefault()
                    ?.GetAttributeValue("href", "");
                string? resultsUrl = tds.Skip(4)
                    .First()
                    .Descendants("a")
                    .FirstOrDefault()
                    ?.GetAttributeValue("href", "");


                _cloutShoots.Add(new CloutShoot
                {
                    Name = name ?? "",
                    MapUrl = mapUrl,
                    FormUrl = formUrl,
                    Date = dateTime,
                    WhatThreeWords = whatThreeWords,
                    ResultsUrl = resultsUrl
                });
            }
        }

        public void Save()
        {
            if (!Directory.Exists(SaveLocation))
            {
                var dinfo = Directory.CreateDirectory(SaveLocation);
                Console.WriteLine("Created: " + dinfo.FullName);
            }

            string json = JsonSerializer.Serialize(this._cloutShoots);

            Console.WriteLine(json);

            using (StreamWriter streamWriter = new StreamWriter(SaveLocation + "/" + SaveFile))
            {
                streamWriter.Write(json);
            }
        }
    }
}
