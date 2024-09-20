using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GTCore.Models
{
    [Serializable()]
    public class UserBetsforAgent
    {
        public string UserOdd { get; set; }
        public string Amount { get; set; }
        public string BetType { get; set; }
        public Nullable<bool> isMatched { get; set; }
        public string SelectionName { get; set; }
        public string MarketBookname { get; set; }
        public string Name { get; set; }
        public long ID { get; set; }
        public string SelectionID { get; set; }
        public string LiveOdd { get; set; }
        public string Status { get; set; }
        public string MarketBookID { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string AgentRate { get; set; }
        public string Liabality { get; set; }
        public string BetSize { get; set; }
        public Nullable<decimal> PendingAmount { get; set; }
        public string location { get; set; }
        public Nullable<long> ParentID { get; set; }
        public bool TransferAdmin { get; set; }
        public int CreatedbyID { get; set; }
        public int AgentOwnBets { get; set; }
        public int TransferAgentIDB { get; set; }
        public int TransferAgentRateB { get; set; }
        public int TransferAdminPercentage { get; set; }
    }

    public partial class BetSlipKeys
    {

        public int UserID { get; set; }
        public string SimpleBtn1 { get; set; }
        public string SimpleBtn2 { get; set; }
        public string SimpleBtn3 { get; set; }
        public string SimpleBtn4 { get; set; }
        public string SimpleBtn5 { get; set; }
        public string SimpleBtn6 { get; set; }
        public string SimpleBtn7 { get; set; }
        public string SimpleBtn8 { get; set; }
        public string SimpleBtn9 { get; set; }
        public string SimpleBtn10 { get; set; }
        public string SimpleBtn11 { get; set; }
        public string SimpleBtn12 { get; set; }
        public string MutipleBtn1 { get; set; }
        public string MutipleBtn2 { get; set; }
        public string MutipleBtn3 { get; set; }
        public string MutipleBtn4 { get; set; }
        public string MutipleBtn5 { get; set; }
        public string MutipleBtn6 { get; set; }
        public string MutipleBtn7 { get; set; }
        public string MutipleBtn8 { get; set; }
        public string MutipleBtn9 { get; set; }
        public string MutipleBtn10 { get; set; }
        public string MutipleBtn11 { get; set; }
        public string MutipleBtn12 { get; set; }
    }
}