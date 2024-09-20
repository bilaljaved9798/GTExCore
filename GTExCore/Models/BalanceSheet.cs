using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bfnexchange.Models
{
    public class BalanceSheet
    {
        public string Username { get; set; }
        public decimal Profit { get; set; }
        public decimal StartingBalance { get; set; }
        public decimal Loss { get; set; }
        public int UserID { get; set; }
    }
}