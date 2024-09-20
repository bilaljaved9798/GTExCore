using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GTExCore.Models
{
    public class EventType
    {
        public string ID { get; set; }
        public string Name { get; set; }


    }
    public class LiabalitybyMarket
    {
        public string MarketBookName { get; set; }
        public decimal Liabality { get; set; }
        public string MarketBookID { get; set; }
    }
    public class Competition
    {
        public string ID { get; set; }
        public string Name { get; set; }


    }
    public class Event
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public bool isFavorite { get; set; } = false;
        public int SrNum { get; set; }
        public DateTime EventOpenDate { get; set; }
        public string AssociateEventID { get; set; }

    }

    public class MarketCatalgoue
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public List<Runner> Runner { get; set; }
        public string EventName { get; set; }
        public string SheetName { get; set; }
        public string EventTypeName { get; set; }
        public DateTime EventOpenDate { get; set; }
        public bool BettingAllowed { get; set; }
        public string GetMatchUpdatesFrom { get; set; }
        public string CricketAPIMatchKey { get; set; }
        public string TotalOvers { get; set; }
        public string EventID { get; set; }


    }
    //public class Runner
    //{
    //    public string SelectionID { get; set; }
    //    public string SelectionName { get; set; }
    //}
}