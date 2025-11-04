using BettingServiceReference;
using Global.API;
using GTCore.Models;
using GTExCore.Common;
using GTExCore.HelperClass;
using GTExCore.Models;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.SignalR;
using UserServiceReference;

namespace GTExCore.Controllers
{
    public class AccountsController : Controller
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

        public AccountsController(IRazorViewEngine viewEngine, ITempDataProvider tempDataProvider, IServiceProvider serviceProvider, IConfiguration configuration, IPasswordSettingsService passwordSettingsService, IHubContext<UserBetsHub> hubContext, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
            _passwordSettingsService = passwordSettingsService;
            _hubContext = hubContext;
        }
        public IActionResult Index()
        {
            return View();
        }
		public ActionResult pl()
		{
			return View();
		}
        public ActionResult MyBets()
        {
            return View();
        }
        public ActionResult Stack()
        {
            var context = _httpContextAccessor.HttpContext;
            BetSlipKeys obj = context.Session.GetObject<BetSlipKeys>("BetSlipKeys");         
            return View(obj);
        }
        public ActionResult UserActivites()
        {
            return View();
        }
        public string UpdateBetSlipKeys(BetSlipKeys betSlipKeys)
        {
            // Null check for betSlipKeys to avoid runtime errors
            if (betSlipKeys == null)
            {
                return "False";
            }

            // Add "+" prefix to specific buttons dynamically
            //betSlipKeys.SimpleBtn5 = AddPlusPrefix(betSlipKeys.SimpleBtn5);
            //betSlipKeys.SimpleBtn6 = AddPlusPrefix(betSlipKeys.SimpleBtn6);
            //betSlipKeys.SimpleBtn7 = AddPlusPrefix(betSlipKeys.SimpleBtn7);
            //betSlipKeys.SimpleBtn8 = AddPlusPrefix(betSlipKeys.SimpleBtn8);

            // Check if the user is logged in
            if (LoggedinUserDetail.GetUserID() > 0)
            {
                objUsersServiceCleint.UpdateBetSlipKeys(
                    LoggedinUserDetail.GetUserID(),
                    betSlipKeys.SimpleBtn1,
                    betSlipKeys.SimpleBtn2,
                    betSlipKeys.SimpleBtn3,
                    betSlipKeys.SimpleBtn4,
                    betSlipKeys.SimpleBtn5,
                    betSlipKeys.SimpleBtn6,
                    betSlipKeys.SimpleBtn7,
                    betSlipKeys.SimpleBtn8,
                    betSlipKeys.SimpleBtn9,
                    betSlipKeys.SimpleBtn10,
                    betSlipKeys.SimpleBtn11,
                    betSlipKeys.SimpleBtn12,
                    betSlipKeys.MutipleBtn1,
                    betSlipKeys.MutipleBtn2,
                    betSlipKeys.MutipleBtn3,
                    betSlipKeys.MutipleBtn4,
                    betSlipKeys.MutipleBtn5,
                    betSlipKeys.MutipleBtn6,
                    betSlipKeys.MutipleBtn7,
                    betSlipKeys.MutipleBtn8,
                    betSlipKeys.MutipleBtn9,
                    betSlipKeys.MutipleBtn10,
                    betSlipKeys.MutipleBtn11,
                    betSlipKeys.MutipleBtn12
                );
                _httpContextAccessor.HttpContext.Session.SetObject("BetSlipKeys", betSlipKeys);
                return "True";
            }
            else
            {
                return "False";
            }
        }      
        private string AddPlusPrefix(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return value;
            }
            return value.StartsWith("+") ? value : $"+{value}";
        }
    }
}
