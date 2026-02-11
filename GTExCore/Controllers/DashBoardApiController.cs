using AccountServiceReference;
using BettingServiceReference;
using Global.API;
using GTCore.Models;
using GTCore.ViewModel;
using GTExCore.Common;
using GTExCore.HelperClass;
using GTExCore.HelperClasses;
using GTExCore.Models;
using GTExCore.ViewModel;
using log4net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using System.Globalization;
using System.Reflection;
using UserServiceReference;

namespace GTExCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DashBoardApiController : ControllerBase
    {

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        AccessRightsbyUserType objAccessrightsbyUserType;
        BettingServiceClient BettingServiceClient = new BettingServiceClient();
        UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();
        UserServicesClient objUsersServiceCleint = new UserServicesClient();
        AccountsServiceClient objAccountsService = new AccountsServiceClient();
        private readonly IServiceProvider _serviceProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPasswordSettingsService _passwordSettingsService;
        public static wsnew ws1 = new wsnew();
        public static wsnew ws2 = new wsnew();
        public static wsnew ws4 = new wsnew();
        public static wsnew ws7 = new wsnew();
        public static wsnew ws0 = new wsnew();
        //public static wsnew ws0t = new wsnew();
        public static wsnew ws4339 = new wsnew();
        public static wsnew wsFancy = new wsnew();
        private wsnew wsBFMatch = new wsnew();
        public DashBoardApiController(IServiceProvider serviceProvider, IPasswordSettingsService passwordSettingsService, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _serviceProvider = serviceProvider;
            _passwordSettingsService = passwordSettingsService;
        }

        [Route("GetDefaultPageData")]
        [HttpGet]
        public IActionResult GetDefaultPageData(int userid)
        {
            try
            {

                if (userid == 1)
                {
                    userid = 73;
                }
                // if (LoggedinUserDetail.GetUserTypeID() == 3)
                //{
                var data = GetManagers(userid);
                var model = new DefaultPageModel1();

                model.WelcomeMessage = "Please enjoy the non-stop intriguing betting experience only on www.gt-exch.com. Thanks";
                model.WelcomeHeading = "Notice";
                model.Rule = "Rule & Regs";
                model.WelcomeMessage = "All bets apply to Full Time according to the match officials, plus any stoppage time. Extra - time / penalty shoot - outs are not included.If this market is re - opened for In - Play betting, unmatched bets will be cancelled at kick off and the market turned in play.The market will be suspended if it appears that a goal has been scored, a penalty will be given, or a red card will be shown.With the exception of bets for which the 'keep' option has been selected, unmatched bets will be cancelled in the event of a confirmed goal or sending off.Please note that should our data feeds fail we may be unable to manage this game in-play.Customers should be aware   that:Transmissions described as â€œliveâ€ by some broadcasters may actually be delayed.The extent of any such delay may vary, depending on the set-up through which they are receiving pictures or data.If this market is scheduled to go in-play, but due to unforeseen circumstances we are unable to offer the market in-play, then this market will be re-opened for the half-time interval and suspended again an hour after the scheduled kick-off time.";
                model.AllMarkets = data.Result;
                model.TodayHorseRacing = new List<TodayHorseRacing>();

                model.ModalContent = new List<string>();
                string modalli1 = "Dummy text";
                string modalli2 = "Dummy text";
                model.ModalContent.Add(modalli1);
                model.ModalContent.Add(modalli2);

                return Ok(new { page = model });
                //}

                //return BadRequest("Invalid model");
            }
            catch (System.Exception ex)
            {
                return BadRequest("Invalid model");
            }
        }

        public async Task<List<BettingServiceReference.MarketBook>> GetManagers(int userid)
        {
            List<BettingServiceReference.MarketBook> lstGridMarkets = _httpContextAccessor.HttpContext.Session.GetObject<List<BettingServiceReference.MarketBook>>("marketsData");
            var session = HttpContext.Session;

            if (lstGridMarkets != null)
            {
                _ = Task.Run(async () =>
                {
                    var latestData = await LoadManagersFromService(userid);
                    session.SetObject("marketsData", latestData);
                });
                return lstGridMarkets;
            }
            else
            {
                lstGridMarkets = await LoadManagersFromService(userid);
                _httpContextAccessor.HttpContext.Session.SetObject("marketsData", lstGridMarkets);
                return lstGridMarkets;
            }
        }
        private async Task<List<BettingServiceReference.MarketBook>> LoadManagersFromService(int userid)
        {

            if (userid == 1)
            {
                userid = 73;
            }
            var results = objUsersServiceCleint.GetInPlayMatcheswithRunners1(userid);
            List<InPlayMatches> lstInPlayMatches = JsonConvert.DeserializeObject<List<InPlayMatches>>(results);
            List<string> lstIds = lstInPlayMatches.Where(item => item.EventTypeName == "Cricket").Distinct().Select(item => item.MarketCatalogueID).Distinct().ToList();
            lstIds.AddRange(lstInPlayMatches.Where(item => item.EventTypeName == "Soccer").Distinct().Select(item => item.MarketCatalogueID).Distinct().ToList());
            lstIds.AddRange(lstInPlayMatches.Where(item => item.EventTypeName == "Tennis").Distinct().Select(item => item.MarketCatalogueID).Distinct().ToList());
            lstIds.AddRange(lstInPlayMatches.Where(item => item.EventTypeName == "Horse Racing").Distinct().Select(item => item.MarketCatalogueID).Distinct().ToList());
            lstIds.AddRange(lstInPlayMatches.Where(item => item.EventTypeName == "Greyhound Racing").Distinct().Select(item => item.MarketCatalogueID).Distinct().ToList());
            List<BettingServiceReference.MarketBook> lstGridMarkets = new List<BettingServiceReference.MarketBook>();

            foreach (var item in lstIds)
            {
                try
                {
                    InPlayMatches objMarketLocal = lstInPlayMatches.Where(item2 => item2.MarketCatalogueID == item).FirstOrDefault();
                    BettingServiceReference.MarketBook objGridMarket = new BettingServiceReference.MarketBook();
                    DateTime EventOpenDate = Convert.ToDateTime(objMarketLocal.EventOpenDate);
                    if (objMarketLocal.EventTypeID == "4" || objMarketLocal.EventTypeID == "1")
                    {
                        if (Convert.ToDateTime(objMarketLocal.EventOpenDate.Value.AddHours(5).ToString("dd-MM-yyyy hh:mm tt")) < DateTime.Now && objMarketLocal.EventTypeID == "4")
                        {
                            objGridMarket = MarketBookData(objMarketLocal.MarketCatalogueID, objMarketLocal.MarketCatalogueName, objMarketLocal.EventTypeName);
                            var marketTime = DateTime.ParseExact(objGridMarket.OrignalOpenDate.Value.AddHours(5).ToString("dd-MM-yyyy hh:mm tt"), "dd-MM-yyyy hh:mm tt", CultureInfo.InvariantCulture);
                            objGridMarket.OpenDate = marketTime.ToString();
                            objGridMarket.SheetName = objMarketLocal.EventName;
                        }
                        else
                        {
                            objGridMarket =  MarketBookData(objMarketLocal.MarketCatalogueID, objMarketLocal.MarketCatalogueName, objMarketLocal.EventTypeName);
                            var marketTime = DateTime.ParseExact(objGridMarket.OrignalOpenDate.Value.AddHours(5).ToString("dd-MM-yyyy hh:mm tt"), "dd-MM-yyyy hh:mm tt", CultureInfo.InvariantCulture);
                            objGridMarket.OpenDate = marketTime.ToString();
                            objGridMarket.SheetName = objMarketLocal.EventName;
                        }
                    }

                   
                    //List<InPlayMatches> lstRunnersID = lstInPlayMatches.Where(item2 => item2.MarketCatalogueID == item).ToList();

                    lstGridMarkets.Add(objGridMarket);
                }
                catch (System.Exception ex)
                {
                }
            }
            if (lstGridMarkets == null) lstGridMarkets = new List<BettingServiceReference.MarketBook>();
            return lstGridMarkets;
        }
        BettingServiceClient objBettingClient = new BettingServiceClient();
        List<RootSCT> rootsct = new List<RootSCT>();

        public BettingServiceReference.MarketBook MarketBookData(string ID, string sheetname, string MainSportsCategory)
        {
            DateTime marketopendate = DateTime.Now;
            List<string> lstIDs = new List<string>();
            lstIDs = new List<string>();
            lstIDs.Add(ID);
            var marketbook12 = objBettingClient.GetMarketDatabyIDAsync(lstIDs, sheetname, marketopendate, MainSportsCategory, _passwordSettingsService.PasswordForValidate);
            var resultList = marketbook12.Result;
            return resultList != null && resultList.Count > 0 ? resultList.FirstOrDefault() : null;
        }
        public BettingServiceReference.MarketBook GetMarketDatabyID(string[] marketID, string sheetname, DateTime OrignalOpenDate, string MainSportsCategory)
        {
            if (1 == 1)
            {
                try
                {
                    var marketbooks = new List<BettingServiceReference.MarketBook>();
                    var marketbook = GetCurrentMarketBook(marketID[0], sheetname, MainSportsCategory, OrignalOpenDate);

                    if (marketbook.Result.MarketId != null)
                    {
                        marketbooks.Add(marketbook.Result);
                    }
                    return marketbooks[0];
                }
                catch (System.Exception ex)
                {
                    var marketbooks = new List<BettingServiceReference.MarketBook>();
                    return marketbooks[0];
                }
            }
            else
            {
                var marketbooks = new List<BettingServiceReference.MarketBook>();
                return marketbooks[0];
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
                List<string> marketIds = new List<string>
                   {
                    marketid
                   };
                if (LoggedinUserDetail.GetCricketDataFrom == "Live")
                {
                    var marketbook = await objBettingClient.GetMarketDatabyIDAsync(marketIds, sheetname, marketopendate, MainSportsCategory, _passwordSettingsService.PasswordForValidate);
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


                    var marketbook = await objBettingClient.GetMarketDatabyIDAsync(marketIds, sheetname, marketopendate, MainSportsCategory, _passwordSettingsService.PasswordForValidate);
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
                //APIConfig.LogError(ex);
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
                        runner.ExchangePrices.AvailableToBack = lstpricelist;
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

                        runner.ExchangePrices.AvailableToLay = new List<BettingServiceReference.PriceSize>();
                        runner.ExchangePrices.AvailableToLay = lstpricelist;
                        lstRunners.Add(runner);
                    }
                    marketbook.Runners = new List<BettingServiceReference.Runner>(lstRunners);

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
                            if (favoriteitem.ExchangePrices.AvailableToBack != null && favoriteitem.ExchangePrices.AvailableToBack.Count > 0)
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
                        if (favoriteteam.ExchangePrices.AvailableToBack.Count > 0)
                        {
                            lastback = favoriteteam.ExchangePrices.AvailableToBack[0].Price;
                            lastbackSize = favoriteteam.ExchangePrices.AvailableToBack[0].Size;


                        }
                        if (favoriteteam.ExchangePrices.AvailableToLay.Count > 0)
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
                //APIConfig.LogError(ex);
                return new BettingServiceReference.MarketBook();
            }
        }
        public void SetURLsData()
        {
            LoggedinUserDetail.URLsData = JsonConvert.DeserializeObject<List<SP_URLsData_GetAllData_Result>>(objUsersServiceCleint.GetURLsData());
            ws1.Url = LoggedinUserDetail.URLsData.Where(item => item.EventType == "Soccer").FirstOrDefault().URLForData;
            ws2.Url = LoggedinUserDetail.URLsData.Where(item => item.EventType == "Tennis").FirstOrDefault().URLForData;
            ws4.Url = LoggedinUserDetail.URLsData.Where(item => item.EventType == "Cricket").FirstOrDefault().URLForData;
            ws7.Url = LoggedinUserDetail.URLsData.Where(item => item.EventType == "Horse Racing").FirstOrDefault().URLForData;
            ws4339.Url = LoggedinUserDetail.URLsData.Where(item => item.EventType == "GreyHound Racing").FirstOrDefault().URLForData;
            wsFancy.Url = LoggedinUserDetail.URLsData.Where(item => item.EventType == "Fancy").FirstOrDefault().URLForData;

            ws0.Url = LoggedinUserDetail.URLsData.Where(item => item.EventType == "Other").FirstOrDefault().URLForData;
            // ws0t.Url = LoggedinUserDetail.URLsData.Where(item => item.EventType == "BP").FirstOrDefault().URLForData;
            LoggedinUserDetail.SecurityCode = LoggedinUserDetail.URLsData.FirstOrDefault().Scd;
            LoggedinUserDetail.GetCricketDataFrom = LoggedinUserDetail.URLsData.Where(item => item.EventType == "Cricket").FirstOrDefault().GetDataFrom;
        }


    }
}
