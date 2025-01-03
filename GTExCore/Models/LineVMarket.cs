namespace GTExCore.Models
{
    public class LineVMarket
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
        public string AssociateeventID { get; set; }
        public bool isOpenedbyUser { get; set; } = false;
    }
}
