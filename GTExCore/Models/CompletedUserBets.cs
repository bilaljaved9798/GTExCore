using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GTCore.Models
{
    public class CompletedUserBets
    {
        public string UserOdd { get; set; }
        public string Amount { get; set; }
        public string BetType { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string SelectionName { get; set; }
        public string MarketBookname { get; set; }
        public string Steak { get; set; }
        public string Winnername { get; set; }
    }
}