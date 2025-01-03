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
    }
}
