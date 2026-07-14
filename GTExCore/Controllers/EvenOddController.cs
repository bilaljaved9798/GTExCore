using BettingServiceReference;
using Global.API;
using GTCore.Models;
using GTExCore.Common;
using GTExCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using UserServiceReference;

namespace GTExCore.Controllers
{
    public class EvenOddController : Controller
    {
		UserServicesClient objUsersServiceCleint = new UserServicesClient();
        BettingServiceReference.MarketBook MarketBook = new BettingServiceReference.MarketBook();
		UserBetsUpdateUnmatcedBets _objUserbets;
		private readonly IPasswordSettingsService _passwordSettingsService;
		private readonly IHttpContextAccessor _httpContextAccessor;
		public EvenOddController( IPasswordSettingsService passwordSettingsService, UserBetsUpdateUnmatcedBets objUserbets, IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
			_passwordSettingsService = passwordSettingsService;
                _objUserbets = objUserbets;
        }
		public IActionResult Index()
        {
            return View();
        }


        public IActionResult LoadMarketKJ(string EventID)
        {
            //LoggedinUserDetail.CheckifUserLogin();
            if (string.IsNullOrEmpty(EventID))
            {
                return BadRequest("Invalid event ID");
            }
            return ViewComponent("EvenOddMarket", new { EventID });
        }
	}
}
