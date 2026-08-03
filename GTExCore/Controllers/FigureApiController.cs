using Global.API;
using GTCore.Models;
using GTExCore.Common;
using GTExCore.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UserServiceReference;

namespace GTExCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class FigureApiController : ControllerBase
    {
        UserServicesClient objUsersServiceCleint = new UserServicesClient();
        BettingServiceReference.MarketBook MarketBook = new BettingServiceReference.MarketBook();
        private readonly UserBetsUpdateUnmatcedBets _objUserBets = new UserBetsUpdateUnmatcedBets();
        private readonly IPasswordSettingsService _passwordSettingsService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserBetCacheService _betCache;
        public FigureApiController(IPasswordSettingsService passwordSettingsService, UserBetCacheService betCache, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _passwordSettingsService = passwordSettingsService;
            _betCache = betCache;
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

        [HttpGet("GetEvenOdd")]
        public async Task<IActionResult> GetEvenOdd(string eventID)
        {
            try
            {
                int userId = LoggedinUserDetailAPI.GetUserId(HttpContext);
                var result = await objUsersServiceCleint.KJMarketsbyEventIDAsync(eventID, userId);

                var markets = JsonConvert.DeserializeObject<List<KJMarketDto>>(result);

                List<KJMarketDto> market = markets.Where(x =>  x.EventName == "Kali v Jut").ToList();
                if (market == null || !market.Any())
                    return Ok(new List<MarketBook>());
                               
                var response = market.Select(m => CreateMarketBook(m, userId)).ToList();


                return Ok(response);
            }catch(Exception ex)
            {
                return BadRequest();
            }
        }

        private BettingServiceReference.MarketBook CreateMarketBook(
    KJMarketDto market,
    int userId)
        {
            var book = new BettingServiceReference.MarketBook
            {
                MarketId = market.MarketCatalogueID,
                MarketBookName = market.MarketCatalogueName,
                MainSportsname = "Fancy",
                MarketStatusstr = "In Play",
                BettingAllowed = market.BettingAllowed,
                BettingAllowedOverAll = true,
                Runners = Array.Empty<BettingServiceReference.Runner>() // Empty array
            };

            var runner = CreateRunner(
                "369646",
                market.MarketCatalogueID,
                userId);

            var runners = book.Runners?.ToList() ?? new List<BettingServiceReference.Runner>();
            runners.Add(runner);

            return book;
        }
        private BettingServiceReference.Runner CreateRunner(
    string selectionId,
    string marketId,
    int userId)
        {
            var runner = new BettingServiceReference.Runner
            {
                SelectionId = selectionId,
                Handicap = 0,
                //ExchangePrices = CreatePrices()
            };

            SetProfitLoss(runner, marketId, userId);

            return runner;
        }
        private void SetProfitLoss(
      BettingServiceReference.Runner runner,
      string marketId,
      int userId)
        {
            try
            {
                var marketPL = GetBookPosition(marketId, userId);

                if (marketPL?.RunnersForindianFancy == null ||
                    marketPL.RunnersForindianFancy.Count() == 0)
                    return;

                runner.ProfitandLoss =
                    (long)marketPL.RunnersForindianFancy.Max(x => x.ProfitandLoss);

                runner.Loss =
                    (long)marketPL.RunnersForindianFancy.Min(x => x.ProfitandLoss);
            }
            catch
            {
            }
        }

        private BettingServiceReference.MarketBookForindianFancy GetBookPosition(
    string marketId,
    int userId)
        {
            var lstUserBets = _betCache.GetUserBets(userId);
            return _objUserBets.GetBookPositioninKJAPI(marketId, lstUserBets);
        }
    }
    public class KJMarketDto
    {
        public bool BettingAllowed { get; set; }

        public string CompetitionID { get; set; }

        public string CompetitionName { get; set; }

        public string EventID { get; set; }

        public string EventName { get; set; }

        public string MarketCatalogueID { get; set; }

        public string MarketCatalogueName { get; set; }

        public bool IsOpenedbyUser { get; set; }
    }
}
