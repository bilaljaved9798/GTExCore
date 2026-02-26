using BettingServiceReference;
using Global.API;
using GTCore.Models;
using GTExCore.Common;
using GTExCore.HelperClass;
using GTExCore.Models;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using UserServiceReference;

namespace GTExCore.Component
{
    public class FigureMarketViewComponent : ViewComponent
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private BettingServiceClient objBettingClient = new BettingServiceClient();
        UserServicesClient objUsersServiceCleint = new UserServicesClient();
        UserBetsUpdateUnmatcedBets objUserbets = new UserBetsUpdateUnmatcedBets();
        private readonly IPasswordSettingsService _passwordSettingsService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        BettingServiceReference.MarketBook FigureMarket = new BettingServiceReference.MarketBook();
        List<BettingServiceReference.Runner> lstRunners = new List<BettingServiceReference.Runner>();
        public FigureMarketViewComponent(IPasswordSettingsService passwordSettingsService, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _passwordSettingsService = passwordSettingsService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string EventID)
        {
            var model = await LoadMarketFigure(EventID);
            if (model == null || !model.Any())
            {
                return Content("");
            }
            return View(model);
        }
        public List<BettingServiceReference.MarketBook> LastloadedLinMarkets1 = new List<BettingServiceReference.MarketBook>();
        public List<BettingServiceReference.MarketBook> MarketBookFigure = new List<BettingServiceReference.MarketBook>();
        public async Task<List<BettingServiceReference.MarketBook>> LoadMarketFigure(string eventID)
        {
            var Figurevmarket = _httpContextAccessor.HttpContext.Session.GetObject<List<BettingServiceReference.SP_UserMarket_GetDistinctKJMarketsbyEventID_Result>>("Allmarkets");

            if (Figurevmarket !=null && Figurevmarket.Count > 0)
            {
                FigureMarket.FigureMarkets = Figurevmarket.ToArray();
                FigureMarket.FigureMarkets.FirstOrDefault().EventIDk__BackingField = eventID;
                FigureMarket.FigureMarkets = FigureMarket.FigureMarkets.Where(item => item.isOpenedbyUserk__BackingField == true && item.EventNamek__BackingField == "Figure").ToArray();
            }

            if (FigureMarket.FigureMarkets != null)
            {
                foreach (var bfobject in FigureMarket.FigureMarkets)
                {
                    try
                    {
                        LastloadedLinMarkets1.Add(ConvertJsontoMarketObjectBFNewSourceFigure(bfobject.MarketCatalogueIDk__BackingField, bfobject.MarketCatalogueNamek__BackingField, "Cricket", bfobject.BettingAllowedk__BackingField));
                        MarketBookFigure = LastloadedLinMarkets1;
                        UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();
                        if (MarketBookFigure != null)
                        {
                            MarketBookFigure[0].MarketBookName = MarketBookFigure[0].MarketBookName;
                            MarketBookFigure[0].MainSportsname = MarketBookFigure[0].MainSportsname;
                            if (LoggedinUserDetail.GetUserTypeID() == 3)
                            {
                                List<UserBets> lstUserBets = JsonConvert.DeserializeObject<List<UserBets>>(objUsersServiceCleint.GetUserbetsbyUserID(LoggedinUserDetail.GetUserID(), _passwordSettingsService.PasswordForValidate));
                                //List<UserBets> lstUserBets = _httpContextAccessor.HttpContext.Session.GetObject<List<UserBets>>("userbets");
                                //List<UserBets> lstUserBets = JsonConvert.DeserializeObject<List<Models.UserBets>>(objUsersServiceCleint.GetUserbetsbyUserID(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]));
                                lstUserBets = lstUserBets.Where(item2 => item2.isMatched == true && item2.MarketBookID == MarketBookFigure[0].MarketId).ToList();
                                if (lstUserBets.Count > 0)
                                {
                                    MarketBookFigure[0].DebitCredit = objUserBets.ceckProfitandLossFig(MarketBookFigure[0], lstUserBets);
                                    foreach (var runner in MarketBookFigure[0].Runners)
                                    {
                                        runner.ProfitandLoss = Convert.ToInt64(MarketBookFigure[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - MarketBookFigure[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                    }
                                }
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {

                    }
                }
                return MarketBookFigure;

            }
            else
            {
                return MarketBookFigure;
            }
        }

        public BettingServiceReference.MarketBook ConvertJsontoMarketObjectBFNewSourceFigure(string marketid, string sheetname, string MainSportsCategory, bool BettingAllowed)
        {
            var marketbook = new BettingServiceReference.MarketBook();
            marketbook.MarketId = marketid;
            marketbook.BettingAllowed = BettingAllowed;
            marketbook.MainSportsname = "Fancy";
            marketbook.MarketBookName = sheetname;
            marketbook.BettingAllowedOverAll = BettingAllowed;
            marketbook.MarketStatusstr = "In Play";

            int seletionID = 001;
            int RunnerName = 0;

            for (int i = 0; i <= 9; i++)
            {
                var runner = new BettingServiceReference.Runner();

                runner.SelectionId = seletionID + i.ToString();
                runner.RunnerName = sheetname + i.ToString();

                var lstpricelist = new List<BettingServiceReference.PriceSize>();

                var pricesize = new BettingServiceReference.PriceSize();
                pricesize.Size = 900;
                pricesize.Price = i;
                lstpricelist.Add(pricesize);
                runner.ExchangePrices = new BettingServiceReference.ExchangePrices();
                runner.ExchangePrices.AvailableToBack = lstpricelist.ToArray();
                lstpricelist = new List<BettingServiceReference.PriceSize>();

                var pricesize1 = new BettingServiceReference.PriceSize();
                pricesize1.Size = 1025;
                pricesize1.Price = i;
                lstpricelist.Add(pricesize1);

                runner.ExchangePrices.AvailableToLay = new List<BettingServiceReference.PriceSize>().ToArray();
                runner.ExchangePrices.AvailableToLay = lstpricelist.ToArray();
                lstRunners.Add(runner);

            }
            marketbook.Runners = new List<BettingServiceReference.Runner>(lstRunners).ToArray();
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
