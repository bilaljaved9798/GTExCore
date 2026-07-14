using BettingServiceReference;
using Census.API.Controllers;
using Global.API;
using GTCore.Models;
using GTCore.ViewModel;
using GTExCore.Common;
using GTExCore.Models;
using log4net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Engines;
using System.Configuration;
using UserServiceReference;

namespace GTExCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FancyApiController : ControllerBase
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
        UserBetsUpdateUnmatcedBets _objUserBets;
        UserServicesClient objUsersServiceCleint = new UserServicesClient();
        public static wsnew ws1 = new wsnew();
        public static wsnew ws2 = new wsnew();
        public static wsnew ws4 = new wsnew();
        public static wsnew ws7 = new wsnew();
        public static wsnew ws0 = new wsnew();
        public static wsnew ws4339 = new wsnew();
        public static wsnew wsFancy = new wsnew();

        private wsnew wsBFMatch = new wsnew();
        public FancyApiController(IRazorViewEngine viewEngine, ITempDataProvider tempDataProvider,UserBetsUpdateUnmatcedBets objUserBets, IServiceProvider serviceProvider, IConfiguration configuration, IPasswordSettingsService passwordSettingsService, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
            _configuration = configuration;
            _passwordSettingsService = passwordSettingsService;
            _objUserBets = objUserBets;
        }
        [Route("GetFancyMarket")]
        [HttpGet]
        public async Task<string> GetFancyMarket(string EventID, int UserId)
        {
            //var resultslinev = objUsersServiceCleint.GetEventDetailsbyMarketBook("1.256902053");   
            //var linevmarkets12 = JsonConvert.DeserializeObject<List<LineMarket>>(objUsersServiceCleint.GetLinevMarketsbyEventID(EventID, resultslinev.EventOpenDate.Value, UserId));
            var linevmarkets = JsonConvert.DeserializeObject<List<LineMarket>>(objUsersServiceCleint.GetLinevMarketsbyEventID(EventID,DateTime.Now, UserId));
            List<string> lstIds = linevmarkets.Select(item => item.MarketCatalogueID).ToList();
            string[] marketIds = lstIds.ToArray();


            if (LoggedinUserDetail.GetCricketDataFrom == "BP")
            {
                //var list = JsonConvert.DeserializeObject<List<bfnexchange.Services.SampleResponse1>>(objBettingClient.GetAllMarketsBPFancy(marketIds));

                //if (list.Count() > 0)
                //{

                //    MarketController objcontroller = new MarketController();
                //    bool BettingAllowedoverall = objcontroller.CheckForAllowedBettingOverAll("Cricket", "Line");
                //    List<ExternalAPI.TO.BettingServiceReference.MarketBook> LastloadedLinMarkets1 = new List<ExternalAPI.TO.BettingServiceReference.MarketBook>();
                //    foreach (var bfobject in linevmarkets)
                //    {
                //        try
                //        {
                //            SampleResponse1 objmarketbookBF1 = list.Where(item => item.MarketId == bfobject.MarketCatalogueID).First();
                //            //LastloadedLinMarkets1.Add(ConvertJsontoMarketObjectBF123(objmarketbookBF1, bfobject.MarketCatalogueID, EventOpendate, bfobject.MarketCatalogueName, "Cricket", bfobject.BettingAllowed, BettingAllowedoverall));
                //        }
                //        catch (System.Exception ex)
                //        {

                //        }



                //    }
                //    //return PartialView("FancyMarketBookNew", LastloadedLinMarkets1);
                //    return PartialView("FancyMarketBook", LastloadedLinMarkets1);

                //}
                //else
                //{
                //    //return PartialView("FancyMarketBookNew", new List<BettingServiceReference.MarketBook>());
                //    return PartialView("FancyMarketBook", new List<BettingServiceReference.MarketBook>());
                //}
                return "";
            }
            else
            {
                if (LoggedinUserDetail.GetCricketDataFrom == "Live")
                {
                    var list = (objBettingClient.GetAllMarketsFancy(marketIds));

                    if (list.Count() > 0)
                    {

                        //MarketController objcontroller = new MarketController();
                        bool BettingAllowedoverall = _objUserBets.CheckForAllowedBettingOverAll("Cricket", "Line", UserId);
                        List<BettingServiceReference.MarketBook> LastloadedLinMarkets1 = new List<BettingServiceReference.MarketBook>();
                        foreach (var bfobject in linevmarkets)
                        {
                            try
                            {
                                BettingServiceReference.MarketBook objmarketbookBF1 = list.Where(item => item.MarketId == bfobject.MarketCatalogueID).First();
                                LastloadedLinMarkets1.Add(ConvertJsontoMarketObjectLive(objmarketbookBF1, bfobject.MarketCatalogueID, DateTime.Now, bfobject.MarketCatalogueName, "Cricket", bfobject.BettingAllowed, BettingAllowedoverall));
                            }
                            catch (System.Exception ex)
                            {

                            }
                        }
                        return LastloadedLinMarkets1.ToString();
                    }
                    else
                    {
                        return new List<BettingServiceReference.MarketBook>().ToString();
                    }
                }
                else
                {
                    var list = (objBettingClient.GetAllMarketsOthersFancy(marketIds));
                    if (list.Count() > 0)
                    {
                        bool BettingAllowedoverall = _objUserBets.CheckForAllowedBettingOverAll("Cricket", "Line", UserId);
                        List<BettingServiceReference.MarketBook> LastloadedLinMarkets1 = new List<BettingServiceReference.MarketBook>();
                        foreach (var bfobject in linevmarkets)
                        {
                            try
                            {
                                MarketBookString objmarketbookBF1 = list.Where(item => item.MarketBookId == bfobject.MarketCatalogueID).First();
                                LastloadedLinMarkets1.Add(ConvertJsontoMarketObjectBFNewSource(objmarketbookBF1, bfobject.MarketCatalogueID, DateTime.Now, bfobject.MarketCatalogueName, "Cricket", bfobject.BettingAllowed, BettingAllowedoverall));
                            }
                            catch (System.Exception ex)
                            {

                            }
                        }
                        return JsonConvert.SerializeObject(LastloadedLinMarkets1);
                    }
                    else
                    {
                        return new List<BettingServiceReference.MarketBook>().ToString();
                    }
                }
            }

        }
        public BettingServiceReference.MarketBook ConvertJsontoMarketObjectBFNewSource(MarketBookString BFMarketbook, string marketid, DateTime marketopendate, string sheetname, string MainSportsCategory, bool BettingAllowed, bool BettingAllowedoverall)
        {

            if (1 == 1)
            {
                var marketbook = new BettingServiceReference.MarketBook();
                string[] newres = BFMarketbook.MarketBookData.Split(':').Select(tag => tag.Trim()).ToArray();
                string[] BFMarketBookDetail = newres[0].Split(new string[] { "~" }, StringSplitOptions.None).Select(tag => tag.Trim()).ToArray();

                marketbook.MarketId = BFMarketbook.MarketBookId;
                marketbook.SheetName = "";
                marketbook.IsMarketDataDelayed = false;
                marketbook.PoundRate = LoggedinUserDetail.GetPoundRate();
                marketbook.NumberOfWinners = Convert.ToInt32(BFMarketBookDetail[6]);
                marketbook.MarketBookName = sheetname;
                marketbook.MainSportsname = MainSportsCategory;
                marketbook.OrignalOpenDate = marketopendate;
                marketbook.BettingAllowed = BettingAllowed;
                marketbook.BettingAllowedOverAll = BettingAllowedoverall;
                //List<Models.UserBets> lstUserBets = (List<Models.UserBets>)Session["userbet"];
                //CurrentMarketProfitandloss = objUserbets.GetBookPosition(objnewmarketbook.MarketId, new List<Models.UserBetsForAdmin>(), new List<Models.UserBetsforSuper>(), new List<Models.UserBetsforAgent>(), lstUserBets);

                try
                {
                    marketbook.TotalMatched = Convert.ToInt64(Convert.ToDouble(BFMarketBookDetail[10]) * Convert.ToDouble(marketbook.PoundRate));
                }
                catch (System.Exception ex)
                {


                }
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

                if (Convert.ToInt32(BFMarketBookDetail[5]) == 1 && BFMarketBookDetail[2].Trim().ToString() == "OPEN")
                {

                    marketbook.MarketStatusstr = "In Play";
                }
                else
                {
                    if (BFMarketBookDetail[2].Trim().ToString() == "CLOSED")
                    {
                        marketbook.MarketStatusstr = "Closed";
                    }
                    else
                    {
                        if (BFMarketBookDetail[2].Trim().ToString() == "SUSPENDED")
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


                for (int i = 1; i < newres.Count(); i++)
                {
                    string[] runnerdetails = newres[i].Split(new string[] { "|" }, StringSplitOptions.None).Select(tag => tag.Trim()).ToArray();
                    string[] runnerinfo = runnerdetails[0].Split(new string[] { "~" }, StringSplitOptions.None).Select(tag => tag.Trim()).ToArray();
                    string[] runnerbackdata = runnerdetails[1].Split(new string[] { "~" }, StringSplitOptions.None).Select(tag => tag.Trim()).Where(tag => !string.IsNullOrEmpty(tag)).ToArray();
                    string[] runnerlaydata = runnerdetails[2].Split(new string[] { "~" }, StringSplitOptions.None).Select(tag => tag.Trim()).Where(tag => !string.IsNullOrEmpty(tag)).ToArray();
                    var runner = new BettingServiceReference.Runner();
                    runner.Handicap = 0;
                    runner.StatusStr = runnerinfo[6].Trim();
                    runner.SelectionId = runnerinfo[0].Trim().ToString();
                    runner.RunnerName = sheetname;
                    try
                    {
                        runner.LastPriceTraded = Convert.ToDouble(runnerinfo[3].Trim().ToString());
                        runner.TotalMatchedStr = APIConfig.FormatNumber(Convert.ToInt64(Convert.ToDouble(runnerinfo[2]) * Convert.ToDouble(marketbook.PoundRate)));
                    }
                    catch (System.Exception ex)
                    {


                    }
                    var lstpricelist = new List<BettingServiceReference.PriceSize>();
                    if (runnerbackdata.Count() > 0)
                    {
                        if (newres.Count() == 2)
                        {
                            try
                            {


                                if (runnerbackdata[0].ToString().Contains("."))
                                {
                                    for (int j = 0; j < runnerlaydata.Count();)
                                    {
                                        if (j < runnerlaydata.Count())
                                        {
                                            var pricesize = new BettingServiceReference.PriceSize();
                                            pricesize.OrignalSize = Convert.ToDouble(runnerlaydata[j + 1]);
                                            pricesize.Size = Convert.ToInt64(Convert.ToDouble(runnerlaydata[j + 1]) * Convert.ToDouble(marketbook.PoundRate));
                                            pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
                                            pricesize.Price = Convert.ToDouble((Convert.ToDouble(runnerlaydata[j]) + 0.5).ToString("F2"));

                                            lstpricelist.Add(pricesize);
                                            j = j + 4;

                                        }

                                    }
                                }
                                else
                                {
                                    for (int j = 0; j < runnerbackdata.Count();)
                                    {
                                        if (j < runnerbackdata.Count())
                                        {
                                            var pricesize = new BettingServiceReference.PriceSize();
                                            pricesize.OrignalSize = Convert.ToDouble(runnerbackdata[j + 1]);
                                            pricesize.Size = Convert.ToInt64(Convert.ToDouble(runnerbackdata[j + 1]) * Convert.ToDouble(marketbook.PoundRate));
                                            pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
                                            pricesize.Price = Convert.ToDouble((Convert.ToDouble(runnerbackdata[j])).ToString("F2"));

                                            lstpricelist.Add(pricesize);
                                            j = j + 4;

                                        }

                                    }
                                }
                            }
                            catch (System.Exception ex)
                            {

                            }
                        }
                        else
                        {
                            for (int j = 0; j < runnerbackdata.Count();)
                            {
                                if (j < runnerbackdata.Count())
                                {
                                    var pricesize = new BettingServiceReference.PriceSize();
                                    pricesize.OrignalSize = Convert.ToDouble(runnerbackdata[j + 1]);
                                    pricesize.Size = Convert.ToInt64(Convert.ToDouble(runnerbackdata[j + 1]) * Convert.ToDouble(marketbook.PoundRate));
                                    pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
                                    pricesize.Price = Convert.ToDouble((Convert.ToDouble(runnerbackdata[j])).ToString("F2"));

                                    lstpricelist.Add(pricesize);
                                    j = j + 4;

                                }

                            }
                        }
                    }
                    else
                    {
                        for (int ii = 0; ii < 3; ii++)
                        {
                            var pricesize = new BettingServiceReference.PriceSize();
                            pricesize.OrignalSize = 0;
                            pricesize.Size = 0;
                            pricesize.SizeStr = "0";
                            pricesize.Price = 0;

                            lstpricelist.Add(pricesize);
                        }
                    }

                    runner.ExchangePrices = new BettingServiceReference.ExchangePrices();
                    runner.ExchangePrices.AvailableToBack = lstpricelist.ToArray();
                    lstpricelist = new List<BettingServiceReference.PriceSize>();
                    if (runnerlaydata.Count() > 0)
                    {
                        if (newres.Count() == 2)
                        {
                            try
                            {


                                if (runnerlaydata[0].ToString().Contains("."))
                                {
                                    for (int j = 0; j < runnerbackdata.Count();)
                                    {
                                        if (j < runnerbackdata.Count())
                                        {
                                            var pricesize = new BettingServiceReference.PriceSize();
                                            pricesize.OrignalSize = Convert.ToDouble(runnerbackdata[j + 1]);
                                            pricesize.Size = Convert.ToInt64(Convert.ToDouble(runnerbackdata[j + 1]) * Convert.ToDouble(marketbook.PoundRate));
                                            pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
                                            pricesize.Price = Convert.ToDouble((Convert.ToDouble(runnerbackdata[j]) + 0.5).ToString("F2"));

                                            lstpricelist.Add(pricesize);
                                            j = j + 4;

                                        }

                                    }
                                }
                                else
                                {
                                    for (int j = 0; j < runnerlaydata.Count();)
                                    {
                                        if (j < runnerlaydata.Count())
                                        {
                                            var pricesize = new BettingServiceReference.PriceSize();
                                            pricesize.OrignalSize = Convert.ToDouble(runnerlaydata[j + 1]);
                                            pricesize.Size = Convert.ToInt64(Convert.ToDouble(runnerlaydata[j + 1]) * Convert.ToDouble(marketbook.PoundRate));
                                            pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
                                            pricesize.Price = Convert.ToDouble((Convert.ToDouble(runnerlaydata[j])).ToString("F2"));

                                            lstpricelist.Add(pricesize);
                                            j = j + 4;

                                        }

                                    }
                                }
                            }
                            catch (System.Exception ex)
                            {

                            }
                        }
                        else
                        {
                            for (int j = 0; j < runnerlaydata.Count();)
                            {
                                if (j < runnerlaydata.Count())
                                {
                                    var pricesize = new BettingServiceReference.PriceSize();
                                    pricesize.OrignalSize = Convert.ToDouble(runnerlaydata[j + 1]);
                                    pricesize.Size = Convert.ToInt64(Convert.ToDouble(runnerlaydata[j + 1]) * Convert.ToDouble(marketbook.PoundRate));
                                    pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
                                    pricesize.Price = Convert.ToDouble((Convert.ToDouble(runnerlaydata[j])).ToString("F2"));

                                    lstpricelist.Add(pricesize);
                                    j = j + 4;

                                }

                            }
                        }
                    }
                    else
                    {
                        for (int ii = 0; ii < 3; ii++)
                        {
                            var pricesize = new BettingServiceReference.PriceSize();
                            pricesize.OrignalSize = 0;
                            pricesize.Size = 0;
                            pricesize.SizeStr = "0";
                            pricesize.Price = 0;

                            lstpricelist.Add(pricesize);
                        }
                    }

                    runner.ExchangePrices.AvailableToLay = new List<BettingServiceReference.PriceSize>().ToArray();
                    runner.ExchangePrices.AvailableToLay = lstpricelist.ToArray();

                    lstRunners.Add(runner);
                }


                marketbook.Runners = new List<BettingServiceReference.Runner>(lstRunners).ToArray();

                return marketbook;

            }
            else
            {
                return new BettingServiceReference.MarketBook();
            }
        }
        public BettingServiceReference.MarketBook ConvertJsontoMarketObjectBF(BettingServiceReference.MarketBook BFMarketbook, string marketid, DateTime marketopendate, string sheetname, string MainSportsCategory, bool BettingAllowed, bool BettingAllowedoverall)
        {
            try
            {

                if (1 == 1)
                {
                    var marketbook = new BettingServiceReference.MarketBook();

                    marketbook.MarketId = BFMarketbook.MarketId;
                    marketbook.SheetName = "";
                    marketbook.IsMarketDataDelayed = BFMarketbook.IsMarketDataDelayed;
                    marketbook.PoundRate = LoggedinUserDetail.GetPoundRate();
                    marketbook.NumberOfWinners = BFMarketbook.NumberOfWinners;
                    marketbook.MarketBookName = sheetname;
                    marketbook.MainSportsname = MainSportsCategory;
                    marketbook.OrignalOpenDate = marketopendate;
                    marketbook.BettingAllowed = BettingAllowed;
                    marketbook.Version = BFMarketbook.Version;
                    marketbook.TotalMatched = BFMarketbook.TotalMatched;
                    marketbook.BettingAllowedOverAll = BettingAllowedoverall;
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
                        runner.RunnerName = sheetname;
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
                                            pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
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
                                            pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
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
                                    pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
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
                                        pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
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
                                        pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
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
                                    pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
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
        public BettingServiceReference.MarketBook ConvertJsontoMarketObjectLive(BettingServiceReference.MarketBook BFMarketbook, string marketid, DateTime marketopendate, string sheetname, string MainSportsCategory, bool BettingAllowed, bool BettingAllowedoverall)
        {
            try
            {



                if (1 == 1)
                {
                    var marketbook = new BettingServiceReference.MarketBook();

                    marketbook.MarketId = BFMarketbook.MarketId;
                    marketbook.SheetName = "";
                    marketbook.IsMarketDataDelayed = BFMarketbook.IsMarketDataDelayed;
                    marketbook.PoundRate = LoggedinUserDetail.GetPoundRate();
                    marketbook.NumberOfWinners = BFMarketbook.NumberOfWinners;
                    marketbook.MarketBookName = sheetname;
                    marketbook.MainSportsname = MainSportsCategory;
                    marketbook.OrignalOpenDate = marketopendate;
                    marketbook.BettingAllowed = BettingAllowed;
                    marketbook.Version = BFMarketbook.Version;
                    marketbook.TotalMatched = BFMarketbook.TotalMatched;
                    marketbook.BettingAllowedOverAll = BettingAllowedoverall;
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
                        runner.RunnerName = sheetname;
                        var lstpricelist = new List<BettingServiceReference.PriceSize>();
                        if (runneritem.ExchangePrices.AvailableToBack != null && runneritem.ExchangePrices.AvailableToBack.Count() > 0)
                        {
                            if (BFMarketbook.Runners.Count() == 1)
                            {
                                try
                                {


                                    if (runneritem.ExchangePrices.AvailableToBack[0].Price.ToString().Contains("."))
                                    {
                                        foreach (var backitems in runneritem.ExchangePrices.AvailableToLay.Take(3))
                                        {
                                            var pricesize = new BettingServiceReference.PriceSize();

                                            pricesize.Size = Convert.ToInt64(backitems.Size * Convert.ToDouble(marketbook.PoundRate));
                                            pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
                                            pricesize.Price = Convert.ToDouble((backitems.Price + 0.5).ToString("F2"));

                                            lstpricelist.Add(pricesize);
                                        }
                                    }
                                    else
                                    {
                                        foreach (var backitems in runneritem.ExchangePrices.AvailableToBack.Take(3))
                                        {
                                            var pricesize = new BettingServiceReference.PriceSize();

                                            pricesize.Size = Convert.ToInt64(backitems.Size * Convert.ToDouble(marketbook.PoundRate));
                                            pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
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
                                foreach (var backitems in runneritem.ExchangePrices.AvailableToBack.Take(3))
                                {
                                    var pricesize = new BettingServiceReference.PriceSize();

                                    pricesize.Size = Convert.ToInt64(backitems.Size * Convert.ToDouble(marketbook.PoundRate));
                                    pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
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
                                    foreach (var backitems in runneritem.ExchangePrices.AvailableToBack.Take(3))
                                    {
                                        var pricesize = new BettingServiceReference.PriceSize();

                                        pricesize.Size = Convert.ToInt64((backitems.Size) * Convert.ToDouble(marketbook.PoundRate));
                                        pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
                                        pricesize.Price = Convert.ToDouble((backitems.Price + 0.5).ToString("F2"));

                                        lstpricelist.Add(pricesize);
                                    }
                                }
                                else
                                {
                                    foreach (var backitems in runneritem.ExchangePrices.AvailableToLay.Take(3))
                                    {
                                        var pricesize = new BettingServiceReference.PriceSize();

                                        pricesize.Size = Convert.ToInt64(backitems.Size * Convert.ToDouble(marketbook.PoundRate));
                                        pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
                                        pricesize.Price = backitems.Price;

                                        lstpricelist.Add(pricesize);
                                    }
                                }
                            }
                            else
                            {
                                foreach (var backitems in runneritem.ExchangePrices.AvailableToLay.Take(3))
                                {
                                    var pricesize = new BettingServiceReference.PriceSize();

                                    pricesize.Size = Convert.ToInt64(backitems.Size * Convert.ToDouble(marketbook.PoundRate));
                                    pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
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
        
        public  class LineMarket
        {
            public string MarketCatalogueID { get; set; }
            public string MarketCatalogueName { get; set; }
            public string SelectionID { get; set; }
            public string SelectionName { get; set; }
            public string EventName { get; set; }
            public string EventID { get; set; }
            public string CompetitionName { get; set; }
            public string CompetitionID { get; set; }
            public bool BettingAllowed { get; set; }
            public Nullable<bool> isOpenedbyUser { get; set; }
        }
    }
}
