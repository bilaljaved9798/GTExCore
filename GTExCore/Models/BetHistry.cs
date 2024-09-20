using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GTCore.Models
{
    public class BetHistry
    {        
            public string BetType { get; set; }
            public string UserOdd { get; set; }
            public string Amount { get; set; }
            public Nullable<System.DateTime> CreatedDate { get; set; }
            public Nullable<System.DateTime> UpdateDate { get; set; }
            public string SelectionName { get; set; }
            public string MarketbookName { get; set; }
            public string BetSize { get; set; }       
    }
}