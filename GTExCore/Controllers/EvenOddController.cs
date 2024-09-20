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
		UserBetsUpdateUnmatcedBets objUserbets = new UserBetsUpdateUnmatcedBets();
		private readonly IPasswordSettingsService _passwordSettingsService;
		private readonly IHttpContextAccessor _httpContextAccessor;
		public EvenOddController( IPasswordSettingsService passwordSettingsService, IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
			_passwordSettingsService = passwordSettingsService;
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


        public BettingServiceReference.MarketBook ConvertJsontoMarketObjectBFNewSource(string SelectionID, string marketid, string MarketCatalogueName, bool BettingAllowed)
		{

			if (1 == 1)
			{
				var marketbook = new BettingServiceReference.MarketBook();
				marketbook.MarketId = marketid;
				marketbook.SheetName = "";
				marketbook.MarketBookName = MarketCatalogueName;
				marketbook.MainSportsname = "Fancy";
				marketbook.MarketStatusstr = "In Play";
				marketbook.BettingAllowed = BettingAllowed;
				marketbook.BettingAllowedOverAll = true;
				List<BettingServiceReference.Runner> lstRunners = new List<BettingServiceReference.Runner>();
				var runner = new BettingServiceReference.Runner();
				runner.Handicap = 0;
				runner.SelectionId = SelectionID;
				try
				{
					BettingServiceReference.MarketBookForindianFancy currentmarketsfancyPL = new BettingServiceReference.MarketBookForindianFancy();

					if (LoggedinUserDetail.GetUserTypeID() == 3)
					{
						List<UserBets> lstUserBets = JsonConvert.DeserializeObject<List<UserBets>>(objUsersServiceCleint.GetUserbetsbyUserID(LoggedinUserDetail.GetUserID(), _passwordSettingsService.PasswordForValidate));
						lstUserBets = lstUserBets.Where(item => item.MarketBookID == marketbook.MarketId).ToList();
						if (lstUserBets.Count != 0)
						{
							currentmarketsfancyPL = objUserbets.GetBookPositioninKJ(marketid, new List<UserBetsForAdmin>(), new List<UserBetsforSuper>(), new List<UserBetsforSamiadmin>(), new List<UserBetsforAgent>(), lstUserBets);
						}
					}
					else
					{
						if (LoggedinUserDetail.GetUserTypeID() == 2)
						{
							List<UserBetsforAgent> lstUserBets = JsonConvert.DeserializeObject<List<UserBetsforAgent>>(objUsersServiceCleint.GetUserbetsbyUserIDandAgentID(LoggedinUserDetail.GetUserID(), _passwordSettingsService.PasswordForValidate));
							lstUserBets = lstUserBets.Where(item => item.MarketBookID == marketbook.MarketId).ToList();
							if (lstUserBets.Count != 0)
							{
								currentmarketsfancyPL = objUserbets.GetBookPositioninKJ(marketid, new List<UserBetsForAdmin>(), new List<UserBetsforSuper>(), new List<UserBetsforSamiadmin>(), lstUserBets, new List<UserBets>());
							}
						}
						else
						{
							if (LoggedinUserDetail.GetUserTypeID() == 8)
							{
								List<UserBetsforSuper> lstUserBets = JsonConvert.DeserializeObject<List<UserBetsforSuper>>(objUsersServiceCleint.GetUserBetsbySuperID(LoggedinUserDetail.GetUserID(), _passwordSettingsService.PasswordForValidate));
								lstUserBets = lstUserBets.Where(item => item.MarketBookID == marketbook.MarketId).ToList();
								if (lstUserBets.Count != 0)
								{
									currentmarketsfancyPL = objUserbets.GetBookPositioninKJ(marketid, new List<UserBetsForAdmin>(), lstUserBets, new List<UserBetsforSamiadmin>(), new List<UserBetsforAgent>(), new List<UserBets>());
								}
							}
							if (LoggedinUserDetail.GetUserTypeID() == 9)
							{
								List<UserBetsforSamiadmin> lstUserBets = JsonConvert.DeserializeObject<List<UserBetsforSamiadmin>>(objUsersServiceCleint.GetUserBetsbySamiAdmin(LoggedinUserDetail.GetUserID(), _passwordSettingsService.PasswordForValidate));
								lstUserBets = lstUserBets.Where(item => item.MarketBookID == marketbook.MarketId).ToList();
								if (lstUserBets.Count != 0)
								{
									currentmarketsfancyPL = objUserbets.GetBookPositioninKJ(marketid, new List<UserBetsForAdmin>(), new List<UserBetsforSuper>(), lstUserBets, new List<UserBetsforAgent>(), new List<UserBets>());
								}
							}
							else
							{
								List<UserBetsForAdmin> lstUserBets = JsonConvert.DeserializeObject<List<UserBetsForAdmin>>(objUsersServiceCleint.GetUserbetsbyUserID(LoggedinUserDetail.GetUserID(), _passwordSettingsService.PasswordForValidate));
								lstUserBets = lstUserBets.Where(item => item.MarketBookID == marketbook.MarketId).ToList();
								if (lstUserBets.Count != 0)
								{
									currentmarketsfancyPL = objUserbets.GetBookPositioninKJ(marketid, lstUserBets, new List<UserBetsforSuper>(), new List<UserBetsforSamiadmin>(), new List<UserBetsforAgent>(), new List<UserBets>());
								}
							}
						}
					}
					double TotalProfit = 0;
					double TotalLoss = 0;
					if (currentmarketsfancyPL.RunnersForindianFancy != null)
					{
						TotalProfit = Convert.ToDouble(currentmarketsfancyPL.RunnersForindianFancy.Max(t => t.ProfitandLoss));
						TotalLoss = Convert.ToDouble(currentmarketsfancyPL.RunnersForindianFancy.Min(t => t.ProfitandLoss));
						runner.ProfitandLoss = Convert.ToInt64(TotalProfit);
						runner.Loss = Convert.ToInt64(TotalLoss);
					}
					else
					{
						runner.ProfitandLoss = Convert.ToInt64(TotalProfit);
						runner.Loss = Convert.ToInt64(TotalLoss);
					}
				}
				catch (System.Exception ex)
				{

				}

				var lstpricelist = new List<BettingServiceReference.PriceSize>();

				var pricesize = new BettingServiceReference.PriceSize();
				pricesize.OrignalSize = Convert.ToDouble(0);
				pricesize.Size = 98;
				pricesize.Price = 1;
				lstpricelist.Add(pricesize);

				runner.ExchangePrices = new BettingServiceReference.ExchangePrices();
				runner.ExchangePrices.AvailableToBack = lstpricelist;
				lstpricelist = new List<BettingServiceReference.PriceSize>();

				var pricesize1 = new BettingServiceReference.PriceSize();
				pricesize1.OrignalSize = Convert.ToDouble(0);
				pricesize1.Size = 102;
				pricesize1.Price = 1;
				lstpricelist.Add(pricesize1);

				runner.ExchangePrices.AvailableToLay = new List<BettingServiceReference.PriceSize>();
				runner.ExchangePrices.AvailableToLay = lstpricelist;
				lstRunners.Add(runner);
				marketbook.Runners = new List<BettingServiceReference.Runner>(lstRunners);

				return marketbook;
			}
			else
			{
				return new BettingServiceReference.MarketBook();
			}
		}

	}
}
