using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GameStatistics.Test.Models
{

    public class GameData
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("gameName")]
        [Required]
        public string GameName { get; set; }


        [JsonProperty("category")]
        public string Category { get; set; }

        private int _totalBets;

        [JsonProperty("totalBets")]
        public int TotalBets
        {
            get => _totalBets;
            set
            {
                if (value >= 0)  // Validación simple, ajusta según tus necesidades
                    _totalBets = value;
                else
                    throw new ArgumentException("TotalBets must be non-negative.");
            }
        }

        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime LastUpdated { get; set; }
    }

}
