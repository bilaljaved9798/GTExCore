using BettingServiceReference;
using Global.API;
using GTCore.Models;
using GTExCore.Common;
using GTExCore.HelperClass;
using GTExCore.Models;
using GTExCore.ViewModel;
using log4net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using System.Reflection;
using System.Security.Claims;
using UserServiceReference;

namespace GTExCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MarketApiController : ControllerBase
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private BettingServiceClient objBettingClient = new BettingServiceClient();
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IRazorViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;
        private readonly IPasswordSettingsService _passwordSettingsService;
        UserServicesClient objUserServiceClient = new UserServicesClient();
        UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();
        UserServicesClient objUsersServiceCleint = new UserServicesClient();
        public static wsnew ws1 = new wsnew();
        public static wsnew ws2 = new wsnew();
        public static wsnew ws4 = new wsnew();
        public static wsnew ws7 = new wsnew();
        public static wsnew ws0 = new wsnew();
        public static wsnew ws4339 = new wsnew();
        public static wsnew wsFancy = new wsnew();

        private wsnew wsBFMatch = new wsnew();
        public MarketApiController(IRazorViewEngine viewEngine, ITempDataProvider tempDataProvider, IServiceProvider serviceProvider, IConfiguration configuration, IPasswordSettingsService passwordSettingsService, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
            _configuration = configuration;
            _passwordSettingsService = passwordSettingsService;
        }
        [Route("MarketBook")]
        [HttpGet]
        public async Task<string> MarketBook(string ID, string Username, string Password)
        {
            try
            {
                if (LoggedinUserDetail.GetUserTypeID() == null || LoggedinUserDetail.GetUserTypeID() == 0)
                {
                    var resultsUser = objUserServiceClient.GetUserbyUsernameandPasswordNew(Crypto.Encrypt(Username), Crypto.Encrypt(Password));
                    if (resultsUser != "")
                    {
                        var result = JsonConvert.DeserializeObject<UserIDandUserType>(resultsUser);

                        if (result.UserTypeID != 1)
                        {
                            if (result.Loggedin == true)
                            {
                                if (result.isBlocked == true)
                                {
                                    return "Account is Blocked.";
                                }
                                if (result.isDeleted == true)
                                {
                                    return "Account is Deleted.";
                                }
                                result.PoundRate = result.PoundRate;
                                LoggedinUserDetail.CurrentAccountBalance = Convert.ToDouble(result.AccountBalance);
                                LoggedinUserDetail.IsCom = result.IsCom;
                                LoggedinUserDetail.isFancyMarketAllowed = result.isFancyMarketAllowed;
                                LoggedinUserDetail.BetPlaceWaitandInterval.CancelBetTime = result.CancelBetTime;
                                LoggedinUserDetail.BetPlaceWaitandInterval.CompletedMatchBetPlaceWait = result.CompletedMatchBetPlaceWait;
                                LoggedinUserDetail.BetPlaceWaitandInterval.CompletedMatchTimerInterval = result.CompletedMatchTimerInterval;
                                LoggedinUserDetail.BetPlaceWaitandInterval.CricketMatchOddsBetPlaceWait = result.CricketMatchOddsBetPlaceWait;
                                LoggedinUserDetail.BetPlaceWaitandInterval.CricketMatchOddsTimerInterval = result.CricketMatchOddsTimerInterval;
                                LoggedinUserDetail.BetPlaceWaitandInterval.FancyBetPlaceWait = result.FancyBetPlaceWait;
                                LoggedinUserDetail.BetPlaceWaitandInterval.FancyTimerInterval = result.FancyTimerInterval;
                                LoggedinUserDetail.BetPlaceWaitandInterval.GrayHoundBetPlaceWait = result.GrayHoundBetPlaceWait;
                                LoggedinUserDetail.BetPlaceWaitandInterval.GrayHoundTimerInterval = result.GrayHoundTimerInterval;
                                LoggedinUserDetail.BetPlaceWaitandInterval.HorseRaceBetPlaceWait = result.HorseRaceBetPlaceWait;
                                LoggedinUserDetail.BetPlaceWaitandInterval.HorseRaceTimerInterval = result.HorseRaceTimerInterval;
                                LoggedinUserDetail.BetPlaceWaitandInterval.InningsRunsBetPlaceWait = result.InningsRunsBetPlaceWait;
                                LoggedinUserDetail.BetPlaceWaitandInterval.InningsRunsTimerInterval = result.InningsRunsTimerInterval;
                                LoggedinUserDetail.BetPlaceWaitandInterval.RaceMinutesBeforeStart = result.RaceMinutesBeforeStart;
                                LoggedinUserDetail.BetPlaceWaitandInterval.SoccerBetPlaceWait = result.SoccerBetPlaceWait;
                                LoggedinUserDetail.BetPlaceWaitandInterval.SoccerTimerInterval = result.SoccerTimerInterval;
                                LoggedinUserDetail.BetPlaceWaitandInterval.TennisBetPlaceWait = result.TennisBetPlaceWait;
                                LoggedinUserDetail.BetPlaceWaitandInterval.TennisTimerInterval = result.TennisTimerInterval;
                                LoggedinUserDetail.BetPlaceWaitandInterval.TiedMatchBetPlaceWait = result.TiedMatchBetPlaceWait;
                                LoggedinUserDetail.BetPlaceWaitandInterval.TiedMatchTimerInterval = result.TiedMatchBetPlaceWait;
                                LoggedinUserDetail.BetPlaceWaitandInterval.WinnerBetPlaceWait = result.WinnerBetPlaceWait;
                                LoggedinUserDetail.BetPlaceWaitandInterval.WinnerTimerInterval = result.WinnerTimerInterval;
                                _httpContextAccessor.HttpContext.Session.SetObject("User", result);
                            }
                        }
                    }
                }

                if (LoggedinUserDetail.URLsData.Count == 0)
                {
                    SetURLsData();
                }

                if (LoggedinUserDetail.GetCricketDataFrom != "Live")
                {
                    if (LoggedinUserDetail.GetUserTypeID() != 1)
                    {
                        objUsersServiceCleint.UpdateAllMarketClosedbyUserID(LoggedinUserDetail.GetUserID());
                    }
                    if (ID != "" && LoggedinUserDetail.GetUserTypeID() != 1)
                    {
                        var resultsalreadyopened = JsonConvert.DeserializeObject<List<MarketCatalgoue>>(objUsersServiceCleint.GetMarketsOpenedbyUser(LoggedinUserDetail.GetUserID()));
                        if (resultsalreadyopened != null && resultsalreadyopened.Count >= 10)
                        {
                            return "Limit exceed";
                        }
                        objUsersServiceCleint.SetMarketBookOpenbyUSer(LoggedinUserDetail.GetUserID(), ID);
                    }
                    var results = JsonConvert.DeserializeObject<List<MarketCatalgoue>>(objUsersServiceCleint.GetMarketsOpenedbyUser(LoggedinUserDetail.GetUserID()));
                    if (results != null)
                    {
                        results = results.Where(item => item.ID == ID).ToList();
                        if (results.FirstOrDefault().EventTypeName == "Horse Racing")
                        {
                            var marketbooks = new List<BettingServiceReference.MarketBook>();
                            List<string> lstIDs = new List<string>();
                            foreach (var item in results)
                            {
                                lstIDs = new List<string>();
                                lstIDs.Add(item.ID);
                                var marketbook = GetMarketDatabyID(lstIDs.ToArray(), item.Name, item.EventOpenDate, item.EventTypeName);
                                if (marketbook.Result.Count() > 0)
                                {
                                    if (marketbook.Result[0].Runners != null)
                                    {
                                        marketbooks.Add(marketbook.Result[0]);
                                    }
                                }
                            }
                            foreach (var item in results)
                            {
                                foreach (var item2 in marketbooks)
                                {
                                    if (item.ID == item2.MarketId)
                                    {
                                        item2.MarketBookName = item.EventName + " / " + item.Name;
                                        item2.OrignalOpenDate = item.EventOpenDate;
                                        item2.MainSportsname = item.EventTypeName;
                                        item2.BettingAllowed = item.BettingAllowed;
                                        //item2.BettingAllowedOverAll = await CheckForAllowedBettingOverAll(item.EventTypeName, item2.MarketBookName);
                                        item2.GetMatchUpdatesFrom = item.GetMatchUpdatesFrom;
                                        item2.EventID = item.EventID;

                                        var runnerdesc = objUsersServiceCleint.GetSelectionNamesbyMarketID(item2.MarketId);
                                        foreach (var runnermarketitem in runnerdesc)
                                        {
                                            foreach (var runneritem in item2.Runners)
                                            {
                                                if (runnermarketitem.SelectionID == runneritem.SelectionId.Trim())
                                                {
                                                    runneritem.RunnerName = runnermarketitem.SelectionName;
                                                    runneritem.JockeyName = runnermarketitem.JockeyName;
                                                    runneritem.WearingURL = runnermarketitem.Wearing;
                                                    runneritem.WearingDesc = runnermarketitem.WearingDesc;
                                                    runneritem.Clothnumber = runnermarketitem.ClothNumber;
                                                    runneritem.StallDraw = runnermarketitem.StallDraw;
                                                }
                                            }
                                        }
                                    }
                                }

                            }
                            if (marketbooks.Count == 0)
                            {
                                BettingServiceReference.MarketBook item2 = new BettingServiceReference.MarketBook();
                                var item = results[0];
                                item2.MarketId = item.ID;
                                item2.MarketBookName = item.EventName + " / " + item.Name;
                                item2.OrignalOpenDate = item.EventOpenDate;
                                item2.MainSportsname = item.EventTypeName;
                                item2.BettingAllowed = item.BettingAllowed;
                                //item2.BettingAllowedOverAll = await CheckForAllowedBettingOverAll(item.EventTypeName, item2.MarketBookName);
                                item2.GetMatchUpdatesFrom = item.GetMatchUpdatesFrom;
                                item2.EventID = item.EventID;

                                DateTime OpenDate = item.EventOpenDate.AddHours(5);
                                DateTime CurrentDate = DateTime.Now;
                                TimeSpan remainingdays = (CurrentDate - OpenDate);
                                if (OpenDate < CurrentDate)
                                {
                                    item2.OpenDate = "-" + remainingdays.Days.ToString() + ":" + remainingdays.Hours.ToString() + ":" + remainingdays.Minutes.ToString() + ":" + remainingdays.Seconds.ToString();
                                }
                                else
                                {
                                    item2.OpenDate = (-1 * remainingdays.Days).ToString() + ":" + (-1 * remainingdays.Hours).ToString() + ":" + (-1 * remainingdays.Minutes).ToString() + ":" + (-1 * remainingdays.Seconds).ToString();
                                }
                                item2.MarketStatusstr = "Active";

                                var runnerdesc = await objUsersServiceCleint.GetSelectionNamesbyMarketIDAsync(item2.MarketId);
                                item2.Runners = new List<BettingServiceReference.Runner>().ToArray();
                                var lstRunners = new List<BettingServiceReference.Runner>();
                                foreach (var runnermarketitem in runnerdesc)
                                {
                                    var runneritem = new BettingServiceReference.Runner();
                                    runneritem.SelectionId = runnermarketitem.SelectionID;
                                    runneritem.RunnerName = runnermarketitem.SelectionName;
                                    runneritem.JockeyName = runnermarketitem.JockeyName;
                                    runneritem.WearingURL = runnermarketitem.Wearing;
                                    runneritem.WearingDesc = runnermarketitem.WearingDesc;
                                    runneritem.Clothnumber = runnermarketitem.ClothNumber;
                                    runneritem.StallDraw = runnermarketitem.StallDraw;

                                    runneritem.ExchangePrices = new BettingServiceReference.ExchangePrices();
                                    var lstpricelist = new List<BettingServiceReference.PriceSize>();
                                    for (int i = 0; i < 3; i++)
                                    {
                                        var pricesize = new BettingServiceReference.PriceSize();

                                        pricesize.Size = 0;

                                        pricesize.Price = 0;

                                        lstpricelist.Add(pricesize);
                                    }
                                    runneritem.ExchangePrices.AvailableToBack = lstpricelist.ToArray();
                                    lstpricelist = new List<BettingServiceReference.PriceSize>();
                                    for (int i = 0; i < 3; i++)
                                    {
                                        var pricesize = new BettingServiceReference.PriceSize();

                                        pricesize.Size = 0;

                                        pricesize.Price = 0;

                                        lstpricelist.Add(pricesize);
                                    }
                                    runneritem.ExchangePrices.AvailableToLay = lstpricelist.ToArray();
                                    lstRunners.Add(runneritem);
                                 
                                }
                                item2.Runners = lstRunners.ToArray();
                                item2.FavoriteID = "0";
                                item2.FavoriteBack = "0";
                                item2.FavoriteBackSize = "0";
                                item2.FavoriteLay = "0";
                                item2.FavoriteLaySize = "0";
                                item2.FavoriteSelectionName = "";
                                marketbooks.Add(item2);
                            }

                            // List<UserBets> lstUserBet = _httpContextAccessor.HttpContext.Session.GetObject<List<UserBets>>("userbets");
                            var lstUserBet = JsonConvert.DeserializeObject<List<UserBets>>(objUsersServiceCleint.GetUserbetsbyUserID(LoggedinUserDetail.GetUserID(), _passwordSettingsService.PasswordForValidate));
                            List<UserBets> lstUserBets = lstUserBet.Where(item => item.isMatched == true && item.location != "9").ToList();
                            var lstMarketIDS = lstUserBets.Select(item => item.MarketBookID).Distinct().ToArray();
                            foreach (var item in lstMarketIDS)
                            {
                                var objMarketbook = marketbooks.Where(item2 => item2.MarketId == item).FirstOrDefault();
                                if (objMarketbook != null)
                                {
                                    objMarketbook.DebitCredit = objUserBets.ceckProfitandLoss(objMarketbook, lstUserBets);
                                    foreach (var runner in objMarketbook.Runners)
                                    {
                                        runner.ProfitandLoss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                    }
                                }
                            }
                            return JsonConvert.SerializeObject(marketbooks[0]);
                        }
                        else
                        {
                            return "";
                        }
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    return "";
                }
            }
            catch (System.Exception ex)
            {
                //LoggedinUserDetail.LogError(ex);
                return "";
            }
        }


        [Route("MarketBookData")]
        [HttpGet]
        public async Task<string> MarketBookData(string ID,string sheetname,string MainSportsCategory)
        {
            try
            {
                DateTime marketopendate = DateTime.Now;
                List<string> lstIDs = new List<string>();
                lstIDs = new List<string>();
                lstIDs.Add(ID);
                var marketbook12 = await objBettingClient.GetMarketDatabyIDAsync(lstIDs.ToArray(), sheetname, marketopendate, MainSportsCategory, _passwordSettingsService.PasswordForValidate);        
                return JsonConvert.SerializeObject(marketbook12[0]);
            }
            catch (System.Exception ex)
            {
                //LoggedinUserDetail.LogError(ex);
                return "";
            }
        }
        public async Task<List<BettingServiceReference.MarketBook>> GetMarketDatabyID(string[] marketID, string sheetname, DateTime OrignalOpenDate, string MainSportsCategory)
        {
            if (1 == 1)
            {
                try
                {
                    var marketbooks = new List<BettingServiceReference.MarketBook>();
                    var marketbook = await GetCurrentMarketBook(marketID[0], sheetname, MainSportsCategory, OrignalOpenDate);

                    if (marketbook.MarketId != null)
                    {
                        marketbooks.Add(marketbook);
                    }
                    return marketbooks;
                }
                catch (System.Exception ex)
                {
                    APIConfig.LogError(ex);
                    var marketbooks = new List<BettingServiceReference.MarketBook>();
                    return marketbooks;
                }
            }
            else
            {
                var marketbooks = new List<BettingServiceReference.MarketBook>();
                return marketbooks;
            }
        }
        public async Task<BettingServiceReference.MarketBook> GetCurrentMarketBook(string marketid, string sheetname, string MainSportsCategory, DateTime marketopendate)
        {
            try
            {

                if (LoggedinUserDetail.URLsData.Count == 0)
                {
                    SetURLsData();
                }
                List<string> marketIds = new List<string>()
                {
                 marketid
                };
                if (LoggedinUserDetail.GetCricketDataFrom == "Live")
                {
                    var marketbook = await objBettingClient.GetMarketDatabyIDAsync(marketIds.ToArray(), sheetname, marketopendate, MainSportsCategory, _passwordSettingsService.PasswordForValidate);
                    if (marketbook.Count() > 0)
                    {
                        return marketbook[0];
                    }
                    else
                    {
                        return new BettingServiceReference.MarketBook();
                    }
                }
                else
                {
                    var marketbook = await objBettingClient.GetMarketDatabyIDAsync(marketIds.ToArray(), sheetname, marketopendate, MainSportsCategory, _passwordSettingsService.PasswordForValidate);
                    if (marketbook.Count() > 0)
                    {
                        return marketbook[0];
                    }
                    else
                    {
                        return new BettingServiceReference.MarketBook();
                    }
                    IList<BettingServiceReference.MarketBook> list;
                    if (MainSportsCategory == "Soccer")
                    {
                        list = await ws1.GDAsync(LoggedinUserDetail.SecurityCode, marketIds.ToArray(), 1);
                    }
                    else if (MainSportsCategory == "Tennis")
                    {
                        list = await ws2.GDAsync(LoggedinUserDetail.SecurityCode, marketIds.ToArray(), 2);
                    }
                    else if (MainSportsCategory == "Cricket")
                    {
                        list = await ws4.GDAsync(LoggedinUserDetail.SecurityCode, marketIds.ToArray(), 4);
                    }
                    else if (MainSportsCategory.Contains("Horse Racing"))
                    {
                        list = await ws7.GDAsync(LoggedinUserDetail.SecurityCode, marketIds.ToArray(), 7);
                    }
                    else if (MainSportsCategory.Contains("Greyhound Racing"))
                    {
                        list = await ws4339.GDAsync(LoggedinUserDetail.SecurityCode, marketIds.ToArray(), 13);
                    }
                    else
                    {
                        list = await ws0.GDAsync(LoggedinUserDetail.SecurityCode, marketIds.ToArray(), 0);
                    }
                    if (list.Count > 0)
                    {
                        BettingServiceReference.MarketBook objMarketbook = ConvertJsontoMarketObjectBF(list[0], marketid, marketopendate, sheetname, MainSportsCategory);
                        return objMarketbook;
                    }
                    else
                    {
                        return new BettingServiceReference.MarketBook();
                    }
                }
            }
            catch (System.Exception ex)
            {
                APIConfig.LogError(ex);
                return new BettingServiceReference.MarketBook();
            }
        }
        public BettingServiceReference.MarketBook ConvertJsontoMarketObjectBF(BettingServiceReference.MarketBook BFMarketbook, string marketid, DateTime marketopendate, string sheetname, string MainSportsCategory)
        {
            try
            {
                if (1 == 1)
                {
                    var marketbook = new BettingServiceReference.MarketBook();

                    marketbook.MarketId = BFMarketbook.MarketId;
                    marketbook.SheetName = "";
                    marketbook.IsMarketDataDelayed = BFMarketbook.IsMarketDataDelayed;
                    marketbook.PoundRate = objUsersServiceCleint.GetPoundRatebyUserID(LoggedinUserDetail.GetUserID());
                    marketbook.NumberOfWinners = BFMarketbook.NumberOfWinners;
                    marketbook.MarketBookName = sheetname;
                    marketbook.MainSportsname = MainSportsCategory;
                    marketbook.OrignalOpenDate = marketopendate;
                    marketbook.Version = BFMarketbook.Version;
                    marketbook.TotalMatched = BFMarketbook.TotalMatched;
                    DateTime OpenDate = marketbook.OrignalOpenDate.Value.AddHours(5);
                    DateTime CurrentDate = DateTime.Now;
                    TimeSpan remainingdays = (CurrentDate - OpenDate);
                    if (OpenDate < CurrentDate)
                    {
                        marketbook.OpenDate = "-" + remainingdays.Days.ToString() + ":" + remainingdays.Hours.ToString() + ":" + remainingdays.Minutes.ToString() + ":" + remainingdays.Seconds.ToString();
                    }
                    else
                    {
                        marketbook.OpenDate = (-1 * remainingdays.Days).ToString() + ":" + (-1 * remainingdays.Hours).ToString() + ":" + (-1 * remainingdays.Minutes).ToString() + ":" + (-1 * remainingdays.Seconds).ToString();
                    }

                    if (BFMarketbook.IsInplay == true && BFMarketbook.Status.ToString() == "OPEN")
                    {

                        marketbook.MarketStatusstr = "In Play";
                    }
                    else
                    {
                        if (BFMarketbook.Status.ToString() == "CLOSED")
                        {
                            marketbook.MarketStatusstr = "Closed";
                        }
                        else
                        {
                            if (BFMarketbook.Status.ToString() == "SUSPENDED")
                            {
                                marketbook.MarketStatusstr = "Suspended";
                            }
                            else
                            {
                                marketbook.MarketStatusstr = "Active";
                            }

                        }

                    }

                    List<BettingServiceReference.Runner> lstRunners = new List<BettingServiceReference.Runner>();
                    var runnerList = BFMarketbook.Runners?.ToList();
                    foreach (var runneritem in BFMarketbook.Runners)
                    {
                        var runner = new BettingServiceReference.Runner();
                        runner.Handicap = runneritem.Handicap;
                        runner.StatusStr = runneritem.Status.ToString();
                        runner.SelectionId = runneritem.SelectionId.ToString();
                        runner.LastPriceTraded = runneritem.LastPriceTraded;
                        var lstpricelist = new List<BettingServiceReference.PriceSize>();
                        if (runneritem.ExchangePrices.AvailableToBack != null && runneritem.ExchangePrices.AvailableToBack.Count() > 0)
                        {
                            if (BFMarketbook.Runners.Count() == 1)
                            {
                                try
                                {


                                    if (runneritem.ExchangePrices.AvailableToBack[0].Price.ToString().Contains("."))
                                    {
                                        foreach (var backitems in runneritem.ExchangePrices.AvailableToLay)
                                        {
                                            var pricesize = new BettingServiceReference.PriceSize();

                                            pricesize.Size = Convert.ToInt64(backitems.Size * Convert.ToDouble(marketbook.PoundRate));

                                            pricesize.Price = Convert.ToDouble((backitems.Price + 0.5).ToString("F2"));

                                            lstpricelist.Add(pricesize);
                                        }
                                    }
                                    else
                                    {
                                        foreach (var backitems in runneritem.ExchangePrices.AvailableToBack)
                                        {
                                            var pricesize = new BettingServiceReference.PriceSize();
                                            pricesize.Size = Convert.ToInt64(backitems.Size * Convert.ToDouble(marketbook.PoundRate));
                                            pricesize.Price = backitems.Price;
                                            lstpricelist.Add(pricesize);
                                        }
                                    }
                                }
                                catch (System.Exception ex)
                                {

                                }
                            }
                            else
                            {
                                foreach (var backitems in runneritem.ExchangePrices.AvailableToBack)
                                {
                                    var pricesize = new BettingServiceReference.PriceSize();
                                    pricesize.Size = Convert.ToInt64(backitems.Size * Convert.ToDouble(marketbook.PoundRate));
                                    pricesize.Price = backitems.Price;
                                    lstpricelist.Add(pricesize);
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                var pricesize = new BettingServiceReference.PriceSize();
                                pricesize.Size = 0;
                                pricesize.Price = 0;
                                lstpricelist.Add(pricesize);
                            }
                        }

                        runner.ExchangePrices = new BettingServiceReference.ExchangePrices();
                        runner.ExchangePrices.AvailableToBack = lstpricelist.ToArray();
                        lstpricelist = new List<BettingServiceReference.PriceSize>();
                        if (runneritem.ExchangePrices.AvailableToLay != null && runneritem.ExchangePrices.AvailableToLay.Count() > 0)
                        {
                            if (BFMarketbook.Runners.Count() == 1)
                            {
                                if (runneritem.ExchangePrices.AvailableToLay[0].Price.ToString().Contains("."))
                                {
                                    foreach (var backitems in runneritem.ExchangePrices.AvailableToBack)
                                    {
                                        var pricesize = new BettingServiceReference.PriceSize();

                                        pricesize.Size = Convert.ToInt64((backitems.Size) * Convert.ToDouble(marketbook.PoundRate));

                                        pricesize.Price = Convert.ToDouble((backitems.Price + 0.5).ToString("F2"));

                                        lstpricelist.Add(pricesize);
                                    }
                                }
                                else
                                {
                                    foreach (var backitems in runneritem.ExchangePrices.AvailableToLay)
                                    {
                                        var pricesize = new BettingServiceReference.PriceSize();

                                        pricesize.Size = Convert.ToInt64(backitems.Size * Convert.ToDouble(marketbook.PoundRate));

                                        pricesize.Price = backitems.Price;

                                        lstpricelist.Add(pricesize);
                                    }
                                }
                            }
                            else
                            {
                                foreach (var backitems in runneritem.ExchangePrices.AvailableToLay)
                                {
                                    var pricesize = new BettingServiceReference.PriceSize();

                                    pricesize.Size = Convert.ToInt64(backitems.Size * Convert.ToDouble(marketbook.PoundRate));

                                    pricesize.Price = backitems.Price;

                                    lstpricelist.Add(pricesize);
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                var pricesize = new BettingServiceReference.PriceSize();

                                pricesize.Size = 0;

                                pricesize.Price = 0;

                                lstpricelist.Add(pricesize);
                            }
                        }

                        runner.ExchangePrices.AvailableToLay = new List<BettingServiceReference.PriceSize>().ToArray();
                        runner.ExchangePrices.AvailableToLay = lstpricelist.ToArray();
                        lstRunners.Add(runner);
                    }
                    marketbook.Runners = new List<BettingServiceReference.Runner>(lstRunners).ToArray();

                    double lastback = 0;
                    double lastbackSize = 0;
                    double lastLaySize = 0;
                    double lastlay = 0;

                    if (marketbook.MarketStatusstr != "Suspended")
                    {
                        double favBack = marketbook.Runners[0].ExchangePrices.AvailableToBack[0].Price;
                        string selectionIDfav = marketbook.Runners[0].SelectionId;
                        foreach (var favoriteitem in marketbook.Runners)
                        {
                            if (favoriteitem.ExchangePrices.AvailableToBack != null && favoriteitem.ExchangePrices.AvailableToBack.Length > 0)
                                if (marketbook.MainSportsname.Contains("Racing"))
                                {
                                    if (favoriteitem.ExchangePrices.AvailableToBack[0].Price < favBack && favoriteitem.ExchangePrices.AvailableToBack[0].Price > 0)
                                    {
                                        favBack = favoriteitem.ExchangePrices.AvailableToBack[0].Price;
                                        selectionIDfav = favoriteitem.SelectionId;
                                    }
                                }
                                else
                                {
                                    if (favoriteitem.ExchangePrices.AvailableToBack[0].Price < favBack)
                                    {
                                        favBack = favoriteitem.ExchangePrices.AvailableToBack[0].Price;
                                        selectionIDfav = favoriteitem.SelectionId;
                                    }
                                }
                        }
                        if (marketbook.MarketStatusstr == "Closed")
                        {
                            var resultsfav = marketbook.Runners.Where(item => item.StatusStr == "WINNER");
                            if (resultsfav != null && resultsfav.Count() > 0)
                            {
                                selectionIDfav = resultsfav.FirstOrDefault().SelectionId;
                            }

                        }
                        var favoriteteam = marketbook.Runners.Where(ii => ii.SelectionId == selectionIDfav).FirstOrDefault();
                        string selectionname = favoriteteam.RunnerName;
                        if (favoriteteam.ExchangePrices.AvailableToBack.Length > 0)
                        {
                            lastback = favoriteteam.ExchangePrices.AvailableToBack[0].Price;
                            lastbackSize = favoriteteam.ExchangePrices.AvailableToBack[0].Size;


                        }
                        if (favoriteteam.ExchangePrices.AvailableToLay.Length > 0)
                        {

                            lastLaySize = favoriteteam.ExchangePrices.AvailableToLay[0].Size;
                            lastlay = favoriteteam.ExchangePrices.AvailableToLay[0].Price;

                        }
                        marketbook.FavoriteBack = (lastback - 1).ToString("F2");
                        marketbook.FavoriteLay = (lastlay - 1).ToString("F2");
                        marketbook.FavoriteSelectionName = selectionname;
                        marketbook.FavoriteBackSize = lastbackSize.ToString();
                        marketbook.FavoriteLaySize = lastLaySize.ToString();
                        marketbook.FavoriteID = selectionIDfav;

                    }
                    else
                    {
                        marketbook.FavoriteBack = (lastback - 1).ToString("F2");
                        marketbook.FavoriteLay = (lastlay - 1).ToString("F2");
                        marketbook.FavoriteSelectionName = "";
                        marketbook.FavoriteBackSize = lastbackSize.ToString();
                        marketbook.FavoriteLaySize = lastLaySize.ToString();
                        marketbook.FavoriteID = "0";
                    }
                    if (Convert.ToDouble(marketbook.FavoriteBack) < 0)
                    {
                        marketbook.FavoriteBack = "0";
                        marketbook.FavoriteBackSize = "0";
                    }
                    if (Convert.ToDouble(marketbook.FavoriteLay) < 0)
                    {
                        marketbook.FavoriteLay = "0";
                        marketbook.FavoriteLaySize = "0";
                    }
                    return marketbook;
                }
                else
                {
                    return new BettingServiceReference.MarketBook();
                }
            }
            catch (System.Exception ex)
            {
                APIConfig.LogError(ex);
                return new BettingServiceReference.MarketBook();
            }
        }

        public void SetURLsData()
        {
            LoggedinUserDetail.URLsData = JsonConvert.DeserializeObject<List<SP_URLsData_GetAllData_Result>>(objUsersServiceCleint.GetURLsData());
            ws1.Url = LoggedinUserDetail.URLsData.FirstOrDefault(item => item.EventType == "Soccer").URLForData;
            ws2.Url = LoggedinUserDetail.URLsData.Where(item => item.EventType == "Tennis").FirstOrDefault().URLForData;
            ws4.Url = LoggedinUserDetail.URLsData.Where(item => item.EventType == "Cricket").FirstOrDefault().URLForData;
            ws7.Url = LoggedinUserDetail.URLsData.Where(item => item.EventType == "Horse Racing").FirstOrDefault().URLForData;
            ws4339.Url = LoggedinUserDetail.URLsData.Where(item => item.EventType == "GreyHound Racing").FirstOrDefault().URLForData;
            wsFancy.Url = LoggedinUserDetail.URLsData.Where(item => item.EventType == "Fancy").FirstOrDefault().URLForData;

            ws0.Url = LoggedinUserDetail.URLsData.Where(item => item.EventType == "Other").FirstOrDefault().URLForData;
            LoggedinUserDetail.SecurityCode = LoggedinUserDetail.URLsData.FirstOrDefault().Scd;
            LoggedinUserDetail.GetCricketDataFrom = LoggedinUserDetail.URLsData.Where(item => item.EventType == "Cricket").FirstOrDefault().GetDataFrom;
        }
    }

}
