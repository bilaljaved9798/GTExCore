using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bfnexchange
{
    public class EventsItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string SelectionId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string RunnerName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string LayPrice1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string LaySize1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string LayPrice2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string LaySize2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string LayPrice3 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string LaySize3 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string BackPrice1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string BackSize1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string BackPrice2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string BackSize2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string BackPrice3 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string BackSize3 { get; set; }
    }

    public class MarketItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string marketId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string inplay { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string totalMatched { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string totalAvailable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string priceStatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<EventsItem> events { get; set; }
    }

    public class SessionItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string SelectionId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string RunnerName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string LayPrice1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string LaySize1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string BackPrice1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string BackSize1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string GameStatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FinalStatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string isActive { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int min { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int max { get; set; }
    }

    public class Root
    {
        /// <summary>
        /// 
        /// </summary>
        public List<MarketItem> market { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<SessionItem> session { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string commentary { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string MarketID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string marketId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string update_at { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string score { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int counter { get; set; }
    }
}