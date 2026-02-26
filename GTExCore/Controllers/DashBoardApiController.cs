using AccountServiceReference;
using BettingServiceReference;
using Global.API;
using GTCore.Models;
using GTCore.ViewModel;
using GTExCore.Common;
using GTExCore.HelperClass;
using GTExCore.HelperClasses;
using GTExCore.Models;
using GTExCore.ViewModel;
using log4net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
using UserServiceReference;

namespace GTExCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DashBoardApiController : ControllerBase
    {

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        AccessRightsbyUserType objAccessrightsbyUserType;
        BettingServiceClient BettingServiceClient = new BettingServiceClient();
        UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();
        UserServicesClient objUsersServiceCleint = new UserServicesClient();
        AccountsServiceClient objAccountsService = new AccountsServiceClient();
        private readonly IServiceProvider _serviceProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPasswordSettingsService _passwordSettingsService;
        public static wsnew ws1 = new wsnew();
        public static wsnew ws2 = new wsnew();
        public static wsnew ws4 = new wsnew();
        public static wsnew ws7 = new wsnew();
        public static wsnew ws0 = new wsnew();
        //public static wsnew ws0t = new wsnew();
        public static wsnew ws4339 = new wsnew();
        public static wsnew wsFancy = new wsnew();
        private wsnew wsBFMatch = new wsnew();
        public DashBoardApiController(IServiceProvider serviceProvider, IPasswordSettingsService passwordSettingsService, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _serviceProvider = serviceProvider;
            _passwordSettingsService = passwordSettingsService;
        }

        [Route("GetDefaultPageData")]
        [HttpGet]
        public async Task<IActionResult> GetDefaultPageData(int userid)
        {
            try
            {

                if (userid == 1)
                {
                    userid = 73;
                }
                // if (LoggedinUserDetail.GetUserTypeID() == 3)
                //{
                //var data = GetManagers(userid);
                BettingServiceReference.MarketBook[] lstGridMarkets = objBettingClient.GetMarketDataList(_passwordSettingsService.PasswordForValidate);
                var model = new DefaultPageModel1();

                model.WelcomeMessage = "Please enjoy the non-stop intriguing betting experience only on www.gt-exch.com. Thanks";
                model.WelcomeHeading = "Notice";
                model.Rule = "Rule & Regs";
                model.WelcomeMessage = "All bets apply to Full Time according to the match officials, plus any stoppage time. Extra - time / penalty shoot - outs are not included.If this market is re - opened for In - Play betting, unmatched bets will be cancelled at kick off and the market turned in play.The market will be suspended if it appears that a goal has been scored, a penalty will be given, or a red card will be shown.With the exception of bets for which the 'keep' option has been selected, unmatched bets will be cancelled in the event of a confirmed goal or sending off.Please note that should our data feeds fail we may be unable to manage this game in-play.Customers should be aware   that:Transmissions described as â€œliveâ€ by some broadcasters may actually be delayed.The extent of any such delay may vary, depending on the set-up through which they are receiving pictures or data.If this market is scheduled to go in-play, but due to unforeseen circumstances we are unable to offer the market in-play, then this market will be re-opened for the half-time interval and suspended again an hour after the scheduled kick-off time.";
                model.AllMarkets = lstGridMarkets.ToList();
                model.TodayHorseRacing = new List<TodayHorseRacing>();

                model.ModalContent = new List<string>();
                string modalli1 = "Dummy text";
                string modalli2 = "Dummy text";
                model.ModalContent.Add(modalli1);
                model.ModalContent.Add(modalli2);

                return Ok(new { page = model });
                //}

                //return BadRequest("Invalid model");
            }
            catch (System.Exception ex)
            {
                return BadRequest("Invalid model");
            }
        }
        BettingServiceClient objBettingClient = new BettingServiceClient();
        List<RootSCT> rootsct = new List<RootSCT>();
        public void SetURLsData()
        {
            LoggedinUserDetail.URLsData = JsonConvert.DeserializeObject<List<SP_URLsData_GetAllData_Result>>(objUsersServiceCleint.GetURLsData());
            ws1.Url = LoggedinUserDetail.URLsData.Where(item => item.EventType == "Soccer").FirstOrDefault().URLForData;
            ws2.Url = LoggedinUserDetail.URLsData.Where(item => item.EventType == "Tennis").FirstOrDefault().URLForData;
            ws4.Url = LoggedinUserDetail.URLsData.Where(item => item.EventType == "Cricket").FirstOrDefault().URLForData;
            ws7.Url = LoggedinUserDetail.URLsData.Where(item => item.EventType == "Horse Racing").FirstOrDefault().URLForData;
            ws4339.Url = LoggedinUserDetail.URLsData.Where(item => item.EventType == "GreyHound Racing").FirstOrDefault().URLForData;
            wsFancy.Url = LoggedinUserDetail.URLsData.Where(item => item.EventType == "Fancy").FirstOrDefault().URLForData;

            ws0.Url = LoggedinUserDetail.URLsData.Where(item => item.EventType == "Other").FirstOrDefault().URLForData;
            // ws0t.Url = LoggedinUserDetail.URLsData.Where(item => item.EventType == "BP").FirstOrDefault().URLForData;
            LoggedinUserDetail.SecurityCode = LoggedinUserDetail.URLsData.FirstOrDefault().Scd;
            LoggedinUserDetail.GetCricketDataFrom = LoggedinUserDetail.URLsData.Where(item => item.EventType == "Cricket").FirstOrDefault().GetDataFrom;
        }


    }
}
