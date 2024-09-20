using BettingServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GTCore.Models
{
    public class InPlayMatches
    {
        public string EventTypeID { get; set; }
        public string EventName { get; set; }
        public string MarketStatus { get; set; }
        public string CompetitionID { get; set; }    
        public string CompetitionName { get; set; }
        public string EventID { get; set; }
        public string MarketCatalogueID { get; set; }
        public string MarketCatalogueName { get; set; }
        public string EventTypeName { get; set; }
        public List<Runner> Runners { get; set; }
        public string SheetName { get; set; }
        public string AssociateEventID { get; set; }
        public Nullable<System.DateTime> EventOpenDate { get; set; }
        public string SelectionName { get; set; }
        public string SelectionID { get; set; }
        public string CountryCode { get; set; }
    }


}