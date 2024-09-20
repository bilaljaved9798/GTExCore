using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GTCore.Models
{
    public class AllUserMarkets
    {
        public string EventTypeID { get; set; }
        public string CompetitionID { get; set; }
        public string EventID { get; set; }
        public string MarketCatalogueID { get; set; }
        public Nullable<int> UpdatedbyID { get; set; }
        public string EventTypeName { get; set; }
        public string CompetitionName { get; set; }
        public string EventName { get; set; }
        public string MarketCatalogueName { get; set; }
        public Nullable<System.DateTime> EventOpenDate { get; set; }
        public string SheetName { get; set; }
    }
}