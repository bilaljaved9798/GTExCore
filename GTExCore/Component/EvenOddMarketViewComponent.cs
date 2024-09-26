using BettingServiceReference;
using Global.API;
using GTCore.Models;
using GTExCore.Common;
using GTExCore.HelperClass;
using GTExCore.Models;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UserServiceReference;

namespace GTExCore.Component
{
    public class EvenOddMarketViewComponent : ViewComponent

    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		private BettingServiceClient objBettingClient = new BettingServiceClient();
		UserServicesClient objUsersServiceCleint = new UserServicesClient();
		UserBetsUpdateUnmatcedBets objUserbets = new UserBetsUpdateUnmatcedBets();
		private readonly IPasswordSettingsService _passwordSettingsService;
		private readonly IHttpContextAccessor _httpContextAccessor;
		public EvenOddMarketViewComponent(IPasswordSettingsService passwordSettingsService, IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
			_passwordSettingsService = passwordSettingsService;
		}

		BettingServiceReference.MarketBook MarketBook = new BettingServiceReference.MarketBook();
		public async Task<IViewComponentResult> InvokeAsync(string EventID, string MarketBookID)
		{
			var model = await LoadMarketEvenOdd(EventID);
			if (model == null || !model.Any()) 
			{
				return Content("");
			}
			return View(model);
		}

		public async Task<List<BettingServiceReference.MarketBook>> LoadMarketEvenOdd(string eventID)
		{
			string jsonResult = await objUsersServiceCleint.KJMarketsbyEventIDAsync(eventID, LoggedinUserDetail.GetUserID());
			var KJvmarket = JsonConvert.DeserializeObject<List<EvenOddMarkets>>(jsonResult);                   
            if (KJvmarket.Count > 0)
			{
				List<BettingServiceReference.SP_UserMarket_GetDistinctKJMarketsbyEventID_Result> convertedList = KJvmarket
										 .Select(l => new BettingServiceReference.SP_UserMarket_GetDistinctKJMarketsbyEventID_Result
										 {
											 MarketCatalogueIDk__BackingField = l.MarketCatalogueID,
											 EventIDk__BackingField = l.EventID,
											 CompetitionIDk__BackingField = l.CompetitionID,
											 isOpenedbyUserk__BackingField = l.isOpenedbyUser,
											 MarketCatalogueNamek__BackingField = l.MarketCatalogueName,
											 BettingAllowedk__BackingField = l.BettingAllowed,
											 CompetitionNamek__BackingField = l.CompetitionName,
											 EventNamek__BackingField = l.EventName
										 })
										 .ToList();
                _httpContextAccessor.HttpContext.Session.SetObject("Allmarkets", convertedList);
                MarketBook.KJMarkets = convertedList.Where(item => item.EventNamek__BackingField == "Kali v Jut").ToList();
				MarketBook.KJMarkets.FirstOrDefault().EventIDk__BackingField = eventID;
				MarketBook.KJMarkets = MarketBook.KJMarkets.Where(item => item.isOpenedbyUserk__BackingField == true).ToList();
			}

			List<BettingServiceReference.MarketBook> LastloadedLinMarkets = new List<BettingServiceReference.MarketBook>();
			LastloadedLinMarkets.Clear();
			if (MarketBook.KJMarkets != null)
			{
				foreach (var bfobject in MarketBook.KJMarkets)
				{
					try
					{
						LastloadedLinMarkets.Add(ConvertJsontoMarketObjectBFNewSource("369646", bfobject.MarketCatalogueIDk__BackingField, bfobject.MarketCatalogueNamek__BackingField, bfobject.BettingAllowedk__BackingField));
					}
					catch (System.Exception ex)
					{

					}
				}
				return LastloadedLinMarkets;
			}
			else
			{
				return new List<BettingServiceReference.MarketBook>();
			}
		}
		public BettingServiceReference.MarketBook ConvertJsontoMarketObjectBFNewSource(string SelectionID, string marketid, string MarketCatalogueName, bool BettingAllowed)
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

		public int GetUserTypeID()
		{
			var context = _httpContextAccessor.HttpContext;

			if (context?.Session.GetObject<UserIDandUserType>("User") != null)
			{
				var user = context.Session.GetObject<UserIDandUserType>("User");

				if (user != null)
				{
					return Convert.ToInt32(user.UserTypeID);
				}
			}

			return 0;
		}

		public int GetUserID()
		{
			var context = _httpContextAccessor.HttpContext;

			if (context.Session.GetObject<UserIDandUserType>("User") != null)
			{
				var user = context.Session.GetObject<UserIDandUserType>("User");

				if (user != null)
				{
					LoggedinUserDetail.UserID = user.ID;
					return user.ID;
				}
				else
				{
					return 0;
				}
			}
			else
			{
				return 0;
			}
		}
	}
}
