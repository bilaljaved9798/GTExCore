﻿using BettingServiceReference;
using Global.API;
using GTCore.Models;
using GTExCore.Common;
using GTExCore.HelperClass;
using GTExCore.Models;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using UserServiceReference;

namespace GTExCore.Component
{
    public class IndianFancyMarketViewComponent : ViewComponent
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private BettingServiceClient objBettingClient = new BettingServiceClient();
        UserServicesClient objUsersServiceCleint = new UserServicesClient();
        UserBetsUpdateUnmatcedBets objUserbets = new UserBetsUpdateUnmatcedBets();
		private readonly IPasswordSettingsService _passwordSettingsService;
		private readonly IHttpContextAccessor _httpContextAccessor;
		public IndianFancyMarketViewComponent( IPasswordSettingsService passwordSettingsService, IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
			_passwordSettingsService = passwordSettingsService;
		}
		List<BettingServiceReference.RunnerForIndianFancy> lstMarketBookRunnersIndianFancy = new List<BettingServiceReference.RunnerForIndianFancy>();

        public async Task<IViewComponentResult> InvokeAsync(string EventID, string MarketBookID)
        {
            var model =await GetInFancyMarket(EventID, MarketBookID);
            return View(model);
        }

       
        public async Task<List<BettingServiceReference.RunnerForIndianFancy>> GetInFancyMarket(string EventID, string MarketBookID)
        {
            await Task.Delay(500);

            int UserIDforLinevmarkets = 0;
           
            List<BettingServiceReference.MarketBookForindianFancy> LastloadedLinMarkets2 = new List<BettingServiceReference.MarketBookForindianFancy>();
            try
            {
                lstMarketBookRunnersIndianFancy.Clear();
                string jsonResult = await objBettingClient.GetRunnersForFancyAsync(EventID, MarketBookID);
                LineMarketData getDataFancy = JsonConvert.DeserializeObject<LineMarketData>(jsonResult);
                getDataFancy.session = getDataFancy.session.Take(40).OrderBy(s => s.SelectionId).ToList();
                foreach (var runners in getDataFancy.session.Take(15))
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

                    if (GetUserTypeID() == 3)
                    {
                        List<UserBets> lstUserBets = JsonConvert.DeserializeObject<List<UserBets>>(objUsersServiceCleint.GetUserbetsbyUserID(GetUserID(), _passwordSettingsService.PasswordForValidate));
                        if (lstUserBets.Count != 0)
                        {
                            currentmarketsfancyPL = objUserbets.GetBookPositionINNew(runner.SelectionId, new List<UserBetsForAdmin>(), new List<UserBetsforSuper>(), new List<UserBetsforSamiadmin>(), new List<UserBetsforAgent>(), lstUserBets);
                        }
                    }

                    if (GetUserTypeID() == 2)
                    {
                        List<UserBetsforAgent> lstUserBets = JsonConvert.DeserializeObject<List<UserBetsforAgent>>(objUsersServiceCleint.GetUserbetsbyUserIDandAgentID(GetUserID(), _passwordSettingsService.PasswordForValidate));
                        if (lstUserBets.Count != 0)
                        {
                            currentmarketsfancyPL = objUserbets.GetBookPositionINNew(runner.SelectionId, new List<UserBetsForAdmin>(), new List<UserBetsforSuper>(), new List<UserBetsforSamiadmin>(), lstUserBets, new List<UserBets>());
                        }
                    }

                    if (GetUserTypeID() == 8)
                    {
                        List<UserBetsforSuper> lstUserBets = JsonConvert.DeserializeObject<List<UserBetsforSuper>>(objUsersServiceCleint.GetUserBetsbySuperID(GetUserID(), _passwordSettingsService.PasswordForValidate));
                        lstUserBets = lstUserBets.Where(item => item.SelectionID == runner.SelectionId).ToList();
                        if (lstUserBets.Count != 0)
                        {
                            currentmarketsfancyPL = objUserbets.GetBookPositionINNew(runner.SelectionId, new List<UserBetsForAdmin>(), lstUserBets, new List<UserBetsforSamiadmin>(), new List<UserBetsforAgent>(), new List<UserBets>());
                        }
                    }
                    if (GetUserTypeID() == 9)
                    {
                        List<UserBetsforSamiadmin> lstUserBets = JsonConvert.DeserializeObject<List<UserBetsforSamiadmin>>(objUsersServiceCleint.GetUserBetsbySamiAdmin(GetUserID(), _passwordSettingsService.PasswordForValidate));
                        lstUserBets = lstUserBets.Where(item => item.SelectionID == runner.SelectionId).ToList();
                        if (lstUserBets.Count != 0)
                        {
                            currentmarketsfancyPL = objUserbets.GetBookPositionINNew(runner.SelectionId, new List<UserBetsForAdmin>(), new List<UserBetsforSuper>(), lstUserBets, new List<UserBetsforAgent>(), new List<UserBets>());
                        }
                    }
                    if (GetUserTypeID() == 9)
                    {
                        {
                            List<UserBetsForAdmin> lstUserBets = JsonConvert.DeserializeObject<List<UserBetsForAdmin>>(objUsersServiceCleint.GetUserbetsbyUserID(GetUserID(), _passwordSettingsService.PasswordForValidate));
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

                return  lstMarketBookRunnersIndianFancy;
            }
            catch (System.Exception ex)
            {
                return  new List<BettingServiceReference.RunnerForIndianFancy>();
            }

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

		public  int GetUserID()
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
