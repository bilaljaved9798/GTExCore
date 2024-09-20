using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GTCore.Models
{
    public class TodayHorseRacing
    {
        public string TodayHorseRace { get; set; }
        public string MarketCatalogueID { get; set; }
        public DateTime EventOpenDate { get; set; }
        public string EventName { get; set; }
        public string MarketCatalogueName { get; set; }
        public string EventID { get; set; }
        public string CountryCode { get; set; }
    }
    public class TodayHorseRacingNew
    {
        public string TodayHorseRace { get; set; }
        public string EventTypeName { get; set; }
        public string MarketCatalogueID { get; set; }
        public Nullable<System.DateTime> EventOpenDate { get; set; }
        public string EventName { get; set; }
        public string MarketCatalogueName { get; set; }
        public string EventID { get; set; }
        public string CountryCode { get; set; }
    }
}