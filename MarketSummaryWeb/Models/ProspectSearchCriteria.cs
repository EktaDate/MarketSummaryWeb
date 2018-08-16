using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MarketSummaryWeb.Models
{
    public class ProspectSearchCriteria
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "prospectname")]
        [DisplayName("Prospect Name")]
        [Required]
        public string ProspectName { get; set; }

        [JsonProperty(PropertyName = "searchstring")]
        [DisplayName("Search String")]
        [Required]
        public string SearchString { get; set; }

        [JsonProperty(PropertyName = "twitterupdates")]
        [DisplayName("Twitter Updates")]
        public bool TwitterUpdates { get; set; }

        [JsonProperty(PropertyName = "fbupdates")]
        [DisplayName("Facebook Updates")]
        public bool FacebookUpdates { get; set; }

        [JsonProperty(PropertyName = "linkedinupdates")]
        [DisplayName("LinkedIn Updates")]
        public bool LinkedinUpdates { get; set; }

        [JsonProperty(PropertyName = "emailupdates")]
        [DisplayName("Email Updates")]
        public bool EmailUpdates { get; set; }

        [JsonProperty(PropertyName = "bingsearchupdates")]
        [DisplayName("Bing Search")]
        public bool BingSearchUpdates { get; set; }
    }
}