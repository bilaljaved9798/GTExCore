using BettingServiceReference;
using Global.API;
using GTCore.Models;
using GTExCore.Common;
using GTExCore.HelperClass;
using GTExCore.Models;
using GTExCore.ViewModel;
using log4net;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Crypto.Engines;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using UserServiceReference;

namespace Census.API.Controllers
{

    public class MarketController : Controller
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private BettingServiceClient objBettingClient = new BettingServiceClient();
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IRazorViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;
        private readonly IPasswordSettingsService _passwordSettingsService;
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
        public MarketController(IRazorViewEngine viewEngine, ITempDataProvider tempDataProvider, IServiceProvider serviceProvider, IConfiguration configuration, IPasswordSettingsService passwordSettingsService, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
            _configuration = configuration;
            _passwordSettingsService = passwordSettingsService;
        }

        public PartialViewResult Index()
        {

            if (LoggedinUserDetail.GetUserTypeID() == 1 || LoggedinUserDetail.GetUserTypeID() == 2 || LoggedinUserDetail.GetUserTypeID() == 8 || LoggedinUserDetail.GetUserTypeID() == 9)
            {
                ViewBag.backgrod = "-webkit-linear-gradient(top, rgb(29, 155, 240), rgb(10, 10, 10))";
                ViewBag.color = "white";
            }

            //LoggedinUserDetail.CheckifUserLogin();
            var marketFilter = new MarketFilter();

            if (LoggedinUserDetail.GetUserTypeID() != 1)
            {
                List<GTExCore.Models.EventType> lstClientlist = JsonConvert.DeserializeObject<List<GTExCore.Models.EventType>>(objUsersServiceCleint.GetEventTypeIDs(LoggedinUserDetail.GetUserID()));

                return PartialView("EventType", lstClientlist);
            }
            else
            {
                List<GTExCore.Models.EventType> lstClientlist = new List<GTExCore.Models.EventType>();


                return PartialView("EventType", lstClientlist);
            }



        }
        public async Task<string> MarketBook(string ID)
        {
            try
            {
                UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();
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
                if (ID != "" && LoggedinUserDetail.GetUserTypeID() == 1 || LoggedinUserDetail.GetUserTypeID() == 8 || LoggedinUserDetail.GetUserTypeID() == 9)
                {

                    objUsersServiceCleint.SetMarketBookOpenbyUSer(73, ID);
                }
                if (LoggedinUserDetail.GetUserTypeID() == 3)
                {
                    var results = JsonConvert.DeserializeObject<List<MarketCatalgoue>>(objUsersServiceCleint.GetMarketsOpenedbyUser(LoggedinUserDetail.GetUserID()));
                    if (results != null)
                    {
                        results = results.Where(item => item.ID == ID).ToList();
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
                                    item2.BettingAllowedOverAll = await CheckForAllowedBettingOverAll(item.EventTypeName, item2.MarketBookName);
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
                                    //if (item2.MarketBookName.Contains("Match Odds") && item2.MainSportsname == "Cricket" && item2.MarketStatusstr != "Closed")
                                    //{
                                    //    item2.CricketMatchKey = objUsersServiceCleint.GetCricketMatchKey(item2.MarketId);
                                    //    var resultslinev = objUsersServiceCleint.GetEventDetailsbyMarketBook(item2.MarketId);
                                    //    int UserIDforLinevmarkets = 0;
                                    //    if (LoggedinUserDetail.GetUserTypeID() == 1)
                                    //    {
                                    //        UserIDforLinevmarkets = 73;
                                    //    }
                                    //    else
                                    //    {
                                    //        UserIDforLinevmarkets = LoggedinUserDetail.GetUserID();
                                    //    }
                                    //    var linevmarkets = JsonConvert.DeserializeObject<List<LineVMarket>>(objUsersServiceCleint.GetLinevMarketsbyEventID(resultslinev.EventID, resultslinev.EventOpenDate.Value, UserIDforLinevmarkets));

                                    //    List<BettingServiceReference.LinevMarkets> convertedList = linevmarkets
                                    //     .Select(l => new BettingServiceReference.LinevMarkets
                                    //     {
                                    //         MarketCatalogueIDk__BackingField = l.MarketCatalogueID,
                                    //         EventIDk__BackingField = l.EventID,
                                    //         CompetitionIDk__BackingField = l.CompetitionID,
                                    //         isOpenedbyUserk__BackingField = l.isOpenedbyUser,
                                    //         SelectionNamek__BackingField = l.SelectionName,
                                    //         MarketCatalogueNamek__BackingField = l.MarketCatalogueName,
                                    //         SelectionIDk__BackingField = l.SelectionID,
                                    //         BettingAllowedk__BackingField = l.BettingAllowed,
                                    //         CompetitionNamek__BackingField = l.CompetitionName,
                                    //         EventNamek__BackingField = l.EventName
                                    //     })
                                    //     .ToList();

                                    //    if (linevmarkets.Count() > 0)
                                    //    {
                                    //        item2.LineVMarkets = convertedList;
                                    //    }
                                    //}

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
                            item2.BettingAllowedOverAll = await CheckForAllowedBettingOverAll(item.EventTypeName, item2.MarketBookName);
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

                            var runnerdesc = objUsersServiceCleint.GetSelectionNamesbyMarketID(item2.MarketId);
                            item2.Runners = new List<BettingServiceReference.Runner>();

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
                                runneritem.ExchangePrices.AvailableToBack = lstpricelist;
                                lstpricelist = new List<BettingServiceReference.PriceSize>();
                                for (int i = 0; i < 3; i++)
                                {
                                    var pricesize = new BettingServiceReference.PriceSize();

                                    pricesize.Size = 0;

                                    pricesize.Price = 0;

                                    lstpricelist.Add(pricesize);
                                }
                                runneritem.ExchangePrices.AvailableToLay = lstpricelist;
                                item2.Runners.Add(runneritem);
                            }
                            item2.FavoriteID = "0";
                            item2.FavoriteBack = "0";
                            item2.FavoriteBackSize = "0";
                            item2.FavoriteLay = "0";
                            item2.FavoriteLaySize = "0";
                            item2.FavoriteSelectionName = "";
                            marketbooks.Add(item2);
                        }

                        List<UserBets> lstUserBet = _httpContextAccessor.HttpContext.Session.GetObject<List<UserBets>>("userbets");
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
                                if (objMarketbook.MainSportsname == "Cricket" && objMarketbook.MarketBookName.Contains("Match Odds"))
                                {

                                }
                            }
                        }

                        //Session["userbets"] = lstUserBet;
                        //long Liabality = 0;
                        //Session["liabality"] = Liabality;
                        //lstUserBet = lstUserBet.Where(x => x.location != "9").ToList();
                        //decimal TotLiabality = objUserBets.GetLiabalityofCurrentUser(LoggedinUserDetail.GetUserID(), lstUserBet);
                        //Session["totliabality"] = TotLiabality;
                        //ViewBag.totliabalityNew = TotLiabality;

