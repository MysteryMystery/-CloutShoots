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

        public void ParseShoots(string html, ParseSpec parseSpec) { 
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var trs = htmlDocument.DocumentNode.Descendants("tr")
                .Where(e => e.GetAttributeValue("class", "").Equals("dsil2C wixui-table__row"))
                .ToList();

            foreach (var tr in trs)
            {
                var tds = tr.Descendants("td");
                CloutShootTableParser parser = new CloutShootTableParser(tds, parseSpec);

                _cloutShoots.Add(new CloutShoot
                {
                    Name = parser.ParseName() ?? "",
                    MapUrl = parser.ParseMapUrl(),
                    FormUrl = parser.ParseFormUrl(),
                    Date = parser.ParseDate(),
                    WhatThreeWords = parser.ParseWhatThreeWords(),
                    ResultsUrl = parser.ParseResultsUrl()
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

        public void Reset()
        {
            _cloutShoots = new List<CloutShoot>();
        }
    }
}
