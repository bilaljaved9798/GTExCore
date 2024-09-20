using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace GTCore.Models
{
    public class MarketBookKJ
    {
        private string _SelectionID;
        private string _Selection;
        private string _ImageURL;
        private string _PL;
        private string _Loss;
        private string _Price;
        private string _Backprice2;
        private string _Backprice1;
        private string _Backprice0;
        private string _Backsize2;
        private string _Backsize1;
        private string _Backsize0;
        private string _Layprice0;
        private string _Layprice1;
        private string _Layprice2;
        private string _Laysize0;
        private string _Laysize1;
        private string _Laysize2;
        private bool _Selectionchk;
        private string _WearingURL;
        private string _JockeyName;
        private string _WearingDesc;
        private string _StatusStr;
        private string _Clothnumber;
        private string _StallDraw;
        private string _RunnerStatusstr;
        private bool _isShow;
        private bool _isSelected;
        private int _NoofWinners;
        private bool _isSelectedForLK;
        private string _Average;
        private bool _isFav;
        private String _totalmatched;

        public bool isFav
        {
            get { return _isFav; }
            set
            {
                if (_isFav == value)
                    return;
                _isFav = value; OnPropertyChanged("isFav");

            }
        }
        public string Average
        {
            get { return _Average; }
            set
            {
                if (_Average == value)
                    return;
                _Average = value; OnPropertyChanged("Average");
            }
        }
        public bool isSelectedForLK
        {
            get { return _isSelectedForLK; }
            set
            {
                if (_isSelectedForLK == value)
                    return;
                _isSelectedForLK = value; OnPropertyChanged("isSelectedForLK");
            }
        }
        public string CategoryName { get; set; }
        public string MarketbooknameBet { get; set; }
        public string Marketstatusstr { get; set; }
        public bool BettingAllowed { get; set; }
        public string OpenDate { get; set; }
        public string runnerscount { get; set; }
        public string CurrentMarketBookId { get; set; }
        public string JockeyHeading { get; set; }
        public bool isShow
        {
            get { return _isShow; }
            set
            {
                if (_isShow == value)
                    return;
                _isShow = value; OnPropertyChanged("isShow");
            }
        }
        public bool isSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value; OnPropertyChanged("isSelected");
            }
        }
        public string SelectionID
        {
            get { return _SelectionID; }
            set
            {
                if (_SelectionID == value)
                    return;
                _SelectionID = value; OnPropertyChanged("SelectionID");
            }
        }
        public string Clothnumber { get { return _Clothnumber; } set { _Clothnumber = value; OnPropertyChanged("Clothnumber"); } }
        public string StallDraw { get { return _StallDraw; } set { _StallDraw = value; OnPropertyChanged("StallDraw"); } }
        public string Selection { get { return _Selection; } set { _Selection = value; OnPropertyChanged("Selection"); } }
        public string ImageURL { get { return _ImageURL; } set { _ImageURL = value; OnPropertyChanged("ImageURL"); } }
        public string PL
        {
            get { return _PL; }
            set
            {
                if (_PL == value)
                    return;
                _PL = value; OnPropertyChanged("PL");
            }
        }
        public string Loss
        {
            get { return _Loss; }
            set
            {
                if (_Loss == value)
                    return;
                _Loss = value; OnPropertyChanged("Loss");
            }
        }
        public int NoofWinners
        {
            get { return _NoofWinners; }
            set
            {
                if (_NoofWinners == value)
                    return;
                _NoofWinners = value; OnPropertyChanged("NoofWinners");
            }
        }
        //public string totalmatched
        //{
        //    get { return _totalmatched; }
        //    set
        //    {
        //        if (_totalmatched == value)
        //            return;

        //        _totalmatched = value;

        //        OnPropertyChanged("TotalMatched");

        //    }
        //}

        public string Price
        {
            get { return _Price; }
            set
            {
                if (_Price == value)
                    return;

                _Price = value;

                OnPropertyChanged("Price");

            }
        }
        [DefaultValue("0")]
        public string Backprice2
        {
            get { return _Backprice2; }
            set
            {
                if (_Backprice2 == value)
                    return;
                _Backprice2 = value; OnPropertyChanged("Backprice2");
            }
        }
        [DefaultValue("0")]
        public string Backprice1
        {
            get { return _Backprice1; }
            set
            {
                if (_Backprice1 == value)
                    return; _Backprice1 = value; OnPropertyChanged("Backprice1");
            }
        }
        [DefaultValue("0")]
        public string Backprice0
        {
            get { return _Backprice0; }
            set
            {
                if (_Backprice0 == value)
                    return; _Backprice0 = value; OnPropertyChanged("Backprice0");
            }
        }

        public string Backsize2
        {
            get { return _Backsize2; }
            set
            {
                if (_Backsize2 == value)
                    return;
                _Backsize2 = value; OnPropertyChanged("Backsize2");
            }
        }

        public string Backsize1
        {
            get { return _Backsize1; }
            set
            {
                if (_Backsize1 == value)
                    return; _Backsize1 = value; OnPropertyChanged("Backsize1");
            }
        }
        public string Backsize0
        {
            get { return _Backsize0; }
            set
            {
                if (_Backsize0 == value)
                    return; _Backsize0 = value; OnPropertyChanged("Backsize0");
            }
        }
        [DefaultValue("0")]
        public string Layprice0
        {
            get { return _Layprice0; }
            set
            {
                if (_Layprice0 == value)
                    return; _Layprice0 = value; OnPropertyChanged("Layprice0");
            }
        }
        [DefaultValue("0")]
        public string Layprice1
        {
            get { return _Layprice1; }
            set
            {
                if (_Layprice1 == value)
                    return; _Layprice1 = value; OnPropertyChanged("Layprice1");
            }
        }
        [DefaultValue("0")]
        public string Layprice2
        {
            get { return _Layprice2; }
            set
            {
                if (_Layprice2 == value)
                    return;
                _Layprice2 = value; OnPropertyChanged("Layprice2");
            }
        }
        public string Laysize0
        {
            get { return _Laysize0; }
            set
            {
                if (_Laysize0 == value)
                    return; _Laysize0 = value; OnPropertyChanged("Laysize0");
            }
        }
        public string Laysize1
        {
            get { return _Laysize1; }
            set
            {
                if (_Laysize1 == value)
                    return;
                _Laysize1 = value; OnPropertyChanged("Laysize1");
            }
        }
        public string Laysize2
        {
            get { return _Laysize2; }
            set
            {
                if (_Laysize2 == value)
                    return; _Laysize2 = value; OnPropertyChanged("Laysize2");
            }
        }
        public bool Selectionchk { get { return _Selectionchk; } set { _Selectionchk = value; OnPropertyChanged("Selectionchk"); } }
        public string WearingURL { get { return _WearingURL; } set { _WearingURL = value; OnPropertyChanged("WearingURL"); } }
        public string JockeyName { get { return _JockeyName; } set { _JockeyName = value; OnPropertyChanged("JockeyName"); } }
        public string WearingDesc { get { return _WearingDesc; } set { _WearingDesc = value; OnPropertyChanged("WearingDesc"); } }
        public string StatusStr { get { return _StatusStr; } set { _StatusStr = value; OnPropertyChanged("StatusStr"); } }


        public string RunnerStatusstr
        {
            get { return _RunnerStatusstr; }
            set
            {
                if (_RunnerStatusstr == value)
                    return; _RunnerStatusstr = value; OnPropertyChanged("RunnerStatusstr");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string p)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(p));
            }
        }
    }
}