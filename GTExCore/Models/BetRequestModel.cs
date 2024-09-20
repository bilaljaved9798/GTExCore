namespace GTExcgange.API.Models
{
    public class BetRequestModel
    {
        public int UserId { get; set; }
        public int UserType { get; set; }
        public string Amount { get; set; }
        public string Odd { get; set; }
        public string BetType { get; set; }
        public string[] SelectionID { get; set; }
        public string MarketbookID { get; set; }
        public string MarketbookName { get; set; }
        public string RunnersCount { get; set; }
        public string Betslipamountlabel { get; set; }
        public int Clickedlocation { get; set; }
        public string Betslipsize { get; set; }
        public string Selectionname { get; set; }
        

    }
}
