using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GTCore.Models
{
    [Serializable()]
    public class UserIDandUserType
    {
        public int ID { get; set; }
        public string UserType { get; set; }
        public string UserName { get; set; }
        public int UserTypeID { get; set; }
        public bool isBlocked { get; set; }
        public bool isDeleted { get; set; }
        public string Password { get; set; }
        public string PasswordforValidate { get; set; }
        public string PasswordforValidateS { get; set; }
        public bool IsCom { get; set; }
        public string AccountBalance { get; set; }
        public string RatePercent { get; set; }
        public bool Loggedin { get; set; }
        public decimal BetLowerLimit { get; set; }
        public decimal BetUpperLimit { get; set; }
        public bool CheckConditionsforPlacingBet { get; set; }
        public bool isGrayHoundRaceAllowed { get; set; }
        public bool isHorseRaceAllowed { get; set; }
        public bool isFancyMarketAllowed { get; set; }
        public decimal BetLowerLimitHorsePlace { get; set; }
        public decimal BetUpperLimitHorsePlace { get; set; }
        public decimal BetUpperLimitGrayHoundPlace { get; set; }
        public decimal BetLowerLimitGrayHoundPlace { get; set; }
        public decimal BetUpperLimitGrayHoundWin { get; set; }
        public decimal BetLowerLimitGrayHoundWin { get; set; }
        public decimal BetUpperLimitMatchOdds { get; set; }
        public decimal BetLowerLimitMatchOdds { get; set; }
        public decimal BetUpperLimitInningRuns { get; set; }
        public decimal BetLowerLimitInningRuns { get; set; }
        public decimal BetUpperLimitCompletedMatch { get; set; }
        public decimal BetLowerLimitCompletedMatch { get; set; }
        public bool isTennisAllowed { get; set; }
        public bool isSoccerAllowed { get; set; }
        public decimal BetUpperLimitMatchOddsSoccer { get; set; }
        public decimal BetLowerLimitMatchOddsSoccer { get; set; }
        public decimal BetUpperLimitMatchOddsTennis { get; set; }
        public decimal BetLowerLimitMatchOddsTennis { get; set; }
        public decimal BetUpperLimitTiedMatch { get; set; }
        public decimal BetLowerLimitTiedMatch { get; set; }
        public decimal BetUpperLimitWinner { get; set; }
        public decimal BetLowerLimitWinner { get; set; }
        public decimal MaxOddBack { get; set; }
        public decimal MaxOddLay { get; set; }
        public bool CheckforMaxOddBack { get; set; }
        public bool CheckforMaxOddLay { get; set; }
        public long CurrentLoggedInID { get; set; }
        public decimal BetLowerLimitFancy { get; set; }
        public decimal BetUpperLimitFancy { get; set; }
        public bool TransferAdminAmount { get; set; }
        public bool isBetSlipKeysAllowed { get; set; }
        public bool isJoriBetAllowed { get; set; }
        public bool GTLogin { get; set; }
        public int CancelBetTime { get; set; }
        public int CompletedMatchBetPlaceWait { get; set; }
        public int CompletedMatchTimerInterval { get; set; }
        public int CricketMatchOddsBetPlaceWait { get; set; }
        public int CricketMatchOddsTimerInterval { get; set; }
        public int FancyBetPlaceWait { get; set; }
        public int FancyTimerInterval { get; set; }
        public int GrayHoundBetPlaceWait { get; set; }
        public int GrayHoundTimerInterval { get; set; }
        public int HorseRaceBetPlaceWait { get; set; }
        public int HorseRaceTimerInterval { get; set; }
        public int InningsRunsBetPlaceWait { get; set; }
        public int InningsRunsTimerInterval { get; set; }
        public decimal PoundRate { get; set; }
        public int RaceMinutesBeforeStart { get; set; }
        public int SoccerBetPlaceWait { get; set; }
        public int SoccerTimerInterval { get; set; }
        public int TennisBetPlaceWait { get; set; }
        public int TennisTimerInterval { get; set; }
        public int TiedMatchBetPlaceWait { get; set; }
        public int WinnerBetPlaceWait { get; set; }
        public int WinnerTimerInterval { get; set; }
        public string SimpleBtn1 { get; set; }
        public string SimpleBtn2 { get; set; }
        public string SimpleBtn3 { get; set; }
        public string SimpleBtn4 { get; set; }
        public string SimpleBtn5 { get; set; }
        public string SimpleBtn6 { get; set; }
        public string SimpleBtn7 { get; set; }
        public string SimpleBtn8 { get; set; }
        public string SimpleBtn9 { get; set; }
        public string SimpleBtn10 { get; set; }
        public string SimpleBtn11 { get; set; }
        public string SimpleBtn12 { get; set; }
        public string MutipleBtn1 { get; set; }
        public string MutipleBtn2 { get; set; }
        public string MutipleBtn3 { get; set; }
        public string MutipleBtn4 { get; set; }
        public string MutipleBtn5 { get; set; }
        public string MutipleBtn6 { get; set; }
        public string MutipleBtn7 { get; set; }
        public string MutipleBtn8 { get; set; }
        public string MutipleBtn9 { get; set; }
        public string MutipleBtn10 { get; set; }
        public string MutipleBtn11 { get; set; }
        public string MutipleBtn12 { get; set; }

    }


    public class UserIDandUserTypeNew
    {
        public int ID { get; set; } = 0;
        public string UserName { get; set; }
        public string UserType { get; set; }
        public Nullable<int> UserTypeID { get; set; }
        public bool isBlocked { get; set; }
        public bool isDeleted { get; set; }
        public string Password { get; set; }
        public string RatePercent { get; set; }
        public bool Loggedin { get; set; }
        public decimal BetLowerLimit { get; set; }
        public decimal BetUpperLimit { get; set; }
        public bool CheckConditionsforPlacingBet { get; set; }
        public bool isGrayHoundRaceAllowed { get; set; }
        public bool isHorseRaceAllowed { get; set; }
        public decimal BetLowerLimitHorsePlace { get; set; }
        public decimal BetUpperLimitHorsePlace { get; set; }
        public decimal BetUpperLimitGrayHoundPlace { get; set; }
        public decimal BetLowerLimitGrayHoundPlace { get; set; }
        public decimal BetUpperLimitGrayHoundWin { get; set; }
        public decimal BetLowerLimitGrayHoundWin { get; set; }
        public decimal BetUpperLimitMatchOdds { get; set; }
        public decimal BetLowerLimitMatchOdds { get; set; }
        public decimal BetUpperLimitInningRuns { get; set; }

        public decimal BetLowerLimitInningRuns { get; set; }
        public decimal BetUpperLimitCompletedMatch { get; set; }
        public decimal BetLowerLimitCompletedMatch { get; set; }
        public decimal BetUpperLimitMatchOddsSoccer { get; set; }
        public decimal BetLowerLimitMatchOddsSoccer { get; set; }
        public decimal BetUpperLimitMatchOddsTennis { get; set; }
        public decimal BetLowerLimitMatchOddsTennis { get; set; }
        public decimal BetUpperLimitTiedMatch { get; set; }
        public decimal BetLowerLimitTiedMatch { get; set; }
        public decimal BetUpperLimitWinner { get; set; }
        public decimal BetLowerLimitWinner { get; set; }
        public long CurrentLoggedInID { get; set; }
        public decimal BetUpperLimitFancy { get; set; }
        public decimal BetLowerLimitFancy { get; set; }
        public decimal PoundRate { get; set; }
        public bool GTLogin { get; set; }
        public bool isBetSlipKeysAllowed { get; set; }
        public bool isJoriBetAllowed { get; set; }



    }

}