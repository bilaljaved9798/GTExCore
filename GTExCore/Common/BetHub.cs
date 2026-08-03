using BettingServiceReference;
using Global.API;
using GTExCore.HelperClass;
using GTExCore.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic.ApplicationServices;
using Newtonsoft.Json;
using System.Configuration;
using UserServiceReference;

namespace GTExCore.Common
{
    public class BetHub : Hub
    {
        private readonly UserBetCacheService _betCache;

        public static Dictionary<string, string>
            UserConnections = new();

        public BetHub(
            UserBetCacheService betCache)
        {
            _betCache = betCache;
        }

        // ============================================
        // CONNECTED
        // ============================================
        public override async Task OnConnectedAsync()
        {
            var httpContext =
                Context.GetHttpContext();

            var userId = httpContext?.Request.Query["userId"].ToString();
                

            if (!string.IsNullOrEmpty(userId))
            {
                UserConnections[userId] =
                    Context.ConnectionId;
            }

            await base.OnConnectedAsync();
        }

        // ============================================
        // DISCONNECTED
        // ============================================
        public override async Task OnDisconnectedAsync(
            Exception? exception)
        {
            var item = UserConnections
                .FirstOrDefault(x =>
                    x.Value == Context.ConnectionId);

            if (!string.IsNullOrEmpty(item.Key))
            {
                UserConnections.Remove(item.Key);
            }

            await base.OnDisconnectedAsync(exception);
        }

        // ============================================
        // GET USER BETS
        // ============================================
        public async Task GetUserBets(int userId)
        {
            var bets = _betCache.GetUserBets1(userId);

            await Clients.Caller.SendAsync(
                "ReceiveUserBets",
                bets
            );
        }

        // ============================================
        // SEND USER BETS
        // ============================================
        public async Task SendUserBets(
            int userId)
        {
            var bets = _betCache.GetUserBets(userId);

            await Clients.User(userId.ToString())
                .SendAsync(
                    "ReceiveUserBets",
                    bets
                );
        }
        public async Task JoinUserGroup(int userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"User_{userId}");
        }
    }
}
