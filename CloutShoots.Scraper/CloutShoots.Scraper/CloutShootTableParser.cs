using CloutShoots.Common.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CloutShoots.Scraper
{
    class CloutShootTableParser
    {
        private IEnumerable<HtmlNode> _tds;
        private ParseSpec _parseSpec;
        private TableStructure _tableStructure;

        public CloutShootTableParser(IEnumerable<HtmlNode> tds, ParseSpec parseSpec)
        {
            _tds = tds;
            _parseSpec = parseSpec;
            _tableStructure = parseSpec.TableStructure;
        }

        public string? ParseName()
        {
            var venueTd = _tds.Skip(_tableStructure.Name).First();
            return venueTd.Descendants("div")
                .FirstOrDefault()
                ?.InnerHtml;
        } 

        public DateTime ParseDate()
        {
            return DateTime.Parse(_tds.First().Descendants("div").First().InnerHtml);
        }

        public string? ParseMapUrl()
        {
            if(_tableStructure.MapUrl is null)
                return null;

            var venueTd = _tds.Skip(_tableStructure.MapUrl.Value).First();
            return venueTd.Descendants("a")
                    .FirstOrDefault()
                    ?.GetAttributeValue("href", "");
        } 

        public string? ParseFormUrl()
        {
            if (_tableStructure.FormUrl is null)
                return null;

            return _tds.Skip(_tableStructure.FormUrl.Value)
            .First()
            .Descendants("a")
            .FirstOrDefault()
            ?.GetAttributeValue("href", "");
        }

        public string? ParseWhatThreeWords()
        {
            if (_tableStructure.What3Words is null)
                return null;

            return _tds.Skip(_tableStructure.What3Words.Value)
                    .First()
                    .Descendants("a")
                    .FirstOrDefault()
                    ?.GetAttributeValue("href", "");
        }

        public string? ParseResultsUrl()
        {
            if (_tableStructure.ResultsUrl is null)
                return null;
            
            return _tds.Skip(_tableStructure.ResultsUrl.Value)
            .First()
            .Descendants("a")
            .FirstOrDefault()
            ?.GetAttributeValue("href", "");
        }
    }
}
