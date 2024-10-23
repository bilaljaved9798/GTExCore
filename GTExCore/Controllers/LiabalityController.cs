using BettingServiceReference;
using Global.API;
using GTCore.Models;
using GTCore.ViewModel;
using GTExCore.Common;
using GTExCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using UserServiceReference;

namespace GTExCore.Controllers
{
    public class LiabalityController : Controller
    {
        AccessRightsbyUserType objAccessrightsbyUserType;
        BettingServiceClient BettingServiceClient = new BettingServiceClient();
        UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();
        UserServicesClient objUsersServiceCleint = new UserServicesClient();
        private readonly IRazorViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPasswordSettingsService _passwordSettingsService;

        public LiabalityController(IRazorViewEngine viewEngine, ITempDataProvider tempDataProvider, IServiceProvider serviceProvider, IPasswordSettingsService passwordSettingsService, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
            _passwordSettingsService = passwordSettingsService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public PartialViewResult LoadLiabalitybyMarket()
        {
            objAccessrightsbyUserType = new AccessRightsbyUserType();
            try
            {
                List<LiabalitybyMarket> lstLibalitybymrakets = new List<LiabalitybyMarket>();
                if (LoggedinUserDetail.GetUserTypeID() == 3)
                {
                    var lstUserBets = JsonConvert.DeserializeObject<List<Models.UserBets>>(objUsersServiceCleint.GetUserbetsbyUserID(LoggedinUserDetail.GetUserID(), _passwordSettingsService.PasswordForValidate));
                    lstLibalitybymrakets = objUserBets.GetLiabalityofCurrentUserbyMarkets(LoggedinUserDetail.GetUserID(), lstUserBets);
                    decimal liab = Convert.ToDecimal(lstLibalitybymrakets.Sum(item => Convert.ToDecimal((item.Liabality))));
                    objAccessrightsbyUserType.CurrentLiabality = "Liab: " + liab.ToString("F2");
                    return PartialView("LiabalitybyMarket", lstLibalitybymrakets);
                }
                else
                {
                    if (LoggedinUserDetail.GetUserTypeID() == 2)
                    {
                        string userbets = objUsersServiceCleint.GetUserBetsbyAgentID(LoggedinUserDetail.GetUserID(), _passwordSettingsService.PasswordForValidate);
                        var lstUserBet = JsonConvert.DeserializeObject<List<UserBetsforAgent>>(userbets);
                        UserBetsUpdateUnmatcedBets objUserbet = new UserBetsUpdateUnmatcedBets();
                        lstLibalitybymrakets = objUserBets.GetLiabalityofCurrentAgentbyMarkets(lstUserBet);
                        decimal liab = Convert.ToDecimal(lstLibalitybymrakets.Sum(item => Convert.ToDecimal((item.Liabality))));
                        objAccessrightsbyUserType.CurrentLiabality = "Liab: " + liab.ToString("F2");
                        return PartialView("LiabalitybyMarket", lstLibalitybymrakets);
                    }
                    else
                    {
                        if (LoggedinUserDetail.GetUserTypeID() == 1)
                        {
                            string userbets = objUsersServiceCleint.GetUserbetsForAdmin(_passwordSettingsService.PasswordForValidate);
                            List<UserBetsForAdmin> lstUserBet = JsonConvert.DeserializeObject<List<UserBetsForAdmin>>(userbets);
                            UserBetsUpdateUnmatcedBets objUserbet = new UserBetsUpdateUnmatcedBets();
                            lstLibalitybymrakets = objUserBets.GetLiabalityofAdminbyMarkets(lstUserBet);
                            decimal liab = Convert.ToDecimal(lstLibalitybymrakets.Sum(item => Convert.ToDecimal((item.Liabality))));
                            objAccessrightsbyUserType.CurrentLiabality = "Liab: " + liab.ToString("F2");
                            return PartialView("LiabalitybyMarket", lstLibalitybymrakets);
                        }
                        else
                        {
                            if (LoggedinUserDetail.GetUserTypeID() == 8)
                            {
                                string userbets = objUsersServiceCleint.GetUserBetsbySuperID(LoggedinUserDetail.GetUserID(), _passwordSettingsService.PasswordForValidate);
                                List<UserBetsforSuper> lstUserBet = JsonConvert.DeserializeObject<List<UserBetsforSuper>>(userbets);
                                UserBetsUpdateUnmatcedBets objUserbet = new UserBetsUpdateUnmatcedBets();
                                lstLibalitybymrakets = objUserBets.GetLiabalityofSuperbyMarkets(lstUserBet);
                                decimal liab = Convert.ToDecimal(lstLibalitybymrakets.Sum(item => Convert.ToDecimal((item.Liabality))));
                                objAccessrightsbyUserType.CurrentLiabality = "Liab: " + liab.ToString("F2");
                                return PartialView("LiabalitybyMarket", lstLibalitybymrakets);
                            }
                            else
                            {
                                return PartialView("LiabalitybyMarket", lstLibalitybymrakets);
                            }
                        }

                    }
                }
            }
            catch (System.Exception ex)
            {
                List<LiabalitybyMarket> lstLibalitybymrakets = new List<LiabalitybyMarket>();
                return PartialView("LiabalitybyMarket", lstLibalitybymrakets);
            }
        }


        public ActionResult LoadLiabalitybyMarketall()
        {
            objAccessrightsbyUserType = new AccessRightsbyUserType();
            UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();
            try
            {
                List<LiabalitybyMarket> lstLibalitybymrakets = new List<LiabalitybyMarket>();
                if (LoggedinUserDetail.GetUserTypeID() == 3)
                {
                    var lstUserBets = JsonConvert.DeserializeObject<List<Models.UserBets>>(objUsersServiceCleint.GetUserbetsbyUserID(LoggedinUserDetail.GetUserID(), _passwordSettingsService.PasswordForValidate));
                    UserBetsUpdateUnmatcedBets objUserbet = new UserBetsUpdateUnmatcedBets();
                    lstLibalitybymrakets = objUserBets.GetLiabalityofCurrentUserbyMarkets(LoggedinUserDetail.GetUserID(), lstUserBets);
                    decimal liab = Convert.ToDecimal(lstLibalitybymrakets.Sum(item => Convert.ToDecimal((item.Liabality))));
                    objAccessrightsbyUserType.CurrentLiabality = "Liab: " + liab.ToString("F2");
                    return View(objAccessrightsbyUserType);

                }
                else
                {
                    if (LoggedinUserDetail.GetUserTypeID() == 2)
                    {

                        string userbets = objUsersServiceCleint.GetUserBetsbyAgentID(LoggedinUserDetail.GetUserID(), _passwordSettingsService.PasswordForValidate);
                        var lstUserBet = JsonConvert.DeserializeObject<List<UserBetsforAgent>>(userbets);
                        UserBetsUpdateUnmatcedBets objUserbet = new UserBetsUpdateUnmatcedBets();
                        lstLibalitybymrakets = objUserBets.GetLiabalityofCurrentAgentbyMarkets(lstUserBet);
                        decimal liab = Convert.ToDecimal(lstLibalitybymrakets.Sum(item => Convert.ToDecimal((item.Liabality))));
                        objAccessrightsbyUserType.CurrentLiabality = "Liab: " + liab.ToString("F2");
                        return View(objAccessrightsbyUserType);
                    }
                    else
                    {
                        if (LoggedinUserDetail.GetUserTypeID() == 1)
                        {
                            string userbets = objUsersServiceCleint.GetUserbetsForAdmin(_passwordSettingsService.PasswordForValidate);
                            List<UserBetsForAdmin> lstUserBet = JsonConvert.DeserializeObject<List<UserBetsForAdmin>>(userbets);
                            UserBetsUpdateUnmatcedBets objUserbet = new UserBetsUpdateUnmatcedBets();
                            lstLibalitybymrakets = objUserBets.GetLiabalityofAdminbyMarkets(lstUserBet);
                            decimal liab = Convert.ToDecimal(lstLibalitybymrakets.Sum(item => Convert.ToDecimal((item.Liabality))));
                            objAccessrightsbyUserType.CurrentLiabality = "Liab: " + liab.ToString("F2");

                            return View(objAccessrightsbyUserType);
                        }
                        else
                        {
                            if (LoggedinUserDetail.GetUserTypeID() == 8)
                            {
                                string userbets = objUsersServiceCleint.GetUserBetsbySuperID(LoggedinUserDetail.GetUserID(), _passwordSettingsService.PasswordForValidate);
                                List<UserBetsforSuper> lstUserBet = JsonConvert.DeserializeObject<List<UserBetsforSuper>>(userbets);
                                UserBetsUpdateUnmatcedBets objUserbet = new UserBetsUpdateUnmatcedBets();
                                lstLibalitybymrakets = objUserBets.GetLiabalityofSuperbyMarkets(lstUserBet);
                                decimal liab = Convert.ToDecimal(lstLibalitybymrakets.Sum(item => Convert.ToDecimal((item.Liabality))));
                                objAccessrightsbyUserType.CurrentLiabality = "Liab: " + liab.ToString("F2");

                                return View(objAccessrightsbyUserType);
                            }
                            else
                            {
                                return View(objAccessrightsbyUserType);
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {

                return View(objAccessrightsbyUserType);
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
    }
}
