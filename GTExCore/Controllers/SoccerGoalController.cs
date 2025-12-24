using GTExCore.Common;
using GTExCore.HelperClass;
using GTExCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.SignalR;
using UserServiceReference;

namespace GTExCore.Controllers
{
    public class SoccerGoalController : Controller
    {
        UserServicesClient objUsersServiceCleint = new UserServicesClient();
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SoccerGoalController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;           
        }
        public List<string> CheckforSoccergoalMarket(string EventID)
        {
            ViewBag.backgrod = "-webkit-linear-gradient(top, rgb(29, 155, 240), rgb(10, 10, 10))";
            ViewBag.color = "white";

            var data = new List<string>();

            int userIdForLiveMarkets =
                LoggedinUserDetail.GetUserTypeID() == 1
                ? 73
                : LoggedinUserDetail.GetUserID();

            var soccerGoalMarket =
                objUsersServiceCleint.GetSoccergoalbyeventId(userIdForLiveMarkets, EventID);

            if (soccerGoalMarket != null)
            {
                foreach (var item in soccerGoalMarket)
                {
                    if (!string.IsNullOrEmpty(item.MarketCatalogueID))
                    {
                        data.Add(item.MarketCatalogueID);
                    }
                }
            }

            return data; // ✅ always return List<string>
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
