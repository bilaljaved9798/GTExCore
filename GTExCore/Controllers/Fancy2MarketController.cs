using BettingServiceReference;
using Global.API;
using GTCore.Models;
using GTExCore.Common;
using GTExCore.Models;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using UserServiceReference;

namespace Census.API.Controllers
{

    public class Fancy2MarketController : Controller
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private BettingServiceClient objBettingClient = new BettingServiceClient();
        UserServicesClient objUsersServiceCleint = new UserServicesClient();
        UserBetsUpdateUnmatcedBets objUserbets = new UserBetsUpdateUnmatcedBets();
        private readonly IRazorViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;
        private readonly IPasswordSettingsService _passwordSettingsService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public Fancy2MarketController(IRazorViewEngine viewEngine, ITempDataProvider tempDataProvider, IServiceProvider serviceProvider, IConfiguration configuration, IPasswordSettingsService passwordSettingsService, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
            _passwordSettingsService = passwordSettingsService;
        }

        List<BettingServiceReference.RunnerForIndianFancy> lstMarketBookRunnersIndianFancy = new List<BettingServiceReference.RunnerForIndianFancy>();

       
		public IActionResult LoadIndianFancyMarket(string EventID, string MarketBookID)
		{
			LoggedinUserDetail.CheckifUserLogin();

			return ViewComponent("IndianFancyMarket", new { EventID, MarketBookID });
		}


	}
}
