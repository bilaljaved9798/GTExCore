namespace GTExCore.ViewModel
{
	public partial class SP_BetPlaceWaitandInterval_GetAllData_Result
	{
		public int HorseRaceTimerInterval { get; set; }
		public int HorseRaceBetPlaceWait { get; set; }
		public int GrayHoundTimerInterval { get; set; }
		public int GrayHoundBetPlaceWait { get; set; }
		public int CricketMatchOddsTimerInterval { get; set; }
		public int CricketMatchOddsBetPlaceWait { get; set; }
		public int CompletedMatchTimerInterval { get; set; }
		public int CompletedMatchBetPlaceWait { get; set; }
		public int TiedMatchTimerInterval { get; set; }
		public int TiedMatchBetPlaceWait { get; set; }
		public int InningsRunsTimerInterval { get; set; }
		public int InningsRunsBetPlaceWait { get; set; }
		public int WinnerTimerInterval { get; set; }
		public int WinnerBetPlaceWait { get; set; }
		public int TennisTimerInterval { get; set; }
		public int TennisBetPlaceWait { get; set; }
		public int SoccerTimerInterval { get; set; }
		public int SoccerBetPlaceWait { get; set; }
		public Nullable<decimal> PoundRate { get; set; }
		public Nullable<int> FancyTimerInterval { get; set; }
		public Nullable<int> FancyBetPlaceWait { get; set; }
		public Nullable<int> RaceMinutesBeforeStart { get; set; }
		public Nullable<int> CancelBetTime { get; set; }
	}
}
