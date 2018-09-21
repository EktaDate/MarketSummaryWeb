using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MarketSummaryWeb.Models
{
    public class ProspectMarketSummary :TableEntity
    {
        
        public int Id { get; set; }        
        [DisplayName("Prospect Name")]        
        public string ProspectName { get; set; }        
        [DisplayName("Url")]
        public string Url { get; set; }        
        [DisplayName("Summary")]
        public string Summary { get; set; }        
    }
}