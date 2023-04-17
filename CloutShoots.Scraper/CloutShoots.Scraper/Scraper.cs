using CloutShoots.Common.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
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
                string name = tr.Descendants("div")
                    .Where(e => e.GetAttributeValue("class", "").Equals("tY1czp") && !e.InnerHtml.Equals("Entry"))
                    .First().InnerText;

                string date = tr.Descendants("div")
                    .Where(e => e.GetAttributeValue("class", "").Equals("L0MOmM") && !e.InnerHtml.Equals("Entry"))
                    .First().InnerText;
                DateTime dateTime = DateTime.Parse(date);

                var links = tr.Descendants("a")
                    .Where(e => !string.IsNullOrEmpty(e.GetAttributeValue("href", "")));
                string mapUrl = links.First()
                    .GetAttributeValue("href", null);

                string? formUrl = links.Skip(1)
                    .FirstOrDefault()
                    ?.GetAttributeValue("href", null);



                _cloutShoots.Add(new CloutShoot
                {
                    Name = name,
                    MapUrl = mapUrl,
                    FormUrl = formUrl,
                    Date = dateTime
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
            using (StreamWriter streamWriter = new StreamWriter(SaveLocation + "/" + SaveFile))
            {
                streamWriter.Write(json);
            }
        }
    }
}
