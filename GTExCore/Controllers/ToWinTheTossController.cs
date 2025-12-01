using BettingServiceReference;
using GTExCore.Models;
using Microsoft.AspNetCore.Mvc;
using UserServiceReference;

namespace GTExCore.Controllers
{
    public class ToWinTheTossController : Controller
    {
        UserServicesClient objUsersServiceCleint = new UserServicesClient();
        BettingServiceClient objBettingClient = new BettingServiceClient();
        public IActionResult Index()
        {
            return View();
        }
        public string CheckforToWintheTossMarket(string EventID)
        {
            if (LoggedinUserDetail.GetUserTypeID() != 3)
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
            var wintethossmarket = objUsersServiceCleint.GetToWintheTossbyeventId(UserIDforLinevmarkets, EventID);
            if (wintethossmarket != null)
            {
                if (wintethossmarket.MarketCatalogueID != null)
                {
                    //Session["TWT"] = wintethossmarket.MarketCatalogueID;
                    return wintethossmarket.MarketCatalogueID;
                }
            }
            //Session["TWT"] = "";
            return "";
        }
    }
}
