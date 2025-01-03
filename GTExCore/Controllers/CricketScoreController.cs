using BettingServiceReference;
using GTExCore.Models;
using Microsoft.AspNetCore.Mvc;

namespace GTExCore.Controllers
{
    public class CricketScoreController : Controller
    {
        private BettingServiceClient objBettingClient = new BettingServiceClient();
        public IActionResult Index()
        {
            return View();
        }
        public PartialViewResult InitializeSoccerCard()
        {
            return PartialView("SoccerScores");
        }
        public async Task<string> CreateSoccerCardNew(string EventID)
        {
            try
            {
                string jsonString = "";
                jsonString =await objBettingClient.GetSoccorUpdateAsync(EventID);
                try
                {
                    return jsonString;
                }
                catch (System.Exception ex)
                {
                    APIConfig.LogError(ex);
                    return jsonString;
                }
            }
            catch (System.Exception ex)
            {
                return "";
            }

        }
    }
}
