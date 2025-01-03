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
            List<string> data = new List<string>();
            int UserIDforLinevmarkets = 0;
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {
                UserIDforLinevmarkets = 73;
            }
            else
            {
                UserIDforLinevmarkets = LoggedinUserDetail.GetUserID();
            }
            var Soccergoalmarket = objUsersServiceCleint.GetSoccergoalbyeventId(UserIDforLinevmarkets, EventID);
            if (Soccergoalmarket != null)
            {
                foreach (var item in Soccergoalmarket)
                {
                    data.Add(item.MarketCatalogueID);
                }
                _httpContextAccessor.HttpContext.Session.SetObject("SGMarket", data);
                return data;
            }
            _httpContextAccessor.HttpContext.Session.SetObject("SGMarket", null);
            
            return data;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
