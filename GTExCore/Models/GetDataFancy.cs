using BettingServiceReference;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTExCore.Models
{
    public class GetDataFancy
    {

        [JsonProperty("market")]
        public List<Market> market { get; set; }
        [JsonProperty("session")]
        public List<Session> session { get; set; }
        [JsonProperty("diamondRoot")]
        public DiamondRoot diamondRoot { get; set; }
        [JsonProperty("commentary")]
        public object commentary { get; set; }
        [JsonProperty("MarketID")]
        public string MarketID { get; set; }
        [JsonProperty("marketId")]
        public object marketId { get; set; }
        [JsonProperty("LinevMarkets")]
        public List<LinevMarkets> LinevMarkets = new List<LinevMarkets>();
        [JsonProperty("update_at")]
        public object update_at { get; set; }
        [JsonProperty("score")]
        public object score { get; set; }
        [JsonProperty("counter")]
        public int counter { get; set; }
    }
    public class Events
    {
        public string SelectionId { get; set; }
        public string RunnerName { get; set; }
        public string LayPrice1 { get; set; }
        public string LaySize1 { get; set; }
        public string LayPrice2 { get; set; }
        public string LaySize2 { get; set; }
        public string LayPrice3 { get; set; }
        public string LaySize3 { get; set; }
        public string BackPrice1 { get; set; }
        public string BackSize1 { get; set; }
        public string BackPrice2 { get; set; }
        public string BackSize2 { get; set; }
        public string BackPrice3 { get; set; }
        public string BackSize3 { get; set; }
    }

    public class Market
    {
        public string marketId { get; set; }
        public bool inplay { get; set; }
        public object totalMatched { get; set; }
        public object totalAvailable { get; set; }
        public string priceStatus { get; set; }
        public List<Events> events { get; set; }
    }

    public class Datum
    {
        public int gmid { get; set; }
        public string mid { get; set; }
        public object pmid { get; set; }
        public string mname { get; set; }
        public string rem { get; set; }
        public string gtype { get; set; }
        public string status { get; set; }
        public int rc { get; set; }
        public bool visible { get; set; }
        public int pid { get; set; }
        public int gscode { get; set; }
        public int maxb { get; set; }
        public double sno { get; set; }
        public int dtype { get; set; }
        public int ocnt { get; set; }
        public int m { get; set; }
        public int max { get; set; }
        public int min { get; set; }
        public bool biplay { get; set; }
        public int umaxbof { get; set; }
        public bool boplay { get; set; }
        public bool iplay { get; set; }
        public int btcnt { get; set; }
        public object company { get; set; }
        public List<Section> section { get; set; }
        public List<DebitCredit> DebitCredit { get; set; }
    }
    public class Section
    {
        public object mid { get; set; }
        public int sid { get; set; }
        public int psid { get; set; }
        public int sno { get; set; }
        public int psrno { get; set; }
        public string gstatus { get; set; }
        public string nat { get; set; }
        public int gscode { get; set; }
        public int max { get; set; }
        public int min { get; set; }
        public string rem { get; set; }
        public bool br { get; set; }
        public object rname { get; set; }
        public object jname { get; set; }
        public object tname { get; set; }
        public int hage { get; set; }
        public object himg { get; set; }
        public int adfa { get; set; }
        public object rdt { get; set; }
        public object cno { get; set; }
        public object sdraw { get; set; }
        public decimal? pnl { get; set; }
        public List<Odd> odds { get; set; }
    }
    public class Odd
    {
        public int sid { get; set; }
        public int psid { get; set; }
        public double odds { get; set; }
        public string otype { get; set; }
        public string oname { get; set; }
        public int tno { get; set; }
        public double size { get; set; }
    }


    public class DiamondRoot
    {
        public bool success { get; set; }
        public string msg { get; set; }
        public int status { get; set; }
        public object EventID { get; set; }
        public int counter { get; set; }
        public List<Datum> data { get; set; }
    }

    public class LinevMarket
    {
        public string MarketCatalogueID { get; set; }
        public string MarketCatalogueName { get; set; }
        public string SelectionID { get; set; }
        public string SelectionName { get; set; }
        public string EventName { get; set; }
        public string EventID { get; set; }
        public string CompetitionName { get; set; }
        public string CompetitionID { get; set; }
        public bool BettingAllowed { get; set; }
        public object AssociateeventID { get; set; }
        public bool isOpenedbyUser { get; set; }
    }

    public class AllMarketRoot
    {
        public List<LinevMarket> LinevMarkets { get; set; }
        public object market { get; set; }
        public List<Session> session { get; set; }
        public DiamondRoot diamondRoot { get; set; }
        public object commentary { get; set; }
        public string MarketID { get; set; }
        public object marketId { get; set; }
        public object update_at { get; set; }
        public object score { get; set; }
        public int counter { get; set; }
    }

    public class Session
    {
        public string SelectionId { get; set; }
        public string RunnerName { get; set; }
        public string LayPrice1 { get; set; }
        public string LaySize1 { get; set; }
        public string BackPrice1 { get; set; }
        public string BackSize1 { get; set; }
        public string GameStatus { get; set; }
        public object FinalStatus { get; set; }
        public bool isActive { get; set; }
        public double min { get; set; }
        public double max { get; set; }
        public object sortingOrder { get; set; }
        public string Type { get; set; }
        public double Profit { get; set; }
        public double Lose { get; set; }
        public MarketBookForindianFancy currentmarketsfancyPL { get; set; }
    }

}
