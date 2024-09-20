using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GTCore.Models
{
    public class ManageUser
    {
        public bool CanDeleteUser { get; set; }
        public bool CanBlockUser { get; set; }
        public string ResetPassword { get; set; }
        public bool CanChangeAgentRate { get; set; }
        public string AgentRate { get; set; }
        public bool CanChangeLoggedIN { get; set; }
        public decimal BetLowerLimitU { get; set; }
        public decimal BetUpperLimitU { get; set; }
        public bool isGrayHoundRaceAllowed { get; set; }
        public bool isHorseRaceAllowed { get; set; }
        public Nullable<decimal> BetLowerLimitHorsePlaceU { get; set; }
        public Nullable<decimal> BetUpperLimitHorsePlaceU { get; set; }
        public Nullable<decimal> BetUpperLimitGrayHoundPlaceU { get; set; }
        public Nullable<decimal> BetLowerLimitGrayHoundPlaceU { get; set; }
        public Nullable<decimal> BetUpperLimitGrayHoundWinU { get; set; }
        public Nullable<decimal> BetLowerLimitGrayHoundWinU { get; set; }
        public Nullable<decimal> BetUpperLimitMatchOddsU { get; set; }
        public Nullable<decimal> BetLowerLimitMatchOddsU { get; set; }
        public Nullable<decimal> BetUpperLimitInningRunsU { get; set; }
        public Nullable<decimal> BetLowerLimitInningRunsU { get; set; }
        public Nullable<decimal> BetUpperLimitCompletedMatchU { get; set; }
        public Nullable<decimal> BetLowerLimitCompletedMatchU { get; set; }
        public Nullable<bool> isTennisAllowed { get; set; }
        public Nullable<bool> isSoccerAllowed { get; set; }
        public Nullable<int> CommissionRate { get; set; }
        public string PhoneNumber { get; set; }
        public Nullable<decimal> BetUpperLimitMatchOddsSoccerU { get; set; }
        public Nullable<decimal> BetLowerLimitMatchOddsSoccerU { get; set; }
        public Nullable<decimal> BetUpperLimitMatchOddsTennisU { get; set; }
        public Nullable<decimal> BetLowerLimitMatchOddsTennisU { get; set; }
        public Nullable<decimal> BetUpperLimitTiedMatchU { get; set; }
        public Nullable<decimal> BetLowerLimitTiedMatchU { get; set; }
        public Nullable<decimal> BetUpperLimitWinnerU { get; set; }
        public Nullable<decimal> BetLowerLimitWinnerU { get; set; }

        public bool isBetSlipKeysAllowed { get; set; }
        public bool isJoriBetAllowed { get; set; }

    }
}