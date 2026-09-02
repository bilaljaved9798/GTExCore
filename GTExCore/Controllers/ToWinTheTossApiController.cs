using BettingServiceReference;
using Global.API;
using GTCore.Models;
using GTExCore.Common;
using GTExCore.Models;
using log4net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.SignalR;
using Microsoft.VisualBasic.ApplicationServices;
using Newtonsoft.Json;
using UserServiceReference;

namespace GTExCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ToWinTheTossApiController : ControllerBase
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
       
        private readonly IHubContext<BetHub> _hubContext;
        private readonly UserBetCacheService _betCache;

        public ToWinTheTossApiController(IPasswordSettingsService passwordSettingsService, UserBetCacheService betCache, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _passwordSettingsService = passwordSettingsService;
            _betCache = betCache;
        }

        [HttpGet("CheckforToWintheTossMarket")]
        public async Task<string> CheckforToWintheTossMarket(string eventId)
        {
            int userId = LoggedinUserDetailAPI.GetUserId(HttpContext);
            var wintethossmarket = await objUsersServiceCleint.GetToWintheTossbyeventIdAsync(userId, eventId);
            if (wintethossmarket != null)
            {
                if (wintethossmarket.MarketCatalogueID != null)
                {
                    return wintethossmarket.MarketCatalogueID;
                }
            }
            return null;
        }

        [HttpGet("WinTheToss")]
        public async Task<string> WinTheToss(string ID)
        {
            try
            {
                int userId = LoggedinUserDetailAPI.GetUserId(HttpContext);
                UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();
                await objUsersServiceCleint.SetMarketBookOpenbyUSerAsync(160,ID);

                var results = JsonConvert.DeserializeObject<List<Models.MarketCatalgoue>>(await objUsersServiceCleint.GetMarketsOpenedbyUserAsync(userId));

                if (results != null)
                {
                    results = results.Where(item => item.ID == ID).ToList();
                    var marketbooks = new List<BettingServiceReference.MarketBook>();
                    List<string> lstIDs = new List<string>();
                    foreach (var item in results)
                    {
                        lstIDs = new List<string>();

                        lstIDs.Add(item.ID);

                        var marketbook = await objBettingClient.GetMarketDatabyIDAsync(lstIDs.ToArray(), item.Name, item.EventOpenDate, item.EventTypeName, _passwordSettingsService.PasswordForValidate);
                        marketbook[0].BettingAllowed = objUserServiceClient.GetBettingAllowedbyMarketIDandUserID(userId, ID);//await CheckForAllowedBettingOverAll(MainSportsCategory, sheetname, userId);
                        var lstUserBet = JsonConvert.DeserializeObject<List<UserBets>>(objUsersServiceCleint.GetUserbetsbyUserID(userId, _passwordSettingsService.PasswordForValidate));
                        var lstUserBets1 = _betCache.GetUserBets(userId);
                        List<UserBets> lstUserBets = lstUserBets1.Where(item => item.isMatched == true && item.location != "9").ToList();
                        var lstMarketIDS = lstUserBets.Select(item => item.MarketBookID).Distinct().ToArray();
                        //marketbook[0].DebitCredit = _objUserBets.ceckProfitandLoss(marketbook[0], lstUserBets);
                        foreach (var runner in marketbook[0].Runners)
                        {
                            // Fix: 'marketbook12' is not defined. Assuming it should be 'marketbook'.
                            //runner.ProfitandLoss = Convert.ToInt64(marketbook[0].DebitCredit.Where(dc => dc.SelectionID == runner.SelectionId).Sum(dc => dc.Debit) - marketbook[0].DebitCredit.Where(dc => dc.SelectionID == runner.SelectionId).Sum(dc => dc.Credit));
                        }



                        return JsonConvert.SerializeObject(marketbook[0]);
                    }
                    var market = new BettingServiceReference.MarketBook();
                    return JsonConvert.SerializeObject(market);
                }

                else
                {
                    var market = new BettingServiceReference.MarketBook();
                    return JsonConvert.SerializeObject(market);
                }

            }
            catch (System.Exception ex)
            {
                var market = new BettingServiceReference.MarketBook();
                return JsonConvert.SerializeObject(market);
            }
        }


    }
}
