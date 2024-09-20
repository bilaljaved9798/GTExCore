using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GTCore.Models
{
    public class UserAccounts
    {
        public string AccountsTitle { get; set; }
        public string Debit { get; set; }
        public string Credit { get; set; }
        public string CreatedDate { get; set; }
        public decimal OpeningBalance { get; set; }
        public Nullable<int> UserID { get; set; }
        public string MarketBookID { get; set; }
        public string UserName { get; set; }
        public decimal NetProfitorLoss { get; set; }
        public string UserType { get; set; }
        public string ComissionRate { get; set; }
        public string AgentRate { get; set; }
        public string SuperRate { get; set; }
        public string SamiadminRate { get; set; }

    }
}