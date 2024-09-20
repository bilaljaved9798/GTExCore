using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GTCore.Models
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
        public string CurrentLiabality { get; set; }
        public bool CanChangeAgentRate { get; set; }
        public bool CanChangeLoggedIN { get; set; }
        public string StartingBalance { get; set; }
    }
    public partial class EventLineMarketandOddsForAssociation_Result
    {
        public string EventName { get; set; }
        public string EventID { get; set; }
        public int isLineMarket { get; set; }
    }
    public partial class GetMarketForAllowedBetting_Result
    {
        public string Market { get; set; }
        public string MarketCatalogueID { get; set; }
        public string EventTypeName { get; set; }
        public bool BettingAllowed { get; set; }
        public Nullable<System.DateTime> EventOpenDate { get; set; }
        public string EventID { get; set; }
    }
    public partial class GetReferrerRateandReferrerIDbyUserID_Result
    {
        public Nullable<int> ReferrerID { get; set; }
        public Nullable<int> ReferrerRate { get; set; }
        public string RefferrerName { get; set; }
    }
}