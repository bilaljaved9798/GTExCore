using BettingServiceReference;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace GTExCore.Models
{
    [Serializable, GeneratedCode("System.Xml", "4.6.1064.2"), DebuggerStepThrough, DesignerCategory("code"), XmlType(Namespace = "http://forexdatawsnew.org/")]
    public class BFMarketBook
    {
        private int betDelayField;
        private bool isBspReconciledField;
        private bool isCompleteField;
        private bool isCrossMatchingField;
        private bool isInplayField;
        private bool isMarketDataDelayedField;
        private bool isRunnersVoidableField;
        private DateTime? lastMatchTimeField;
        private string marketIdField;
        private int numberOfActiveRunnersField;
        private int numberOfRunnersField;
        private int numberOfWinnersField;
        private Runner[] runnersField;
        private string sPricesField;
        private MarketStatus statusField;
        private double totalAvailableField;
        private double totalMatchedField;
        private long versionField;

        public int BetDelay
        {
            get
            {
                return this.betDelayField;
            }

            set
            {
                this.betDelayField = value;
            }
        }

        public bool IsBspReconciled
        {
            get
            {
                return this.isBspReconciledField;
            }

            set
            {
                this.isBspReconciledField = value;
            }
        }

        public bool IsComplete
        {
            get
            {
                return this.isCompleteField;
            }

            set
            {
                this.isCompleteField = value;
            }
        }

        public bool IsCrossMatching
        {
            get
            { return this.isCrossMatchingField; }
            set
            {
                this.isCrossMatchingField = value;
            }
        }

        public bool IsInplay
        {
            get
            { return this.isInplayField; }
            set
            {
                this.isInplayField = value;
            }
        }

        public bool IsMarketDataDelayed
        {
            get
            {
                return this.isMarketDataDelayedField;
            }

            set
            {
                this.isMarketDataDelayedField = value;
            }
        }

        public bool IsRunnersVoidable
        {
            get
            {
                return this.isRunnersVoidableField;
            }

            set
            {
                this.isRunnersVoidableField = value;
            }
        }

        [XmlElement(IsNullable = true)]
        public DateTime? LastMatchTime
        {
            get
            {
                return this.lastMatchTimeField;
            }

            set
            {
                this.lastMatchTimeField = value;
            }
        }

        public string MarketId
        {
            get
            {
                return this.marketIdField;
            }

            set
            {
                this.marketIdField = value;
            }
        }

        public int NumberOfActiveRunners
        {
            get
            {
                return this.numberOfActiveRunnersField;
            }

            set
            {
                this.numberOfActiveRunnersField = value;
            }
        }

        public int NumberOfRunners
        {
            get
            {
                return this.numberOfRunnersField;
            }

            set
            {
                this.numberOfRunnersField = value;
            }
        }

        public int NumberOfWinners
        {
            get
            {
                return this.numberOfWinnersField;
            }

            set
            {
                this.numberOfWinnersField = value;
            }
        }

        public Runner[] Runners
        {
            get
            {
                return this.runnersField;
            }

            set
            {
                this.runnersField = value;
            }
        }

        public string sPrices
        {
            get
            {
                return this.sPricesField;
            }

            set
            {
                this.sPricesField = value;
            }
        }

        public MarketStatus Status
        {
            get
            {
                return this.statusField;
            }

            set
            {
                this.statusField = value;
            }
        }

        public double TotalAvailable
        {
            get { return this.totalAvailableField; }
            set
            {
                this.totalAvailableField = value;
            }
        }

        public double TotalMatched
        {
            get { return this.totalMatchedField; }
            set
            {
                this.totalMatchedField = value;
            }
        }

        public long Version
        {
            get { return this.versionField; }
            set
            {
                this.versionField = value;
            }
        }
    }
}
