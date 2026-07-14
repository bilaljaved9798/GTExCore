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
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using UserServiceReference;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GTExCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class Fancy2MarketAPIController : ControllerBase
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private BettingServiceClient objBettingClient = new BettingServiceClient();
        UserServicesClient objUsersServiceCleint = new UserServicesClient();
        UserBetsUpdateUnmatcedBets _objUserbets = new UserBetsUpdateUnmatcedBets();
        private readonly IRazorViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;
        private readonly IPasswordSettingsService _passwordSettingsService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        List<UserBetsForAdmin> adminList = new List<UserBetsForAdmin>();
        List<UserBetsforSuper> superList = new List<UserBetsforSuper>();
        List<UserBetsforSamiadmin> samiList = new List<UserBetsforSamiadmin>();
        List<UserBetsforAgent> agentList = new List<UserBetsforAgent>();
        public Fancy2MarketAPIController(IRazorViewEngine viewEngine, ITempDataProvider tempDataProvider, IServiceProvider serviceProvider, IConfiguration configuration, IPasswordSettingsService passwordSettingsService, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
            _passwordSettingsService = passwordSettingsService;
        }

        [Route("LoadFancyMarketIN")]
        [HttpGet]
        public async Task<IActionResult> LoadFancyMarketIN(string EventID, string MarketBookID, int UserId)
        {
            try
            {
                AllMarketRoot objRoot = JsonConvert.DeserializeObject<AllMarketRoot>(await objBettingClient.GetRunnersForFancyAsync(EventID, MarketBookID));
                List<Models.UserBets> lstUserBets = JsonConvert.DeserializeObject<List<Models.UserBets>>(objUsersServiceCleint.GetUserbetsbyUserID(UserId, _passwordSettingsService.PasswordForValidate));
                if (lstUserBets.Any(x => x.location == "9"))
                {
                    var plLookup = lstUserBets.GroupBy(b => b.SelectionID)
                   .ToDictionary(
                    g => g.Key,
                    g => _objUserbets.GetBookPositionINNew(
                    g.Key,
                    adminList,
                    superList,
                    samiList,
                    agentList,
                    g.ToList()
                    )
                   );
                    foreach (var runner in objRoot.session)
                    {
                        // Get correct PL for this runner
                        var pl = plLookup.TryGetValue(runner.SelectionId, out var value)
                            ? value
                            : null;

                        runner.currentmarketsfancyPL = pl;

                        if (pl?.RunnersForindianFancy != null && pl.RunnersForindianFancy.Any())
                        {
                            runner.Profit = Convert.ToDouble(
                                pl.RunnersForindianFancy.Max(t => t.ProfitandLoss)
                            );

                            runner.Lose = Convert.ToDouble(
                                pl.RunnersForindianFancy.Min(t => t.ProfitandLoss)
                            );
                        }
                        else
                        {
                            // Safe defaults
                            runner.Profit = 0;
                            runner.Lose = 0;
                        }
                    }
                }
                if (lstUserBets.Any(x => x.location == "8"))
                {
                    var figureMarkets = objRoot?.diamondRoot?.data?
                        .Where(x => x.gtype == "cricketcasino")
                        .ToList();

                    if (figureMarkets != null)
                    {
                        foreach (var market in figureMarkets)
                        {
                            var marketBets = lstUserBets
                                .Where(x => x.MarketBookID == market.mid)
                                .ToList();

                            if (marketBets.Any())
                            {
                                _objUserbets.ceckProfitandLossFigApi(
                                    market,
                                    marketBets
                                );
                            }
                        }
                    }
                }
                return Ok(new { page = JsonConvert.SerializeObject(objRoot) });
            }
            catch (System.Exception ex)
            {
                return Ok(new { page = "" });
            }
        }
    }
}
