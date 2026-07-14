using BettingServiceReference;
using GTExCore.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GTExCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CricketScoreApiController : ControllerBase
    {
        private BettingServiceClient objBettingClient = new BettingServiceClient();

        [HttpGet]
        [Route("CreateTinnesCardNew")]
        public string CreateTinnesCardNew(string EventID)
        {
            try
            {
                string jsonString = "";
                jsonString = objBettingClient.GetSoccorUpdate(EventID);
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
        [HttpGet]
        [Route("CreateScoreCard")]
        public async Task<string> CreateScoreCard(string EventId)
        {
            try
            {
                var result =  objBettingClient.GetSoccorUpdate(EventId);                
                return JsonConvert.SerializeObject(result);

            }
            catch (System.Exception ex)
            {
                return "";
            }

        }
    }
}
