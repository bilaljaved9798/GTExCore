using Global.API;
using GTExCore.Common;
using Microsoft.AspNetCore.Mvc;
using UserServiceReference;

namespace GTExCore.Controllers
{
	public class FigureController : Controller
	{
		UserServicesClient objUsersServiceCleint = new UserServicesClient();
		BettingServiceReference.MarketBook MarketBook = new BettingServiceReference.MarketBook();
		UserBetsUpdateUnmatcedBets _objUserbets;
		private readonly IPasswordSettingsService _passwordSettingsService;
		private readonly IHttpContextAccessor _httpContextAccessor;
		public FigureController(IPasswordSettingsService passwordSettingsService, IHttpContextAccessor httpContextAccessor, UserBetsUpdateUnmatcedBets objUserbets)
		{
			_httpContextAccessor = httpContextAccessor;
			_objUserbets = objUserbets;
			_passwordSettingsService = passwordSettingsService;
		}
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult GetMarketFigure(string EventID)
		{
			//LoggedinUserDetail.CheckifUserLogin();
			if (string.IsNullOrEmpty(EventID))
			{
				return BadRequest("Invalid event ID");
			}
			return ViewComponent("FigureMarket", new { EventID });
		}
	}
}
