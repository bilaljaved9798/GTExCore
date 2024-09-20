using GTCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GTExCore.HelperClasses
{
    public class UserBetsCalculations
    {
    }
    public class AllMarketsInPlay
    {
        public string MarketBookID { get; set; }
        public string MarketBookName { get; set; }
        public string ImagePath { get; set; }
        public string MarketStatus { get; set; }
        public string MarketStartTime { get; set; }
        public string Runner1 { get; set; }
        public string Runner2 { get; set; }
        public string Runner3 { get; set; }
        
        public string Runner1Back { get; set; }
        public string Runner1Lay { get; set; }
        public string Runner2Back { get; set; }
        public string Runner2Lay { get; set; }
        public string Runner3Back { get; set; }
        public string Runner3Lay { get; set; }
        public string Runner1BackSize { get; set; }
        public string Runner1LaySize { get; set; }
        public string Runner2BackSize { get; set; }
        public string Runner2LaySize { get; set; }
        public string Runner3BackSize { get; set; }
        public string Runner3LaySize { get; set; }
        public string Matched { get; set; }
        public string CountryCode { get; set; }
        public string CategoryName { get; set; }
        public string EventName { get; set; }
        public string CompetitionName { get; set; }
       
    }
    public class TodayHorserace
    {
        public string RaceName { get; set; }
        public string MarketBookID { get; set; }
    }

  

    public class DefaultPageModel
    {
        public string WelcomeMessage { get; set; }
        public string WelcomeHeading { get; set; }
        public string Rule { get; set; }      
        public List<string> ModalContent { get; set; }
        public List<AllMarketsInPlay> AllMarkets { get; set; }
        public List<TodayHorseRacing> TodayHorseRacing { get; set; }
        public List<TodayHorseRacing> TodayGreyRacing { get; set; }
        public string ViewType { get; set; }

    }
}