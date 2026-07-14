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
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic.ApplicationServices;
using Newtonsoft.Json;
using NuGet.Common;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
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
        List<ProfitandLossEventType> lstProfitandLossAll = new List<ProfitandLossEventType>();
        public DashBoardApiController(IServiceProvider serviceProvider, IPasswordSettingsService passwordSettingsService, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _serviceProvider = serviceProvider;
            _passwordSettingsService = passwordSettingsService;
        }

        [Route("GetDefaultPageData")]
        [HttpGet]
        public async Task<IActionResult> GetDefaultPageData(int userid)
        {
            try
            {
                if (userid == 1)
                {
                    userid = 73;
                }
                //var results = objUsersServiceCleint.GetInPlayMatcheswithRunners1(userid);
                //List<InPlayMatches> lstInPlayMatches = JsonConvert.DeserializeObject<List<InPlayMatches>>(results);
                //List<string> lstIds = lstInPlayMatches.Where(item => item.EventTypeName == "Cricket").Distinct().Select(item => item.MarketCatalogueID).Distinct().ToList();
                //lstIds.AddRange(lstInPlayMatches.Where(item => item.EventTypeName == "Soccer").Distinct().Select(item => item.MarketCatalogueID).Distinct().ToList());
                //lstIds.AddRange(lstInPlayMatches.Where(item => item.EventTypeName == "Tennis").Distinct().Select(item => item.MarketCatalogueID).Distinct().ToList());
                //lstIds.AddRange(lstInPlayMatches.Where(item => item.EventTypeName == "Horse Racing").Distinct().Select(item => item.MarketCatalogueID).Distinct().ToList());
                //lstIds.AddRange(lstInPlayMatches.Where(item => item.EventTypeName == "Greyhound Racing").Distinct().Select(item => item.MarketCatalogueID).Distinct().ToList());
                var results = objUsersServiceCleint.GetInPlayMatcheswithRunners1(userid);
                List<InPlayMatches> lstInPlayMatches = JsonConvert.DeserializeObject<List<InPlayMatches>>(results);

                List<BettingServiceReference.MarketBook> marketBooks = lstInPlayMatches
                .Select(x => new BettingServiceReference.MarketBook
                {
                    EventID = x.EventID,
                    MarketId = x.MarketCatalogueID,

                    MainSportsname = x.EventTypeName,
                    MarketStatusstr = x.MarketStatus,
                    MarketBookName = x.EventName,
                    OrignalOpenDate = x.EventOpenDate,
                    SheetName = x.SheetName,


                    // map remaining properties
                })
                .ToList();

                //BettingServiceReference.MarketBook[] lstGridMarkets = objBettingClient.GetMarketDataList(_passwordSettingsService.PasswordForValidate);
                var model = new DefaultPageModel1();
                model.AllMarkets = marketBooks;
                return Ok(new { page = model });
            }
            catch (System.Exception ex)
            {
                return BadRequest("Invalid model");
            }
        }
        BettingServiceClient objBettingClient = new BettingServiceClient();
        List<RootSCT> rootsct = new List<RootSCT>();
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

        [Route("InPlayMatches")]
        [HttpGet]
        public async Task<IActionResult> InPlayMatches(int userid, int mainSportsCategory)
        {
            try
            {
                List<TodayHorseRacing> lstTodayHorseRacing = JsonConvert.DeserializeObject<List<TodayHorseRacing>>(await objUsersServiceCleint.GetTodayHorseRacingAsync(userid, mainSportsCategory.ToString()));
                return Ok(new { page = lstTodayHorseRacing });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Route("GetBalnceDetails")]
        [HttpGet]
        public async Task<IActionResult> GetBalnceDetails(int userId)
        {

            double CurrentAccountBalance = 0;
            string CurrentLiabality = "";
            try
            {
                LoggedinUserDetail.CurrentAccountBalance = Convert.ToDouble(await objUsersServiceCleint.GetStartingBalanceAsync(userId, _passwordSettingsService.PasswordForValidate));
                CurrentAccountBalance = Convert.ToDouble(objUsersServiceCleint.GetCurrentBalancebyUser(userId, _passwordSettingsService.PasswordForValidate));
            }
            catch (System.Exception ex)
            {
            }

            double laboddmarket = 0;
            double othermarket = 0;
            List<UserBets> lstUserBets = JsonConvert.DeserializeObject<List<Models.UserBets>>(await objUsersServiceCleint.GetUserbetsbyUserIDAsync(userId, _passwordSettingsService.PasswordForValidate));
            List<UserBets> lstUserBetsOdds = lstUserBets.Where(x => x.location != "9").ToList();
            List<UserBets> lstUserBetsfncy = lstUserBets.Where(x => x.location == "9").ToList();
            laboddmarket = objUserBets.GetLiabalityofCurrentUser(userId, lstUserBetsOdds);
            othermarket = objUserBets.GetLiabalityofCurrentUserfancy(userId, lstUserBetsfncy);
            CurrentLiabality = (laboddmarket + othermarket).ToString("F2");
            LoggedinUserDetail.CurrentAvailableBalance = CurrentAccountBalance + Convert.ToDouble(CurrentLiabality);

            List<UserLiabality> lstUserLiabality = JsonConvert.DeserializeObject<List<UserLiabality>>(objUsersServiceCleint.GetCurrentLiabality(userId));
            decimal totalLiability = lstUserLiabality
    .Sum(x =>
    {
        decimal value;
        return decimal.TryParse(x.Liabality, out value)
            ? value
            : 0;
    });
            var NetBalance = objUsersServiceCleint.GetProfitorLossbyUserID(userId, false, _passwordSettingsService.PasswordForValidate).ToString();


            return Ok(new { page = CurrentLiabality });

        }

        [Route("ProfitandLoss")]
        [HttpPost]
        public async Task<IActionResult> ProfitandLoss([FromBody] ProfitLossRequest request)
        {
            try
            {
                //string DateFrom = ConvertDateFormat(request.DateFrom);
                //string DateTo = ConvertDateFormat(request.DateTo);

                if (request.chkfancy == true && request.chkseassion == true)
                {
                    Getdataforfancybysession(request.DateFrom, request.DateTo, request.chkfancy.Value, request.userid.Value);
                }

                if (request.chkByMarket == false)
                {
                    List<ProfitandLossEventType> lstProfitandlossEventtype = new List<ProfitandLossEventType>();
                    lstProfitandlossEventtype = JsonConvert.DeserializeObject<List<ProfitandLossEventType>>(objUsersServiceCleint.GetAccountsDatabyEventtypeuserIDandDateRange(request.userid.Value, request.DateFrom, request.DateTo, _passwordSettingsService.PasswordForValidate));
                    var data = JsonConvert.DeserializeObject(await objUsersServiceCleint.GetDatabyAgentIDForCommisionandDateRangeAsync(request.userid.Value, request.DateFrom, request.DateTo, _passwordSettingsService.PasswordForValidate));
                    ProfitandLossEventType objProfitandLossCommission = new ProfitandLossEventType();
                    objProfitandLossCommission.EventType = "Commission";
                    objProfitandLossCommission.NetProfitandLoss = Convert.ToDecimal(data); //lstProfitandlossEventtypeCommission.Sum(item => item.NetProfitandLoss);
                    lstProfitandlossEventtype.Add(objProfitandLossCommission);

                    if (lstProfitandlossEventtype.Count > 0)
                    {
                        if (request.chkfancy == true)
                        {
                            lstProfitandlossEventtype = lstProfitandlossEventtype.Where(item => item.EventType.Contains("Fancy")).ToList();
                        }
                        if (request.chkByMarketCricket == true)
                        {
                            lstProfitandlossEventtype = lstProfitandlossEventtype.Where(item => item.EventType == ("Cricket") || item.EventType == ("Fancy")).ToList();
                        }


                        //ViewBag.NetProfitorLoss1 = lstProfitandlossEventtype.Where(item => item.EventType != "Commission").Sum(item => item.NetProfitandLoss).ToString("N0");

                    }

                    lstProfitandLossAll = lstProfitandlossEventtype;
                }
                else
                {
                    try
                    {

                        List<ProfitandLossEventType> lstProfitandlossEventtype = new List<ProfitandLossEventType>();

                        lstProfitandlossEventtype = JsonConvert.DeserializeObject<List<ProfitandLossEventType>>(objUsersServiceCleint.GetAccountsDatabyEventNameuserIDandDateRange(request.userid.Value, request.DateFrom, request.DateTo, _passwordSettingsService.PasswordForValidate));

                        if (lstProfitandlossEventtype.Count > 0)
                        {
                            if (request.chkfancy == true)
                            {
                                lstProfitandlossEventtype = lstProfitandlossEventtype.Where(item => item.EventType.Contains("Fancy")).ToList();
                            }
                            if (request.chkByMarketCricket == true)
                            {
                                lstProfitandlossEventtype = lstProfitandlossEventtype.Where(item => item.Eventtype1 == ("Cricket") || item.Eventtype1 == ("Fancy")).ToList();
                            }
                        }

                        lstProfitandLossAll = lstProfitandlossEventtype;

                    }

                    catch (System.Exception ex)
                    {

                    }

                    // GetDistinctMatchesfromResults();
                }

                return Ok(lstProfitandLossAll);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [Route("LedgerDetails")]
        [HttpGet]
        public async Task<IActionResult> LedgerDetails(string DateFrom, string DateTo, int UserID, bool isCredit)
        {
            try
            {
                DateFrom = ConvertDateFormat(DateFrom);
                DateTo = ConvertDateFormat(DateTo);

                List<UserAccounts> lstUserAccounts = JsonConvert.DeserializeObject<List<UserAccounts>>(objUsersServiceCleint.GetAccountsDatabyUserIDandDateRange(UserID, DateFrom, DateTo, isCredit, _passwordSettingsService.PasswordForValidate));
                if (lstUserAccounts.Count > 0)
                {
                    UserAccounts objUseraccounts = new UserAccounts();
                    objUseraccounts.AccountsTitle = "Opening Balance";
                    objUseraccounts.Debit = lstUserAccounts[0].OpeningBalance.ToString("F2");
                    objUseraccounts.Credit = "0.00";
                    objUseraccounts.CreatedDate = lstUserAccounts[0].CreatedDate;
                    objUseraccounts.OpeningBalance = lstUserAccounts[0].OpeningBalance;
                    lstUserAccounts.Insert(0, objUseraccounts);
                    for (int i = 0; i <= lstUserAccounts.Count - 1; i++)
                    {
                        if (i + 1 < lstUserAccounts.Count)
                        {
                            if (lstUserAccounts[i + 1].Debit == "" || lstUserAccounts[i + 1].Debit == "0.00")
                            {
                                lstUserAccounts[i + 1].OpeningBalance = lstUserAccounts[i].OpeningBalance - Convert.ToDecimal(lstUserAccounts[i + 1].Credit);
                                lstUserAccounts[i + 1].Credit = (-1 * Convert.ToDecimal(lstUserAccounts[i + 1].Credit)).ToString();
                                lstUserAccounts[i + 1].Debit = "0.00";
                            }
                            else
                            {
                                lstUserAccounts[i + 1].OpeningBalance = lstUserAccounts[i].OpeningBalance + Convert.ToDecimal(lstUserAccounts[i + 1].Debit);
                                lstUserAccounts[i + 1].Credit = "0.00";
                            }
                        }
                    }
                }
                var NetProfitorLoss = objUsersServiceCleint.GetProfitorLossbyUserID(UserID, isCredit, _passwordSettingsService.PasswordForValidate);
                return Ok(new
                {
                    useraccount = lstUserAccounts,
                    netpl = NetProfitorLoss
                });
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex);
            }
        }

        public void Getdataforfancybysession(string DateFrom, string DateTo, bool chkfancy, int userid)
        {
            try
            {
                List<ProfitandLossEventType> lstProfitandlossEventtype = new List<ProfitandLossEventType>();

                lstProfitandlossEventtype = JsonConvert.DeserializeObject<List<ProfitandLossEventType>>(objUsersServiceCleint.GetAccountsDatabyEventNameuserIDandDateRangeFancywithMArketName(userid, DateFrom, DateTo, _passwordSettingsService.PasswordForValidate));

                if (lstProfitandlossEventtype.Count > 0)
                {
                    if (chkfancy == true)
                    {
                        lstProfitandlossEventtype = lstProfitandlossEventtype.Where(item => item.EventType.Contains("Fancy")).ToList();
                    }
                    lstProfitandlossEventtype = lstProfitandlossEventtype.OrderBy(o => o.EventID).ThenBy(o => o.EventType).ToList();
                    lstProfitandLossAll = lstProfitandlossEventtype;
                }
            }
            catch (System.Exception ex)
            {

            }
        }
        public string ConvertDateFormat(string datetoconvert)
        {
            string[] datearr = datetoconvert.Split('-');
            datetoconvert = datearr[2].ToString() + "-" + datearr[1].ToString() + "-" + datearr[0].ToString();
            return datetoconvert;
        }

        [Route("GetSoccerMarkets")]
        [HttpGet]
        public async Task<List<string>> GetSoccerMarkets(string eventId)
        {
            var soccerGoalMarket =
                await objUsersServiceCleint.GetSoccergoalbyeventIdAsync(73, eventId);

            return soccerGoalMarket
                .Where(x => !string.IsNullOrWhiteSpace(x.MarketCatalogueID))
                .Select(x => x.MarketCatalogueID)
                .Distinct()
                .ToList();
        }
        [Route("GetRelatedEvent")]
        [HttpGet]
        public async Task<List<InPlayMatches>> GetRelatedEvent(string eventtype, string marketbookID)
        {
            var results = await objUsersServiceCleint.GetInPlayMatcheswithRunners1Async(73);

            var lstInPlayMatches =
                JsonConvert.DeserializeObject<List<InPlayMatches>>(results) ?? new List<InPlayMatches>();

            return lstInPlayMatches
                .Where(x => x.EventTypeName == eventtype)
                .GroupBy(x => x.MarketCatalogueID)
                .Select(g => g.First())
                .Where(x => x.MarketCatalogueID != marketbookID)
                .OrderBy(x => x.EventOpenDate)
                .ToList();
        }

        public class ProfitLossRequest
        {
            public string DateFrom { get; set; }
            public string DateTo { get; set; }
            public bool? chkseassion { get; set; }
            public bool? chkfancy { get; set; }
            public bool? chkByMarket { get; set; }
            public bool? chkByMarketCricket { get; set; }
            public int? userid { get; set; }
        }
    }
}
