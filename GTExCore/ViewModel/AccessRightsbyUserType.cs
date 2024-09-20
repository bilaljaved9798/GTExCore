using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GTCore.ViewModel
{
    public class AccessRightsbyUserType
    {
        public bool CreateUser { get; set; }
        public bool ViewActivityofUser { get; set; }
        public bool AddCredittoUser { get; set; }
        public bool ViewProfitandLossStatementofUser { get; set; }
        public bool ViewLedgerofUsers { get; set; }
        public bool ViewMarket { get; set; }
        public string Username { get; set; }
        public string AccountBalance { get; set; }
        public bool CanBlockUser { get; set; }
        public bool CanDeleteUser { get; set; }
        public  string CurrentLiabality { get; set; }
        public bool CanChangeAgentRate { get; set; }
        public bool CanChangeLoggedIN { get; set; }
        public string StartingBalance { get; set; }

        public  string NetBalance  { get; set; }

        public double CurrentAvailableBalance { get; set; }
       // public static double CurrentAccountBalance { get; set; }
      
    }
}