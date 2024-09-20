using BettingServiceReference;
using Newtonsoft.Json;

namespace GTCore.ViewModel
{
    public static class APIConfigforResults
    {
        public static string Url = "";
        public static string AppKey = "";
        //public static string Certfilename = ConfigurationManager.AppSettings["BefatirCert"];
        private static string Username = "";
        private static string Password = "";
        public static string SessionKey = "";
        public static decimal PoundRate = 0;
        public static string JsonResultsArr { get; set; }
        public static string JsonResultsArrCricketMatchOdds { get; set; }
        public static string JsonResultsArrCricketCompletedMatch { get; set; }
        public static string JsonResultsArrCricketInningsRuns { get; set; }
        public static string JsonResultsArrTennis { get; set; }
        public static string JsonResultsArrSoccer { get; set; }
        public static string JsonResultsArrHorseRacePlace { get; set; }
        public static string JsonResultsArrHorseRaceWin { get; set; }
        public static string JsonResultsArrGrayHoundPlace { get; set; }
        public static string JsonResultsArrGrayHoundWin { get; set; }
        public static string JsonResultsArrWinner { get; set; }
        public static List<SampleResponse1> BFMarketBooksFancy = new List<SampleResponse1>();
        public static IList<MarketBook> BFMarketBooksOtherFancy = new List<MarketBook>();
        public class LoginResponse
        {
            [JsonProperty(PropertyName = "sessionToken")]
            public string sessionToken { get; set; }

            [JsonProperty(PropertyName = "loginStatus")]
            public string loginStatus { get; set; }
        }    
    }
}