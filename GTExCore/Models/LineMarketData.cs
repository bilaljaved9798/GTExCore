
namespace GTExCore.Models
{
	public class LineMarketData
	{
		public List<LineVMarket> LinevMarkets { get; set; }
		public List<Market> market { get; set; }
		public List<Session> session { get; set; }
		public object commentary { get; set; }
		public string MarketID { get; set; }
		public object marketId { get; set; }
		public object update_at { get; set; }
		public List<object> score { get; set; }
		public int counter { get; set; }
	}
}
