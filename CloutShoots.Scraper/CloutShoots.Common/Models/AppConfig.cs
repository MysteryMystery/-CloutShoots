using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloutShoots.Common.Models
{
    public class TableStructure
    {
        public int Date { get; set; }
        public int Name { get; set; }
        public int? What3Words { get; set; }
        public int? MapUrl { get; set; }
        public int? FormUrl { get; set; }
        public int? ResultsUrl { get; set; }
    }

    public class ParseSpec
    {
        /*
          "url": "https://iandownham.wixsite.com/clout-shoots",
          "tableStructure": {
            "date": 1,
            "name": 2,
            "what3Words": 3,
            "mapUrl": 2,
            "formUrl": 4,
            "resultsUrl": 5
          }
        }
        */

        public string Url { get; set; }
        public TableStructure TableStructure { get; set; }
    }

    public class AppConfig
    {
        public string SaveFolder { get; set; }
        public Dictionary<string, ParseSpec> PagesToParse { get; set; }
    }
}