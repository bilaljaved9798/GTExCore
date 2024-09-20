using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace GTCore.ViewModel
{
    [System.SerializableAttribute()]
    public partial class UserLoginObject : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {

        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DeviceIdField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PasswordField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SoftwareIdField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private double TimezoneOffsetField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string UsernameField;

        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string DeviceId
        {
            get
            {
                return this.DeviceIdField;
            }
            set
            {
                if ((object.ReferenceEquals(this.DeviceIdField, value) != true))
                {
                    this.DeviceIdField = value;
                    this.RaisePropertyChanged("DeviceId");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Password
        {
            get
            {
                return this.PasswordField;
            }
            set
            {
                if ((object.ReferenceEquals(this.PasswordField, value) != true))
                {
                    this.PasswordField = value;
                    this.RaisePropertyChanged("Password");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SoftwareId
        {
            get
            {
                return this.SoftwareIdField;
            }
            set
            {
                if ((object.ReferenceEquals(this.SoftwareIdField, value) != true))
                {
                    this.SoftwareIdField = value;
                    this.RaisePropertyChanged("SoftwareId");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public double TimezoneOffset
        {
            get
            {
                return this.TimezoneOffsetField;
            }
            set
            {
                if ((this.TimezoneOffsetField.Equals(value) != true))
                {
                    this.TimezoneOffsetField = value;
                    this.RaisePropertyChanged("TimezoneOffset");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Username
        {
            get
            {
                return this.UsernameField;
            }
            set
            {
                if ((object.ReferenceEquals(this.UsernameField, value) != true))
                {
                    this.UsernameField = value;
                    this.RaisePropertyChanged("Username");
                }
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    [Serializable]
    public class sessionHeader : INotifyPropertyChanged
    {
        private int idField;

        private string tokenField;

        private DateTime lastReqField;

        private DateTime currentReqField;

        private string usernameField;
        public event PropertyChangedEventHandler PropertyChanged;

        [XmlElement(Order = 0)]
        public int id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
                this.RaisePropertyChanged("id");
            }
        }

        [XmlElement(Order = 1)]
        public string token
        {
            get
            {
                return this.tokenField;
            }
            set
            {
                this.tokenField = value;
                this.RaisePropertyChanged("token");
            }
        }

        [XmlElement(Order = 2)]
        public DateTime lastReq
        {
            get
            {
                return this.lastReqField;
            }
            set
            {
                this.lastReqField = value;
                this.RaisePropertyChanged("lastReq");
            }
        }

        [XmlElement(Order = 3)]
        public DateTime CurrentReq
        {
            get
            {
                return this.currentReqField;
            }
            set
            {
                this.currentReqField = value;
                this.RaisePropertyChanged("CurrentReq");
            }
        }
        [XmlElement(Order = 4)]
        public string Username
        {
            get
            {
                return this.usernameField;
            }
            set
            {
                this.usernameField = value;
                this.RaisePropertyChanged("Username");
            }
        }
        private bool isAllowdBettingField;
        private bool activeField;
        [XmlElement(Order = 5)]
        public bool isAllowedBetting
        {
            get
            {
                return this.isAllowdBettingField;
            }
            set
            {
                this.isAllowdBettingField = value;
                this.RaisePropertyChanged("isAllowedBetting");
            }
        }
        [XmlElement(Order = 6)]
        public bool active
        {
            get
            {
                return this.activeField;
            }
            set
            {
                this.activeField = value;
                this.RaisePropertyChanged("active");
            }
        }
        private string anouncementField;
        [XmlElement(Order = 7)]
        public string anouncement
        {
            get
            {
                return this.anouncementField;
            }
            set
            {
                this.anouncementField = value;
                this.RaisePropertyChanged("anouncement");
            }
        }
        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
    [Serializable]
    public class BetterInfo
    {
        private string accountTypeField;

        private double balanceAtLoginField;

        private string betterCountryField;

        private int betterIdField;

        private string betterNameField;

        private string betterPasswordField;

        private string betterPhoneField;

        private string betterReferenceField;

        private decimal commissionField;

        private string currencyNameField;

        private decimal currencyRateField;

        private int isActiveField;

        private int isBetField;

        private int isMessageField;

        private int isRegisterField;

        private int isTrustedField;

        private string messageDetailField;

        private string privateKeyField;

        private string publicKeyField;

        private double toBFCricketMultiField;

        private double toBFCricketSingleField;

        private double toBFOtherMultiField;

        private double toBFOtherSingleField;

        private double toBFRaceMultiField;

        private double toBFRaceSingleField;

        private double toBFSoccerMultiField;

        private double toBFSoccerSingleField;

        private double toBFTennisMultiField;

        private double toBFTennisSingleField;

        private DateTime lastReqField;

        private double maxBetSizeField;

        private double limit1Field;

        private double limit2Field;

        private double limit3Field;

        private double limit4Field;

        private double limit5Field;

        private int bettingAllowedField;

        private int fullAccessToMarketsField;

        private string softwareIdField;

        private int isRCPField;

        private int isCreditField;

        private bool marketAccessOverrideField;

        public string AccountType
        {
            get
            {
                return this.accountTypeField;
            }
            set
            {
                this.accountTypeField = value;
            }
        }

        public double BalanceAtLogin
        {
            get
            {
                return this.balanceAtLoginField;
            }
            set
            {
                this.balanceAtLoginField = value;
            }
        }

        public string BetterCountry
        {
            get
            {
                return this.betterCountryField;
            }
            set
            {
                this.betterCountryField = value;
            }
        }

        public int BetterId
        {
            get
            {
                return this.betterIdField;
            }
            set
            {
                this.betterIdField = value;
            }
        }

        public string BetterName
        {
            get
            {
                return this.betterNameField;
            }
            set
            {
                this.betterNameField = value;
            }
        }

        public string BetterPassword
        {
            get
            {
                return this.betterPasswordField;
            }
            set
            {
                this.betterPasswordField = value;
            }
        }

        public string BetterPhone
        {
            get
            {
                return this.betterPhoneField;
            }
            set
            {
                this.betterPhoneField = value;
            }
        }

        public string BetterReference
        {
            get
            {
                return this.betterReferenceField;
            }
            set
            {
                this.betterReferenceField = value;
            }
        }

        public decimal Commission
        {
            get
            {
                return this.commissionField;
            }
            set
            {
                this.commissionField = value;
            }
        }

        public string CurrencyName
        {
            get
            {
                return this.currencyNameField;
            }
            set
            {
                this.currencyNameField = value;
            }
        }

        public decimal CurrencyRate
        {
            get
            {
                return this.currencyRateField;
            }
            set
            {
                this.currencyRateField = value;
            }
        }

        public int IsActive
        {
            get
            {
                return this.isActiveField;
            }
            set
            {
                this.isActiveField = value;
            }
        }

        public int IsBet
        {
            get
            {
                return this.isBetField;
            }
            set
            {
                this.isBetField = value;
            }
        }

        public int IsMessage
        {
            get
            {
                return this.isMessageField;
            }
            set
            {
                this.isMessageField = value;
            }
        }

        public int IsRegister
        {
            get
            {
                return this.isRegisterField;
            }
            set
            {
                this.isRegisterField = value;
            }
        }

        public int IsTrusted
        {
            get
            {
                return this.isTrustedField;
            }
            set
            {
                this.isTrustedField = value;
            }
        }

        public string MessageDetail
        {
            get
            {
                return this.messageDetailField;
            }
            set
            {
                this.messageDetailField = value;
            }
        }

        public string PrivateKey
        {
            get
            {
                return this.privateKeyField;
            }
            set
            {
                this.privateKeyField = value;
            }
        }

        public string PublicKey
        {
            get
            {
                return this.publicKeyField;
            }
            set
            {
                this.publicKeyField = value;
            }
        }

        public double ToBFCricketMulti
        {
            get
            {
                return this.toBFCricketMultiField;
            }
            set
            {
                this.toBFCricketMultiField = value;
            }
        }

        public double ToBFCricketSingle
        {
            get
            {
                return this.toBFCricketSingleField;
            }
            set
            {
                this.toBFCricketSingleField = value;
            }
        }

        public double ToBFOtherMulti
        {
            get
            {
                return this.toBFOtherMultiField;
            }
            set
            {
                this.toBFOtherMultiField = value;
            }
        }

        public double ToBFOtherSingle
        {
            get
            {
                return this.toBFOtherSingleField;
            }
            set
            {
                this.toBFOtherSingleField = value;
            }
        }

        public double ToBFRaceMulti
        {
            get
            {
                return this.toBFRaceMultiField;
            }
            set
            {
                this.toBFRaceMultiField = value;
            }
        }

        public double ToBFRaceSingle
        {
            get
            {
                return this.toBFRaceSingleField;
            }
            set
            {
                this.toBFRaceSingleField = value;
            }
        }

        public double ToBFSoccerMulti
        {
            get
            {
                return this.toBFSoccerMultiField;
            }
            set
            {
                this.toBFSoccerMultiField = value;
            }
        }

        public double ToBFSoccerSingle
        {
            get
            {
                return this.toBFSoccerSingleField;
            }
            set
            {
                this.toBFSoccerSingleField = value;
            }
        }

        public double ToBFTennisMulti
        {
            get
            {
                return this.toBFTennisMultiField;
            }
            set
            {
                this.toBFTennisMultiField = value;
            }
        }

        public double ToBFTennisSingle
        {
            get
            {
                return this.toBFTennisSingleField;
            }
            set
            {
                this.toBFTennisSingleField = value;
            }
        }

        public DateTime lastReq
        {
            get
            {
                return this.lastReqField;
            }
            set
            {
                this.lastReqField = value;
            }
        }

        public double MaxBetSize
        {
            get
            {
                return this.maxBetSizeField;
            }
            set
            {
                this.maxBetSizeField = value;
            }
        }

        public double limit1
        {
            get
            {
                return this.limit1Field;
            }
            set
            {
                this.limit1Field = value;
            }
        }

        public double limit2
        {
            get
            {
                return this.limit2Field;
            }
            set
            {
                this.limit2Field = value;
            }
        }

        public double limit3
        {
            get
            {
                return this.limit3Field;
            }
            set
            {
                this.limit3Field = value;
            }
        }

        public double limit4
        {
            get
            {
                return this.limit4Field;
            }
            set
            {
                this.limit4Field = value;
            }
        }

        public double limit5
        {
            get
            {
                return this.limit5Field;
            }
            set
            {
                this.limit5Field = value;
            }
        }

        public int BettingAllowed
        {
            get
            {
                return this.bettingAllowedField;
            }
            set
            {
                this.bettingAllowedField = value;
            }
        }

        public int FullAccessToMarkets
        {
            get
            {
                return this.fullAccessToMarketsField;
            }
            set
            {
                this.fullAccessToMarketsField = value;
            }
        }

        public string softwareId
        {
            get
            {
                return this.softwareIdField;
            }
            set
            {
                this.softwareIdField = value;
            }
        }

        public int isRCP
        {
            get
            {
                return this.isRCPField;
            }
            set
            {
                this.isRCPField = value;
            }
        }

        public int isCredit
        {
            get
            {
                return this.isCreditField;
            }
            set
            {
                this.isCreditField = value;
            }
        }

        public bool MarketAccessOverride
        {
            get
            {
                return this.marketAccessOverrideField;
            }
            set
            {
                this.marketAccessOverrideField = value;
            }
        }
    }
}