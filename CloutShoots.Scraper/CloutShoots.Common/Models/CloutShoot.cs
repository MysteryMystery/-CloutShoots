using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Text;
using System.Web;

namespace CloutShoots.Common.Models
{
    public class CloutShoot
    {
        [JsonIgnore]
        public Guid Guid { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        public string NormalisedName { get => Name.Normalize(); }

        public DateTime Date { get; set; }

        public string? MapUrl { get; set; }
        public string? FormUrl { get; set; }
    }
}
