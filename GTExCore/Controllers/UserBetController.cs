using BettingServiceReference;
using Census.API.Common;
using Global.API;
using GTCore.Models;
using GTExcgange.API.Models;
using GTExCore.Common;
using GTExCore.HelperClass;
using GTExCore.Models;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Engines;
using System.Configuration;
using System.Data;
using System.Linq.Expressions;
using UserServiceReference;

namespace Census.API.Controllers
{

    public class UserBetController : Controller
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private BettingServiceClient objBettingClient = new BettingServiceClient();
        UserServicesClient objUsersServiceCleint = new UserServicesClient();
        private UserBetsUpdateUnmatcedBets _userBetsUpdateUnmatcedBets = new UserBetsUpdateUnmatcedBets();
        private readonly IRazorViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;
        private readonly IPasswordSettingsService _passwordSettingsService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHubContext<UserBetsHub> _hubContext;

        public UserBetController(IRazorViewEngine viewEngine, ITempDataProvider tempDataProvider, IServiceProvider serviceProvider, IConfiguration configuration, IPasswordSettingsService passwordSettingsService, IHubContext<UserBetsHub> hubContext, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
            _passwordSettingsService = passwordSettingsService;
            _hubContext = hubContext;
        }

        public ActionResult Index()
        {
            return View();
        }

        //public JsonResult UpdateUserBets()
        //{
        //    List<UserBets> lstUserBets = JsonConvert.DeserializeObject<List<UserBets>>(objUsersServiceCleint.GetUserbetsbyUserID(LoggedinUserDetail.GetUserID(), _passwordSettingsService.PasswordForValidate));

