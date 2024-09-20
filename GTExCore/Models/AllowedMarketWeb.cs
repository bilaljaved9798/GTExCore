using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GTCore.Models
{
    public class AllowedMarketWeb
    {
        public bool isCricketMatchOddsAllowedForBet { get; set; }
        public bool isCricketTiedMatchAllowedForBet { get; set; }
        public bool isCricketCompletedMatchAllowedForBet { get; set; }
        public bool isCricketInningsRunsAllowedForBet { get; set; }
        public bool isSoccerAllowedForBet { get; set; }
        public bool isTennisAllowedForBet { get; set; }
        public bool isHorseRaceWinAllowedForBet { get; set; }
        public bool isHorseRacePlaceAllowedForBet { get; set; }
        public bool isGrayHoundRaceWinAllowedForBet { get; set; }
        public bool isGrayHoundRacePlaceAllowedForBet { get; set; }
        public bool isWinnerMarketAllowedForBet { get; set; }
        public bool isFancyMarketAllowed { get; set; }
    }
}