                        return await RenderRazorViewToStringAsync("MarketBook", marketbooks);
                    }
                    else
                    {
                        //List<UserBets> lstUserBet = JsonConvert.DeserializeObject<List<UserBets>>(objUsersServiceCleint.GetUserbetsbyUserID(LoggedinUserDetail.GetUserID(), _passwordSettingsService.PasswordForValidate));
                        //Session["userbets"] = lstUserBet;
                        //Session["liabality"] = 0;

                        //decimal TotLiabality = objUserBets.GetLiabalityofCurrentUser(LoggedinUserDetail.GetUserID(), lstUserBet);
                        //Session["totliabality"] = TotLiabality;
                        //ViewBag.totliabalityNew = TotLiabality;
                        var marketbooks = new List<BettingServiceReference.MarketBook>();
                        return await RenderRazorViewToStringAsync("MarketBook", marketbooks);
                    }
                }
                else
                {
                    if (LoggedinUserDetail.GetUserTypeID() == 2)
                    {
                        ViewBag.backgrod = "-webkit-linear-gradient(top, rgb(29, 155, 240), rgb(10, 10, 10))";
                        ViewBag.color = "white";
                        var results = JsonConvert.DeserializeObject<List<MarketCatalgoue>>(objUsersServiceCleint.GetMarketsOpenedbyUser(LoggedinUserDetail.GetUserID()));
                        if (results != null)
                        {
                            results = results.Where(item => item.ID == ID).ToList();
                            var marketbooks2 = new List<BettingServiceReference.MarketBook>();
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
                                        marketbooks2.Add(marketbook.Result[0]);
                                    }
                                }                               
                            }
                            foreach (var item in results)
                            {
                                foreach (var item2 in marketbooks2)
                                {
                                    if (item.ID == item2.MarketId)
                                    {
                                        //item2.MarketBookName = item.Name + " / " + item.EventName;
                                        item2.MarketBookName = item.EventName + " / " + item.Name;
                                        item2.OrignalOpenDate = item.EventOpenDate;
                                        item2.MainSportsname = item.EventTypeName;
                                        item2.BettingAllowed = item.BettingAllowed;
                                        item2.BettingAllowedOverAll = true;
                                        item2.GetMatchUpdatesFrom = item.GetMatchUpdatesFrom;
                                        item2.EventID = item.EventID;
                                        var runnerdesc = objUsersServiceCleint.GetSelectionNamesbyMarketID(item2.MarketId);
                                        foreach (var runnermarketitem in runnerdesc)
                                        {
                                            foreach (var runneritem in item2.Runners)
                                            {
                                                if (runnermarketitem.SelectionID == runneritem.SelectionId)
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
                                        //if (item2.MarketBookName.Contains("Match Odds") && item2.MainSportsname == "Cricket" && item2.MarketStatusstr != "Closed")
                                        //{
                                        //    item2.CricketMatchKey = objUsersServiceCleint.GetCricketMatchKey(item2.MarketId);
                                        //    var resultslinev = objUsersServiceCleint.GetEventDetailsbyMarketBook(item2.MarketId);
                                        //    int UserIDforLinevmarkets = 0;
                                        //    if (LoggedinUserDetail.GetUserTypeID() == 1)
                                        //    {
                                        //        UserIDforLinevmarkets = 73;
                                        //    }
                                        //    else
                                        //    {
                                        //        UserIDforLinevmarkets = LoggedinUserDetail.GetUserID();
                                        //    }
                                        //    var linevmarkets = JsonConvert.DeserializeObject<List<BettingServiceReference.LinevMarkets>>(objUsersServiceCleint.GetLinevMarketsbyEventID(resultslinev.EventID, resultslinev.EventOpenDate.Value, UserIDforLinevmarkets));
                                        //    if (linevmarkets.Count() > 0)
                                        //    {
                                        //        item2.LineVMarkets = linevmarkets;
                                        //    }
                                        //}
                                    }
                                }
                            }
                            if (marketbooks2.Count == 0)
                            {
                                BettingServiceReference.MarketBook item2 = new BettingServiceReference.MarketBook();
                                var item = results[0];
                                item2.MarketId = item.ID;
                                //item2.MarketBookName = item.Name + " / " + item.EventName;
                                item2.MarketBookName = item.EventName + " / " + item.Name;
                                item2.OrignalOpenDate = item.EventOpenDate;
                                item2.MainSportsname = item.EventTypeName;
                                item2.BettingAllowed = item.BettingAllowed;
                                item2.BettingAllowedOverAll = true;
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

                                var runnerdesc = objUsersServiceCleint.GetSelectionNamesbyMarketID(item2.MarketId);
                                item2.Runners = new List<BettingServiceReference.Runner>();

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
                                    runneritem.ExchangePrices.AvailableToBack = lstpricelist;
                                    lstpricelist = new List<BettingServiceReference.PriceSize>();
                                    for (int i = 0; i < 3; i++)
                                    {
                                        var pricesize = new BettingServiceReference.PriceSize();

                                        pricesize.Size = 0;

                                        pricesize.Price = 0;

                                        lstpricelist.Add(pricesize);
                                    }
                                    runneritem.ExchangePrices.AvailableToLay = lstpricelist;
                                    item2.Runners.Add(runneritem);
                                }
                                item2.FavoriteID = "0";
                                item2.FavoriteBack = "0";
                                item2.FavoriteBackSize = "0";
                                item2.FavoriteLay = "0";
                                item2.FavoriteLaySize = "0";
                                item2.FavoriteSelectionName = "";
                                //if (item2.MarketBookName.Contains("Match Odds") && item2.MainSportsname == "Cricket" && item2.MarketStatusstr != "Closed")
                                //{
                                //    item2.CricketMatchKey = objUsersServiceCleint.GetCricketMatchKey(item2.MarketId);
                                //    var resultslinev = objUsersServiceCleint.GetEventDetailsbyMarketBook(item2.MarketId);
                                //    int UserIDforLinevmarkets = 0;
                                //    if (LoggedinUserDetail.GetUserTypeID() == 1)
                                //    {
                                //        UserIDforLinevmarkets = 73;
                                //    }
                                //    else
                                //    {
                                //        UserIDforLinevmarkets = LoggedinUserDetail.GetUserID();
                                //    }
                                //    var linevmarkets = JsonConvert.DeserializeObject<List<BettingServiceReference.LinevMarkets>>(objUsersServiceCleint.GetLinevMarketsbyEventID(resultslinev.EventID, resultslinev.EventOpenDate.Value, UserIDforLinevmarkets));


                                //    if (linevmarkets.Count() > 0)
                                //    {
                                //        item2.LineVMarkets = linevmarkets;
                                //    }
                                //}
                                marketbooks2.Add(item2);
                            }
							return await RenderRazorViewToStringAsync("MarketBook", marketbooks2);
						}
                        else
                        {                        
                            var marketbooks2 = new List<BettingServiceReference.MarketBook>();
							return await RenderRazorViewToStringAsync("MarketBook", marketbooks2);
						}
                    }
                    //    else
                    //    {
                    //        if (LoggedinUserDetail.GetUserTypeID() == 1)
                    //        {
                    //            ViewBag.backgrod = "#1D9BF0";
                    //            ViewBag.color = "white";
                    //            var results = JsonConvert.DeserializeObject<List<Models.MarketCatalgoue>>(objUsersServiceCleint.GetMarketsOpenedbyUser(73));
                    //            if (results != null)
                    //            {
                    //                results = results.Where(item => item.ID == ID).ToList();
                    //                var marketbooks = new List<BettingServiceReference.MarketBook>();
                    //                List<string> lstIDs = new List<string>();
                    //                foreach (var item in results)
                    //                {
                    //                    lstIDs = new List<string>();

                    //                    lstIDs.Add(item.ID);
                    //                    var marketbook = GetMarketDatabyID(lstIDs.ToArray(), item.Name, item.EventOpenDate, item.EventTypeName);
                    //                    if (marketbook.Count() > 0)

                    //                    {
                    //                        if (marketbook[0].Runners != null)
                    //                        {
                    //                            marketbooks.Add(marketbook[0]);
                    //                        }

                    //                    }
                    //                    else
                    //                    {

                    //                    }

                    //                }

                    //                foreach (var item in results)
                    //                {
                    //                    foreach (var item2 in marketbooks)
                    //                    {
                    //                        if (item.ID == item2.MarketId)
                    //                        {
                    //                            //item2.MarketBookName = item.Name + " / " + item.EventName;
                    //                            item2.MarketBookName = item.EventName + " / " + item.Name;
                    //                            item2.OrignalOpenDate = item.EventOpenDate;
                    //                            item2.MainSportsname = item.EventTypeName;
                    //                            item2.BettingAllowed = item.BettingAllowed;
                    //                            item2.BettingAllowedOverAll = CheckForAllowedBettingOverAll(item.EventTypeName, item2.MarketBookName);
                    //                            item2.GetMatchUpdatesFrom = item.GetMatchUpdatesFrom;
                    //                            item2.EventID = item.EventID;
                    //                            Session["Eventid"] = item.EventID;


                    //                            var runnerdesc = objUsersServiceCleint.GetSelectionNamesbyMarketID(item2.MarketId);
                    //                            foreach (var runnermarketitem in runnerdesc)
                    //                            {
                    //                                foreach (var runneritem in item2.Runners)
                    //                                {
                    //                                    if (runnermarketitem.SelectionID == runneritem.SelectionId)
                    //                                    {
                    //                                        runneritem.RunnerName = runnermarketitem.SelectionName;
                    //                                        runneritem.JockeyName = runnermarketitem.JockeyName;
                    //                                        runneritem.WearingURL = runnermarketitem.Wearing;
                    //                                        runneritem.WearingDesc = runnermarketitem.WearingDesc;
                    //                                        runneritem.Clothnumber = runnermarketitem.ClothNumber;
                    //                                        runneritem.StallDraw = runnermarketitem.StallDraw;

                    //                                    }
                    //                                }

                    //                            }

                    //                            // 


                    //                            if (item2.MarketBookName.Contains("Match Odds") && item2.MainSportsname == "Cricket" && item2.MarketStatusstr != "Closed")
                    //                            {
                    //                                item2.CricketMatchKey = objUsersServiceCleint.GetCricketMatchKey(item2.MarketId);
                    //                                var resultslinev = objUsersServiceCleint.GetEventDetailsbyMarketBook(item2.MarketId);
                    //                                int UserIDforLinevmarkets = 0;
                    //                                if (LoggedinUserDetail.GetUserTypeID() == 1)
                    //                                {
                    //                                    UserIDforLinevmarkets = 73;
                    //                                }
                    //                                else
                    //                                {
                    //                                    UserIDforLinevmarkets = LoggedinUserDetail.GetUserID();
                    //                                }
                    //                                var linevmarkets = JsonConvert.DeserializeObject<List<LinevMarkets>>(objUsersServiceCleint.GetLinevMarketsbyEventID(resultslinev.EventID, resultslinev.EventOpenDate.Value, UserIDforLinevmarkets));


                    //                                if (linevmarkets.Count() > 0)
                    //                                {
                    //                                    item2.LineVMarkets = linevmarkets;
                    //                                }
                    //                            }

                    //                            ////

                    //                        }
                    //                    }

                    //                }
                    //                if (marketbooks.Count == 0)
                    //                {
                    //                    BettingServiceReference.MarketBook item2 = new BettingServiceReference.MarketBook();
                    //                    var item = results[0];
                    //                    item2.MarketId = item.ID;
                    //                    //item2.MarketBookName = item.Name + " / " + item.EventName;
                    //                    item2.MarketBookName = item.EventName + " / " + item.Name;
                    //                    item2.OrignalOpenDate = item.EventOpenDate;
                    //                    item2.MainSportsname = item.EventTypeName;
                    //                    item2.BettingAllowed = item.BettingAllowed;
                    //                    item2.BettingAllowedOverAll = CheckForAllowedBettingOverAll(item.EventTypeName, item2.MarketBookName);
                    //                    item2.GetMatchUpdatesFrom = item.GetMatchUpdatesFrom;
                    //                    item2.EventID = item.EventID;
                    //                    DateTime OpenDate = item.EventOpenDate.AddHours(5);
                    //                    DateTime CurrentDate = DateTime.Now;
                    //                    TimeSpan remainingdays = (CurrentDate - OpenDate);
                    //                    if (OpenDate < CurrentDate)
                    //                    {
                    //                        item2.OpenDate = "-" + remainingdays.Days.ToString() + ":" + remainingdays.Hours.ToString() + ":" + remainingdays.Minutes.ToString() + ":" + remainingdays.Seconds.ToString();
                    //                    }
                    //                    else
                    //                    {
                    //                        item2.OpenDate = (-1 * remainingdays.Days).ToString() + ":" + (-1 * remainingdays.Hours).ToString() + ":" + (-1 * remainingdays.Minutes).ToString() + ":" + (-1 * remainingdays.Seconds).ToString();
                    //                    }
                    //                    item2.MarketStatusstr = "Active";

                    //                    var runnerdesc = objUsersServiceCleint.GetSelectionNamesbyMarketID(item2.MarketId);
                    //                    item2.Runners = new List<Runner>();

                    //                    foreach (var runnermarketitem in runnerdesc)
                    //                    {
                    //                        var runneritem = new Runner();
                    //                        runneritem.SelectionId = runnermarketitem.SelectionID;
                    //                        runneritem.RunnerName = runnermarketitem.SelectionName;
                    //                        runneritem.JockeyName = runnermarketitem.JockeyName;
                    //                        runneritem.WearingURL = runnermarketitem.Wearing;
                    //                        runneritem.WearingDesc = runnermarketitem.WearingDesc;
                    //                        runneritem.Clothnumber = runnermarketitem.ClothNumber;
                    //                        runneritem.StallDraw = runnermarketitem.StallDraw;
                    //                        runneritem.ExchangePrices = new ExchangePrices();
                    //                        var lstpricelist = new List<PriceSize>();
                    //                        for (int i = 0; i < 3; i++)
                    //                        {
                    //                            var pricesize = new PriceSize();

                    //                            pricesize.Size = 0;

                    //                            pricesize.Price = 0;

                    //                            lstpricelist.Add(pricesize);
                    //                        }
                    //                        runneritem.ExchangePrices.AvailableToBack = lstpricelist;
                    //                        lstpricelist = new List<PriceSize>();
                    //                        for (int i = 0; i < 3; i++)
                    //                        {
                    //                            var pricesize = new PriceSize();

                    //                            pricesize.Size = 0;

                    //                            pricesize.Price = 0;

                    //                            lstpricelist.Add(pricesize);
                    //                        }
                    //                        runneritem.ExchangePrices.AvailableToLay = lstpricelist;
                    //                        item2.Runners.Add(runneritem);


                    //                    }
                    //                    item2.FavoriteID = "0";
                    //                    item2.FavoriteBack = "0";
                    //                    item2.FavoriteBackSize = "0";
                    //                    item2.FavoriteLay = "0";
                    //                    item2.FavoriteLaySize = "0";
                    //                    item2.FavoriteSelectionName = "";
                    //                    // 
                    //                    // 

                    //                    if (item2.MarketBookName.Contains("Match Odds") && item2.MainSportsname == "Cricket" && item2.MarketStatusstr != "Closed")
                    //                    {
                    //                        item2.CricketMatchKey = objUsersServiceCleint.GetCricketMatchKey(item2.MarketId);
                    //                        var resultslinev = objUsersServiceCleint.GetEventDetailsbyMarketBook(item2.MarketId);
                    //                        int UserIDforLinevmarkets = 0;
                    //                        if (LoggedinUserDetail.GetUserTypeID() == 1)
                    //                        {
                    //                            UserIDforLinevmarkets = 73;
                    //                        }
                    //                        else
                    //                        {
                    //                            UserIDforLinevmarkets = LoggedinUserDetail.GetUserID();
                    //                        }
                    //                        var linevmarkets = JsonConvert.DeserializeObject<List<LinevMarkets>>(objUsersServiceCleint.GetLinevMarketsbyEventID(resultslinev.EventID, resultslinev.EventOpenDate.Value, UserIDforLinevmarkets));


                    //                        if (linevmarkets.Count() > 0)
                    //                        {
                    //                            item2.LineVMarkets = linevmarkets;
                    //                        }
                    //                    }

                    //                    marketbooks.Add(item2);
                    //                }

                    //                List<UserBetsForAdmin> lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBetsForAdmin>>(objUsersServiceCleint.GetUserbetsForAdmin());

                    //                Session["userbets"] = lstUserBet;
                    //                Session["liabality"] = 0;
                    //                lstUserBet = lstUserBet.Where(x => x.location != "9").ToList();
                    //                decimal TotLiabality = objUserBets.GetLiabalityofAdmin(lstUserBet);
                    //                Session["totliabality"] = 0;
                    //                // Updateunmatchbets(marketbooks);
                    //                ViewBag.totliabalityNew = 0;
                    //                return RenderRazorViewToString("BettingServiceReference.MarketBook", marketbooks);
                    //            }
                    //            else
                    //            {
                    //                List<UserBetsForAdmin> lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBetsForAdmin>>(objUsersServiceCleint.GetUserbetsForAdmin());

                    //                Session["userbets"] = lstUserBet;
                    //                Session["liabality"] = 0;

                    //                decimal TotLiabality = objUserBets.GetLiabalityofAdmin(lstUserBet);
                    //                Session["totliabality"] = 0;
                    //                ViewBag.totliabalityNew = 0;
                    //                var marketbooks = new List<BettingServiceReference.MarketBook>();
                    //                return RenderRazorViewToString("BettingServiceReference.MarketBook", marketbooks);
                    //            }

                    //        }

                    //        else
                    //        {
                    //            if (LoggedinUserDetail.GetUserTypeID() == 8)
                    //            {
                    //                ViewBag.backgrod = "#1D9BF0";
                    //                ViewBag.color = "white";
                    //                var results = JsonConvert.DeserializeObject<List<Models.MarketCatalgoue>>(objUsersServiceCleint.GetMarketsOpenedbyUser(73));
                    //                if (results != null)
                    //                {
                    //                    results = results.Where(item => item.ID == ID).ToList();
                    //                    var marketbooks = new List<BettingServiceReference.MarketBook>();
                    //                    List<string> lstIDs = new List<string>();
                    //                    foreach (var item in results)
                    //                    {
                    //                        lstIDs = new List<string>();
                    //                        lstIDs.Add(item.ID);
                    //                        var marketbook = GetMarketDatabyID(lstIDs.ToArray(), item.Name, item.EventOpenDate, item.EventTypeName);
                    //                        if (marketbook.Count() > 0)

                    //                        {
                    //                            if (marketbook[0].Runners != null)
                    //                            {
                    //                                marketbooks.Add(marketbook[0]);
                    //                            }
                    //                        }
                    //                        else
                    //                        {

                    //                        }
                    //                    }

                    //                    foreach (var item in results)
                    //                    {
                    //                        foreach (var item2 in marketbooks)
                    //                        {
                    //                            if (item.ID == item2.MarketId)
                    //                            {
                    //                                item2.MarketBookName = item.EventName + " / " + item.Name;
                    //                                item2.OrignalOpenDate = item.EventOpenDate;
                    //                                item2.MainSportsname = item.EventTypeName;
                    //                                item2.BettingAllowed = item.BettingAllowed;
                    //                                item2.BettingAllowedOverAll = CheckForAllowedBettingOverAll(item.EventTypeName, item2.MarketBookName);
                    //                                item2.GetMatchUpdatesFrom = item.GetMatchUpdatesFrom;
                    //                                item2.EventID = item.EventID;
                    //                                Session["Eventid"] = item.EventID;


                    //                                var runnerdesc = objUsersServiceCleint.GetSelectionNamesbyMarketID(item2.MarketId);
                    //                                foreach (var runnermarketitem in runnerdesc)
                    //                                {
                    //                                    foreach (var runneritem in item2.Runners)
                    //                                    {
                    //                                        if (runnermarketitem.SelectionID == runneritem.SelectionId)
                    //                                        {
                    //                                            runneritem.RunnerName = runnermarketitem.SelectionName;
                    //                                            runneritem.JockeyName = runnermarketitem.JockeyName;
                    //                                            runneritem.WearingURL = runnermarketitem.Wearing;
                    //                                            runneritem.WearingDesc = runnermarketitem.WearingDesc;
                    //                                            runneritem.Clothnumber = runnermarketitem.ClothNumber;
                    //                                            runneritem.StallDraw = runnermarketitem.StallDraw;

                    //                                        }
                    //                                    }
                    //                                }
                    //                                // 


                    //                                if (item2.MarketBookName.Contains("Match Odds") && item2.MainSportsname == "Cricket" && item2.MarketStatusstr != "Closed")
                    //                                {
                    //                                    item2.CricketMatchKey = objUsersServiceCleint.GetCricketMatchKey(item2.MarketId);
                    //                                    var resultslinev = objUsersServiceCleint.GetEventDetailsbyMarketBook(item2.MarketId);
                    //                                    int UserIDforLinevmarkets = 0;
                    //                                    if (LoggedinUserDetail.GetUserTypeID() == 1)
                    //                                    {
                    //                                        UserIDforLinevmarkets = 73;
                    //                                    }
                    //                                    else
                    //                                    {
                    //                                        UserIDforLinevmarkets = LoggedinUserDetail.GetUserID();
                    //                                    }
                    //                                    var linevmarkets = JsonConvert.DeserializeObject<List<LinevMarkets>>(objUsersServiceCleint.GetLinevMarketsbyEventID(resultslinev.EventID, resultslinev.EventOpenDate.Value, UserIDforLinevmarkets));


                    //                                    if (linevmarkets.Count() > 0)
                    //                                    {
                    //                                        item2.LineVMarkets = linevmarkets;
                    //                                    }
                    //                                }

                    //                                ////

                    //                            }
                    //                        }

                    //                    }
                    //                    if (marketbooks.Count == 0)
                    //                    {
                    //                        BettingServiceReference.MarketBook item2 = new BettingServiceReference.MarketBook();
                    //                        var item = results[0];
                    //                        item2.MarketId = item.ID;
                    //                        //item2.MarketBookName = item.Name + " / " + item.EventName;
                    //                        item2.MarketBookName = item.EventName + " / " + item.Name;
                    //                        item2.OrignalOpenDate = item.EventOpenDate;
                    //                        item2.MainSportsname = item.EventTypeName;
                    //                        item2.BettingAllowed = item.BettingAllowed;
                    //                        item2.BettingAllowedOverAll = CheckForAllowedBettingOverAll(item.EventTypeName, item2.MarketBookName);
                    //                        item2.GetMatchUpdatesFrom = item.GetMatchUpdatesFrom;
                    //                        item2.EventID = item.EventID;
                    //                        DateTime OpenDate = item.EventOpenDate.AddHours(5);
                    //                        DateTime CurrentDate = DateTime.Now;
                    //                        TimeSpan remainingdays = (CurrentDate - OpenDate);
                    //                        if (OpenDate < CurrentDate)
                    //                        {
                    //                            item2.OpenDate = "-" + remainingdays.Days.ToString() + ":" + remainingdays.Hours.ToString() + ":" + remainingdays.Minutes.ToString() + ":" + remainingdays.Seconds.ToString();
                    //                        }
                    //                        else
                    //                        {
                    //                            item2.OpenDate = (-1 * remainingdays.Days).ToString() + ":" + (-1 * remainingdays.Hours).ToString() + ":" + (-1 * remainingdays.Minutes).ToString() + ":" + (-1 * remainingdays.Seconds).ToString();
                    //                        }
                    //                        item2.MarketStatusstr = "Active";

                    //                        var runnerdesc = objUsersServiceCleint.GetSelectionNamesbyMarketID(item2.MarketId);
                    //                        item2.Runners = new List<Runner>();

                    //                        foreach (var runnermarketitem in runnerdesc)
                    //                        {
                    //                            var runneritem = new Runner();
                    //                            runneritem.SelectionId = runnermarketitem.SelectionID;
                    //                            runneritem.RunnerName = runnermarketitem.SelectionName;
                    //                            runneritem.JockeyName = runnermarketitem.JockeyName;
                    //                            runneritem.WearingURL = runnermarketitem.Wearing;
                    //                            runneritem.WearingDesc = runnermarketitem.WearingDesc;
                    //                            runneritem.Clothnumber = runnermarketitem.ClothNumber;
                    //                            runneritem.StallDraw = runnermarketitem.StallDraw;
                    //                            runneritem.ExchangePrices = new ExchangePrices();
                    //                            var lstpricelist = new List<PriceSize>();
                    //                            for (int i = 0; i < 3; i++)
                    //                            {
                    //                                var pricesize = new PriceSize();

                    //                                pricesize.Size = 0;

                    //                                pricesize.Price = 0;

                    //                                lstpricelist.Add(pricesize);
                    //                            }
                    //                            runneritem.ExchangePrices.AvailableToBack = lstpricelist;
                    //                            lstpricelist = new List<PriceSize>();
                    //                            for (int i = 0; i < 3; i++)
                    //                            {
                    //                                var pricesize = new PriceSize();

                    //                                pricesize.Size = 0;

                    //                                pricesize.Price = 0;

                    //                                lstpricelist.Add(pricesize);
                    //                            }
                    //                            runneritem.ExchangePrices.AvailableToLay = lstpricelist;
                    //                            item2.Runners.Add(runneritem);


                    //                        }
                    //                        item2.FavoriteID = "0";
                    //                        item2.FavoriteBack = "0";
                    //                        item2.FavoriteBackSize = "0";
                    //                        item2.FavoriteLay = "0";
                    //                        item2.FavoriteLaySize = "0";
                    //                        item2.FavoriteSelectionName = "";
                    //                        // 



                    //                        if (item2.MarketBookName.Contains("Match Odds") && item2.MainSportsname == "Cricket" && item2.MarketStatusstr != "Closed")
                    //                        {
                    //                            item2.CricketMatchKey = objUsersServiceCleint.GetCricketMatchKey(item2.MarketId);
                    //                            var resultslinev = objUsersServiceCleint.GetEventDetailsbyMarketBook(item2.MarketId);
                    //                            int UserIDforLinevmarkets = 0;
                    //                            if (LoggedinUserDetail.GetUserTypeID() == 1)
                    //                            {
                    //                                UserIDforLinevmarkets = 73;
                    //                            }
                    //                            else
                    //                            {
                    //                                UserIDforLinevmarkets = LoggedinUserDetail.GetUserID();
                    //                            }
                    //                            var linevmarkets = JsonConvert.DeserializeObject<List<LinevMarkets>>(objUsersServiceCleint.GetLinevMarketsbyEventID(resultslinev.EventID, resultslinev.EventOpenDate.Value, UserIDforLinevmarkets));


                    //                            if (linevmarkets.Count() > 0)
                    //                            {
                    //                                item2.LineVMarkets = linevmarkets;
                    //                            }
                    //                        }

                    //                        ////

                    //                        if (item2.MarketBookName.Contains("Match Odds") && item2.MainSportsname == "Cricket" && item2.MarketStatusstr != "Closed")
                    //                        {
                    //                            item2.CricketMatchKey = objUsersServiceCleint.GetCricketMatchKey(item2.MarketId);
                    //                            var resultslinev = objUsersServiceCleint.GetEventDetailsbyMarketBook(item2.MarketId);
                    //                            int UserIDforLinevmarkets = 0;
                    //                            if (LoggedinUserDetail.GetUserTypeID() == 1)
                    //                            {
                    //                                UserIDforLinevmarkets = 73;
                    //                            }
                    //                            else
                    //                            {
                    //                                UserIDforLinevmarkets = LoggedinUserDetail.GetUserID();
                    //                            }
                    //                            var linevmarkets = JsonConvert.DeserializeObject<List<LinevMarkets>>(objUsersServiceCleint.GetLinevMarketsbyEventID(resultslinev.EventID, resultslinev.EventOpenDate.Value, UserIDforLinevmarkets));


                    //                            if (linevmarkets.Count() > 0)
                    //                            {
                    //                                item2.LineVMarkets = linevmarkets;
                    //                            }
                    //                        }

                    //                        marketbooks.Add(item2);
                    //                    }

                    //                    List<UserBetsForAdmin> lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBetsForAdmin>>(objUsersServiceCleint.GetUserbetsForAdmin());

                    //                    Session["userbets"] = lstUserBet;
                    //                    Session["liabality"] = 0;
                    //                    lstUserBet = lstUserBet.Where(x => x.location != "9").ToList();
                    //                    decimal TotLiabality = objUserBets.GetLiabalityofAdmin(lstUserBet);
                    //                    Session["totliabality"] = 0;
                    //                    // Updateunmatchbets(marketbooks);
                    //                    ViewBag.totliabalityNew = 0;
                    //                    return RenderRazorViewToString("BettingServiceReference.MarketBook", marketbooks);
                    //                }
                    //                else
                    //                {
                    //                    List<UserBetsForAdmin> lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBetsForAdmin>>(objUsersServiceCleint.GetUserbetsForAdmin());

                    //                    Session["userbets"] = lstUserBet;
                    //                    Session["liabality"] = 0;

                    //                    decimal TotLiabality = objUserBets.GetLiabalityofAdmin(lstUserBet);
                    //                    Session["totliabality"] = 0;
                    //                    ViewBag.totliabalityNew = 0;
                    //                    var marketbooks = new List<BettingServiceReference.MarketBook>();
                    //                    return RenderRazorViewToString("BettingServiceReference.MarketBook", marketbooks);
                    //                }

                    //            }
                    //            if (LoggedinUserDetail.GetUserTypeID() == 9)
                    //            {
                    //                ViewBag.backgrod = "#1D9BF0";
                    //                ViewBag.color = "white";
                    //                var results = JsonConvert.DeserializeObject<List<Models.MarketCatalgoue>>(objUsersServiceCleint.GetMarketsOpenedbyUser(73));
                    //                if (results != null)
                    //                {
                    //                    results = results.Where(item => item.ID == ID).ToList();
                    //                    var marketbooks = new List<BettingServiceReference.MarketBook>();
                    //                    List<string> lstIDs = new List<string>();
                    //                    foreach (var item in results)
                    //                    {
                    //                        lstIDs = new List<string>();
                    //                        lstIDs.Add(item.ID);
                    //                        var marketbook = GetMarketDatabyID(lstIDs.ToArray(), item.Name, item.EventOpenDate, item.EventTypeName);
                    //                        if (marketbook.Count() > 0)

                    //                        {
                    //                            if (marketbook[0].Runners != null)
                    //                            {
                    //                                marketbooks.Add(marketbook[0]);
                    //                            }
                    //                        }
                    //                        else
                    //                        {

                    //                        }
                    //                    }

                    //                    foreach (var item in results)
                    //                    {
                    //                        foreach (var item2 in marketbooks)
                    //                        {
                    //                            if (item.ID == item2.MarketId)
                    //                            {
                    //                                item2.MarketBookName = item.EventName + " / " + item.Name;
                    //                                item2.OrignalOpenDate = item.EventOpenDate;
                    //                                item2.MainSportsname = item.EventTypeName;
                    //                                item2.BettingAllowed = item.BettingAllowed;
                    //                                item2.BettingAllowedOverAll = CheckForAllowedBettingOverAll(item.EventTypeName, item2.MarketBookName);
                    //                                item2.GetMatchUpdatesFrom = item.GetMatchUpdatesFrom;
                    //                                item2.EventID = item.EventID;
                    //                                Session["Eventid"] = item.EventID;


                    //                                var runnerdesc = objUsersServiceCleint.GetSelectionNamesbyMarketID(item2.MarketId);
                    //                                foreach (var runnermarketitem in runnerdesc)
                    //                                {
                    //                                    foreach (var runneritem in item2.Runners)
                    //                                    {
                    //                                        if (runnermarketitem.SelectionID == runneritem.SelectionId)
                    //                                        {
                    //                                            runneritem.RunnerName = runnermarketitem.SelectionName;
                    //                                            runneritem.JockeyName = runnermarketitem.JockeyName;
                    //                                            runneritem.WearingURL = runnermarketitem.Wearing;
                    //                                            runneritem.WearingDesc = runnermarketitem.WearingDesc;
                    //                                            runneritem.Clothnumber = runnermarketitem.ClothNumber;
                    //                                            runneritem.StallDraw = runnermarketitem.StallDraw;

                    //                                        }
                    //                                    }
                    //                                }
                    //                                // 


                    //                                if (item2.MarketBookName.Contains("Match Odds") && item2.MainSportsname == "Cricket" && item2.MarketStatusstr != "Closed")
                    //                                {
                    //                                    item2.CricketMatchKey = objUsersServiceCleint.GetCricketMatchKey(item2.MarketId);
                    //                                    var resultslinev = objUsersServiceCleint.GetEventDetailsbyMarketBook(item2.MarketId);
                    //                                    int UserIDforLinevmarkets = 0;
                    //                                    if (LoggedinUserDetail.GetUserTypeID() == 1)
                    //                                    {
                    //                                        UserIDforLinevmarkets = 73;
                    //                                    }
                    //                                    else
                    //                                    {
                    //                                        UserIDforLinevmarkets = LoggedinUserDetail.GetUserID();
                    //                                    }
                    //                                    var linevmarkets = JsonConvert.DeserializeObject<List<LinevMarkets>>(objUsersServiceCleint.GetLinevMarketsbyEventID(resultslinev.EventID, resultslinev.EventOpenDate.Value, UserIDforLinevmarkets));


                    //                                    if (linevmarkets.Count() > 0)
                    //                                    {
                    //                                        item2.LineVMarkets = linevmarkets;
                    //                                    }
                    //                                }

                    //                                ////

                    //                            }
                    //                        }

                    //                    }
                    //                    if (marketbooks.Count == 0)
                    //                    {
                    //                        BettingServiceReference.MarketBook item2 = new BettingServiceReference.MarketBook();
                    //                        var item = results[0];
                    //                        item2.MarketId = item.ID;
                    //                        //item2.MarketBookName = item.Name + " / " + item.EventName;
                    //                        item2.MarketBookName = item.EventName + " / " + item.Name;
                    //                        item2.OrignalOpenDate = item.EventOpenDate;
                    //                        item2.MainSportsname = item.EventTypeName;
                    //                        item2.BettingAllowed = item.BettingAllowed;
                    //                        item2.BettingAllowedOverAll = CheckForAllowedBettingOverAll(item.EventTypeName, item2.MarketBookName);
                    //                        item2.GetMatchUpdatesFrom = item.GetMatchUpdatesFrom;
                    //                        item2.EventID = item.EventID;
                    //                        DateTime OpenDate = item.EventOpenDate.AddHours(5);
                    //                        DateTime CurrentDate = DateTime.Now;
                    //                        TimeSpan remainingdays = (CurrentDate - OpenDate);
                    //                        if (OpenDate < CurrentDate)
                    //                        {
                    //                            item2.OpenDate = "-" + remainingdays.Days.ToString() + ":" + remainingdays.Hours.ToString() + ":" + remainingdays.Minutes.ToString() + ":" + remainingdays.Seconds.ToString();
                    //                        }
                    //                        else
                    //                        {
                    //                            item2.OpenDate = (-1 * remainingdays.Days).ToString() + ":" + (-1 * remainingdays.Hours).ToString() + ":" + (-1 * remainingdays.Minutes).ToString() + ":" + (-1 * remainingdays.Seconds).ToString();
                    //                        }
                    //                        item2.MarketStatusstr = "Active";

                    //                        var runnerdesc = objUsersServiceCleint.GetSelectionNamesbyMarketID(item2.MarketId);
                    //                        item2.Runners = new List<Runner>();

                    //                        foreach (var runnermarketitem in runnerdesc)
                    //                        {
                    //                            var runneritem = new Runner();
                    //                            runneritem.SelectionId = runnermarketitem.SelectionID;
                    //                            runneritem.RunnerName = runnermarketitem.SelectionName;
                    //                            runneritem.JockeyName = runnermarketitem.JockeyName;
                    //                            runneritem.WearingURL = runnermarketitem.Wearing;
                    //                            runneritem.WearingDesc = runnermarketitem.WearingDesc;
                    //                            runneritem.Clothnumber = runnermarketitem.ClothNumber;
                    //                            runneritem.StallDraw = runnermarketitem.StallDraw;
                    //                            runneritem.ExchangePrices = new ExchangePrices();
                    //                            var lstpricelist = new List<PriceSize>();
                    //                            for (int i = 0; i < 3; i++)
                    //                            {
                    //                                var pricesize = new PriceSize();

                    //                                pricesize.Size = 0;

                    //                                pricesize.Price = 0;

                    //                                lstpricelist.Add(pricesize);
                    //                            }
                    //                            runneritem.ExchangePrices.AvailableToBack = lstpricelist;
                    //                            lstpricelist = new List<PriceSize>();
                    //                            for (int i = 0; i < 3; i++)
                    //                            {
                    //                                var pricesize = new PriceSize();

                    //                                pricesize.Size = 0;

                    //                                pricesize.Price = 0;

                    //                                lstpricelist.Add(pricesize);
                    //                            }
                    //                            runneritem.ExchangePrices.AvailableToLay = lstpricelist;
                    //                            item2.Runners.Add(runneritem);


                    //                        }
                    //                        item2.FavoriteID = "0";
                    //                        item2.FavoriteBack = "0";
                    //                        item2.FavoriteBackSize = "0";
                    //                        item2.FavoriteLay = "0";
                    //                        item2.FavoriteLaySize = "0";
                    //                        item2.FavoriteSelectionName = "";
                    //                        // 
                    //                        // 


                    //                        if (item2.MarketBookName.Contains("Match Odds") && item2.MainSportsname == "Cricket" && item2.MarketStatusstr != "Closed")
                    //                        {
                    //                            item2.CricketMatchKey = objUsersServiceCleint.GetCricketMatchKey(item2.MarketId);
                    //                            var resultslinev = objUsersServiceCleint.GetEventDetailsbyMarketBook(item2.MarketId);
                    //                            int UserIDforLinevmarkets = 0;
                    //                            if (LoggedinUserDetail.GetUserTypeID() == 1)
                    //                            {
                    //                                UserIDforLinevmarkets = 73;
                    //                            }
                    //                            else
                    //                            {
                    //                                UserIDforLinevmarkets = LoggedinUserDetail.GetUserID();
                    //                            }
                    //                            var linevmarkets = JsonConvert.DeserializeObject<List<LinevMarkets>>(objUsersServiceCleint.GetLinevMarketsbyEventID(resultslinev.EventID, resultslinev.EventOpenDate.Value, UserIDforLinevmarkets));
                    //                            if (linevmarkets.Count() > 0)
                    //                            {
                    //                                item2.LineVMarkets = linevmarkets;
                    //                            }
                    //                        }
                    //                        marketbooks.Add(item2);
                    //                    }

                    //                    List<UserBetsForAdmin> lstUserBet = JsonConvert.DeserializeObject<List<UserBetsForAdmin>>(objUsersServiceCleint.GetUserbetsForAdmin());

                    //                    Session["userbets"] = lstUserBet;
                    //                    Session["liabality"] = 0;
                    //                    lstUserBet = lstUserBet.Where(x => x.location != "9").ToList();
                    //                    decimal TotLiabality = objUserBets.GetLiabalityofAdmin(lstUserBet);
                    //                    Session["totliabality"] = 0;
                    //                    ViewBag.totliabalityNew = 0;
                    //                    return RenderRazorViewToString("BettingServiceReference.MarketBook", marketbooks);
                    //                }
                    //                else
                    //                {
                    //                    List<UserBetsForAdmin> lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBetsForAdmin>>(objUsersServiceCleint.GetUserbetsForAdmin());

                    //                    Session["userbets"] = lstUserBet;
                    //                    Session["liabality"] = 0;
                    //                    lstUserBet = lstUserBet.Where(x => x.location != "9").ToList();
                    //                    decimal TotLiabality = objUserBets.GetLiabalityofAdmin(lstUserBet);
                    //                    Session["totliabality"] = 0;
                    //                    ViewBag.totliabalityNew = 0;
                    //                    var marketbooks = new List<BettingServiceReference.MarketBook>();
                    //                    return RenderRazorViewToString("BettingServiceReference.MarketBook", marketbooks);
                    //                }

                    //            }

                    //            else
                    //            {
                    var marketbooks = new List<BettingServiceReference.MarketBook>();
                    return await RenderRazorViewToStringAsync("MarketBook", marketbooks);

                }

                //        }
                //    }
                //}
            }

            catch (System.Exception ex)
            {
                //LoggedinUserDetail.LogError(ex);
                return "";
            }
        }

        public async Task<string> LoadActiveMarket(string ID, string sheetname, DateTime OrignalOpenDate, string MainSportsCategory, string RunnersForAverage)
        {
            try
            {
                if (ID != "")
                {
                    List<string> lstIDs = new List<string>();
                    lstIDs.Add(ID);
                    var marketbooks = await GetMarketDatabyID(lstIDs.ToArray(), sheetname, OrignalOpenDate, MainSportsCategory);
                    if (marketbooks.Count() > 0)
                    {
                        marketbooks[0].MarketBookName = sheetname;
                        marketbooks[0].MainSportsname = MainSportsCategory;
                        marketbooks[0].OrignalOpenDate = OrignalOpenDate;
                        marketbooks[0].BettingAllowedOverAll = await CheckForAllowedBettingOverAll(MainSportsCategory, sheetname);
                        //ViewBag.TotalMarketsOpened = marketbooks[0].marketsopened;
                        if (LoggedinUserDetail.GetUserTypeID() == 3)
                        {
                            var lstUserBet = JsonConvert.DeserializeObject<List<UserBets>>(objUsersServiceCleint.GetUserbetsbyUserID(LoggedinUserDetail.GetUserID(), _passwordSettingsService.PasswordForValidate));
                            List<UserBets> lstUserBets = lstUserBet.Where(item2 => item2.isMatched == true && item2.MarketBookID == marketbooks[0].MarketId).ToList();
                            marketbooks[0].DebitCredit = objUserBets.ceckProfitandLoss(marketbooks[0], lstUserBets);
                            if (marketbooks[0].MarketBookName.Contains("To Be Placed"))
                            {
                                foreach (var runner in marketbooks[0].Runners)
                                {
                                    runner.ProfitandLoss = Convert.ToInt64(marketbooks[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                                    runner.Loss = Convert.ToInt64(marketbooks[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                }
                            }
                            else
                            {
                                if (marketbooks[0].Runners.Count() == 1)
                                {
                                    foreach (var runner in marketbooks[0].Runners)
                                    {
                                        runner.ProfitandLoss = Convert.ToInt64(marketbooks[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - marketbooks[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                        if (runner.ProfitandLoss > 0)
                                        {
                                            runner.ProfitandLoss = -1 * runner.ProfitandLoss;
                                        }
                                    }
                                }
                                else
                                {
                                    foreach (var runner in marketbooks[0].Runners)
                                    {
                                        runner.ProfitandLoss = Convert.ToInt64(marketbooks[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - marketbooks[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                    }
                                    if (marketbooks[0].MainSportsname == "Cricket" && marketbooks[0].MarketBookName.Contains("Match Odds"))
                                    {
                                        CalculateAvearageforAllUsers(marketbooks[0], RunnersForAverage);
                                    }
                                }

                            }

                            //Session["userbets"] = lstUserBet.Where(item2 => item2.MarketBookID == marketbooks[0].MarketId).ToList();
                            //Session["userbet"] = lstUserBet;
                            //List<UserBets> lstAllUserBets = new List<UserBets>();
                            //if (Session["linevmarkets"] != null)
                            //{
                            //    List<LinevMarkets> linevmarketsfig = new List<LinevMarkets>();
                            //    List<LinevMarkets> linevmarkets = (List<LinevMarkets>)Session["linevmarkets"];
                            //    if (linevmarkets != null)
                            //    {
                            //        linevmarketsfig = linevmarkets.GroupBy(item => item.MarketCatalogueName).Select(g => g.First()).Where(item2 => item2.EventName == "Figure").ToList();
                            //        linevmarkets = linevmarkets.Where(item => item.EventName != "Figure").ToList();

                            //        List<UserBets> lstFancyBets = new List<UserBets>();
                            //        foreach (var lineitem in linevmarkets)
                            //        {
                            //            lstFancyBets = lstUserBet.Where(item => item.MarketBookID == lineitem.MarketCatalogueID).ToList();

                            //            lstAllUserBets.AddRange(lstFancyBets);
                            //        }
                            //        List<UserBets> lstFancyfigBets = new List<UserBets>();
                            //        foreach (var lineitem in linevmarketsfig)
                            //        {
                            //            lstFancyfigBets = lstUserBet.Where(item => item.MarketBookID == lineitem.MarketCatalogueID).ToList();

                            //            lstAllUserBets.AddRange(lstFancyfigBets);
                            //        }
                            //    }
                            //}
                            //lstAllUserBets.AddRange(lstUserBets);
                            //lstAllUserBets = lstAllUserBets.Where(x => x.location != "9").ToList();
                            //long Liabality = objUserBets.GetLiabalityofCurrentUser(LoggedinUserDetail.GetUserID(), lstAllUserBets);
                            //Session["liabality"] = Liabality;
                            //lstUserBet = lstUserBet.Where(x => x.location != "9").ToList();
                            //decimal TotLiabality = objUserBets.GetLiabalityofCurrentUser(LoggedinUserDetail.GetUserID(), lstUserBet);
                            //Session["totliabality"] = TotLiabality;
                            //ViewBag.totliabalityNew = 0;

                        }
                        //if (LoggedinUserDetail.GetUserTypeID() == 2)
                        //{
                        //    string userbets = objUsersServiceCleint.GetUserBetsbyAgentID(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                        //    var lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBetsforAgent>>(userbets);
                        //    List<UserBetsforAgent> lstUserBets = lstUserBet.Where(item2 => item2.isMatched == true && item2.MarketBookID == marketbooks[0].MarketId).ToList();
                        //    var lstUsers = lstUserBets.Select(item1 => new { item1.UserID }).Distinct().ToArray();
                        //    foreach (var userid in lstUsers)
                        //    {
                        //        List<UserBetsforAgent> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID)).ToList();
                        //        marketbooks[0].DebitCredit = objUserBets.ceckProfitandLossAgent(marketbooks[0], lstuserbet);
                        //        if (marketbooks[0].MarketBookName.Contains("To Be Placed"))
                        //        {
                        //            foreach (var runner in marketbooks[0].Runners)
                        //            {
                        //                long profitorloss = Convert.ToInt64(marketbooks[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                        //                long loss = Convert.ToInt64(marketbooks[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                        //                decimal profit = LoggedinUserDetail.GetProfitorlossbyAgentPercentageandTransferRate(lstuserbet[0].AgentOwnBets, lstuserbet[0].TransferAdmin, lstuserbet[0].TransferAgentIDB, lstuserbet[0].CreatedbyID, profitorloss, Convert.ToDecimal(lstuserbet[0].AgentRate));
                        //                runner.ProfitandLoss += Convert.ToInt64(-1 * profit);
                        //                profit = LoggedinUserDetail.GetProfitorlossbyAgentPercentageandTransferRate(lstuserbet[0].AgentOwnBets, lstuserbet[0].TransferAdmin, lstuserbet[0].TransferAgentIDB, lstuserbet[0].CreatedbyID, loss, Convert.ToDecimal(lstuserbet[0].AgentRate));
                        //                runner.Loss += Convert.ToInt64(-1 * profit);


                        //            }
                        //        }
                        //        else
                        //        {
                        //            foreach (var runner in marketbooks[0].Runners)
                        //            {
                        //                long profitorloss = Convert.ToInt64(marketbooks[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - marketbooks[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                        //                decimal profit = LoggedinUserDetail.GetProfitorlossbyAgentPercentageandTransferRate(lstuserbet[0].AgentOwnBets, lstuserbet[0].TransferAdmin, lstuserbet[0].TransferAgentIDB, lstuserbet[0].CreatedbyID, profitorloss, Convert.ToDecimal(lstuserbet[0].AgentRate));
                        //                runner.ProfitandLoss += Convert.ToInt64(-1 * profit);

                        //            }
                        //            if (marketbooks[0].MainSportsname == "Cricket" && marketbooks[0].MarketBookName.Contains("Match Odds"))
                        //            {
                        //                CalculateAvearageforAllUsers(marketbooks[0], RunnersForAverage);
                        //            }
                        //        }
                        //    }

                        //    Session["userbets"] = lstUserBet;
                        //    lstUserBet = lstUserBet.Where(x => x.location != "9").ToList();
                        //    decimal CurrentLiabality = objUserBets.GetLiabalityofCurrentAgent(lstUserBet);
                        //    Session["liabality"] = CurrentLiabality;
                        //    decimal TotLiabality = objUserBets.GetLiabalityofCurrentAgent(lstUserBet);
                        //    Session["totliabality"] = TotLiabality;
                        //    ViewBag.totliabalityNew = 0;

                        //}
                        //if (LoggedinUserDetail.GetUserTypeID() == 1)
                        //{

                        //    string userbets = objUsersServiceCleint.GetUserbetsForAdmin(ConfigurationManager.AppSettings["PasswordForValidate"]);
                        //    List<UserBetsForAdmin> lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBetsForAdmin>>(userbets);
                        //    List<UserBetsForAdmin> lstUserBets = lstUserBet.Where(item2 => item2.isMatched == true && item2.MarketBookID == marketbooks[0].MarketId).ToList();

                        //    var lstUsers = lstUserBets.Select(item1 => new { item1.UserID }).Distinct().ToArray();
                        //    foreach (var userid in lstUsers)
                        //    {
                        //        List<UserBetsForAdmin> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID)).ToList();
                        //        var agentrate = lstuserbet[0].AgentRate;
                        //        var superrate = lstuserbet[0].SuperAgentRateB;
                        //        bool TransferAdminAmount = lstuserbet[0].TransferAdmin;
                        //        var TransferAdminPercentage = lstuserbet[0].TransferAdminPercentage;
                        //        decimal adminrate1 = ((100 - Convert.ToDecimal(TransferAdminPercentage)) / 100);
                        //        decimal adminrateafteragnetrateminus = (100 - Convert.ToDecimal(agentrate));
                        //        decimal adminrateacc = adminrateafteragnetrateminus * adminrate1;

                        //        var superpercent = 0;
                        //        if (superrate > 0)
                        //        {
                        //            superpercent = superrate - Convert.ToInt32(agentrate);
                        //        }
                        //        else
                        //        {
                        //            superpercent = 0;
                        //        }
                        //        marketbooks[0].DebitCredit = objUserBets.ceckProfitandLossAdmin(marketbooks[0], lstuserbet);

                        //        if (marketbooks[0].MarketBookName.Contains("To Be Placed"))
                        //        {
                        //            foreach (var runner in marketbooks[0].Runners)
                        //            {
                        //                long profitorloss = Convert.ToInt64(marketbooks[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                        //                long loss = Convert.ToInt64(marketbooks[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                        //                decimal adminrate = TransferAdminAmount == false ? 100 - Convert.ToDecimal(agentrate) : 0;
                        //                decimal profit = (adminrate / 100) * profitorloss;
                        //                runner.ProfitandLoss += Convert.ToInt64(-1 * profit);
                        //                profit = (adminrate / 100) * loss;
                        //                runner.Loss += Convert.ToInt64(-1 * profit);

                        //            }
                        //        }
                        //        else
                        //        {
                        //            foreach (var runner in marketbooks[0].Runners)
                        //            {

                        //                long profitorloss = Convert.ToInt64(marketbooks[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - marketbooks[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                        //                if (marketbooks[0].Runners.Count == 1)
                        //                {
                        //                    if (profitorloss > 0)
                        //                    {
                        //                        profitorloss = -1 * profitorloss;
                        //                    }
                        //                }

                        //                decimal adminrate = TransferAdminAmount == false ? 100 - Convert.ToDecimal(agentrate) - Convert.ToDecimal(superpercent) : adminrateacc;
                        //                decimal profit = (adminrate / 100) * profitorloss;
                        //                runner.ProfitandLoss += Convert.ToInt64(-1 * profit);
                        //            }
                        //            if (marketbooks[0].MainSportsname == "Cricket" && marketbooks[0].MarketBookName.Contains("Match Odds"))
                        //            {
                        //                CalculateAvearageforAllUsers(marketbooks[0], RunnersForAverage);
                        //            }
                        //        }
                        //    }
                        //    Session["userbets"] = lstUserBet;

                        //    decimal CurrentLiabality = objUserBets.GetLiabalityofAdmin(lstUserBets);
                        //    Session["liabality"] = CurrentLiabality;

                        //    decimal TotLiabality = objUserBets.GetLiabalityofAdmin(lstUserBet);
                        //    Session["totliabality"] = TotLiabality;
                        //    ViewBag.totliabalityNew = 0;


                        //}

                        //if (LoggedinUserDetail.GetUserTypeID() == 8)
                        //{
                        //    string userbets = objUsersServiceCleint.GetUserBetsbySuperID(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                        //    var lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBetsforSuper>>(userbets);
                        //    List<UserBetsforSuper> lstUserBets = lstUserBet.Where(item2 => item2.isMatched == true && item2.MarketBookID == marketbooks[0].MarketId).ToList();

                        //    var lstUsers = lstUserBets.Select(item1 => new { item1.UserID }).Distinct().ToArray();
                        //    foreach (var userid in lstUsers)
                        //    {
                        //        List<UserBetsforSuper> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID)).ToList();
                        //        var agentrate = lstuserbet[0].AgentRate;
                        //        var supertrate = lstuserbet[0].SuperAgentRateB;
                        //        bool TransferAdminAmount = lstuserbet[0].TransferAdmin;
                        //        var TransferAdminPercentage = lstuserbet[0].TransferAdminPercentage;
                        //        var superpercent = supertrate - Convert.ToInt32(agentrate);


                        //        var adminpercent = 100 - (superpercent + Convert.ToInt32(agentrate) + TransferAdminPercentage);

                        //        marketbooks[0].DebitCredit = objUserBets.ceckProfitandLossSuper(marketbooks[0], lstuserbet);
                        //        if (marketbooks[0].MarketBookName.Contains("To Be Placed"))
                        //        {
                        //            foreach (var runner in marketbooks[0].Runners)
                        //            {
                        //                long profitorloss = Convert.ToInt64(marketbooks[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                        //                long loss = Convert.ToInt64(marketbooks[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                        //                decimal superfinal = TransferAdminAmount == false ? 100 - Convert.ToDecimal(agentrate) : 100 - Convert.ToDecimal(agentrate) - Convert.ToDecimal(adminpercent) - Convert.ToDecimal(TransferAdminPercentage);
                        //                decimal profit = (superfinal / 100) * profitorloss;
                        //                runner.ProfitandLoss += Convert.ToInt64(-1 * profit);
                        //                profit = (superfinal / 100) * loss;
                        //                runner.Loss += Convert.ToInt64(-1 * profit);
                        //            }
                        //        }
                        //        else
                        //        {
                        //            foreach (var runner in marketbooks[0].Runners)
                        //            {
                        //                long profitorloss = Convert.ToInt64(marketbooks[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - marketbooks[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                        //                if (marketbooks[0].Runners.Count == 1)
                        //                {
                        //                    if (profitorloss > 0)
                        //                    {
                        //                        profitorloss = -1 * profitorloss;
                        //                    }
                        //                }
                        //               decimal adminrate = TransferAdminAmount == false ? 100 - Convert.ToDecimal(agentrate) : 100 - Convert.ToDecimal(agentrate) - Convert.ToDecimal(adminpercent) - Convert.ToDecimal(TransferAdminPercentage);
                        //                decimal profit = (adminrate / 100) * profitorloss;
                        //                runner.ProfitandLoss += Convert.ToInt64(-1 * profit);
                        //            }
                        //            if (marketbooks[0].MainSportsname == "Cricket" && marketbooks[0].MarketBookName.Contains("Match Odds"))
                        //            {
                        //                CalculateAvearageforAllUsers(marketbooks[0], RunnersForAverage);
                        //            }
                        //        }
                        //    }
                        //    Session["userbets"] = lstUserBet;
                        //    lstUserBet = lstUserBet.Where(x => x.location != "9").ToList();
                        //    decimal CurrentLiabality = objUserBets.GetLiabalityofSuper(lstUserBet);
                        //    Session["liabality"] = CurrentLiabality;

                        //    decimal TotLiabality = objUserBets.GetLiabalityofSuper(lstUserBet);
                        //    Session["totliabality"] = TotLiabality;
                        //    ViewBag.totliabalityNew = 0;
                        //}

                        //if (LoggedinUserDetail.GetUserTypeID() == 9)
                        //{
                        //    string userbets = objUsersServiceCleint.GetUserBetsbySamiAdmin(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                        //    var lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBetsforSamiadmin>>(userbets);
                        //    List<UserBetsforSamiadmin> lstUserBets = lstUserBet.Where(item2 => item2.isMatched == true && item2.MarketBookID == marketbooks[0].MarketId).ToList();

                        //    var lstUsers = lstUserBets.Select(item1 => new { item1.UserID }).Distinct().ToArray();
                        //    foreach (var userid in lstUsers)
                        //    {
                        //        // var agentrate = 0;
                        //        List<UserBetsforSamiadmin> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID)).ToList();
                        //        var agentrate = lstuserbet[0].AgentRate;
                        //        var supertrate = lstuserbet[0].SuperAgentRateB;
                        //        var samiadminrate = lstuserbet[0].SamiAdminRate;
                        //        bool TransferAdminAmount = lstuserbet[0].TransferAdmin;
                        //        var TransferAdminPercentage = lstuserbet[0].TransferAdminPercentage;
                        //        var superpercent = 0;
                        //        if (supertrate > 0)
                        //        {
                        //            superpercent = supertrate - Convert.ToInt32(agentrate);
                        //        }
                        //        else
                        //        {
                        //            superpercent = 0;
                        //        }
                        //        var samiadminpercent = 0;
                        //        if (samiadminrate > 0)
                        //        {
                        //            samiadminpercent = Convert.ToInt32(samiadminrate) - (superpercent + Convert.ToInt32(agentrate));
                        //        }
                        //        else
                        //        {
                        //            samiadminpercent = 0;
                        //        }

                        //        var adminpercent = 100 - (samiadminpercent + superpercent + Convert.ToInt32(agentrate) + TransferAdminPercentage);

                        //        marketbooks[0].DebitCredit = objUserBets.ceckProfitandLossSamiadmin(marketbooks[0], lstuserbet);
                        //        if (marketbooks[0].MarketBookName.Contains("To Be Placed"))
                        //        {
                        //            foreach (var runner in marketbooks[0].Runners)
                        //            {
                        //                long profitorloss = Convert.ToInt64(marketbooks[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                        //                long loss = Convert.ToInt64(marketbooks[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                        //                decimal superfinal = TransferAdminAmount == false ? 100 - Convert.ToDecimal(agentrate) : 100 - Convert.ToDecimal(agentrate) - Convert.ToDecimal(adminpercent) - Convert.ToDecimal(TransferAdminPercentage);
                        //                decimal profit = (superfinal / 100) * profitorloss;
                        //                runner.ProfitandLoss += Convert.ToInt64(-1 * profit);
                        //                profit = (superfinal / 100) * loss;
                        //                runner.Loss += Convert.ToInt64(-1 * profit);
                        //            }
                        //        }
                        //        else
                        //        {
                        //            foreach (var runner in marketbooks[0].Runners)
                        //            {
                        //                long profitorloss = Convert.ToInt64(marketbooks[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - marketbooks[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                        //                if (marketbooks[0].Runners.Count == 1)
                        //                {
                        //                    if (profitorloss > 0)
                        //                    {
                        //                        profitorloss = -1 * profitorloss;
                        //                    }
                        //                }
                        //                decimal adminrate = TransferAdminAmount == false ? 100 - Convert.ToDecimal(agentrate) : 100 - Convert.ToDecimal(agentrate) - superpercent - Convert.ToDecimal(adminpercent) - Convert.ToDecimal(TransferAdminPercentage);
                        //                decimal profit = (adminrate / 100) * profitorloss;
                        //                runner.ProfitandLoss += Convert.ToInt64(-1 * profit);
                        //            }
                        //            if (marketbooks[0].MainSportsname == "Cricket" && marketbooks[0].MarketBookName.Contains("Match Odds"))
                        //            {
                        //                CalculateAvearageforAllUsers(marketbooks[0], RunnersForAverage);
                        //            }
                        //        }
                        //    }
                        //    Session["userbets"] = lstUserBet;
                        //    lstUserBet = lstUserBet.Where(x => x.location != "9").ToList();
                        //    decimal CurrentLiabality = objUserBets.GetLiabalityofSamiadmin(lstUserBet);
                        //    Session["liabality"] = CurrentLiabality;

                        //    decimal TotLiabality = objUserBets.GetLiabalityofSamiadmin(lstUserBet);
                        //    Session["totliabality"] = TotLiabality;
                        //    ViewBag.totliabalityNew = 0;
                        //}

                        return ConverttoJSONString(marketbooks);
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

        public void CalculateAvearageforAllUsers(BettingServiceReference.MarketBook objMarketBook, string RunnersForAverage)
        {
            try
            {
                if (RunnersForAverage == "")
                {
                    return;
                }
                string[] runnersarr1 = JsonConvert.DeserializeObject<string[]>(RunnersForAverage);
                if (runnersarr1.Count() == 2)
                {
                    double runner1profit = 0;
                    double runner2profit = 0;

                    BettingServiceReference.Runner runner1 = objMarketBook.Runners.Where(item => item.SelectionId == runnersarr1[0]).FirstOrDefault();
                    BettingServiceReference.Runner runner2 = objMarketBook.Runners.Where(item => item.SelectionId == runnersarr1[1]).FirstOrDefault();
                    if (runner1 != null)
                    {
                        runner1profit = runner1.ProfitandLoss;
                        runner2profit = runner2.ProfitandLoss;
                    }
                    if (runner1profit == 0 || runner2profit == 0)
                    {
                        runner1.Average = "";
                        runner2.Average = "";
                    }
                    if ((runner1profit > 0 && runner2profit > 0) || (runner1profit < 0 && runner2profit < 0))
                    {
                        runner1.Average = "0.00";
                        runner2.Average = "0.00";
                        return;
                    }
                    if (runner1profit > 0)
                    {
                        runner1.Average = " L";
                        runner2.Average = " K";
                    }
                    else
                    {
                        if (runner2profit > 0)
                        {
                            runner1.Average = " K";
                            runner2.Average = " L";
                        }
                    }
                    if (runner1profit < 0) { runner1profit = runner1profit * -1; }
                    if (runner2profit < 0) { runner2profit = runner2profit * -1; }
                    if (runner1profit > 0 && runner2profit > 0)
                    {
                        double result = runner1profit / runner2profit;
                        double result2 = runner2profit / runner1profit;
                        runner1.Average = result.ToString("F2") + runner1.Average;
                        runner2.Average = result2.ToString("F2") + runner2.Average;
                    }

                }

            }
            catch (System.Exception ex)
            {

            }
        }

        public string ConverttoJSONString(object result)
        {
            if (result != null)
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(result.GetType());
                MemoryStream memoryStream = new MemoryStream();
                serializer.WriteObject(memoryStream, result);
                string json = Encoding.Default.GetString(memoryStream.ToArray());
                return json;
            }
            else
            {
                return "";
            }
        }
        public async Task<bool> CheckForAllowedBettingOverAll(string categoryname, string marketbookname)
        {
            AllowedMarketWeb AllowedMarketsForUser = JsonConvert.DeserializeObject<AllowedMarketWeb>(objUsersServiceCleint.GetAllowedMarketsbyUserID(LoggedinUserDetail.GetUserID()));
            bool AllowedBet = false;

            if (marketbookname.Contains("Line"))
            {
                AllowedBet = AllowedMarketsForUser.isFancyMarketAllowed;
                return AllowedBet;
            }

            if (categoryname.Contains("Horse Racing") && !marketbookname.Contains("To Be Placed"))
            {
                AllowedBet = AllowedMarketsForUser.isHorseRaceWinAllowedForBet;
            }
            else
            {
                if (categoryname.Contains("Horse Racing") && marketbookname.Contains("To Be Placed"))
                {
                    AllowedBet = AllowedMarketsForUser.isHorseRacePlaceAllowedForBet;
                }
                else
                {
                    if (categoryname.Contains("Greyhound Racing") && marketbookname.Contains("To Be Placed"))
                    {
                        AllowedBet = AllowedMarketsForUser.isGrayHoundRacePlaceAllowedForBet;
                    }
                    else
                    {
                        if (categoryname.Contains("Greyhound Racing") && !marketbookname.Contains("To Be Placed"))
                        {
                            AllowedBet = AllowedMarketsForUser.isGrayHoundRaceWinAllowedForBet;
                        }
                        else
                        {
                            if (marketbookname.Contains("Completed Match"))
                            {
                                AllowedBet = AllowedMarketsForUser.isCricketCompletedMatchAllowedForBet;
                            }
                            else
                            {
                                if (marketbookname.Contains("Innings Runs") || marketbookname.Contains("Inns Runs"))
                                {
                                    AllowedBet = AllowedMarketsForUser.isCricketInningsRunsAllowedForBet;
                                }
                                else
                                {
                                    if (categoryname == "Tennis")
                                    {
                                        AllowedBet = AllowedMarketsForUser.isTennisAllowedForBet;
                                    }
                                    else
                                    {
                                        if (categoryname == "Soccer")
                                        {
                                            AllowedBet = AllowedMarketsForUser.isSoccerAllowedForBet;
                                        }
                                        else
                                        {
                                            if (marketbookname.Contains("Tied Match"))
                                            {
                                                AllowedBet = AllowedMarketsForUser.isCricketTiedMatchAllowedForBet;
                                            }
                                            else
                                            {
                                                if (marketbookname.Contains("Winner"))
                                                {
                                                    AllowedBet = AllowedMarketsForUser.isWinnerMarketAllowedForBet;

                                                }
                                                else
                                                {
                                                    AllowedBet = AllowedMarketsForUser.isCricketMatchOddsAllowedForBet;
                                                }
                                            }

                                        }
                                    }

                                }
                            }
                        }
                    }
                }
            }
            return AllowedBet;
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
    //    public async Task<string> GetTvLinks(string EventId)
    //    {
    //        try
    //        {
				//string jsonString = await objUsersServiceCleint.GetTvLinksAsync(EventId);
    //            return jsonString;
    //        }
    //        catch (System.Exception ex)
    //        {
    //            return "";
    //        }

    //    }
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
                APIConfig.LogError(ex);
                return new BettingServiceReference.MarketBook();
            }

        }

        public PartialViewResult Events(string ID)
        {
            //LoggedinUserDetail.CheckifUserLogin();

            if (LoggedinUserDetail.GetUserTypeID() != 1)
            {
                List<GTExCore.Models.Event> lstClientlist = JsonConvert.DeserializeObject<List<GTExCore.Models.Event>>(objUsersServiceCleint.GetEventsIDs(ID.ToString(), LoggedinUserDetail.GetUserID()));
                return PartialView("Events", lstClientlist);

            }
            else
            {
                List<GTExCore.Models.Event> lstClientlist = new List<GTExCore.Models.Event>();
                return PartialView("Events", lstClientlist);
            }

        }
        public PartialViewResult InPlayMatchesC()
        {

            if (LoggedinUserDetail.GetUserTypeID() == 2 || LoggedinUserDetail.GetUserTypeID() == 8 || LoggedinUserDetail.GetUserTypeID() == 9 || LoggedinUserDetail.GetUserTypeID() == 1)
            {
                ViewBag.backgrod = "-webkit-linear-gradient(top, rgb(29, 155, 240), rgb(10, 10, 10))";
                ViewBag.color = "white";
                List<TodayHorseRacing> lstTodayHorseRacing = JsonConvert.DeserializeObject<List<TodayHorseRacing>>(objUsersServiceCleint.GetTodayHorseRacing(LoggedinUserDetail.GetUserID(), "4"));
                lstTodayHorseRacing = lstTodayHorseRacing.Where(a => a.EventName != "Line v Markets").ToList();

                return PartialView("TodayHorseRace", lstTodayHorseRacing);
            }
            else
            {

                List<TodayHorseRacing> lstTodayHorseRacing = JsonConvert.DeserializeObject<List<TodayHorseRacing>>(objUsersServiceCleint.GetTodayHorseRacing(LoggedinUserDetail.GetUserID(), "4"));
                return PartialView("TodayHorseRace", lstTodayHorseRacing);
            }

        }
        public PartialViewResult AllMatches()
        {
            if (LoggedinUserDetail.GetUserTypeID() == 3 || LoggedinUserDetail.GetUserTypeID() == 2 || LoggedinUserDetail.GetUserTypeID() == 8)
            {
                List<InPlayMatches> lstInPlayMatches = JsonConvert.DeserializeObject<List<InPlayMatches>>(objUsersServiceCleint.GetAllMatches(LoggedinUserDetail.GetUserID()));
                return PartialView("InPlayMatches", lstInPlayMatches);
            }
            else
            {
                List<InPlayMatches> lstInPlayMatches = JsonConvert.DeserializeObject<List<InPlayMatches>>(objUsersServiceCleint.GetAllMatches(73));
                return PartialView("InPlayMatches", lstInPlayMatches);
            }

        }
        public PartialViewResult InPlayMatchesF()
        {
            if (LoggedinUserDetail.GetUserTypeID() == 2 || LoggedinUserDetail.GetUserTypeID() == 8 || LoggedinUserDetail.GetUserTypeID() == 9 || LoggedinUserDetail.GetUserTypeID() == 1)
            {
                ViewBag.backgrod = "-webkit-linear-gradient(top, rgb(29, 155, 240), rgb(10, 10, 10))"; ;
                ViewBag.color = "white";
            }
            if (LoggedinUserDetail.GetUserTypeID() == 3 || LoggedinUserDetail.GetUserTypeID() == 2 || LoggedinUserDetail.GetUserTypeID() == 8 || LoggedinUserDetail.GetUserTypeID() == 9)
            {


                List<TodayHorseRacing> lstTodayHorseRacing = JsonConvert.DeserializeObject<List<TodayHorseRacing>>(objUsersServiceCleint.GetTodayHorseRacing(LoggedinUserDetail.GetUserID(), "1"));

                return PartialView("TodayHorseRace", lstTodayHorseRacing);
            }
            else
            {
                List<TodayHorseRacing> lstTodayHorseRacing = JsonConvert.DeserializeObject<List<TodayHorseRacing>>(objUsersServiceCleint.GetTodayHorseRacing(LoggedinUserDetail.GetUserID(), "1"));
                return PartialView("TodayHorseRace", lstTodayHorseRacing);
            }

        }
        public PartialViewResult InPlayMatchesT()
        {
            if (LoggedinUserDetail.GetUserTypeID() == 2 || LoggedinUserDetail.GetUserTypeID() == 8 || LoggedinUserDetail.GetUserTypeID() == 9 || LoggedinUserDetail.GetUserTypeID() == 1)
            {
                ViewBag.backgrod = "-webkit-linear-gradient(top, rgb(29, 155, 240), rgb(10, 10, 10))";
                ViewBag.color = "white";
            }
            if (LoggedinUserDetail.GetUserTypeID() == 3 || LoggedinUserDetail.GetUserTypeID() == 2 || LoggedinUserDetail.GetUserTypeID() == 8 || LoggedinUserDetail.GetUserTypeID() == 9)
            {


                List<TodayHorseRacing> lstTodayHorseRacing = JsonConvert.DeserializeObject<List<TodayHorseRacing>>(objUsersServiceCleint.GetTodayHorseRacing(LoggedinUserDetail.GetUserID(), "2"));

                return PartialView("TodayHorseRace", lstTodayHorseRacing);
            }
            else
            {
                List<TodayHorseRacing> lstTodayHorseRacing = JsonConvert.DeserializeObject<List<TodayHorseRacing>>(objUsersServiceCleint.GetTodayHorseRacing(LoggedinUserDetail.GetUserID(), "2"));
                return PartialView("TodayHorseRace", lstTodayHorseRacing);
            }

        }
        public PartialViewResult TodayHorseRacing()
        {
            if (LoggedinUserDetail.GetUserTypeID() == 2 || LoggedinUserDetail.GetUserTypeID() == 8 || LoggedinUserDetail.GetUserTypeID() == 9 || LoggedinUserDetail.GetUserTypeID() == 1)
            {
                ViewBag.backgrod = "-webkit-linear-gradient(top, rgb(29, 155, 240), rgb(10, 10, 10))";
                ViewBag.color = "white";
            }
            //LoggedinUserDetail.CheckifUserLogin();
            if (LoggedinUserDetail.GetUserTypeID() == 3 || LoggedinUserDetail.GetUserTypeID() == 2)
            {

                List<TodayHorseRacing> lstTodayHorseRacing = JsonConvert.DeserializeObject<List<TodayHorseRacing>>(objUsersServiceCleint.GetTodayHorseRacing(LoggedinUserDetail.GetUserID(), "7"));
                return PartialView("TodayHorseRace", lstTodayHorseRacing);
            }
            else
            {
                List<TodayHorseRacing> lstTodayHorseRacing = JsonConvert.DeserializeObject<List<TodayHorseRacing>>(objUsersServiceCleint.GetTodayHorseRacing(LoggedinUserDetail.GetUserID(), "7")); ;
                TodayHorseRacing s = new TodayHorseRacing();

                return PartialView("TodayHorseRace", lstTodayHorseRacing);
            }
        }
        public PartialViewResult TodayGreyHoundRacing()
        {
            if (LoggedinUserDetail.GetUserTypeID() == 2 || LoggedinUserDetail.GetUserTypeID() == 8 || LoggedinUserDetail.GetUserTypeID() == 9 || LoggedinUserDetail.GetUserTypeID() == 1)
            {
                ViewBag.backgrod = "-webkit-linear-gradient(top, rgb(29, 155, 240), rgb(10, 10, 10))";
                ViewBag.color = "white";
            }
            //LoggedinUserDetail.CheckifUserLogin();
            if (LoggedinUserDetail.GetUserTypeID() == 3 || LoggedinUserDetail.GetUserTypeID() == 2 || LoggedinUserDetail.GetUserTypeID() == 8 || LoggedinUserDetail.GetUserTypeID() == 9)
            {
                List<TodayHorseRacing> lstTodayHorseRacing = JsonConvert.DeserializeObject<List<TodayHorseRacing>>(objUsersServiceCleint.GetTodayHorseRacing(LoggedinUserDetail.GetUserID(), "4339"));
                lstTodayHorseRacing = lstTodayHorseRacing.Take(100).ToList();
                return PartialView("TodayHorseRace", lstTodayHorseRacing);
            }
            else
            {
                List<TodayHorseRacing> lstTodayHorseRacing = new List<TodayHorseRacing>();
                lstTodayHorseRacing = lstTodayHorseRacing.Take(100).ToList();
                return PartialView("TodayHorseRace", lstTodayHorseRacing);
            }
        }

        public async Task<string> MarketBooksoccergoal(string EventID)
        {
            try
            {
                List<string> data = new List<string>();
                UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();
                LoggedinUserDetail.CheckifUserLogin();
                var Soccergoalmarket = objUsersServiceCleint.GetSoccergoalbyeventId(LoggedinUserDetail.GetUserID(), EventID);

                if (Soccergoalmarket != null)
                {
                    foreach (var item in Soccergoalmarket)
                    {
                        //  data.Add(item.MarketCatalogueID);
                        if (item.MarketCatalogueID != "" && LoggedinUserDetail.GetUserTypeID() != 1)
                        {
                            objUsersServiceCleint.SetMarketBookOpenbyUSer(LoggedinUserDetail.GetUserID(), item.MarketCatalogueID);
                        }
                        if (item.MarketCatalogueID != "" && LoggedinUserDetail.GetUserTypeID() == 1)
                        {
                            objUsersServiceCleint.SetMarketBookOpenbyUSer(73, item.MarketCatalogueID);
                        }
                    }
                }

                if (LoggedinUserDetail.GetUserTypeID() == 3)
                {
                    var results = JsonConvert.DeserializeObject<List<MarketCatalgoue>>(objUsersServiceCleint.GetMarketsOpenedbyUser(LoggedinUserDetail.GetUserID()));

                    if (results != null)
                    {
                        results = results.Where(item => item.EventID == EventID && item.Name != "Match Odds").ToList();
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
                                    //item2.MarketBookName = item.Name + " / " + item.EventName;
                                    item2.MarketBookName = item.EventName + " / " + item.Name;
                                    item2.OrignalOpenDate = item.EventOpenDate;
                                    item2.MainSportsname = item.EventTypeName;
                                    item2.MarketStatusstr = item2.MarketStatusstr;
                                    item2.BettingAllowed = item.BettingAllowed;
                                    item2.BettingAllowedOverAll =await CheckForAllowedBettingOverAll(item.EventTypeName, item2.MarketBookName);
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
                                            List<UserBets> lstUserBet = _httpContextAccessor.HttpContext.Session.GetObject<List<UserBets>>("userbet");        
                                            List<UserBets> lstUserBets = lstUserBet.Where(item3 => item3.isMatched == true && item3.MarketBookID == item2.MarketId).ToList();
                                            item2.DebitCredit = objUserBets.ceckProfitandLoss(item2, lstUserBets);
                                            runneritem.ProfitandLoss = Convert.ToInt64(item2.DebitCredit.Where(item5 => item5.SelectionID == runneritem.SelectionId).Sum(item5 => item5.Debit) - item2.DebitCredit.Where(item5 => item5.SelectionID == runneritem.SelectionId).Sum(item5 => item5.Credit));

                                        }
                                    }
                                }
                            }
                        }
                        return await RenderRazorViewToStringAsync("MarketBookSoccerGoal", marketbooks.Take(2));
                    }
                    else
                    {
                        var marketbooks = new List<BettingServiceReference.MarketBook>();
                        return await RenderRazorViewToStringAsync("MarketBookSoccerGoal", marketbooks);
                    }
                }                            
                else
                {
                    var marketbooks = new List<BettingServiceReference.MarketBook>();
                    return await RenderRazorViewToStringAsync("MarketBookSoccerGoal", marketbooks);
                }
            }
            catch (System.Exception ex)
            {

                var marketbooks = new List<BettingServiceReference.MarketBook>();
                return await RenderRazorViewToStringAsync("MarketBookSoccerGoal", marketbooks);
            }
        }
		public async Task<string> GetMarketBooksoccergoal(string EventID)
		{
			try
			{
				List<string> data = new List<string>();
				UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();
				LoggedinUserDetail.CheckifUserLogin();
				var Soccergoalmarket = objUsersServiceCleint.GetSoccergoalbyeventId(LoggedinUserDetail.GetUserID(), EventID);

				if (Soccergoalmarket != null)
				{
					foreach (var item in Soccergoalmarket)
					{
						if (item.MarketCatalogueID != "" && LoggedinUserDetail.GetUserTypeID() != 1)
						{
							objUsersServiceCleint.SetMarketBookOpenbyUSer(LoggedinUserDetail.GetUserID(), item.MarketCatalogueID);
						}
						if (item.MarketCatalogueID != "" && LoggedinUserDetail.GetUserTypeID() == 1)
						{
							objUsersServiceCleint.SetMarketBookOpenbyUSer(73, item.MarketCatalogueID);
						}
					}
				}

				if (LoggedinUserDetail.GetUserTypeID() == 3)
				{
					var results = JsonConvert.DeserializeObject<List<MarketCatalgoue>>(objUsersServiceCleint.GetMarketsOpenedbyUser(LoggedinUserDetail.GetUserID()));

					if (results != null)
					{
						results = results.Where(item => item.EventID == EventID && item.Name != "Match Odds").ToList();
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
									//item2.MarketBookName = item.Name + " / " + item.EventName;
									item2.MarketBookName = item.EventName + " / " + item.Name;
									item2.OrignalOpenDate = item.EventOpenDate;
									item2.MainSportsname = item.EventTypeName;
									item2.MarketStatusstr = item2.MarketStatusstr;
									item2.BettingAllowed = item.BettingAllowed;
									item2.BettingAllowedOverAll =await CheckForAllowedBettingOverAll(item.EventTypeName, item2.MarketBookName);
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
											List<UserBets> lstUserBet = _httpContextAccessor.HttpContext.Session.GetObject<List<UserBets>>("userbet");
											List<UserBets> lstUserBets = lstUserBet.Where(item3 => item3.isMatched == true && item3.MarketBookID == item2.MarketId).ToList();
											item2.DebitCredit = objUserBets.ceckProfitandLoss(item2, lstUserBets);
											runneritem.ProfitandLoss = Convert.ToInt64(item2.DebitCredit.Where(item5 => item5.SelectionID == runneritem.SelectionId).Sum(item5 => item5.Debit) - item2.DebitCredit.Where(item5 => item5.SelectionID == runneritem.SelectionId).Sum(item5 => item5.Credit));

										}
									}

								}
							}
						}
						return  ConvertListToJSONString(marketbooks.Take(2));
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
				return "";
			}
		}
		public string ConvertListToJSONString<T>(IEnumerable<T> resultList)
		{
			if (resultList != null)
			{
				DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(IEnumerable<T>));
				MemoryStream memoryStream = new MemoryStream();
				serializer.WriteObject(memoryStream, resultList);

				// Return the results serialized as JSON
				string json = Encoding.UTF8.GetString(memoryStream.ToArray());
				return json;
			}
			else
			{
				return "[]"; // Return an empty JSON array if the list is null
			}
		}

        public async Task<string> GetTvLinks(int sportId, string eventId)
        {
            try
            {
                string jsonString = await objUsersServiceCleint.GetTvLinksAsync(eventId);
                var obj = JsonConvert.DeserializeObject<List<TVlink>>(jsonString);
                var data = obj.Where(x => x.EventID == eventId).FirstOrDefault();
                long dID = Convert.ToInt64(data?.DimondID);
                if (sportId == 1)
                {
                    try
                    {
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                        // Directly create the iframe URL using DimondID (no external API call)
                        string tvlink1 = $"https://e765432.diamondcricketid.com/dtv.php?id={dID}";
                        return tvlink1;
                    }
                    catch (System.Exception ex)
                    {
                        return "";
                    }
                }


                if (sportId == 2)
                {
                    try
                    {
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                        // Directly create the iframe URL using DimondID (no external API call)
                        string tvlink1 = $"https://e765432.diamondcricketid.com/dtv1.php?id={dID}";
                        return tvlink1;
                    }
                    catch (System.Exception ex)
                    {
                        return "";
                    }
                }



                //https://serviceapi.fairgame7.com/getIframeUrl/471734455?sportType=football&isTv=true&isScore=true
                return "";
            }
            catch (System.Exception ex)
            {
                return "";
            }
        }

        public string GetCardLinks(string EventId)
        {
            try
            {
                string jsonString = objUsersServiceCleint.GetTvLinks(EventId);
                return jsonString;
            }
            catch (System.Exception ex)
            {
                return "";
            }
        }
    }
}
