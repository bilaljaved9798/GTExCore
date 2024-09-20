using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GTCore.Models
{
    public class UserDetails
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public decimal AccountBalance { get; set; }
        public string LastLoginTime { get; set; }
        public string Status { get; set; }
        public string ProfitandLoss { get; set; }
        public bool isDeleted { get; set; }
        public bool isBlocked { get; set; }
        public string Password { get; set; }
        public string LastAmountAdded { get; set; }
        public string LastAmountRemoved { get; set; }
        public string AssignedMarket {get;set;}
        public string LastLocation { get; set; }
        public string LastIPAddress { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string RatePercent { get; set; }
        public bool Loggedin { get; set; }
        public decimal BetLowerLimit { get; set; }
        public decimal BetUpperLimit { get; set; }
        public bool isGrayHoundRaceAllowed { get; set; }
        public bool isHorseRaceAllowed { get; set; }
        public Nullable<decimal> BetLowerLimitHorsePlace { get; set; }
        public Nullable<decimal> BetUpperLimitHorsePlace { get; set; }
        public Nullable<decimal> BetUpperLimitGrayHoundPlace { get; set; }
        public Nullable<decimal> BetLowerLimitGrayHoundPlace { get; set; }
        public Nullable<decimal> BetUpperLimitGrayHoundWin { get; set; }
        public Nullable<decimal> BetLowerLimitGrayHoundWin { get; set; }
        public Nullable<decimal> BetUpperLimitMatchOdds { get; set; }
        public Nullable<decimal> BetLowerLimitMatchOdds { get; set; }
        public Nullable<decimal> BetUpperLimitInningRuns { get; set; }
        public Nullable<decimal> BetLowerLimitInningRuns { get; set; }
        public Nullable<decimal> BetUpperLimitCompletedMatch { get; set; }
        public Nullable<decimal> BetLowerLimitCompletedMatch { get; set; }
        public Nullable<bool> isTennisAllowed { get; set; }
        public Nullable<bool> isSoccerAllowed { get; set; }
        public Nullable<int> CommissionRate { get; set; }
        public string PhoneNumber { get; set; }
        public decimal BetUpperLimitMatchOddsSoccer { get; set; }
        public decimal BetLowerLimitMatchOddsSoccer { get; set; }
        public decimal BetUpperLimitMatchOddsTennis { get; set; }
        public decimal BetLowerLimitMatchOddsTennis { get; set; }
        public Nullable<decimal> BetUpperLimitTiedMatch { get; set; }
        public Nullable<decimal> BetLowerLimitTiedMatch { get; set; }
        public Nullable<decimal> BetUpperLimitWinner { get; set; }
        public Nullable<decimal> BetLowerLimitWinner { get; set; }

        public Nullable<bool> isBetSlipKeysAllowed { get; set; }
        public Nullable<bool> isJoriBetAllowed { get; set; }

    }
}