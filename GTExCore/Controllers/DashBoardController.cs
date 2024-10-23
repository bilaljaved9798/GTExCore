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
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using System.Data;
using UserServiceReference;
namespace GTExCore.Controllers
{
    public class DashBoardController : Controller
    {

		private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        AccessRightsbyUserType objAccessrightsbyUserType;
		BettingServiceClient BettingServiceClient = new BettingServiceClient();
        UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();
        UserServicesClient objUsersServiceCleint = new UserServicesClient();
        private readonly IRazorViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPasswordSettingsService _passwordSettingsService;
        public DashBoardController(IRazorViewEngine viewEngine, ITempDataProvider tempDataProvider, IServiceProvider serviceProvider, IPasswordSettingsService passwordSettingsService, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
            _passwordSettingsService = passwordSettingsService;
        }

        [HttpGet]
        [ActionName("Index")]
        public ActionResult Index()
        {
            if (LoggedinUserDetail.GetUserTypeID() == 3)
            {
                _httpContextAccessor.HttpContext.Session.SetObject("userbets", new List<UserBets>());
                _httpContextAccessor.HttpContext.Session.SetObject("userbet", new List<UserBets>());
            }
            else
            {
                if (LoggedinUserDetail.GetUserTypeID() == 2)
                {
                    _httpContextAccessor.HttpContext.Session.SetObject("userbets", new List<UserBetsforAgent>());
                    _httpContextAccessor.HttpContext.Session.SetObject("userbet", new List<UserBetsforAgent>());
                    ViewBag.backgrod = "#1D9BF0";
                    ViewBag.color = "white";

                }
                else
                {
                    if (LoggedinUserDetail.GetUserTypeID() == 8)
                    {
                        _httpContextAccessor.HttpContext.Session.SetObject("userbets", new List<UserBetsforSuper>());
                        _httpContextAccessor.HttpContext.Session.SetObject("userbet", new List<UserBetsforSuper>());
                        ViewBag.backgrod = "#1D9BF0";
                        ViewBag.color = "white";
                    }

                    else
                    {
                        if (LoggedinUserDetail.GetUserTypeID() == 9)
                        {
                            _httpContextAccessor.HttpContext.Session.SetObject("userbets", new List<UserBetsforSamiadmin>());
                            _httpContextAccessor.HttpContext.Session.SetObject("userbet", new List<UserBetsforSamiadmin>());
                            ViewBag.backgrod = "#1D9BF0";
                            ViewBag.color = "white";
                        }
                        else
                        {
                            _httpContextAccessor.HttpContext.Session.SetObject("userbets", new List<UserBetsForAdmin>());
                            _httpContextAccessor.HttpContext.Session.SetObject("userbet", new List<UserBetsForAdmin>());
                            ViewBag.backgrod = "#1D9BF0";
                            ViewBag.color = "white";
                        }
                    }
                }
            }

            if (LoggedinUserDetail.GetUserID() > 0)
            {
                Decimal CurrentLiabality = 0;
                objAccessrightsbyUserType = new AccessRightsbyUserType();
                objAccessrightsbyUserType.CurrentAvailableBalance = LoggedinUserDetail.CurrentAccountBalance + Convert.ToDouble(CurrentLiabality);
                objAccessrightsbyUserType.Username = LoggedinUserDetail.GetUserName();

                if ((bool)_httpContextAccessor.HttpContext.Session.GetObject<bool>("firsttimeload") == true)
                {
                    _httpContextAccessor.HttpContext.Session.SetObject("firsttimeload", false);
                    LoggedinUserDetail.UpdateCurrentLoggedInID();
                }
                _httpContextAccessor.HttpContext.Session.SetObject("linevmarkets", null);
                return View(objAccessrightsbyUserType);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        //[HttpGet]
        //[ActionName("GetInPlayMatchesC")]
        //public IActionResult GetInPlayMatchesC(int eventId, int userId)
        //{
        //    try
        //    {
        //        DataSet ds = new DataSet();
        //        List<SqlParameter> parameters = new List<SqlParameter>();
        //        parameters.Add(new SqlParameter("@EventTypeID", eventId));
        //        parameters.Add(new SqlParameter("@UserID", userId));
        //        ds = _IUserMarketService.ExcuteSp("SP_UserMarket_GetTodaysHorseRacingV1", parameters, 2);
        //        ds.Tables[0].TableName = "InPlayMatchesC";
        //        ds.Tables[1].TableName = "InPlayMatchesOther";
        //        return Ok(ds);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        log.Fatal("GetInPlayMatchesC:" + ex);
        //        return BadRequest(ex.ToString());
        //    }
        //}

        BettingServiceClient objBettingClient = new BettingServiceClient();
        List<RootSCT> rootsct = new List<RootSCT>();
        public List<BettingServiceReference.MarketBook> GetMarketDatabyID(string[] marketID, string sheetname, DateTime OrignalOpenDate, string MainSportsCategory)
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
                    return marketbooks;
                }
                catch (System.Exception ex)
                {
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
        public static wsnew ws1 = new wsnew();
        public static wsnew ws2 = new wsnew();
        public static wsnew ws4 = new wsnew();
        public static wsnew ws7 = new wsnew();
        public static wsnew ws0 = new wsnew();
        //public static wsnew ws0t = new wsnew();
        public static wsnew ws4339 = new wsnew();
        public static wsnew wsFancy = new wsnew();
        private wsnew wsBFMatch = new wsnew();
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
                    var marketbook =await objBettingClient.GetMarketDatabyIDAsync(marketIds, sheetname, marketopendate, MainSportsCategory, _passwordSettingsService.PasswordForValidate);
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


                    var marketbook =await objBettingClient.GetMarketDatabyIDAsync(marketIds, sheetname, marketopendate, MainSportsCategory, _passwordSettingsService.PasswordForValidate);
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
       public async Task<List<AllMarketsInPlay>> GetManagers()
        {
            List<AllMarketsInPlay> lstGridMarkets = _httpContextAccessor.HttpContext.Session.GetObject<List<AllMarketsInPlay>>("marketsData");
            var session = _httpContextAccessor.HttpContext.Session;

            if (lstGridMarkets != null)
            {
                _ = Task.Run(async () =>
                {
                    var latestData = await LoadManagersFromService();
                    session.SetObject("marketsData", latestData);
                });
                return lstGridMarkets;
            }
            else
            {
                lstGridMarkets = await LoadManagersFromService();
                session.SetObject("marketsData", lstGridMarkets);
                return lstGridMarkets;
            }
        }
        private async Task<List<AllMarketsInPlay>> LoadManagersFromService()
        {
            int userid = LoggedinUserDetail.GetUserID();
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
            List<AllMarketsInPlay> lstGridMarkets = new List<AllMarketsInPlay>();

            foreach (var item in lstIds)
            {
                try
                {
                    InPlayMatches objMarketLocal = lstInPlayMatches.Where(item2 => item2.MarketCatalogueID == item).FirstOrDefault();
                    AllMarketsInPlay objGridMarket = new AllMarketsInPlay();
                    DateTime EventOpenDate = Convert.ToDateTime(objMarketLocal.EventOpenDate);
                    List<string> lstIDs = new List<string>();
                    lstIDs.Add(objMarketLocal.MarketCatalogueID);
                    if (objMarketLocal.EventTypeID == "4" || objMarketLocal.EventTypeID == "1")
                    {
                        if (Convert.ToDateTime(objGridMarket.MarketStartTime) < DateTime.Now && objMarketLocal.EventTypeID == "1")
                        {
                            var marketbooks = GetMarketDatabyID(lstIDs.ToArray(), objMarketLocal.MarketCatalogueName, EventOpenDate, objMarketLocal.EventTypeName);

                            if (marketbooks.Count() > 0)
                            {

                                objGridMarket.Runner1Back = marketbooks[0].Runners[0].ExchangePrices.AvailableToBack[0].Price.ToString();
                                objGridMarket.Runner1BackSize = marketbooks[0].Runners[0].ExchangePrices.AvailableToBack[0].SizeStr.ToString();
                                objGridMarket.Runner1Lay = marketbooks[0].Runners[0].ExchangePrices.AvailableToLay[0].Price.ToString();
                                objGridMarket.Runner1LaySize = marketbooks[0].Runners[0].ExchangePrices.AvailableToBack[0].SizeStr.ToString();

                                objGridMarket.Runner2Lay = marketbooks[0].Runners[1].ExchangePrices.AvailableToLay[0].Price.ToString();
                                objGridMarket.Runner2LaySize = marketbooks[0].Runners[1].ExchangePrices.AvailableToLay[0].SizeStr.ToString();
                                objGridMarket.Runner2Back = marketbooks[0].Runners[1].ExchangePrices.AvailableToBack[0].Price.ToString();
                                objGridMarket.Runner2BackSize = marketbooks[0].Runners[1].ExchangePrices.AvailableToBack[0].SizeStr.ToString();

                                if (marketbooks[0].Runners.Count == 3)
                                {
                                    objGridMarket.Runner3Back = marketbooks[0].Runners[2].ExchangePrices.AvailableToBack[0].Price.ToString();
                                    objGridMarket.Runner3BackSize = marketbooks[0].Runners[2].ExchangePrices.AvailableToBack[0].SizeStr.ToString();
                                    objGridMarket.Runner3Lay = marketbooks[0].Runners[2].ExchangePrices.AvailableToLay[0].Price.ToString();
                                    objGridMarket.Runner3LaySize = marketbooks[0].Runners[2].ExchangePrices.AvailableToLay[0].SizeStr.ToString();
                                }
                                else
                                {
                                    objGridMarket.Runner3Lay = "-";
                                    objGridMarket.Runner3Back = "-";
                                }
                            }
                        }
                        else
                        {
                            var marketbooks = GetMarketDatabyID(lstIDs.ToArray(), objMarketLocal.MarketCatalogueName, EventOpenDate, objMarketLocal.EventTypeName);
                            if (marketbooks.Count() > 0)
                            {

                                objGridMarket.Runner1Back = marketbooks[0].Runners[0].ExchangePrices.AvailableToBack[0].Price.ToString();
                                objGridMarket.Runner1BackSize = marketbooks[0].Runners[0].ExchangePrices.AvailableToBack[0].SizeStr.ToString();
                                objGridMarket.Runner1Lay = marketbooks[0].Runners[0].ExchangePrices.AvailableToLay[0].Price.ToString();
                                objGridMarket.Runner1LaySize = marketbooks[0].Runners[0].ExchangePrices.AvailableToBack[0].SizeStr.ToString();

                                objGridMarket.Runner2Lay = marketbooks[0].Runners[1].ExchangePrices.AvailableToLay[0].Price.ToString();
                                objGridMarket.Runner2LaySize = marketbooks[0].Runners[1].ExchangePrices.AvailableToLay[0].SizeStr.ToString();
                                objGridMarket.Runner2Back = marketbooks[0].Runners[1].ExchangePrices.AvailableToBack[0].Price.ToString();
                                objGridMarket.Runner2BackSize = marketbooks[0].Runners[1].ExchangePrices.AvailableToBack[0].SizeStr.ToString();

                                if (marketbooks[0].Runners.Count == 3)
                                {
                                    objGridMarket.Runner3Back = marketbooks[0].Runners[2].ExchangePrices.AvailableToBack[0].Price.ToString();
                                    objGridMarket.Runner3BackSize = marketbooks[0].Runners[2].ExchangePrices.AvailableToBack[0].SizeStr.ToString();
                                    objGridMarket.Runner3Lay = marketbooks[0].Runners[2].ExchangePrices.AvailableToLay[0].Price.ToString();
                                    objGridMarket.Runner3LaySize = marketbooks[0].Runners[2].ExchangePrices.AvailableToLay[0].SizeStr.ToString();
                                }
                                else
                                {
                                    objGridMarket.Runner3Lay = "-";
                                    objGridMarket.Runner3Back = "-";
                                }
                            }
                        }
                    }
                    objGridMarket.CategoryName = objMarketLocal.EventTypeName;
                    objGridMarket.MarketBookID = objMarketLocal.MarketCatalogueID;
                    objGridMarket.MarketBookName = objMarketLocal.MarketCatalogueName;
                    objGridMarket.EventName = objMarketLocal.EventName;
                    objGridMarket.CompetitionName = objMarketLocal.CompetitionName;
                    objGridMarket.MarketStartTime = objMarketLocal.EventOpenDate.Value.AddHours(5).ToString("dd-MM-yyyy hh:mm tt");
                    if (Convert.ToDateTime(objGridMarket.MarketStartTime) < DateTime.Now && objMarketLocal.MarketStatus == null)
                    {
                        objGridMarket.MarketStatus = "In Play";
                    }
                    else
                    {
                        objGridMarket.MarketStatus = objMarketLocal.MarketStatus;
                    }
                    objGridMarket.CountryCode = objMarketLocal.CountryCode;


                    List<InPlayMatches> lstRunnersID = lstInPlayMatches.Where(item2 => item2.MarketCatalogueID == item).ToList();
                    try
                    {
                        objGridMarket.Runner1 = lstRunnersID.Where(x => !x.SelectionName.Contains("Draw")).FirstOrDefault().SelectionName;
                        objGridMarket.Runner2 = lstRunnersID.Where(x => !x.SelectionName.Contains("Draw")).LastOrDefault().SelectionName;
                        if (lstRunnersID.Count == 3)
                        {
                            objGridMarket.Runner3 = lstRunnersID.Where(x => x.SelectionName.Contains("Draw")).FirstOrDefault().SelectionName;
                        }
                    }
                    catch (System.Exception ex)
                    {

                    }
                    lstGridMarkets.Add(objGridMarket);
                }
                catch (System.Exception ex)
                {
                }
            }
            if (lstGridMarkets == null) lstGridMarkets = new List<AllMarketsInPlay>();
            return lstGridMarkets;
        }
        public async Task<string> GetReletedevent(string eventtype, string marketbookID)
        {
            try
            {
                if (LoggedinUserDetail.GetUserTypeID() != 3)
                {
                    ViewBag.backgrod = "#1D9BF0";
                    ViewBag.color = "white";
                }
                int userid = LoggedinUserDetail.GetUserID();
                if (userid == 1)
                {
                    userid = 73;
                }
                var results = objUsersServiceCleint.GetInPlayMatcheswithRunners1(userid);
                List<InPlayMatches> lstInPlayMatches = JsonConvert.DeserializeObject<List<InPlayMatches>>(results);
                lstInPlayMatches = lstInPlayMatches.Where(item => item.EventTypeName == eventtype).ToList();
                lstInPlayMatches = lstInPlayMatches.GroupBy(car => car.MarketCatalogueID).Select(g => g.First()).ToList();
                lstInPlayMatches = lstInPlayMatches.Where(x => x.MarketCatalogueID != marketbookID).ToList();
                return await RenderRazorViewToStringAsync("RelatedEvent", lstInPlayMatches);

            }

            catch (System.Exception ex)
            {
                return await RenderRazorViewToStringAsync("RelatedEvent", new InPlayMatches());

            }
        }
        public async Task<string> RenderRazorViewToStringAsync(string viewName, object model)
        {
            ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                var viewResult = _viewEngine.FindView(ControllerContext, viewName, false);

                if (viewResult.View == null)
                {
                    throw new ArgumentNullException($"View {viewName} not found.");
                }

                var viewContext = new ViewContext(
                    ControllerContext,
                    viewResult.View,
                    ViewData,
                    TempData,
                    sw,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);
                return sw.ToString();
            }
        }
        public PartialViewResult GetDefaultPageData()
        {
            try
            {
                int userid = LoggedinUserDetail.GetUserID();
                if (userid == 1)
                {
                    userid = 73;
                }
                if (LoggedinUserDetail.GetUserTypeID() == 3)
                {
                    var data = GetManagers();
                    var model = new DefaultPageModel();

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

                    return PartialView("AllInPayMatches", model);
                }
                else
                {
                    var results = objUsersServiceCleint.GetInPlayMatcheswithRunners1(userid);
                    List<InPlayMatches> lstInPlayMatches = JsonConvert.DeserializeObject<List<InPlayMatches>>(results);
                    List<string> lstIds = lstInPlayMatches.Where(item => item.EventTypeName == "Cricket").Distinct().Select(item => item.MarketCatalogueID).Distinct().ToList();
                    lstIds.AddRange(lstInPlayMatches.Where(item => item.EventTypeName == "Soccer").Distinct().Select(item => item.MarketCatalogueID).Distinct().ToList());
                    lstIds.AddRange(lstInPlayMatches.Where(item => item.EventTypeName == "Tennis").Distinct().Select(item => item.MarketCatalogueID).Distinct().ToList());
                    List<AllMarketsInPlay> lstGridMarkets = new List<AllMarketsInPlay>();
                    ViewBag.backgrod = "#1D9BF0 !important";
                    ViewBag.color = "white";
                    foreach (var item in lstIds)
                    {
                        try
                        {
                            InPlayMatches objMarketLocal = lstInPlayMatches.Where(item2 => item2.MarketCatalogueID == item).FirstOrDefault();
                            AllMarketsInPlay objGridMarket = new AllMarketsInPlay();
                            objGridMarket.CategoryName = objMarketLocal.EventTypeName;
                            objGridMarket.MarketBookID = objMarketLocal.MarketCatalogueID;
                            objGridMarket.MarketBookName = objMarketLocal.MarketCatalogueName;
                            objGridMarket.EventName = objMarketLocal.EventName;
                            objGridMarket.CompetitionName = objMarketLocal.CompetitionName;
                            objGridMarket.MarketStartTime = objMarketLocal.EventOpenDate.Value.AddHours(5).ToString("dd-MM-yyyy hh:mm tt");
                            objGridMarket.MarketStatus = objMarketLocal.MarketStatus;

                            List<InPlayMatches> lstRunnersID = lstInPlayMatches.Where(item2 => item2.MarketCatalogueID == item).ToList();
                            try
                            {
                                objGridMarket.Runner1 = lstRunnersID.Where(x => !x.SelectionName.Contains("Draw")).FirstOrDefault().SelectionName;
                                objGridMarket.Runner2 = lstRunnersID.Where(x => !x.SelectionName.Contains("Draw")).LastOrDefault().SelectionName;
                                if (lstRunnersID.Count == 3)
                                {
                                    objGridMarket.Runner3 = lstRunnersID.Where(x => x.SelectionName.Contains("Draw")).FirstOrDefault().SelectionName;
                                }
                            }
                            catch (System.Exception ex)
                            { }
                            lstGridMarkets.Add(objGridMarket);
                        }
                        catch (System.Exception ex)
                        { }
                    }


                    var model = new DefaultPageModel();

                    model.WelcomeMessage = "Please enjoy the non-stop intriguing betting experience only on www.gt-exch.com. Thanks";
                    model.WelcomeHeading = "Notice";
                    model.Rule = "Rule & Regs";
                    model.WelcomeMessage = "All bets apply to Full Time according to the match officials, plus any stoppage time. Extra - time / penalty shoot - outs are not included.If this market is re - opened for In - Play betting, unmatched bets will be cancelled at kick off and the market turned in play.The market will be suspended if it appears that a goal has been scored, a penalty will be given, or a red card will be shown.With the exception of bets for which the 'keep' option has been selected, unmatched bets will be cancelled in the event of a confirmed goal or sending off.Please note that should our data feeds fail we may be unable to manage this game in-play.Customers should be aware   that:Transmissions described as â€œliveâ€ by some broadcasters may actually be delayed.The extent of any such delay may vary, depending on the set-up through which they are receiving pictures or data.If this market is scheduled to go in-play, but due to unforeseen circumstances we are unable to offer the market in-play, then this market will be re-opened for the half-time interval and suspended again an hour after the scheduled kick-off time.";

                    model.AllMarkets = lstGridMarkets;


                    model.ModalContent = new List<string>();
                    string modalli1 = "Dummy text";
                    string modalli2 = "Dummy text";
                    model.ModalContent.Add(modalli1);
                    model.ModalContent.Add(modalli2);

                    return PartialView("AllInPayMatches2", model);
                }
            }
            catch (System.Exception ex)
            {

                return PartialView("AllInPayMatches", new DefaultPageModel());
            }
        }
        public string ConvertDateFormat(string datetoconvert)
        {
            string[] datearr = datetoconvert.Split('-');
            datetoconvert = datearr[2].ToString() + "-" + datearr[1].ToString() + "-" + datearr[0].ToString();
            return datetoconvert;
        }
        public async Task<PartialViewResult> showcompleteduserbetsFancyIN(string marektbookID, string selectionID)
        {
            if (LoggedinUserDetail.GetUserTypeID() == 2)
            {
                ViewBag.backgrod = "#1D9BF0";
                ViewBag.color = "white";

                LoggedinUserDetail.CheckifUserLogin();
                UserBetsUpdateUnmatcedBets objUserbets = new UserBetsUpdateUnmatcedBets();
                List<UserBetsforAgent> lstUserBets = JsonConvert.DeserializeObject<List<UserBetsforAgent>>(objUsersServiceCleint.GetUserBetsbyAgentID(LoggedinUserDetail.GetUserID(), _passwordSettingsService.PasswordForValidate));
                BettingServiceReference.MarketBookForindianFancy CurrentMarketProfitandloss = objUserbets.GetBookPositionIN(marektbookID, selectionID, new List<UserBetsForAdmin>(), new List<UserBetsforSuper>(), new List<UserBetsforSamiadmin>(), lstUserBets, new List<Models.UserBets>());
                if (CurrentMarketProfitandloss.RunnersForindianFancy != null)
                {
                    var lstCurrentMarketBets = lstUserBets.Where(item => item.MarketBookID == marektbookID && item.isMatched == true).ToList();
                    if (lstCurrentMarketBets.Count > 0)
                    {
                        lstCurrentMarketBets = lstCurrentMarketBets.OrderBy(item => Convert.ToInt32(item.UserOdd)).ToList();
                        var maxuserodd = -1 * (Convert.ToInt32(lstCurrentMarketBets[0].UserOdd) - 1);
                        var minuserodd = -1 * (Convert.ToInt32(lstCurrentMarketBets[lstCurrentMarketBets.Count - 1].UserOdd) + 1);
                        CurrentMarketProfitandloss.RunnersForindianFancy = CurrentMarketProfitandloss.RunnersForindianFancy.Where(item => item.Handicap >= minuserodd && item.Handicap <= maxuserodd).ToList();
                    }
                }

                return PartialView("BookPositionIN", CurrentMarketProfitandloss);
            }
            else
            {
                if (LoggedinUserDetail.GetUserTypeID() == 8)
                {
                    ViewBag.backgrod = "#1D9BF0";
                    ViewBag.color = "white";

                    //LoggedinUserDetail.CheckifUserLogin();
                    UserBetsUpdateUnmatcedBets objUserbets = new UserBetsUpdateUnmatcedBets();
                    List<UserBetsforSuper> lstUserBets = JsonConvert.DeserializeObject<List<UserBetsforSuper>>(objUsersServiceCleint.GetUserBetsbySuperID(LoggedinUserDetail.GetUserID(), _passwordSettingsService.PasswordForValidate));
                    BettingServiceReference.MarketBookForindianFancy CurrentMarketProfitandloss = objUserbets.GetBookPositionIN(marektbookID, selectionID, new List<UserBetsForAdmin>(), lstUserBets, new List<UserBetsforSamiadmin>(), new List<UserBetsforAgent>(), new List<Models.UserBets>());

                    if (CurrentMarketProfitandloss.RunnersForindianFancy != null)
                    {
                        var lstCurrentMarketBets = lstUserBets.Where(item => item.MarketBookID == marektbookID && item.isMatched == true).ToList();
                        if (lstCurrentMarketBets.Count > 0)
                        {
                            lstCurrentMarketBets = lstCurrentMarketBets.OrderBy(item => Convert.ToInt32(item.UserOdd)).ToList();
                            var maxuserodd = -1 * (Convert.ToInt32(lstCurrentMarketBets[0].UserOdd) - 1);
                            var minuserodd = -1 * (Convert.ToInt32(lstCurrentMarketBets[lstCurrentMarketBets.Count - 1].UserOdd) + 1);
                            CurrentMarketProfitandloss.RunnersForindianFancy = CurrentMarketProfitandloss.RunnersForindianFancy.Where(item => item.Handicap >= minuserodd && item.Handicap <= maxuserodd).ToList();
                        }
                    }

                    return PartialView("BookPositionIN", CurrentMarketProfitandloss);
                }
                else
                {
                    if (LoggedinUserDetail.GetUserTypeID() == 9)
                    {
                        ViewBag.backgrod = "#1D9BF0";
                        ViewBag.color = "white";

                        LoggedinUserDetail.CheckifUserLogin();
                        UserBetsUpdateUnmatcedBets objUserbets = new UserBetsUpdateUnmatcedBets();
                        List<UserBetsforSamiadmin> lstUserBets = JsonConvert.DeserializeObject<List<UserBetsforSamiadmin>>(objUsersServiceCleint.GetUserBetsbySamiAdmin(LoggedinUserDetail.GetUserID(), _passwordSettingsService.PasswordForValidate));
                        BettingServiceReference.MarketBookForindianFancy CurrentMarketProfitandloss = objUserbets.GetBookPositionIN(marektbookID, selectionID, new List<UserBetsForAdmin>(), new List<UserBetsforSuper>(), lstUserBets, new List<UserBetsforAgent>(), new List<Models.UserBets>());

                        if (CurrentMarketProfitandloss.RunnersForindianFancy != null)
                        {
                            var lstCurrentMarketBets = lstUserBets.Where(item => item.MarketBookID == marektbookID && item.isMatched == true).ToList();
                            if (lstCurrentMarketBets.Count > 0)
                            {
                                lstCurrentMarketBets = lstCurrentMarketBets.OrderBy(item => Convert.ToInt32(item.UserOdd)).ToList();
                                var maxuserodd = -1 * (Convert.ToInt32(lstCurrentMarketBets[0].UserOdd) - 1);
                                var minuserodd = -1 * (Convert.ToInt32(lstCurrentMarketBets[lstCurrentMarketBets.Count - 1].UserOdd) + 1);
                                CurrentMarketProfitandloss.RunnersForindianFancy = CurrentMarketProfitandloss.RunnersForindianFancy.Where(item => item.Handicap >= minuserodd && item.Handicap <= maxuserodd).ToList();
                            }
                        }

                        return PartialView("BookPositionIN", CurrentMarketProfitandloss);
                    }
                    else
                    {
                        LoggedinUserDetail.CheckifUserLogin();
                        UserBetsUpdateUnmatcedBets objUserbets = new UserBetsUpdateUnmatcedBets();
                        List<UserBets> lstUserBets = _httpContextAccessor.HttpContext.Session.GetObject<List<UserBets>>("userbet");
                        BettingServiceReference.MarketBookForindianFancy CurrentMarketProfitandloss = objUserbets.GetBookPositionIN(marektbookID, selectionID, new List<UserBetsForAdmin>(), new List<UserBetsforSuper>(), new List<UserBetsforSamiadmin>(), new List<UserBetsforAgent>(), lstUserBets);
                        if (CurrentMarketProfitandloss.RunnersForindianFancy != null)
                        {
                            var lstCurrentMarketBets = lstUserBets.Where(item => item.MarketBookID == marektbookID && item.isMatched == true).ToList();
                            if (lstCurrentMarketBets.Count > 0)
                            {
                                lstCurrentMarketBets = lstCurrentMarketBets.OrderBy(item => Convert.ToInt32(item.UserOdd)).ToList();
                                var maxuserodd = -1 * (Convert.ToInt32(lstCurrentMarketBets[0].UserOdd) - 1);
                                var minuserodd = -1 * (Convert.ToInt32(lstCurrentMarketBets[lstCurrentMarketBets.Count - 1].UserOdd) + 1);
                                CurrentMarketProfitandloss.RunnersForindianFancy = CurrentMarketProfitandloss.RunnersForindianFancy.Where(item => item.Handicap >= minuserodd && item.Handicap <= maxuserodd).ToList();
                            }
                        }

                        return PartialView("BookPositionIN", CurrentMarketProfitandloss);

                    }
                }
            }
        }
		public PartialViewResult Ledger()
		{
			LoggedinUserDetail.CheckifUserLogin();
			return PartialView();
		}
        public PartialViewResult GetBalnceDetails()
        {
            if (LoggedinUserDetail.GetUserID() > 0)
            {
                double CurrentAccountBalance = 0;
                string CurrentLiabality = "";
                try
                {
                    LoggedinUserDetail.CurrentAccountBalance = Convert.ToDouble(objUsersServiceCleint.GetStartingBalance(LoggedinUserDetail.GetUserID(), _passwordSettingsService.PasswordForValidate));
                    CurrentAccountBalance = Convert.ToDouble(objUsersServiceCleint.GetCurrentBalancebyUser(LoggedinUserDetail.GetUserID(), _passwordSettingsService.PasswordForValidate));
                }
                catch (System.Exception ex)
                {
                }
                if (LoggedinUserDetail.GetUserTypeID() == 3)
                {
                    double laboddmarket = 0;
                    double othermarket = 0;
                    List<UserBets> lstUserBets = JsonConvert.DeserializeObject<List<UserBets>>(objUsersServiceCleint.GetUserbetsbyUserID(LoggedinUserDetail.GetUserID(), _passwordSettingsService.PasswordForValidate));
                    List<UserBets> lstUserBetsF = lstUserBets.Where(x => x.location != "9").ToList();
                    List<UserBets> lstUserBetsfncy = lstUserBets.Where(x => x.location == "9").ToList();
                    laboddmarket = objUserBets.GetLiabalityofCurrentUser(LoggedinUserDetail.GetUserID(), lstUserBetsF);
                    othermarket = objUserBets.GetLiabalityofCurrentUserfancy(LoggedinUserDetail.GetUserID(), lstUserBetsfncy);
                    CurrentLiabality = (laboddmarket + othermarket).ToString("F2");
                    ViewBag.CurrentLiabality = CurrentLiabality;
                    LoggedinUserDetail.CurrentAvailableBalance = CurrentAccountBalance + Convert.ToDouble(CurrentLiabality);
                }
                List<UserLiabality> lstUserLiabality = JsonConvert.DeserializeObject<List<UserLiabality>>(objUsersServiceCleint.GetCurrentLiabality(LoggedinUserDetail.GetUserID()));

                objAccessrightsbyUserType = JsonConvert.DeserializeObject<AccessRightsbyUserType>(objUsersServiceCleint.GetAccessRightsbyUserType(LoggedinUserDetail.GetUserTypeID(), _passwordSettingsService.PasswordForValidate));
                if (LoggedinUserDetail.GetUserTypeID() == 8)
                {
                    decimal TotAdminAmount = 0;
                    decimal TotAdmincommession = 0;
                    decimal TotalAdminAmountWithoutMarkets = 0;
                    List<UserAccounts> AgentCommission = new List<UserAccounts>();
                    List<UserAccounts> lstUserAccountsForAgent = JsonConvert.DeserializeObject<List<UserAccounts>>(objUsersServiceCleint.GetAccountsDatabyCreatedByIDForSuper(LoggedinUserDetail.GetUserID(), false, _passwordSettingsService.PasswordForValidate));
                    if (lstUserAccountsForAgent.Count > 0)
                    {
                        AgentCommission = lstUserAccountsForAgent.Where(item1 => item1.AccountsTitle == "Commission").ToList();
                        lstUserAccountsForAgent = lstUserAccountsForAgent.Where(item1 => item1.AccountsTitle != "Commission").ToList();
                        foreach (UserAccounts objuserAccounts in lstUserAccountsForAgent)
                        {
                            if (objuserAccounts.AccountsTitle != "Commission" && objuserAccounts.MarketBookID != "")
                            {
                                int commissionrate = Convert.ToInt32(objuserAccounts.ComissionRate);
                                int AgentRate = Convert.ToInt32(objuserAccounts.AgentRate);
                                int SuperRate = Convert.ToInt32(objuserAccounts.SuperRate);
                                decimal ActualAmount = Convert.ToDecimal(objuserAccounts.Debit) - Convert.ToDecimal(objuserAccounts.Credit);
                                decimal superpercent = SuperRate - AgentRate;
                                if (ActualAmount > 0)
                                {
                                    decimal SuperAmount = Math.Round((Convert.ToDecimal(superpercent) / 100) * ActualAmount, 2);
                                    decimal AgentAmount = Math.Round((Convert.ToDecimal(AgentRate) / 100) * ActualAmount, 2);
                                    decimal Comissionamount = 0;
                                    if (AgentRate == 100)
                                    {
                                        Comissionamount = 0;
                                    }
                                    else
                                    {
                                        Comissionamount = Math.Round(((Convert.ToDecimal(commissionrate) / 100) * ActualAmount), 2);
                                    }

                                    TotAdminAmount += -1 * (ActualAmount - SuperAmount - AgentAmount);
                                }
                                else
                                {
                                    ActualAmount = -1 * ActualAmount;
                                    decimal SuperAmount = Math.Round((Convert.ToDecimal(superpercent) / 100) * ActualAmount, 2);
                                    decimal AgentAmount = Math.Round((Convert.ToDecimal(AgentRate) / 100) * ActualAmount, 2);
                                    TotAdminAmount += ActualAmount - AgentAmount - SuperAmount;
                                }
                            }
                        }
                    }
                    decimal a = 0;
                    try
                    {

                        foreach (UserAccounts objuserAccounts in AgentCommission)
                        {
                            int commissionrate = Convert.ToInt32(objuserAccounts.ComissionRate);
                            int AgentRate = Convert.ToInt32(objuserAccounts.AgentRate);
                            int SuperRate = Convert.ToInt32(objuserAccounts.SuperRate);
                            decimal ActualAmount = Convert.ToDecimal(objuserAccounts.Debit) - Convert.ToDecimal(objuserAccounts.Credit);
                            decimal superpercent = SuperRate - AgentRate;
                            ActualAmount = -1 * ActualAmount;
                            decimal SuperAmount = Math.Round((Convert.ToDecimal(superpercent) / 100) * ActualAmount, 2);
                            decimal AgentAmount = Math.Round((Convert.ToDecimal(AgentRate) / 100) * ActualAmount, 2);
                            TotAdmincommession += ActualAmount - AgentAmount - SuperAmount;
                        }
                    }
                    catch (System.Exception ex)
                    {
                    }
                    if (LoggedinUserDetail.IsCom == true)
                    {
                        a = (-1 * (TotAdminAmount));
                        ViewBag.commission = TotAdmincommession;
                    }
                    else
                    {
                        a = (-1 * (TotAdminAmount) + (-1 * TotAdmincommession));
                    }
                    objAccessrightsbyUserType.NetBalance = a.ToString();
                }
                else
                {
                    if (LoggedinUserDetail.GetUserTypeID() == 3)
                    {
                        ViewBag.NetBalance = objUsersServiceCleint.GetProfitorLossbyUserID(LoggedinUserDetail.GetUserID(), false, _passwordSettingsService.PasswordForValidate).ToString();
                    }

                    else
                    {
                        if (LoggedinUserDetail.GetUserTypeID() == 2)
                        {
                            ViewBag.backgrod = "#1D9BF0";
                            ViewBag.color = "white";
                            decimal TotAdminAmount = 0;
                            decimal TotalAdminAmountWithoutMarkets = 0;
                            decimal SuperAmount = 0;
                            decimal SuperAmount1 = 0;
                            List<UserAccounts> lstUserAccountsForAgent = JsonConvert.DeserializeObject<List<UserAccounts>>(objUsersServiceCleint.GetAccountsDatabyCreatedByID(LoggedinUserDetail.GetUserID(), false, _passwordSettingsService.PasswordForValidate));
                            if (lstUserAccountsForAgent.Count > 0)
                            {
                                lstUserAccountsForAgent = lstUserAccountsForAgent.Where(item1 => item1.AccountsTitle != "Commission").ToList();
                                foreach (UserAccounts objuserAccounts in lstUserAccountsForAgent)
                                {
                                    if (objuserAccounts.AccountsTitle != "Commission" && objuserAccounts.MarketBookID != "")
                                    {
                                        int commissionrate = Convert.ToInt32(objuserAccounts.ComissionRate);
                                        int AgentRate = Convert.ToInt32(objuserAccounts.AgentRate);
                                        int SuperRate = Convert.ToInt32(objuserAccounts.SuperRate);
                                        decimal ActualAmount = Convert.ToDecimal(objuserAccounts.Debit) - Convert.ToDecimal(objuserAccounts.Credit);
                                        decimal superpercent = 0;
                                        if (SuperRate > 0)
                                        {
                                            superpercent = SuperRate - AgentRate;
                                        }
                                        else
                                        {
                                            superpercent = 0;
                                        }
                                        if (ActualAmount > 0)
                                        {
                                            SuperAmount = Math.Round((Convert.ToDecimal(superpercent) / 100) * ActualAmount, 2);
                                            decimal AgentAmount = Math.Round((Convert.ToDecimal(AgentRate) / 100) * ActualAmount, 2);
                                            decimal Comissionamount = 0;
                                            if (AgentRate == 100)
                                            {
                                                Comissionamount = 0;
                                            }
                                            else
                                            {
                                                Comissionamount = Math.Round(((Convert.ToDecimal(commissionrate) / 100) * ActualAmount), 2);
                                            }

                                            TotAdminAmount += -1 * (ActualAmount - (AgentAmount + SuperAmount) - Comissionamount);
                                            SuperAmount1 += -1 * SuperAmount;
                                        }
                                        else
                                        {
                                            ActualAmount = -1 * ActualAmount;
                                            SuperAmount = Math.Round((Convert.ToDecimal(superpercent) / 100) * ActualAmount, 2);
                                            decimal AgentAmount = Math.Round((Convert.ToDecimal(AgentRate) / 100) * ActualAmount, 2);
                                            TotAdminAmount += ActualAmount - (AgentAmount + SuperAmount);
                                            SuperAmount1 += SuperAmount;
                                        }
                                    }
                                }
                            }
                            int createdbyid = objUsersServiceCleint.GetCreatedbyID(LoggedinUserDetail.GetUserID());
                            string a = "";
                            List<UserAccounts> lstAccountsDonebyAdmin = JsonConvert.DeserializeObject<List<UserAccounts>>(objUsersServiceCleint.GetAccountsDataForAdmin(createdbyid, false, _passwordSettingsService.PasswordForValidate));
                            if (lstAccountsDonebyAdmin.Count > 0)
                            {
                                List<UserAccounts> lstAccountsDonebyAdminagainstthisAgent = lstAccountsDonebyAdmin.Where(item => item.AccountsTitle.Contains("(UserID=" + LoggedinUserDetail.GetUserID().ToString() + ")")).ToList();
                                if (lstAccountsDonebyAdminagainstthisAgent.Count > 0)
                                {
                                    TotalAdminAmountWithoutMarkets = lstAccountsDonebyAdminagainstthisAgent.Sum(item => Convert.ToDecimal(item.Debit)) - lstAccountsDonebyAdminagainstthisAgent.Sum(item => Convert.ToDecimal(item.Credit));
                                }
                            }
                            decimal AgentCommission = 0;
                            decimal superCommission = 0;
                            try
                            {
                                AgentCommission = objUsersServiceCleint.GetTotalAgentCommissionbyAgentID(LoggedinUserDetail.GetUserID(), _passwordSettingsService.PasswordForValidate);
                            }
                            catch (System.Exception ex)
                            {
                            }
                            if (LoggedinUserDetail.IsCom == true)
                            {
                                a = ((-1 * (TotAdminAmount) + (-1 * TotalAdminAmountWithoutMarkets) + (-1 * (SuperAmount1)))).ToString();
                                ViewBag.commission = AgentCommission;
                            }
                            else
                            {
                                a = ((-1 * (TotAdminAmount) + (-1 * TotalAdminAmountWithoutMarkets) + (-1 * (SuperAmount1))) + AgentCommission).ToString();
                            }

                            ViewBag.NetBalance = a;
                        }
                        else
                        {
                            if (LoggedinUserDetail.GetUserTypeID() == 9)
                            {
                                ViewBag.backgrod = "#1D9BF0";
                                ViewBag.color = "white";
                                decimal TotAdminAmount = 0;
                                decimal TotAdmincommession = 0;
                                decimal TotalAdminAmountWithoutMarkets = 0;
                                List<UserAccounts> AgentCommission = new List<UserAccounts>();
                                List<UserAccounts> lstUserAccountsForAgent = JsonConvert.DeserializeObject<List<UserAccounts>>(objUsersServiceCleint.GetAccountsDatabyCreatedByIDForSamiAdmin(LoggedinUserDetail.GetUserID(), false, _passwordSettingsService.PasswordForValidate));
                                if (lstUserAccountsForAgent.Count > 0)
                                {
                                    AgentCommission = lstUserAccountsForAgent.Where(item1 => item1.AccountsTitle == "Commission").ToList();
                                    lstUserAccountsForAgent = lstUserAccountsForAgent.Where(item1 => item1.AccountsTitle != "Commission").ToList();
                                    foreach (UserAccounts objuserAccounts in lstUserAccountsForAgent)
                                    {
                                        if (objuserAccounts.AccountsTitle != "Commission" && objuserAccounts.MarketBookID != "")
                                        {
                                            int commissionrate = Convert.ToInt32(objuserAccounts.ComissionRate);
                                            int AgentRate = Convert.ToInt32(objuserAccounts.AgentRate);
                                            int SuperRate = Convert.ToInt32(objuserAccounts.SuperRate);
                                            int SamiadminRate = Convert.ToInt32(objuserAccounts.SamiadminRate);
                                            decimal ActualAmount = Convert.ToDecimal(objuserAccounts.Debit) - Convert.ToDecimal(objuserAccounts.Credit);
                                            decimal superpercent = SuperRate - AgentRate;
                                            decimal samiadminpercent = SamiadminRate - (superpercent + AgentRate);
                                            if (ActualAmount > 0)
                                            {
                                                decimal SuperAmount = Math.Round((Convert.ToDecimal(superpercent) / 100) * ActualAmount, 2);
                                                decimal SamiadminAmount = Math.Round((Convert.ToDecimal(samiadminpercent) / 100) * ActualAmount, 2);
                                                decimal AgentAmount = Math.Round((Convert.ToDecimal(AgentRate) / 100) * ActualAmount, 2);
                                                decimal Comissionamount = 0;
                                                if (AgentRate == 100)
                                                {
                                                    Comissionamount = 0;
                                                }
                                                else
                                                {
                                                    Comissionamount = Math.Round(((Convert.ToDecimal(commissionrate) / 100) * ActualAmount), 2);
                                                }

                                                TotAdminAmount += -1 * (ActualAmount - SuperAmount - AgentAmount - SamiadminAmount);
                                            }
                                            else
                                            {
                                                ActualAmount = -1 * ActualAmount;
                                                decimal SuperAmount = Math.Round((Convert.ToDecimal(superpercent) / 100) * ActualAmount, 2);
                                                decimal SamiadminAmount = Math.Round((Convert.ToDecimal(samiadminpercent) / 100) * ActualAmount, 2);
                                                decimal AgentAmount = Math.Round((Convert.ToDecimal(AgentRate) / 100) * ActualAmount, 2);
                                                TotAdminAmount += ActualAmount - AgentAmount - SuperAmount - SamiadminAmount;
                                            }
                                        }
                                    }
                                }

                                try
                                {
                                    foreach (UserAccounts objuserAccounts in AgentCommission)
                                    {
                                        int commissionrate = Convert.ToInt32(objuserAccounts.ComissionRate);
                                        int AgentRate = Convert.ToInt32(objuserAccounts.AgentRate);
                                        int SuperRate = Convert.ToInt32(objuserAccounts.SuperRate);
                                        int SamiadminRate = Convert.ToInt32(objuserAccounts.SamiadminRate);
                                        decimal ActualAmount = Convert.ToDecimal(objuserAccounts.Debit) - Convert.ToDecimal(objuserAccounts.Credit);
                                        decimal superpercent = SuperRate - AgentRate;
                                        decimal samiadminpercent = SamiadminRate - (superpercent + AgentRate);
                                        ActualAmount = -1 * ActualAmount;
                                        decimal SuperAmount = Math.Round((Convert.ToDecimal(superpercent) / 100) * ActualAmount, 2);
                                        decimal AgentAmount = Math.Round((Convert.ToDecimal(AgentRate) / 100) * ActualAmount, 2);
                                        decimal SamiadminAmount = Math.Round((Convert.ToDecimal(samiadminpercent) / 100) * ActualAmount, 2);
                                        TotAdmincommession += ActualAmount - AgentAmount - SuperAmount - SamiadminAmount;
                                    }
                                }
                                catch (System.Exception ex)
                                {
                                }
                                decimal a = (-1 * (TotAdminAmount) + (-1 * TotAdmincommession));

                                ViewBag.NetBalance = a.ToString();
                            }
                        }

                    }
                }
            }

            return PartialView("BalanceDetails", objAccessrightsbyUserType);
        }
        public PartialViewResult AllUsers()
        {
            List<UserIDandUserType> lstUsers = GetUsersbyUsersType();
            return PartialView(lstUsers);          
        }
        public List<UserIDandUserType> GetUsersbyUsersType()
        {
            LoggedinUserDetail.CheckifUserLogin();
            var results = "";
            if (LoggedinUserDetail.GetUserTypeID() != 3)
            {
                results = objUsersServiceCleint.GetAllUsersbyUserType(LoggedinUserDetail.GetUserID(), LoggedinUserDetail.GetUserTypeID(), _passwordSettingsService.PasswordForValidate);
            }

            if (results != "")
            {
                List<UserIDandUserType> lstUsers = JsonConvert.DeserializeObject<List<UserIDandUserType>>(results);

                foreach (UserIDandUserType objuser in lstUsers)
                {
                    objuser.UserName = Crypto.Decrypt(objuser.UserName);
                    objuser.UserName = objuser.UserName + " (" + objuser.UserType + ")";
                }
                UserIDandUserType userdefult = new UserIDandUserType();
                userdefult.ID = 0;
                userdefult.UserName = "Please Select";
                lstUsers.Insert(0, userdefult);
                return lstUsers;
            }
            else
            {
                List<UserIDandUserType> lstUsers = new List<UserIDandUserType>();
                return lstUsers;
            }
        }
        public int GetBetPlaceInterval(string categoryname, string Marketbookname, string Runnerscount)
        {
            return LoggedinUserDetail.GetBetPlacewaitTimerandInterval(categoryname, Marketbookname, Runnerscount);
        }
    }
}
