//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MarketSummaryWeb.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProspectData
    {
        public int Id { get; set; }
        public string ProspectName { get; set; }
        public string SearchString { get; set; }
        public string SearchResult { get; set; }
        public Nullable<bool> TwitterUpdates { get; set; }
        public Nullable<bool> FacebookUpdates { get; set; }
        public Nullable<bool> LinkedinUpdates { get; set; }
        public Nullable<bool> EmailUpdates { get; set; }
        public Nullable<bool> BingSearchUpdates { get; set; }
        public Nullable<bool> IsProcessed { get; set; }
        public Nullable<System.DateTime> DataProcessedDate { get; set; }
    }
}
