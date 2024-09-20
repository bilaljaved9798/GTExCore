using BettingServiceReference;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace GTExCore.Models
{
    [Serializable()]
    public class UserBets:INotifyPropertyChanged
    {
        public long ID { get; set; }
        public string SelectionID { get; set; }
        public string UserOdd { get; set; }
        public string Amount { get; set; }
        public string BetType { get; set; }
        public string LiveOdd { get; set; }
        public bool isMatched { get; set; }
        public string Status { get; set; }
        public string MarketBookID { get; set; }
        public int UserID { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string SelectionName { get; set; }
        public string MarketBookname { get; set; }
        public string Liabality { get; set; }
        public string TotLiabality { get; set; }
        public List<DebitCredit> lstDebitCredit { get; set; }
        public string BetSize { get; set; }
        public decimal PendingAmount { get; set; }
        public string location { get; set; }
        public long ParentID { get; set; }
        public string Delete { get; set; } = "Delete";
        public int PoundRateB { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string p)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(p));
        }
    }
   
}