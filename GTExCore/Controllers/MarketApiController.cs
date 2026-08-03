using BettingServiceReference;
using Global.API;
using GTCore.Models;
using GTExCore.Common;
using GTExCore.HelperClass;
using GTExCore.Models;
using GTExCore.ViewModel;
using log4net;
using log4net.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.SignalR;
using Microsoft.VisualBasic.ApplicationServices;
using Newtonsoft.Json;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using UserServiceReference;

namespace GTExCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        UserBetsUpdateUnmatcedBets _objUserBets = new UserBetsUpdateUnmatcedBets();
        UserServicesClient objUsersServiceCleint = new UserServicesClient();
        public static wsnew ws1 = new wsnew();
        public static wsnew ws2 = new wsnew();
        public static wsnew ws4 = new wsnew();
        public static wsnew ws7 = new wsnew();
        public static wsnew ws0 = new wsnew();
        public static wsnew ws4339 = new wsnew();
        public static wsnew wsFancy = new wsnew();

        private wsnew wsBFMatch = new wsnew();

        private readonly IHubContext<BetHub> _hubContext;
        private readonly UserBetCacheService _betCache;
        //private readonly IMarketService _marketService;

        public MarketApiController(IRazorViewEngine viewEngine, IHubContext<BetHub> hubContext,
        UserBetCacheService betCache, ITempDataProvider tempDataProvider, IServiceProvider serviceProvider, IConfiguration configuration, IPasswordSettingsService passwordSettingsService, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
            _configuration = configuration;
            _passwordSettingsService = passwordSettingsService;
            _betCache = betCache;
            _hubContext = hubContext;
            // _marketService = marketService;
        }
        [Route("MarketBookData1")]
        [HttpGet]
        public async Task<IActionResult> MarketBookData1(
        string marketId,
        string sheetName,
        string category,
        int userId)
        {
            //var data = await _marketService.GetMarketDataAsync(
            //    "marketId",
            //    sheetName,
            //    category,
            //    userId);

            return Ok("");
        }

        [Route("MarketBookData")]
        [HttpGet]
        public async Task<string> MarketBookData(string ID, string sheetname, string MainSportsCategory)
        {
            try
            {
                int userId = LoggedinUserDetailAPI.GetUserId(HttpContext);
                DateTime marketopendate = DateTime.Now;
                List<string> lstIDs = new List<string>();
                lstIDs.Add(ID);
                var marketbook = await objBettingClient.GetMarketDatabyIDAsync(lstIDs.ToArray(), sheetname, marketopendate, MainSportsCategory, _passwordSettingsService.PasswordForValidate);
                marketbook[0].BettingAllowed = objUserServiceClient.GetBettingAllowedbyMarketIDandUserID(userId, ID);//await CheckForAllowedBettingOverAll(MainSportsCategory, sheetname, userId);
                var lstUserBet = JsonConvert.DeserializeObject<List<UserBets>>(objUsersServiceCleint.GetUserbetsbyUserID(userId, _passwordSettingsService.PasswordForValidate));
                var lstUserBets1 = _betCache.GetUserBets(userId);
                List<UserBets> lstUserBets = lstUserBets1.Where(item => item.isMatched == true && item.location != "9").ToList();
                var lstMarketIDS = lstUserBets.Select(item => item.MarketBookID).Distinct().ToArray();
                marketbook[0].DebitCredit = _objUserBets.ceckProfitandLoss(marketbook[0], lstUserBets);
                foreach (var runner in marketbook[0].Runners)
                {
                    // Fix: 'marketbook12' is not defined. Assuming it should be 'marketbook'.
                    runner.ProfitandLoss = Convert.ToInt64(marketbook[0].DebitCredit.Where(dc => dc.SelectionID == runner.SelectionId).Sum(dc => dc.Debit) - marketbook[0].DebitCredit.Where(dc => dc.SelectionID == runner.SelectionId).Sum(dc => dc.Credit));
                }
                return JsonConvert.SerializeObject(marketbook[0]);
            }
            catch (System.Exception ex)
            {
                var market = new BettingServiceReference.MarketBook();
                return JsonConvert.SerializeObject(market);
            }
        }
        [Route("SetmarketOpen")]
        [HttpGet]
        public async Task<bool> SetmarketOpen(string ID)
        {
            await objUsersServiceCleint.SetMarketBookOpenbyUSerAsync(73, ID);
            return true;
        }
        public async Task<bool> CheckForAllowedBettingOverAll(string categoryname, string marketbookname)
        {
            int userId = LoggedinUserDetailAPI.GetUserId(HttpContext);
            AllowedMarketWeb AllowedMarketsForUser = JsonConvert.DeserializeObject<AllowedMarketWeb>(await objUsersServiceCleint.GetAllowedMarketsbyUserIDAsync(userId));
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
        [Route("GetTvLinks")]
        [HttpGet]
        public async Task<IActionResult> GetTvLinks(int sportId, string eventId)
        {
            try
            {
                string jsonString = await objUsersServiceCleint.GetTvLinksAsync(eventId);
                var obj = JsonConvert.DeserializeObject<List<TVlink>>(jsonString);
                var data = obj?.Where(x => x.EventID == eventId).FirstOrDefault();
                long dID = Convert.ToInt64(data?.DimondID);
                if (sportId == 1)
                {
                    try
                    {
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                        HttpClient _httpClient = new HttpClient();
                        string url = $"https://serviceapi.fairgame7.com/getIframeUrl/{dID}?sportType=football&isTv=true&isScore=true";
                        HttpResponseMessage response = await _httpClient.GetAsync(url);
                        if (!response.IsSuccessStatusCode)
                        {
                            return Ok("Failed to fetch data from API");
                        }
                        string dimondApiResponse = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<DimondRoot>(dimondApiResponse);
                        var tvlink1 = result.tvData.iframeUrl;
                        return Ok(tvlink1 ?? "");
                    }
                    catch (System.Exception ex)
                    {
                        return Ok("");
                    }
                }
                //https://serviceapi.fairgame7.com/getIframeUrl/471734455?sportType=football&isTv=true&isScore=true

                return Ok(data.tvlink1 ?? "");
            }
            catch (System.Exception ex)
            {
                return Ok("");
            }
        }
        [Route("GetCardLinks")]
        [HttpGet]
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

        [Route("GetOtherSoccer")]
        [HttpGet]
        public async Task<string> GetOtherSoccer(string eventId)
        {
            try
            {
                int userId = LoggedinUserDetailAPI.GetUserId(HttpContext);
                List<string> data = new List<string>();
                var Soccergoalmarket = await objUsersServiceCleint.GetSoccergoalbyeventIdAsync(userId, eventId);
                if (Soccergoalmarket != null)
                {
                    foreach (var item in Soccergoalmarket)
                    {
                        if (item.MarketCatalogueID != "")
                        {
                            await objUsersServiceCleint.SetMarketBookOpenbyUSerAsync(userId, item.MarketCatalogueID);
                        }
                    }
                }

                var results = JsonConvert.DeserializeObject<List<Models.MarketCatalgoue>>(await objUsersServiceCleint.GetMarketsOpenedbyUserAsync(userId));

                if (results != null)
                {
                    results = results.Where(item => item.EventID == eventId && item.Name != "Match Odds").ToList();
                    var marketbooks = new List<BettingServiceReference.MarketBook>();
                    List<string> lstIDs = new List<string>();
                    foreach (var item in results)
                    {
                        lstIDs = new List<string>();

                        lstIDs.Add(item.ID);
                        var marketbook = await objBettingClient.GetMarketDatabyIDAsync(lstIDs.ToArray(), item.Name, item.EventOpenDate, item.EventTypeName, _passwordSettingsService.PasswordForValidate);
                        if (marketbook.Count() > 0)
                        {
                            if (marketbook[0].Runners != null)
                            {
                                marketbooks.Add(marketbook[0]);
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
                                //item2.BettingAllowedOverAll = CheckForAllowedBettingOverAll(item.EventTypeName, item2.MarketBookName);
                                item2.GetMatchUpdatesFrom = item.GetMatchUpdatesFrom;
                                item2.EventID = item.EventID;

                                var runnerdesc = await objUsersServiceCleint.GetSelectionNamesbyMarketIDAsync(item2.MarketId);
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
                                        var lstUserBet = _betCache.GetUserBets(userId);
                                        List<UserBets> lstUserBets = lstUserBet.Where(item3 => item3.isMatched == true && item3.MarketBookID == item2.MarketId).ToList();
                                        item2.DebitCredit = _objUserBets.ceckProfitandLoss(item2, lstUserBets);
                                        runneritem.ProfitandLoss = Convert.ToInt64(item2.DebitCredit.Where(item5 => item5.SelectionID == runneritem.SelectionId).Sum(item5 => item5.Debit) - item2.DebitCredit.Where(item5 => item5.SelectionID == runneritem.SelectionId).Sum(item5 => item5.Credit));

                                    }
                                }
                            }
                        }
                    }
                    return JsonConvert.SerializeObject(marketbooks);
                }
                else
                {
                    return "market not found.";
                }
            }
            catch (System.Exception ex)
            {
                return "error occour.";
            }
        }

        [Route("showcompleteduserbetsFancyIN")]
        [HttpGet]
        public async Task<string> showcompleteduserbetsFancyIN(string marektbookID, string selectionID)
        {
            int userId = LoggedinUserDetailAPI.GetUserId(HttpContext);
            var lstUserBets = _betCache.GetUserBets(userId);
            BettingServiceReference.MarketBookForindianFancy CurrentMarketProfitandloss = _objUserBets.GetBookPositionINAPI(marektbookID, selectionID, lstUserBets);
            if (CurrentMarketProfitandloss.RunnersForindianFancy != null)
            {
                var lstCurrentMarketBets = lstUserBets.Where(item => item.MarketBookID == marektbookID && item.isMatched == true).ToList();
                if (lstCurrentMarketBets.Count > 0)
                {
                    lstCurrentMarketBets = lstCurrentMarketBets.OrderBy(item => Convert.ToInt32(item.UserOdd)).ToList();
                    var maxuserodd = -1 * (Convert.ToInt32(lstCurrentMarketBets[0].UserOdd) - 1);
                    var minuserodd = -1 * (Convert.ToInt32(lstCurrentMarketBets[lstCurrentMarketBets.Count - 1].UserOdd) + 1);
                    //CurrentMarketProfitandloss.RunnersForindianFancy = CurrentMarketProfitandloss.RunnersForindianFancy.Where(item => item.Handicap >= minuserodd && item.Handicap <= maxuserodd).ToList();
                }
            }
            return JsonConvert.SerializeObject(CurrentMarketProfitandloss); 
        }
    }

}
