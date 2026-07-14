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
    public class BetValidationRequest
    {
        public int UserId { get; set; }
        public string MarketBookId { get; set; }
        public string MarketBookName { get; set; }
        public string CategoryName { get; set; }
        public string BetType { get; set; }
        public string SelectionId { get; set; }
        public decimal Amount { get; set; }
        public decimal Odd { get; set; }
        public bool IsFancy { get; set; }
    }
    public class BetValidationResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public decimal Balance { get; set; }
        public decimal Liability { get; set; }

        public decimal MinLimit { get; set; }
        public decimal MaxLimit { get; set; }
    }

}
