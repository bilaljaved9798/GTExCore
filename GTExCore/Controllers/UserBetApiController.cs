using BettingServiceReference;
using Global.API;
using GTCore.Models;
using GTExcgange.API.Models;
using GTExCore.Common;
using GTExCore.HelperClass;
using GTExCore.Models;
using log4net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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

    public class UserBetApiController : Controller
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
        private readonly UserBetsUpdateUnmatcedBets _objUserBets = new UserBetsUpdateUnmatcedBets();
        public UserBetApiController(IRazorViewEngine viewEngine, UserBetCacheService betCache, ITempDataProvider tempDataProvider, IServiceProvider serviceProvider, IConfiguration configuration, IPasswordSettingsService passwordSettingsService, IHttpContextAccessor httpContextAccessor, IHubContext<BetHub> betHubContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
            _configuration = configuration;
            _passwordSettingsService = passwordSettingsService;
            _hubContext = betHubContext;
            _betCache = betCache;
        }
        [Route("UserBets")]
        [HttpGet]
        public async Task<IActionResult> UserBets(int userid)
        {
            try
            {
                var lstUserBets = _betCache.GetUserBets(userid);

                var lstAllUserBets = new List<UserBets>();


                if (lstUserBets != null)
                {
                    lstAllUserBets.AddRange(lstUserBets);
                }

                // 👉 Remove duplicates + sort
                lstAllUserBets = lstAllUserBets
                    .GroupBy(x => x.ID)
                    .Select(g => g.First())
                    .OrderByDescending(x => x.ID)
                    .ToList();


                return Ok(lstAllUserBets);   // ✅ FIXED
            }
            catch (Exception)
            {
                return Ok(new List<UserBets>()); // safer than ""
            }
        }
        [Route("UserAllBets")]
        [HttpGet]
        public async Task<IActionResult> UserAllBets(int userid)
        {
            try
            {
                var lstUserBets = JsonConvert.DeserializeObject<List<UserBets>>(objUsersServiceCleint.GetUserbetsbyUserID(userid, _passwordSettingsService.PasswordForValidate));

                var lstAllUserBets = new List<UserBets>();


                if (lstUserBets != null)
                {
                    lstAllUserBets.AddRange(lstUserBets);
                }

                // 👉 Remove duplicates + sort
                lstAllUserBets = lstAllUserBets
                    .GroupBy(x => x.ID)
                    .Select(g => g.First())
                    .OrderByDescending(x => x.ID)
                    .ToList();


                return Ok(lstAllUserBets);   // ✅ FIXED
            }
            catch (Exception)
            {
                return Ok(new List<UserBets>()); // safer than ""
            }
        }
        [Route("InsertUserBet")]
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> InsertUserBet([FromBody] BetRequestModel betRequest)
        {
            try
            {
                var maxOdd = await objUsersServiceCleint.GetMaxOddBackandLayAsync(betRequest.UserId);

                string selectionName = betRequest.Clickedlocation == 9
                    ? betRequest.MarketbookName
                    : betRequest.Selectionname;

                string password = betRequest.Clickedlocation == 8
                    ? _passwordSettingsService.PasswordForValidate
                    : "34RxqHH9EqoJn4ZHLTwN5ag3UfZuKcvFfSE7U5FNg0STZ/6yEjxEDfhuJ3wOcr0m";

                await objUsersServiceCleint.InsertUserBetNewAsync(
                    Convert.ToDecimal(betRequest.Odd),
                    betRequest.SelectionID[0],
                    selectionName,
                    betRequest.BetType,
                    betRequest.Amount,
                    betRequest.Betslipamountlabel,
                    Convert.ToDecimal(maxOdd.MaxOddBack),
                    Convert.ToDecimal(maxOdd.MaxOddLay),
                    Convert.ToBoolean(maxOdd.CheckforMaxOddBack),
                    Convert.ToBoolean(maxOdd.CheckforMaxOddLay),
                    betRequest.Clickedlocation,
                    betRequest.UserId,
                    betRequest.Betslipsize,
                    password,
                    betRequest.MarketbookID,
                    betRequest.MarketbookName,
                    true);

                // Refresh cache
                _betCache.RemoveUserBets(betRequest.UserId);
                var latestBets = _betCache.GetUserBets(betRequest.UserId);

                // Push latest bets to connected client
                await _hubContext.Clients
                    .Group($"User_{betRequest.UserId}")
                    .SendAsync("ReceiveUserBets", latestBets);

                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("CheckforPlaceBet")]
        [HttpPost]
        public BetValidationResponse CheckforPlaceBet([FromBody] BetRequestModel betRequest)
        {
            try
            {
                decimal totalLiability = 0;

                // GET USER BETS FROM CACHE
                var lstUserBets = _betCache.GetUserBets(betRequest.UserId);

                if (lstUserBets == null)
                    lstUserBets = new List<UserBets>();

                // MATCHED ODDS BETS
                List<UserBets> matchedOddsBets = lstUserBets
                    .Where(x => x.isMatched && x.location != "9")
                    .ToList();

                // FANCY BETS
                List<UserBets> matchedFancyBets = lstUserBets
                    .Where(x => x.isMatched && x.location == "9")
                    .ToList();

                // ============================================
                // CURRENT MARKET LIABILITY
                // ============================================

                if (betRequest.SelectionID != null && betRequest.RunnersCount != "0")
                {
                    foreach (var selectionId in betRequest.SelectionID)
                    {
                        totalLiability +=
                            _objUserBets.GetLiabalityofCurrentUserActual(
                                betRequest.UserId,
                                selectionId,
                                betRequest.BetType,
                                betRequest.MarketbookID,
                                betRequest.MarketbookName,
                                _passwordSettingsService.PasswordForValidate,
                                _betCache.GetUserBets(betRequest.UserId)
                            );
                    }
                }

                // ============================================
                // OTHER MARKET LIABILITY
                // ============================================

                totalLiability +=
                    _objUserBets.GetLiabalityofCurrentUserActualforOtherMarkets(
                        betRequest.UserId,
                        "",
                        betRequest.BetType,
                        betRequest.MarketbookID,
                        betRequest.MarketbookName,
                        matchedOddsBets
                    );

                // ============================================
                // FANCY LIABILITY
                // ============================================

                totalLiability +=
                    _objUserBets.GetLiabalityofCurrentUserfancy(
                        betRequest.UserId,
                        matchedFancyBets
                    );

                // ============================================
                // CURRENT BALANCE
                // ============================================

                decimal currentBalance =
                    Convert.ToDecimal(
                        objUsersServiceCleint.GetCurrentBalancebyUser(
                            betRequest.UserId,
                            _passwordSettingsService.PasswordForValidate
                        )
                    );

                // ============================================
                // AVAILABLE BALANCE
                // ============================================

                decimal availableBalance =
                    currentBalance - totalLiability;

                decimal betAmount =
                    Convert.ToDecimal(betRequest.Amount);

                // ============================================
                // CHECK BALANCE
                // ============================================

                if (availableBalance >= betAmount)
                {
                    return new BetValidationResponse
                    {
                        Success = true,
                        Message = "Valid Bet",
                        AvailableBalance = availableBalance,
                        Liability = totalLiability
                    };
                }

                else
                {
                    return new BetValidationResponse
                    {
                        Success = false,
                        Message = $"Insufficient balance. Available: {availableBalance}",
                        AvailableBalance = availableBalance,
                        Liability = totalLiability
                    };
                }
            }
            catch (Exception ex)
            {
                return new BetValidationResponse
                {
                    Success = false,
                    Message = ex.Message,
                    AvailableBalance = 0,
                    Liability = 0
                };
            }
        }

        public class BetValidationResponse
        {
            public bool Success { get; set; }
            public string Message { get; set; }
            public decimal AvailableBalance { get; set; }
            public decimal Liability { get; set; }
        }
    }
}
