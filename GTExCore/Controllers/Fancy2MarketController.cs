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

		public async Task<PartialViewResult> GetInFancyMarket(string EventID, string MarketBookID)
		{
			if ((LoggedinUserDetail.GetUserTypeID() == 1 || LoggedinUserDetail.GetUserTypeID() == 2 || LoggedinUserDetail.GetUserTypeID() == 8 || LoggedinUserDetail.GetUserTypeID() == 9))
			{
				ViewBag.backgrod = "-webkit-linear-gradient(top, rgb(29, 155, 240), rgb(10, 10, 10))";
				ViewBag.color = "white";
			}

			int UserIDforLinevmarkets = 0;
			if (LoggedinUserDetail.GetUserTypeID() == 1)
			{
				UserIDforLinevmarkets = 73;
			}
			else
			{
				UserIDforLinevmarkets = LoggedinUserDetail.GetUserID();
			}

			//var linevmarkets = JsonConvert.DeserializeObject<List<LineVMarket>>(objUsersServiceCleint.GetLinevMarketsbyEventID(EventID, DateTime.Now, UserIDforLinevmarkets));

			try
			{
				lstMarketBookRunnersIndianFancy.Clear();

				lstMarketBookRunnersIndianFancy.Clear();
				string jsonResult = await objBettingClient.GetRunnersForFancyAsync(EventID, MarketBookID);
				LineMarketData getDataFancy = JsonConvert.DeserializeObject<LineMarketData>(jsonResult);

				foreach (var runners in getDataFancy?.session)
				{
					var runner = new BettingServiceReference.RunnerForIndianFancy();
					runner.BettingAllowed = true;
					runner.StallDraw = "IN-PLAY";
					runner.JockeyName = "Fancy";
					runner.RunnerName = runners.RunnerName;
					runner.StatusStr = runners.GameStatus;
					runner.SelectionId = runners.SelectionId;
					if (runners.LaySize1 == "SUSPENDED" || runners.LaySize1 == "Running")
					{
						runner.LaySize = "0";
						runner.Layprice = "0";
						runner.BackSize = "0";
						runner.Backprice = "0";
					}
					else
					{

						string[] arrLS = runners.LaySize1.Split('.').ToArray();
						runner.LaySize = arrLS[0].ToString();
						string[] arrL = runners.LayPrice1.Split('.').ToArray();
						runner.Layprice = arrL[0].ToString();
						string[] arrbS = runners.BackSize1.Split('.').ToArray();
						runner.BackSize = arrbS[0].ToString();
						string[] arrb = runners.BackPrice1.Split('.').ToArray();
						runner.Backprice = arrb[0].ToString();
					}

					BettingServiceReference.MarketBookForindianFancy currentmarketsfancyPL = new BettingServiceReference.MarketBookForindianFancy();

					//if (LoggedinUserDetail.GetUserTypeID() == 3)
					//{
					//	List<UserBets> lstUserBets = JsonConvert.DeserializeObject<List<UserBets>>(objUsersServiceCleint.GetUserbetsbyUserID(LoggedinUserDetail.GetUserID(), _passwordSettingsService.PasswordForValidate));
					//	if (lstUserBets.Count != 0)
					//	{
					//		currentmarketsfancyPL = objUserbets.GetBookPositionINNew(runner.SelectionId, new List<UserBetsForAdmin>(), new List<UserBetsforSuper>(), new List<UserBetsforSamiadmin>(), new List<UserBetsforAgent>(), lstUserBets);
					//	}
					//}

					if (LoggedinUserDetail.GetUserTypeID() == 2)
					{
						List<UserBetsforAgent> lstUserBets = JsonConvert.DeserializeObject<List<UserBetsforAgent>>(objUsersServiceCleint.GetUserbetsbyUserIDandAgentID(LoggedinUserDetail.GetUserID(), _passwordSettingsService.PasswordForValidate));
						if (lstUserBets.Count != 0)
						{
							currentmarketsfancyPL = objUserbets.GetBookPositionINNew(runner.SelectionId, new List<UserBetsForAdmin>(), new List<UserBetsforSuper>(), new List<UserBetsforSamiadmin>(), lstUserBets, new List<UserBets>());
						}
					}

					if (LoggedinUserDetail.GetUserTypeID() == 8)
					{
						List<UserBetsforSuper> lstUserBets = JsonConvert.DeserializeObject<List<UserBetsforSuper>>(objUsersServiceCleint.GetUserBetsbySuperID(LoggedinUserDetail.GetUserID(), _passwordSettingsService.PasswordForValidate));
						lstUserBets = lstUserBets.Where(item => item.SelectionID == runner.SelectionId).ToList();
						if (lstUserBets.Count != 0)
						{
							currentmarketsfancyPL = objUserbets.GetBookPositionINNew(runner.SelectionId, new List<UserBetsForAdmin>(), lstUserBets, new List<UserBetsforSamiadmin>(), new List<UserBetsforAgent>(), new List<UserBets>());
						}
					}
					if (LoggedinUserDetail.GetUserTypeID() == 9)
					{
						List<UserBetsforSamiadmin> lstUserBets = JsonConvert.DeserializeObject<List<UserBetsforSamiadmin>>(objUsersServiceCleint.GetUserBetsbySamiAdmin(LoggedinUserDetail.GetUserID(), _passwordSettingsService.PasswordForValidate));
						lstUserBets = lstUserBets.Where(item => item.SelectionID == runner.SelectionId).ToList();
						if (lstUserBets.Count != 0)
						{
							currentmarketsfancyPL = objUserbets.GetBookPositionINNew(runner.SelectionId, new List<UserBetsForAdmin>(), new List<UserBetsforSuper>(), lstUserBets, new List<UserBetsforAgent>(), new List<UserBets>());
						}
					}
					if (LoggedinUserDetail.GetUserTypeID() == 9)
					{
						{
							List<UserBetsForAdmin> lstUserBets = JsonConvert.DeserializeObject<List<UserBetsForAdmin>>(objUsersServiceCleint.GetUserbetsbyUserID(LoggedinUserDetail.GetUserID(), _passwordSettingsService.PasswordForValidate));
							lstUserBets = lstUserBets.Where(item => item.SelectionID == runner.SelectionId).ToList();
							if (lstUserBets.Count != 0)
							{
								currentmarketsfancyPL = objUserbets.GetBookPositionINNew(runner.SelectionId, lstUserBets, new List<UserBetsforSuper>(), new List<UserBetsforSamiadmin>(), new List<UserBetsforAgent>(), new List<UserBets>());
							}
						}
					}

					double TotalProfit = 0;
					double TotalLoss = 0;
					if (currentmarketsfancyPL.RunnersForindianFancy != null)
					{
						TotalProfit = Convert.ToDouble(currentmarketsfancyPL.RunnersForindianFancy.Max(t => t.ProfitandLoss));
						TotalLoss = Convert.ToDouble(currentmarketsfancyPL.RunnersForindianFancy.Min(t => t.ProfitandLoss));
						runner.ProfitandLoss = TotalProfit;
						runner.Loss = TotalLoss;
					}
					else
					{
						runner.ProfitandLoss = TotalLoss;
						runner.Loss = TotalProfit;
					}

					runner.MarketBookID = EventID;
					runner.WearingURL = getDataFancy.session.Count.ToString();
					lstMarketBookRunnersIndianFancy.Add(runner);

				}

				return PartialView("Fancy2Market", lstMarketBookRunnersIndianFancy);
			}
			catch (System.Exception ex)
			{
				return PartialView("Fancy2Market", new List<BettingServiceReference.RunnerForIndianFancy>());
			}

		}

		//public IActionResult LoadIndianFancyMarket(string EventID, string MarketBookID)
		//{
		//	LoggedinUserDetail.CheckifUserLogin();

		//	return ViewComponent("IndianFancyMarket", new { EventID, MarketBookID });
		//}
		public async Task<string> LoadFancyMarketIN(string EventID, string MarketBookID)
		{
			List<BettingServiceReference.MarketBookForindianFancy> LastloadedLinMarkets2 = new List<BettingServiceReference.MarketBookForindianFancy>();
			try
			{
				lstMarketBookRunnersIndianFancy.Clear();
				string jsonResult = await objBettingClient.GetRunnersForFancyAsync(EventID, MarketBookID);
				LineMarketData getDataFancy = JsonConvert.DeserializeObject<LineMarketData>(jsonResult);
				//getDataFancy.session = getDataFancy?.session.Take(40).OrderBy(s => s.SelectionId).ToList();
				try
				{
					if (getDataFancy != null)
					{
						getDataFancy.session = getDataFancy.session.Take(100).OrderBy(s => s.SelectionId).ToList();

						foreach (var runners in getDataFancy.session.Take(100))
						{
							var runner = new BettingServiceReference.RunnerForIndianFancy();
							runner.BettingAllowed = true;
							runner.StallDraw = "IN-PLAY";
							runner.JockeyName = "Fancy";
							runner.RunnerName = runners.RunnerName;
							runner.StatusStr = runners.GameStatus;
							runner.SelectionId = runners.SelectionId;
							if (runners.LaySize1 == "SUSPENDED" || runners.LaySize1 == "Running")
							{
								runner.LaySize = "0";
								runner.Layprice = "0";
								runner.BackSize = "0";
								runner.Backprice = "0";
							}
							else
							{

								string[] arrLS = runners.LaySize1.Split('.').ToArray();
								runner.LaySize = arrLS[0].ToString();
								string[] arrL = runners.LayPrice1.Split('.').ToArray();
								runner.Layprice = arrL[0].ToString();
								string[] arrbS = runners.BackSize1.Split('.').ToArray();
								runner.BackSize = arrbS[0].ToString();
								string[] arrb = runners.BackPrice1.Split('.').ToArray();
								runner.Backprice = arrb[0].ToString();
							}

							BettingServiceReference.MarketBookForindianFancy currentmarketsfancyPL = new BettingServiceReference.MarketBookForindianFancy();

							if (LoggedinUserDetail.GetUserTypeID() == 3)
							{
								List<UserBets> lstUserBets = JsonConvert.DeserializeObject<List<UserBets>>(objUsersServiceCleint.GetUserbetsbyUserID(LoggedinUserDetail.GetUserID(), _passwordSettingsService.PasswordForValidate));
								if (lstUserBets.Count != 0)
								{
									currentmarketsfancyPL = objUserbets.GetBookPositionINNew(runner.SelectionId, new List<UserBetsForAdmin>(), new List<UserBetsforSuper>(), new List<UserBetsforSamiadmin>(), new List<UserBetsforAgent>(), lstUserBets);
								}
							}

							if (LoggedinUserDetail.GetUserTypeID() == 2)
							{
								List<UserBetsforAgent> lstUserBets = JsonConvert.DeserializeObject<List<UserBetsforAgent>>(objUsersServiceCleint.GetUserbetsbyUserIDandAgentID(LoggedinUserDetail.GetUserID(), _passwordSettingsService.PasswordForValidate));
								if (lstUserBets.Count != 0)
								{
									currentmarketsfancyPL = objUserbets.GetBookPositionINNew(runner.SelectionId, new List<UserBetsForAdmin>(), new List<UserBetsforSuper>(), new List<UserBetsforSamiadmin>(), lstUserBets, new List<UserBets>());
								}
							}

							if (LoggedinUserDetail.GetUserTypeID() == 8)
							{
								List<UserBetsforSuper> lstUserBets = JsonConvert.DeserializeObject<List<UserBetsforSuper>>(objUsersServiceCleint.GetUserBetsbySuperID(LoggedinUserDetail.GetUserID(), _passwordSettingsService.PasswordForValidate));
								lstUserBets = lstUserBets.Where(item => item.SelectionID == runner.SelectionId).ToList();
								if (lstUserBets.Count != 0)
								{
									currentmarketsfancyPL = objUserbets.GetBookPositionINNew(runner.SelectionId, new List<UserBetsForAdmin>(), lstUserBets, new List<UserBetsforSamiadmin>(), new List<UserBetsforAgent>(), new List<UserBets>());
								}
							}
							if (LoggedinUserDetail.GetUserTypeID() == 9)
							{
								List<UserBetsforSamiadmin> lstUserBets = JsonConvert.DeserializeObject<List<UserBetsforSamiadmin>>(objUsersServiceCleint.GetUserBetsbySamiAdmin(LoggedinUserDetail.GetUserID(), _passwordSettingsService.PasswordForValidate));
								lstUserBets = lstUserBets.Where(item => item.SelectionID == runner.SelectionId).ToList();
								if (lstUserBets.Count != 0)
								{
									currentmarketsfancyPL = objUserbets.GetBookPositionINNew(runner.SelectionId, new List<UserBetsForAdmin>(), new List<UserBetsforSuper>(), lstUserBets, new List<UserBetsforAgent>(), new List<UserBets>());
								}
							}
							if (LoggedinUserDetail.GetUserTypeID() == 9)
							{
								{
									List<UserBetsForAdmin> lstUserBets = JsonConvert.DeserializeObject<List<UserBetsForAdmin>>(objUsersServiceCleint.GetUserbetsbyUserID(LoggedinUserDetail.GetUserID(), _passwordSettingsService.PasswordForValidate));
									lstUserBets = lstUserBets.Where(item => item.SelectionID == runner.SelectionId).ToList();
									if (lstUserBets.Count != 0)
									{
										currentmarketsfancyPL = objUserbets.GetBookPositionINNew(runner.SelectionId, lstUserBets, new List<UserBetsforSuper>(), new List<UserBetsforSamiadmin>(), new List<UserBetsforAgent>(), new List<UserBets>());
									}
								}
							}

							double TotalProfit = 0;
							double TotalLoss = 0;
							if (currentmarketsfancyPL.RunnersForindianFancy != null)
							{
								TotalProfit = Convert.ToDouble(currentmarketsfancyPL.RunnersForindianFancy.Max(t => t.ProfitandLoss));
								TotalLoss = Convert.ToDouble(currentmarketsfancyPL.RunnersForindianFancy.Min(t => t.ProfitandLoss));
								runner.ProfitandLoss = TotalProfit;
								runner.Loss = TotalLoss;
							}
							else
							{
								runner.ProfitandLoss = TotalLoss;
								runner.Loss = TotalProfit;
							}

							runner.MarketBookID = EventID;
							runner.WearingURL = getDataFancy.session.Count.ToString();
							lstMarketBookRunnersIndianFancy.Add(runner);
						}
						return LoggedinUserDetail.ConverttoJSONString(lstMarketBookRunnersIndianFancy);
					}
					else return "";
				}
				catch (System.Exception ex)
				{
					return "";
				}
			}
			catch (System.Exception ex)
			{
				return "";
			}
		}

	}
}
