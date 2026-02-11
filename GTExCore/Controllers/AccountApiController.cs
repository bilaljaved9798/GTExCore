using BettingServiceReference;
using GTCore.Models;
using GTExCore.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserServiceReference;

namespace GTExCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountApiController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        BettingServiceClient BettingServiceClient = new BettingServiceClient();
        UserServicesClient objUserServiceClient = new UserServicesClient();
        IConfiguration _configuration;
        public AccountApiController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid model");

            var results = objUserServiceClient.GetUserbyUsernameandPasswordNew(
                Crypto.Encrypt(model.Username),
                Crypto.Encrypt(model.Password)
            );

            if (string.IsNullOrEmpty(results))
                return Unauthorized("Invalid login attempt");

            var result = JsonConvert.DeserializeObject<UserIDandUserType>(results);

            if (result.isBlocked || result.isDeleted)
                return Unauthorized("Account is blocked or deleted");

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, result.ID.ToString()),
        new Claim(ClaimTypes.Name, model.Username),
        new Claim("UserType", result.UserTypeID.ToString())
    };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])
            );

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(
                    Convert.ToDouble(_configuration["Jwt:ExpireMinutes"])
                ),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256
                )
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token),
                expires = tokenDescriptor.Expires,
                user = new
                {
                    result.ID,
                    result.UserTypeID,
                    result.PoundRate,
                    result.AccountBalance,
                    result.IsCom,
                    result.isFancyMarketAllowed
                }
            });
        }


    }
}
