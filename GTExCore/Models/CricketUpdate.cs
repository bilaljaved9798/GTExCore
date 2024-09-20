using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace GTCore.Models
{
    public class CricketUpdate
    {
        [JsonProperty("eventTypeId")]
        public long EventTypeId { get; set; }

        [JsonProperty("eventId")]
        public long EventId { get; set; }

        [JsonProperty("score")]
        public Score Score { get; set; }

        [JsonProperty("currentSet")]
        public long CurrentSet { get; set; }

        [JsonProperty("hasSets")]
        public bool HasSets { get; set; }

        [JsonProperty("stateOfBall")]
        public StateOfBall StateOfBall { get; set; }

        [JsonProperty("currentDay")]
      
        public long CurrentDay { get; set; }

        [JsonProperty("matchType")]
        public string MatchType { get; set; }

        [JsonProperty("fullTimeElapsed")]
        public FullTimeElapsed FullTimeElapsed { get; set; }

        [JsonProperty("matchStatus")]
        public string MatchStatus { get; set; }
    }
    public partial class FullTimeElapsed
    {
        [JsonProperty("hour")]
        public long Hour { get; set; }

        [JsonProperty("min")]
        public long Min { get; set; }

        [JsonProperty("sec")]
        public long Sec { get; set; }
    }

    public partial class Score
    {
        [JsonProperty("home")]
        public Away Home { get; set; }

        [JsonProperty("away")]
        public Away Away { get; set; }
    }

    public partial class Away
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("halfTimeScore")]
        public string HalfTimeScore { get; set; }

        [JsonProperty("fullTimeScore")]
        public string FullTimeScore { get; set; }

        [JsonProperty("penaltiesScore")]
        public string PenaltiesScore { get; set; }

        [JsonProperty("penaltiesSequence")]
        public object[] PenaltiesSequence { get; set; }

        [JsonProperty("highlight")]
        public bool Highlight { get; set; }

        [JsonProperty("inning1")]
        public Inning1 Inning1 { get; set; }
    }

    public partial class Inning1
    {
        [JsonProperty("runs")]
       
        public long Runs { get; set; }

        [JsonProperty("wickets")]
       
        public string Wickets { get; set; }

        [JsonProperty("overs")]
        public string Overs { get; set; }
    }

    public partial class StateOfBall
    {
        [JsonProperty("overNumber")]
       
        public long OverNumber { get; set; }

        [JsonProperty("overBallNumber")]
      
        public long OverBallNumber { get; set; }

        [JsonProperty("bowlerName")]
        public string BowlerName { get; set; }

        [JsonProperty("batsmanName")]
        public string BatsmanName { get; set; }

        [JsonProperty("batsmanRuns")] 
        public long BatsmanRuns { get; set; }

        [JsonProperty("appealId")]
       
        public long AppealId { get; set; }

        [JsonProperty("appealTypeName")]
        public string AppealTypeName { get; set; }

        [JsonProperty("wide")]
      
        public long Wide { get; set; }

        [JsonProperty("bye")]
       
        public long Bye { get; set; }

        [JsonProperty("legBye")]
       
        public long LegBye { get; set; }

        [JsonProperty("noBall")]
       
        public long NoBall { get; set; }

        [JsonProperty("outcomeId")]
       
        public long OutcomeId { get; set; }

        [JsonProperty("dismissalTypeName")]
        public string DismissalTypeName { get; set; }

        [JsonProperty("referralOutcome")]
       
        public long ReferralOutcome { get; set; }
    }

}