using BettingServiceReference;
using Global.API;
using GTCore.Models;
using GTExCore.Common;
using GTExCore.Models;
using log4net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using UserServiceReference;

namespace GTExCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ToWinTheTossApiController : ControllerBase
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private BettingServiceClient objBettingClient = new BettingServiceClient();
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IRazorViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;
        private readonly IPasswordSettingsService _passwordSettingsService;
        UserServicesClient objUserServiceClient = new UserServicesClient();
        UserBetsUpdateUnmatcedBets _objUserBets;
        UserServicesClient objUsersServiceCleint = new UserServicesClient();
        public static wsnew ws1 = new wsnew();
        public static wsnew ws2 = new wsnew();
        public static wsnew ws4 = new wsnew();
        public static wsnew ws7 = new wsnew();
        public static wsnew ws0 = new wsnew();
        public static wsnew ws4339 = new wsnew();
        public static wsnew wsFancy = new wsnew();

        private wsnew wsBFMatch = new wsnew();
        ToWinTheTossApiController(BettingServiceClient objBettingClient, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IRazorViewEngine viewEngine, ITempDataProvider tempDataProvider, IServiceProvider serviceProvider, IPasswordSettingsService passwordSettingsService, UserServicesClient objUserServiceClient, UserBetsUpdateUnmatcedBets objUserBets, UserServicesClient objUsersServiceCleint, wsnew wsBFMatch)
        {
            this.objBettingClient = objBettingClient;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
            _passwordSettingsService = passwordSettingsService;
            this.objUserServiceClient = objUserServiceClient;
            _objUserBets = objUserBets;
            this.objUsersServiceCleint = objUsersServiceCleint;
            this.wsBFMatch = wsBFMatch;
        }

        public async Task<string> CheckforToWintheTossMarket(string EventID, int userId)
        {
            var wintethossmarket = await objUsersServiceCleint.GetToWintheTossbyeventIdAsync(userId, EventID);
            if (wintethossmarket != null)
            {
                if (wintethossmarket.MarketCatalogueID != null)
                {
                    return wintethossmarket.MarketCatalogueID;
                }
            }
            return "";
        }
       
    }
}
