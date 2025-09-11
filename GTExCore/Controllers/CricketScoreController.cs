using BettingServiceReference;
using GTExCore.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        public PartialViewResult InitializeScoreCard()
        {
            return PartialView("CricketScores");
        }
        
        public PartialViewResult InitializeTinnesCard()
        {
            return PartialView("TinnesScores");
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
        Home root = new Home();
        UpdateNew rootnew = new UpdateNew();
        public async Task<string> CreateScoreCardNew(string EventId)
        {
            try
            {
                rootnew = await objBettingClient.GetUpdateNewAsync(EventId);
                string jsonString;
                jsonString = JsonConvert.SerializeObject(root);        
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
