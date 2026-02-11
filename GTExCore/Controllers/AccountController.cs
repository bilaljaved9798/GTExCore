using BettingServiceReference;
using GTCore.Models;
using GTCore.ViewModel;
using GTExCore.HelperClass;
using GTExCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UserServiceReference;

namespace GTExCore.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        BettingServiceClient BettingServiceClient = new BettingServiceClient();
        UserServicesClient objUserServiceClient = new UserServicesClient();
        public AccountController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult LogOff(int ID)
        {
            int UserID = ID;
            objUserServiceClient.SetLoggedinStatus(UserID, false);
            LoggedinUserDetail.InsertActivityLog(UserID, "Logged Out");
            _httpContextAccessor.HttpContext.Session.SetObject("User", new UserIDandUserType());
            _httpContextAccessor.HttpContext.Session.SetObject("firsttimeload", true);
            return RedirectToAction("Login", "Account");
        }
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            var sessionUser = _httpContextAccessor.HttpContext.Session.GetObject<UserIDandUserType>("User");

            if (sessionUser == null)
            {
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }
            else
            {
                if (string.IsNullOrEmpty(sessionUser.UserName))
                {
                    ViewBag.ReturnUrl = returnUrl;
                    return View();
                }
                else
                {
                    if (string.IsNullOrEmpty(returnUrl))
                    {
                        return RedirectToAction("Index", "DashBoard");
                    }
                    else if (returnUrl.Contains("Login"))
                    {
                        return RedirectToAction("Index", "DashBoard");
                    }
                    else
                    {
                        string[] urls = returnUrl.Split('/');
                        return RedirectToAction(urls[2], urls[1]);
                    }
                }
            }
        }
            [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View(model);
            //}
            BetSlipKeys obj = new BetSlipKeys();
            var results =  objUserServiceClient.GetUserbyUsernameandPasswordNew(Crypto.Encrypt(model.Username), Crypto.Encrypt(model.Password));
            if (results != "")
            {
                var result = JsonConvert.DeserializeObject<UserIDandUserType>(results);

                if (result.UserTypeID != 1)
                {
                    if (result.Loggedin == true)
                    {
                        if (result.isBlocked == true)
                        {
                            ModelState.AddModelError("", "Account is Blocked.");
                            return View(model);
                        }
                        if (result.isDeleted == true)
                        {
                            ModelState.AddModelError("", "Account is Deleted.");
                            return View(model);
                        }
                        result.PoundRate = result.PoundRate;
                        LoggedinUserDetail.CurrentAccountBalance = Convert.ToDouble(result.AccountBalance);
                        LoggedinUserDetail.IsCom = result.IsCom;
                        LoggedinUserDetail.isFancyMarketAllowed = result.isFancyMarketAllowed;
                        LoggedinUserDetail.BetPlaceWaitandInterval.CancelBetTime = result.CancelBetTime;
                        LoggedinUserDetail.BetPlaceWaitandInterval.CompletedMatchBetPlaceWait = result.CompletedMatchBetPlaceWait;
                        LoggedinUserDetail.BetPlaceWaitandInterval.CompletedMatchTimerInterval = result.CompletedMatchTimerInterval;
                        LoggedinUserDetail.BetPlaceWaitandInterval.CricketMatchOddsBetPlaceWait = result.CricketMatchOddsBetPlaceWait;
                        LoggedinUserDetail.BetPlaceWaitandInterval.CricketMatchOddsTimerInterval = result.CricketMatchOddsTimerInterval;
                        LoggedinUserDetail.BetPlaceWaitandInterval.FancyBetPlaceWait = result.FancyBetPlaceWait;
                        LoggedinUserDetail.BetPlaceWaitandInterval.FancyTimerInterval = result.FancyTimerInterval;
                        LoggedinUserDetail.BetPlaceWaitandInterval.GrayHoundBetPlaceWait = result.GrayHoundBetPlaceWait;
                        LoggedinUserDetail.BetPlaceWaitandInterval.GrayHoundTimerInterval = result.GrayHoundTimerInterval;
                        LoggedinUserDetail.BetPlaceWaitandInterval.HorseRaceBetPlaceWait = result.HorseRaceBetPlaceWait;
                        LoggedinUserDetail.BetPlaceWaitandInterval.HorseRaceTimerInterval = result.HorseRaceTimerInterval;
                        LoggedinUserDetail.BetPlaceWaitandInterval.InningsRunsBetPlaceWait = result.InningsRunsBetPlaceWait;
                        LoggedinUserDetail.BetPlaceWaitandInterval.InningsRunsTimerInterval = result.InningsRunsTimerInterval;
                        LoggedinUserDetail.BetPlaceWaitandInterval.RaceMinutesBeforeStart = result.RaceMinutesBeforeStart;
                        LoggedinUserDetail.BetPlaceWaitandInterval.SoccerBetPlaceWait = result.SoccerBetPlaceWait;
                        LoggedinUserDetail.BetPlaceWaitandInterval.SoccerTimerInterval = result.SoccerTimerInterval;
                        LoggedinUserDetail.BetPlaceWaitandInterval.TennisBetPlaceWait = result.TennisBetPlaceWait;
                        LoggedinUserDetail.BetPlaceWaitandInterval.TennisTimerInterval = result.TennisTimerInterval;
                        LoggedinUserDetail.BetPlaceWaitandInterval.TiedMatchBetPlaceWait = result.TiedMatchBetPlaceWait;
                        LoggedinUserDetail.BetPlaceWaitandInterval.TiedMatchTimerInterval = result.TiedMatchBetPlaceWait;
                        LoggedinUserDetail.BetPlaceWaitandInterval.WinnerBetPlaceWait = result.WinnerBetPlaceWait;
                        LoggedinUserDetail.BetPlaceWaitandInterval.WinnerTimerInterval = result.WinnerTimerInterval;

                        obj.MutipleBtn1 = result.MutipleBtn1;
                        obj.MutipleBtn2 = result.MutipleBtn2;
                        obj.MutipleBtn3 = result.MutipleBtn3;
                        obj.MutipleBtn4 = result.MutipleBtn4;
                        obj.MutipleBtn5 = result.MutipleBtn5;
                        obj.MutipleBtn6 = result.MutipleBtn6;
                        obj.MutipleBtn7 = result.MutipleBtn7;
                        obj.MutipleBtn8 = result.MutipleBtn8;
                        obj.MutipleBtn9 = result.MutipleBtn9;
                        obj.MutipleBtn10 = result.MutipleBtn10;
                        obj.MutipleBtn11 = result.MutipleBtn11;
                        obj.MutipleBtn12 = result.MutipleBtn12;

                        obj.SimpleBtn1 = result.SimpleBtn1;
                        obj.SimpleBtn2 = result.SimpleBtn2;
                        obj.SimpleBtn3 = result.SimpleBtn3;
                        obj.SimpleBtn4 = result.SimpleBtn4;
                        obj.SimpleBtn5 = result.SimpleBtn5;
                        obj.SimpleBtn6 = result.SimpleBtn6;
                        obj.SimpleBtn7 = result.SimpleBtn7;
                        obj.SimpleBtn8 = result.SimpleBtn8;
                        obj.SimpleBtn9 = result.SimpleBtn9;
                        obj.SimpleBtn10 = result.SimpleBtn10;
                        obj.SimpleBtn11 = result.SimpleBtn11;
                        obj.SimpleBtn12 = result.SimpleBtn12;
                        obj.UserID = result.ID;

                        _httpContextAccessor.HttpContext.Session.SetObject("BetSlipKeys", obj);
                        _httpContextAccessor.HttpContext.Session.SetObject("User", result);
                        _httpContextAccessor.HttpContext.Session.SetObject("Runnserdata", null);
                        _httpContextAccessor.HttpContext.Session.SetObject("firsttimeload", true);
                        return RedirectToAction("Index", "DashBoard");
                    }
                    else
                    {
                        LoggedinUserDetail.IsCom = result.IsCom;
                        LoggedinUserDetail.isFancyMarketAllowed = result.isFancyMarketAllowed;
                        LoggedinUserDetail.CurrentAccountBalance = Convert.ToDouble(result.AccountBalance);
                        LoggedinUserDetail.BetPlaceWaitandInterval.CancelBetTime = result.CancelBetTime;
                        LoggedinUserDetail.BetPlaceWaitandInterval.CompletedMatchBetPlaceWait = result.CompletedMatchBetPlaceWait;
                        LoggedinUserDetail.BetPlaceWaitandInterval.CompletedMatchTimerInterval = result.CompletedMatchTimerInterval;
                        LoggedinUserDetail.BetPlaceWaitandInterval.CricketMatchOddsBetPlaceWait = result.CricketMatchOddsBetPlaceWait;
                        LoggedinUserDetail.BetPlaceWaitandInterval.CricketMatchOddsTimerInterval = result.CricketMatchOddsTimerInterval;
                        LoggedinUserDetail.BetPlaceWaitandInterval.FancyBetPlaceWait = result.FancyBetPlaceWait;
                        LoggedinUserDetail.BetPlaceWaitandInterval.FancyTimerInterval = result.FancyTimerInterval;
                        LoggedinUserDetail.BetPlaceWaitandInterval.GrayHoundBetPlaceWait = result.GrayHoundBetPlaceWait;
                        LoggedinUserDetail.BetPlaceWaitandInterval.GrayHoundTimerInterval = result.GrayHoundTimerInterval;
                        LoggedinUserDetail.BetPlaceWaitandInterval.HorseRaceBetPlaceWait = result.HorseRaceBetPlaceWait;
                        LoggedinUserDetail.BetPlaceWaitandInterval.HorseRaceTimerInterval = result.HorseRaceTimerInterval;
                        LoggedinUserDetail.BetPlaceWaitandInterval.InningsRunsBetPlaceWait = result.InningsRunsBetPlaceWait;
                        LoggedinUserDetail.BetPlaceWaitandInterval.InningsRunsTimerInterval = result.InningsRunsTimerInterval;
                        LoggedinUserDetail.BetPlaceWaitandInterval.RaceMinutesBeforeStart = result.RaceMinutesBeforeStart;
                        LoggedinUserDetail.BetPlaceWaitandInterval.SoccerBetPlaceWait = result.SoccerBetPlaceWait;
                        LoggedinUserDetail.BetPlaceWaitandInterval.SoccerTimerInterval = result.SoccerTimerInterval;
                        LoggedinUserDetail.BetPlaceWaitandInterval.TennisBetPlaceWait = result.TennisBetPlaceWait;
                        LoggedinUserDetail.BetPlaceWaitandInterval.TennisTimerInterval = result.TennisTimerInterval;
                        LoggedinUserDetail.BetPlaceWaitandInterval.TiedMatchBetPlaceWait = result.TiedMatchBetPlaceWait;
                        LoggedinUserDetail.BetPlaceWaitandInterval.TiedMatchTimerInterval = result.TiedMatchBetPlaceWait;
                        LoggedinUserDetail.BetPlaceWaitandInterval.WinnerBetPlaceWait = result.WinnerBetPlaceWait;
                        LoggedinUserDetail.BetPlaceWaitandInterval.WinnerTimerInterval = result.WinnerTimerInterval;
                        obj.MutipleBtn1 = result.MutipleBtn1;
                        obj.MutipleBtn2 = result.MutipleBtn2;
                        obj.MutipleBtn3 = result.MutipleBtn3;
                        obj.MutipleBtn4 = result.MutipleBtn4;
                        obj.MutipleBtn5 = result.MutipleBtn5;
                        obj.MutipleBtn6 = result.MutipleBtn6;
                        obj.MutipleBtn7 = result.MutipleBtn7;
                        obj.MutipleBtn8 = result.MutipleBtn8;
                        obj.MutipleBtn9 = result.MutipleBtn9;
                        obj.MutipleBtn10 = result.MutipleBtn10;
                        obj.MutipleBtn11 = result.MutipleBtn11;
                        obj.MutipleBtn12 = result.MutipleBtn12;

                        obj.SimpleBtn1 = result.SimpleBtn1;
                        obj.SimpleBtn2 = result.SimpleBtn2;
                        obj.SimpleBtn3 = result.SimpleBtn3;
                        obj.SimpleBtn4 = result.SimpleBtn4;
                        obj.SimpleBtn5 = result.SimpleBtn5;
                        obj.SimpleBtn6 = result.SimpleBtn6;
                        obj.SimpleBtn7 = result.SimpleBtn7;
                        obj.SimpleBtn8 = result.SimpleBtn8;
                        obj.SimpleBtn9 = result.SimpleBtn9;
                        obj.SimpleBtn10 = result.SimpleBtn10;
                        obj.SimpleBtn11 = result.SimpleBtn11;
                        obj.SimpleBtn12 = result.SimpleBtn12;
                        obj.UserID = result.ID;
          
                        ViewBag.color = "Red";                       
                        result.PoundRate = result.PoundRate;
                        _httpContextAccessor.HttpContext.Session.SetObject("BetSlipKeys", obj);
                        _httpContextAccessor.HttpContext.Session.SetObject("User", result);
                        _httpContextAccessor.HttpContext.Session.SetObject("Runnserdata", null);
                        _httpContextAccessor.HttpContext.Session.SetObject("firsttimeload", true);
                        return RedirectToAction("Index", "DashBoard");
                    }
                }
                else
                {
                    LoggedinUserDetail.IsCom = result.IsCom;
                    LoggedinUserDetail.isFancyMarketAllowed = result.isFancyMarketAllowed;
                    LoggedinUserDetail.CurrentAccountBalance = Convert.ToDouble(result.AccountBalance);
                    LoggedinUserDetail.BetPlaceWaitandInterval.CancelBetTime = result.CancelBetTime;
                    LoggedinUserDetail.BetPlaceWaitandInterval.CompletedMatchBetPlaceWait = result.CompletedMatchBetPlaceWait;
                    LoggedinUserDetail.BetPlaceWaitandInterval.CompletedMatchTimerInterval = result.CompletedMatchTimerInterval;
                    LoggedinUserDetail.BetPlaceWaitandInterval.CricketMatchOddsBetPlaceWait = result.CricketMatchOddsBetPlaceWait;
                    LoggedinUserDetail.BetPlaceWaitandInterval.CricketMatchOddsTimerInterval = result.CricketMatchOddsTimerInterval;
                    LoggedinUserDetail.BetPlaceWaitandInterval.FancyBetPlaceWait = result.FancyBetPlaceWait;
                    LoggedinUserDetail.BetPlaceWaitandInterval.FancyTimerInterval = result.FancyTimerInterval;
                    LoggedinUserDetail.BetPlaceWaitandInterval.GrayHoundBetPlaceWait = result.GrayHoundBetPlaceWait;
                    LoggedinUserDetail.BetPlaceWaitandInterval.GrayHoundTimerInterval = result.GrayHoundTimerInterval;
                    LoggedinUserDetail.BetPlaceWaitandInterval.HorseRaceBetPlaceWait = result.HorseRaceBetPlaceWait;
                    LoggedinUserDetail.BetPlaceWaitandInterval.HorseRaceTimerInterval = result.HorseRaceTimerInterval;
                    LoggedinUserDetail.BetPlaceWaitandInterval.InningsRunsBetPlaceWait = result.InningsRunsBetPlaceWait;
                    LoggedinUserDetail.BetPlaceWaitandInterval.InningsRunsTimerInterval = result.InningsRunsTimerInterval;
                    LoggedinUserDetail.BetPlaceWaitandInterval.RaceMinutesBeforeStart = result.RaceMinutesBeforeStart;
                    LoggedinUserDetail.BetPlaceWaitandInterval.SoccerBetPlaceWait = result.SoccerBetPlaceWait;
                    LoggedinUserDetail.BetPlaceWaitandInterval.SoccerTimerInterval = result.SoccerTimerInterval;
                    LoggedinUserDetail.BetPlaceWaitandInterval.TennisBetPlaceWait = result.TennisBetPlaceWait;
                    LoggedinUserDetail.BetPlaceWaitandInterval.TennisTimerInterval = result.TennisTimerInterval;
                    LoggedinUserDetail.BetPlaceWaitandInterval.TiedMatchBetPlaceWait = result.TiedMatchBetPlaceWait;
                    LoggedinUserDetail.BetPlaceWaitandInterval.TiedMatchTimerInterval = result.TiedMatchBetPlaceWait;
                    LoggedinUserDetail.BetPlaceWaitandInterval.WinnerBetPlaceWait = result.WinnerBetPlaceWait;
                    LoggedinUserDetail.BetPlaceWaitandInterval.WinnerTimerInterval = result.WinnerTimerInterval;
                    obj.MutipleBtn1 = result.MutipleBtn1;
                    obj.MutipleBtn2 = result.MutipleBtn2;
                    obj.MutipleBtn3 = result.MutipleBtn3;
                    obj.MutipleBtn4 = result.MutipleBtn4;
                    obj.MutipleBtn5 = result.MutipleBtn5;
                    obj.MutipleBtn6 = result.MutipleBtn6;
                    obj.MutipleBtn7 = result.MutipleBtn7;
                    obj.MutipleBtn8 = result.MutipleBtn8;
                    obj.MutipleBtn9 = result.MutipleBtn9;
                    obj.MutipleBtn10 = result.MutipleBtn10;
                    obj.MutipleBtn11 = result.MutipleBtn11;
                    obj.MutipleBtn12 = result.MutipleBtn12;

                    obj.SimpleBtn1 = result.SimpleBtn1;
                    obj.SimpleBtn2 = result.SimpleBtn2;
                    obj.SimpleBtn3 = result.SimpleBtn3;
                    obj.SimpleBtn4 = result.SimpleBtn4;
                    obj.SimpleBtn5 = result.SimpleBtn5;
                    obj.SimpleBtn6 = result.SimpleBtn6;
                    obj.SimpleBtn7 = result.SimpleBtn7;
                    obj.SimpleBtn8 = result.SimpleBtn8;
                    obj.SimpleBtn9 = result.SimpleBtn9;
                    obj.SimpleBtn10 = result.SimpleBtn10;
                    obj.SimpleBtn11 = result.SimpleBtn11;
                    obj.SimpleBtn12 = result.SimpleBtn12;
                    obj.UserID = result.ID;
                    
                    result.PoundRate = result.PoundRate;
                   
                    _httpContextAccessor.HttpContext.Session.SetObject("BetSlipKeys", obj);
                    _httpContextAccessor.HttpContext.Session.SetObject("User", result);
                    _httpContextAccessor.HttpContext.Session.SetObject("Runnserdata", null);
                    _httpContextAccessor.HttpContext.Session.SetObject("firsttimeload", true);

                    return RedirectToAction("Index", "DashBoard");
                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }
        }
    }
}
