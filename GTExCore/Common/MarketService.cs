using BettingServiceReference;
using Global.API;
using GTExCore.Models;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using UserServiceReference;

namespace GTExCore.Common
{
    public class MarketService: IMarketService
    {
        private BettingServiceClient objBettingClient = new BettingServiceClient();
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IRazorViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;
        private readonly IPasswordSettingsService _passwordSettingsService;
        UserServicesClient objUserServiceClient = new UserServicesClient();
        UserBetsUpdateUnmatcedBets _objUserBets = new UserBetsUpdateUnmatcedBets();
        UserServicesClient objUsersServiceCleint = new UserServicesClient();
        private readonly UserBetCacheService _betCache;
        public async Task<object> GetMarketDataAsync(
        string marketId)
        {
        
            List<string> lstIDs = new List<string>();
            lstIDs.Add(marketId);
            DateTime dt = new DateTime();
            var marketbook = await objBettingClient.GetMarketDatabyIDAsync(lstIDs.ToArray(), "", dt, "", _passwordSettingsService.PasswordForValidate);
            marketbook[0].BettingAllowed = objUserServiceClient.GetBettingAllowedbyMarketIDandUserID(160, marketId);//await CheckForAllowedBettingOverAll(MainSportsCategory, sheetname, userId);
            //var lstUserBet = JsonConvert.DeserializeObject<List<UserBets>>(objUsersServiceCleint.GetUserbetsbyUserID(160, _passwordSettingsService.PasswordForValidate));
            var lstUserBets1 = _betCache.GetUserBets(160);
            List<UserBets> lstUserBets = lstUserBets1.Where(item => item.isMatched == true && item.location != "9").ToList();
            var lstMarketIDS = lstUserBets.Select(item => item.MarketBookID).Distinct().ToArray();
            marketbook[0].DebitCredit = _objUserBets.ceckProfitandLoss(marketbook[0], lstUserBets);
            foreach (var runner in marketbook[0].Runners)
            {
                // Fix: 'marketbook12' is not defined. Assuming it should be 'marketbook'.
                runner.ProfitandLoss = Convert.ToInt64(marketbook[0].DebitCredit.Where(dc => dc.SelectionID == runner.SelectionId).Sum(dc => dc.Debit) - marketbook[0].DebitCredit.Where(dc => dc.SelectionID == runner.SelectionId).Sum(dc => dc.Credit));
            }
            

            return marketbook[0];
        }
    }
    public interface IMarketService
    {
        Task<object> GetMarketDataAsync(
            string marketId);
    }
}
