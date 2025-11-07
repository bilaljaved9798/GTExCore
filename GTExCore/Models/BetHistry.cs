using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GTCore.Models
{
    public class BetHistry
    {                  
        public string BetType { get; set; }
        public decimal UserOdd { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string SelectionName { get; set; }
        public string MarketbookName { get; set; }
        public decimal BetSize { get; set; }
    }
}