        //    return Json(lstUserBets);
        //}
        public async Task<string> UserBets()
        {
            try
            {
                if (LoggedinUserDetail.GetUserTypeID() != 3)
                {
                    ViewBag.backgrod = "#1D9BF0";
                    ViewBag.color = "white";
                }
                if (LoggedinUserDetail.GetUserTypeID() == 3)
                {
                    List<UserBets> lstUserBets = JsonConvert.DeserializeObject<List<UserBets>>(objUsersServiceCleint.GetUserbetsbyUserID(LoggedinUserDetail.GetUserID(), _passwordSettingsService.PasswordForValidate));
                    _httpContextAccessor.HttpContext.Session.SetObject("userbets", lstUserBets);
                    List<UserBets> lstAllUserBets = new List<UserBets>();
                    lstAllUserBets.Clear();
                    if (_httpContextAccessor.HttpContext.Session.GetObject<BettingServiceReference.LinevMarkets>("linevmarkets") != null)
                    {
                        //_httpContextAccessor.HttpContext.Session.GetObject<UserIDandUserType>("User");
                        List<UserBets> lstUserBetAll = _httpContextAccessor.HttpContext.Session.GetObject<List<UserBets>>("userbet");
                        List<BettingServiceReference.LinevMarkets> linevmarketsfig = new List<BettingServiceReference.LinevMarkets>();
                        List<BettingServiceReference.LinevMarkets> linevmarkets = _httpContextAccessor.HttpContext.Session.GetObject<List<BettingServiceReference.LinevMarkets>>("linevmarkets");
                        if (linevmarkets != null)
                        {
                            linevmarketsfig = linevmarkets.GroupBy(item => item.MarketCatalogueNamek__BackingField).Select(g => g.First()).Where(item2 => item2.EventNamek__BackingField == "Figure").ToList();
                            linevmarkets = linevmarkets.Where(item => item.EventNamek__BackingField != "Figure").ToList();
                            List<UserBets> lstFancyBets = new List<UserBets>();
                            foreach (var lineitem in linevmarkets)
                            {
                                lstFancyBets = lstUserBetAll.Where(item => item.MarketBookID == lineitem.MarketCatalogueIDk__BackingField).ToList();
                                lstAllUserBets.AddRange(lstFancyBets);
                            }
                            List<UserBets> lstFancyfigBets = new List<UserBets>();
                            foreach (var lineitem in linevmarketsfig)
                            {
                                lstFancyfigBets = lstUserBetAll.Where(item => item.MarketBookID == lineitem.MarketCatalogueIDk__BackingField).ToList();
                                lstAllUserBets.AddRange(lstFancyfigBets);
                            }
                        }
                    }
                    if (_httpContextAccessor.HttpContext.Session.GetObject<string>("TWT") != "")
                    {
                        List<UserBets> lstUserBetAll = _httpContextAccessor.HttpContext.Session.GetObject<List<UserBets>>("userbet");

                        string TWTMarket = (string)_httpContextAccessor.HttpContext.Session.GetObject<string>("TWT");
                        if (TWTMarket != null)
                        {
                            List<UserBets> lstFancyBets = new List<UserBets>();

                            lstFancyBets = lstUserBetAll.Where(item => item.MarketBookID == TWTMarket).ToList();

                            lstAllUserBets.AddRange(lstFancyBets);

                        }
                    }
                    if (lstUserBets != null)
                    {
                        lstAllUserBets.AddRange(lstUserBets);
                        lstAllUserBets = lstAllUserBets.GroupBy(car => car.ID).Select(g => g.First()).ToList();
                        lstAllUserBets = lstAllUserBets.OrderByDescending(item => item.ID).ToList();

                    }

                    ViewData["liabality"] = _httpContextAccessor.HttpContext.Session.GetObject<object>("liabality");
                    ViewBag.totliabality = _httpContextAccessor.HttpContext.Session.GetObject<object>("totliabality");
                    return await RenderRazorViewToStringAsync("UserBets", lstAllUserBets);
                }
                else
                {
                    if (LoggedinUserDetail.GetUserTypeID() == 2)
                    {
                        List<UserBetsforAgent> lstUserBets = _httpContextAccessor.HttpContext.Session.GetObject<List<UserBetsforAgent>>("userbet");
                        ViewData["liabality"] = _httpContextAccessor.HttpContext.Session.GetObject<object>("liabality");
                        ViewData["totliabality"] = _httpContextAccessor.HttpContext.Session.GetObject<object>("totliabality");
                        return await RenderRazorViewToStringAsync("UserBetsAgent", lstUserBets);
                    }
                    else
                    {
                        if (LoggedinUserDetail.GetUserTypeID() == 8)
                        {
                            List<UserBetsforSuper> lstUserBets = _httpContextAccessor.HttpContext.Session.GetObject<List<UserBetsforSuper>>("userbet");
                            ViewData["liabality"] = _httpContextAccessor.HttpContext.Session.GetObject<object>("liabality");
                            ViewData["totliabality"] = _httpContextAccessor.HttpContext.Session.GetObject<object>("totliabality");
                            return await RenderRazorViewToStringAsync("UserBetsForSuper", lstUserBets);
                        }
                        else
                        {
                            if (LoggedinUserDetail.GetUserTypeID() == 1)
                            {
                                List<UserBetsForAdmin> lstUserBets = _httpContextAccessor.HttpContext.Session.GetObject<List<UserBetsForAdmin>>("userbet");
                                ViewData["liabality"] = _httpContextAccessor.HttpContext.Session.GetObject<object>("liabality");
                                ViewData["totliabality"] = _httpContextAccessor.HttpContext.Session.GetObject<object>("totliabality");
                                return await RenderRazorViewToStringAsync("UserBetsForAdmin", lstUserBets);
                            }
                            else
                            {
                                if (LoggedinUserDetail.GetUserTypeID() == 9)
                                {
                                    List<UserBetsforSamiadmin> lstUserBets = _httpContextAccessor.HttpContext.Session.GetObject<List<UserBetsforSamiadmin>>("userbet");
                                    ViewData["liabality"] = _httpContextAccessor.HttpContext.Session.GetObject<object>("liabality");
                                    ViewData["totliabality"] = _httpContextAccessor.HttpContext.Session.GetObject<object>("totliabality");

                                    return await RenderRazorViewToStringAsync("UserBetsForSamiadmin", lstUserBets);
                                }
                                else
                                {
                                    List<UserBets> lstUserBets = new List<UserBets>();
                                    return await RenderRazorViewToStringAsync("UserBets", lstUserBets);
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                List<UserBets> lstUserBets = new List<UserBets>();
                return await RenderRazorViewToStringAsync("UserBets", lstUserBets);
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


        [HttpPost]
        [ActionName("InsertUserBetAdmin")]
        public string InsertUserBetAdmin(string SelectionID, string selecitonname, string userOdd, string amount, string bettype, string liveodd, bool ismatched, string status, string marketbookid, string marketbookname, string Liablaity, decimal BetSize, decimal PendingAmount, string location, long ParentID, int UserID)
        {
            try
            {
                selecitonname = selecitonname.Trim();
                var ID = objUsersServiceCleint.InsertUserBetAsync(SelectionID, UserID, userOdd, amount, bettype, liveodd, ismatched, status, marketbookid, DateTime.Now, DateTime.Now, selecitonname, marketbookname, Liablaity, BetSize.ToString(), PendingAmount, location, ParentID, 0, 0, false, false, "34RxqHH9EqoJn4ZHLTwN5ag3UfZuKcvFfSE7U5FNg0STZ/6yEjxEDfhuJ3wOcr0m");
                return "True" + "|" + ID.ToString();
            }
            catch (System.Exception ex)
            {
                return "False";
            }
        }
        [HttpPost]
        public bool InsertUserBetNew([FromBody] BetRequestModel betRequest)
        {
            try
            {
                if (betRequest.Clickedlocation == 8 || betRequest.Clickedlocation == 9)
                {
                    if (betRequest.Clickedlocation == 9)
                    {
                        var objMaxOddBack = objUsersServiceCleint.GetMaxOddBackandLay(LoggedinUserDetail.GetUserID());
                        objUsersServiceCleint.InsertUserBetNewAsync(Convert.ToDecimal(betRequest.Odd), betRequest.SelectionID[0], betRequest.MarketbookName, betRequest.BetType, betRequest.Amount, betRequest.Betslipamountlabel, Convert.ToDecimal(objMaxOddBack.MaxOddBack), Convert.ToDecimal(objMaxOddBack.MaxOddLay), Convert.ToBoolean(objMaxOddBack.CheckforMaxOddBack), Convert.ToBoolean(objMaxOddBack.CheckforMaxOddLay), betRequest.Clickedlocation, LoggedinUserDetail.GetUserID(), betRequest.Betslipsize, _passwordSettingsService.PasswordForValidate, betRequest.MarketbookID, betRequest.MarketbookName, true);
                        return true;
                    }
                    else
                    {
                        var objMaxOddBack = objUsersServiceCleint.GetMaxOddBackandLayAsync(LoggedinUserDetail.GetUserID());
                        objUsersServiceCleint.InsertUserBetNewAsync(Convert.ToDecimal(betRequest.Odd), betRequest.SelectionID[0], betRequest.Selectionname, betRequest.BetType, betRequest.Amount, betRequest.Betslipamountlabel, Convert.ToDecimal(objMaxOddBack.Result.MaxOddBack), Convert.ToDecimal(objMaxOddBack.Result.MaxOddLay), Convert.ToBoolean(objMaxOddBack.Result.CheckforMaxOddBack), Convert.ToBoolean(objMaxOddBack.Result.CheckforMaxOddLay), betRequest.Clickedlocation, LoggedinUserDetail.GetUserID(), betRequest.Betslipsize, "34RxqHH9EqoJn4ZHLTwN5ag3UfZuKcvFfSE7U5FNg0STZ/6yEjxEDfhuJ3wOcr0m", betRequest.MarketbookID, betRequest.MarketbookName, true);
                        return true;
                    }
                }
                else
                {
                    var objMaxOddBack = objUsersServiceCleint.GetMaxOddBackandLay(LoggedinUserDetail.GetUserID());
                    objUsersServiceCleint.InsertUserBetNew(Convert.ToDecimal(betRequest.Odd), betRequest.SelectionID[0],  betRequest.Selectionname, betRequest.BetType, betRequest.Amount, betRequest.Betslipamountlabel, Convert.ToDecimal(objMaxOddBack.MaxOddBack), Convert.ToDecimal(objMaxOddBack.MaxOddLay), Convert.ToBoolean(objMaxOddBack.CheckforMaxOddBack), Convert.ToBoolean(objMaxOddBack.CheckforMaxOddLay), betRequest.Clickedlocation, LoggedinUserDetail.GetUserID(), betRequest.Betslipsize, "34RxqHH9EqoJn4ZHLTwN5ag3UfZuKcvFfSE7U5FNg0STZ/6yEjxEDfhuJ3wOcr0m", betRequest.MarketbookID, betRequest.MarketbookName, true);
                    return true;
                }
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }
        public bool GetBettingAllowedbyMarketIDandUserID(string MarketBookID)
        {
            return objUsersServiceCleint.GetBettingAllowedbyMarketIDandUserID(LoggedinUserDetail.GetUserID(), MarketBookID);
        }
        public string UpdateUnMatchedStatustoComplete(string[] userbetsIDs)
        {

            List<long> lstUserBetIDs = userbetsIDs.Select(long.Parse).ToList();
            objUsersServiceCleint.UpdateUserBetUnMatchedStatusTocomplete(lstUserBetIDs, _passwordSettingsService.PasswordForValidate);
            return "True";
        }
        public string CheckforPlaceBet(string Amount, string Odd, string BetType, string[] SelectionID, string marketbookID, string MarketbookName, string Runnerscount)
        {
            try
            {
                List<UserBets> lstUserBets = JsonConvert.DeserializeObject<List<UserBets>>(objUsersServiceCleint.GetUserbetsbyUserID(LoggedinUserDetail.GetUserID(), _passwordSettingsService.PasswordForValidate));
                decimal TotLiabality = 0;
                decimal fancylab = 0;
                List<UserBets> UnlstUserBets = lstUserBets.Where(item => item.isMatched == false).ToList();
                List<UserBets> MlstUserBets = lstUserBets.Where(item => item.isMatched == true && item.location != "9").ToList();
                List<UserBets> lstuserbetfancy = lstUserBets.Where(item => item.location == "9").ToList();
                if (LoggedinUserDetail.GetUserTypeID() == 3)
                {
                    lstUserBets = lstUserBets.Where(item => item.isMatched && item.location != "9").ToList();
                    if (Runnerscount != null)
                    {
                        foreach (var selectionIDitem in SelectionID)
                        {
                            TotLiabality += _userBetsUpdateUnmatcedBets.GetLiabalityofCurrentUserActual(LoggedinUserDetail.GetUserID(), selectionIDitem, BetType, marketbookID, MarketbookName, _passwordSettingsService.PasswordForValidate);
                        }
                    }
                    TotLiabality += _userBetsUpdateUnmatcedBets.GetLiabalityofCurrentUserActualforOtherMarkets(LoggedinUserDetail.GetUserID(), "", BetType, marketbookID, MarketbookName, lstUserBets);

                    TotLiabality += _userBetsUpdateUnmatcedBets.GetLiabalityofCurrentUserfancy(LoggedinUserDetail.GetUserID(), lstuserbetfancy.Where(item => item.isMatched == true).ToList());
                    decimal CurrentBalance = Convert.ToDecimal(objUsersServiceCleint.GetCurrentBalancebyUser(LoggedinUserDetail.GetUserID(), _passwordSettingsService.PasswordForValidate));
                    decimal TotBalance = CurrentBalance + TotLiabality;
                    if (TotBalance >= Convert.ToDecimal(Amount))
                    {
                        return "True";
                    }
                    else
                    {
                        return "Available balance is less then your amount";
                    }
                }
                else
                {
                    return "You are not allowed to perform this operation.";
                }
            }
            catch (System.Exception ex)
            {
                //LoggedinUserDetail.LogError(ex);
                return "False";
            }
        }

        public async Task<string> UserBetsAll()
        {
            try
            {

                List<UserBets> lstUserBets = _httpContextAccessor.HttpContext.Session.GetObject<List<UserBets>>("userbets");

                lstUserBets.Where(w => w.location == "8").ToList().ForEach(i => i.UserOdd = (Convert.ToDouble(i.BetSize) / 100).ToString());
                List<UserBets> lstAllUserBets = new List<UserBets>();
                if (_httpContextAccessor.HttpContext.Session.GetObject<List<BettingServiceReference.LinevMarkets>>("linevmarkets") != null)
                {
                    List<UserBets> lstUserBetAll = _httpContextAccessor.HttpContext.Session.GetObject<List<UserBets>>("userbets");
                    List<BettingServiceReference.LinevMarkets> linevmarketsfig = new List<BettingServiceReference.LinevMarkets>();
                    
                    List<BettingServiceReference.LinevMarkets> linevmarkets = _httpContextAccessor.HttpContext.Session.GetObject<List<BettingServiceReference.LinevMarkets>>("linevmarkets");
                    if (linevmarkets != null)
                    {

                        linevmarketsfig = linevmarkets.GroupBy(item => item.MarketCatalogueNamek__BackingField).Select(g => g.First()).Where(item2 => item2.EventNamek__BackingField == "Figure").ToList();
                        linevmarkets = linevmarkets.Where(item => item.EventNamek__BackingField != "Figure").ToList();
                        List<UserBets> lstFancyBets = new List<UserBets>();
                        foreach (var lineitem in linevmarkets)
                        {
                            lstFancyBets = lstUserBetAll.Where(item => item.MarketBookID == lineitem.MarketCatalogueIDk__BackingField).ToList();
                            lstAllUserBets.AddRange(lstFancyBets);
                        }
                        List<UserBets> lstFancyfigBets = new List<UserBets>();
                        foreach (var lineitem in linevmarketsfig)
                        {
                            lstFancyfigBets = lstUserBetAll.Where(item => item.MarketBookID == lineitem.MarketCatalogueIDk__BackingField).ToList();
                            lstAllUserBets.AddRange(lstFancyfigBets);
                        }
                    }
                }
                if (lstUserBets != null)
                {
                    lstAllUserBets.AddRange(lstUserBets);
                    lstAllUserBets = lstAllUserBets.OrderByDescending(item => item.ID).ToList();

                }


                return await RenderRazorViewToStringAsync("UserBetsAll", lstAllUserBets);


            }
            catch (System.Exception ex)
            {
                List<UserBets> lstUserBets = new List<UserBets>();
                return await RenderRazorViewToStringAsync("UserBetsAll", lstUserBets);
            }
        }

    }

}
