using Global.API;
using GTCore.Models;
using GTExCore.Common;
using GTExCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UserServiceReference;

namespace GTExCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FigureApiController : ControllerBase
    {
        UserServicesClient objUsersServiceCleint = new UserServicesClient();
        BettingServiceReference.MarketBook MarketBook = new BettingServiceReference.MarketBook();
        UserBetsUpdateUnmatcedBets _objUserbets;
        private readonly IPasswordSettingsService _passwordSettingsService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public FigureApiController(IPasswordSettingsService passwordSettingsService, IHttpContextAccessor httpContextAccessor, UserBetsUpdateUnmatcedBets objUserbets)
        {
            _httpContextAccessor = httpContextAccessor;
            _objUserbets = objUserbets;
            _passwordSettingsService = passwordSettingsService;
        }
        //public string LoadMarketFig(string EventID, int userid)
        //{
        //    var Figurevmarket = JsonConvert.DeserializeObject<List<BettingServiceReference.SP_UserMarket_GetDistinctKJMarketsbyEventID_Result>>(objUsersServiceCleint.KJMarketsbyEventID(EventID, userid));
        //    if (Figurevmarket.Count > 0)
        //    {
        //        MarketBook.FigureMarkets = Figurevmarket.ToArray();
        //        MarketBook.FigureMarkets.FirstOrDefault().EventIDk__BackingField = EventID;
        //        MarketBook.FigureMarkets = MarketBook.FigureMarkets.Where(item => item.isOpenedbyUserk__BackingField == true && item.EventNamek__BackingField == "Figure").ToArray();

        //    }
        //    if (MarketBook.FigureMarkets != null)
        //    {
        //        foreach (var bfobject in MarketBook.FigureMarkets)
        //        {
        //            try
        //            {
        //                MarketBookFigure = LastloadedLinMarkets1;
        //                UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();
        //                if (MarketBookFigure != null)
        //                {
        //                    MarketBookFigure[0].MarketBookName = MarketBookFigure[0].MarketBookName;
        //                    MarketBookFigure[0].MainSportsname = MarketBookFigure[0].MainSportsname;
        //                    if (LoggedinUserDetail.GetUserTypeID() == 3)
        //                    {
        //                        List<UserBets> lstUserBets = JsonConvert.DeserializeObject<List<Models.UserBets>>(objUsersServiceCleint.GetUserbetsbyUserID(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]));
        //                        lstUserBets = lstUserBets.Where(item2 => item2.isMatched == true && item2.MarketBookID == MarketBookFigure[0].MarketId).ToList();

        //                        MarketBookFigure[0].DebitCredit = objUserBets.ceckProfitandLossFig(MarketBookFigure[0], lstUserBets);
        //                        foreach (var runner in MarketBookFigure[0].Runners)
        //                        {
        //                            runner.ProfitandLoss = Convert.ToInt64(MarketBookFigure[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - MarketBookFigure[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
        //                        }
        //                    }
        //                }
        //            }
        //            catch (System.Exception ex)
        //            {

        //            }
        //        }
        //        return LoggedinUserDetail.ConverttoJSONString(MarketBookFigure);

        //    }
        //    else
        //    {
        //        return "";
        //    }

        //}
    }
}
