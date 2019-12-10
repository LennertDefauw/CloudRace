using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Race.Models
{
    public class Car
    {
        [JsonProperty(PropertyName = "carId")]
        public Guid CarId { get; set; }
        [JsonProperty(PropertyName = "teamId")]
        public int TeamId { get; set; }
        [JsonProperty(PropertyName = "brand")]
        public string Brand { get; set; }
        [JsonProperty(PropertyName = "team")]
        public string Team { get; set; }
        [JsonProperty (PropertyName = "startTime")]
        public DateTime StartTime { get; set; }
        public string EmailAdress { get; set; }
        public string id { get; set; }
    }
}
