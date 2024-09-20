namespace GTExCore.Models
{
	public class EvenOddMarkets
	{
		public bool BettingAllowed { get; set; }
		public string CompetitionID { get; set; }
		public string CompetitionName { get; set; }
		public string EventID { get; set; }
		public string EventName { get; set; }
		public string MarketCatalogueID { get; set; }
		public string MarketCatalogueName { get; set; }
		public bool isOpenedbyUser { get; set; }
	}
}
