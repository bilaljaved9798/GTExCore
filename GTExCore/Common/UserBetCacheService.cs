using GTExCore.Models;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using UserServiceReference;

namespace GTExCore.Common
{
    public class UserBetCacheService
    {
        private readonly IMemoryCache _cache;
        private readonly IPasswordSettingsService _passwordSettingsService;

        UserServicesClient objUsersServiceCleint =
            new UserServicesClient();

        public UserBetCacheService(
            IMemoryCache cache,
            IPasswordSettingsService passwordSettingsService)
        {
            _cache = cache;
            _passwordSettingsService =
                passwordSettingsService;
        }

        // ============================================
        // GET USER BETS
        // ============================================
        public List<UserBets> GetUserBets(int userId)
        {
            string cacheKey = $"USER_BETS_{userId}";

            // CACHE HIT
            if (_cache.TryGetValue(
                cacheKey,
                out List<UserBets> cachedBets))
            {
                return cachedBets;
            }

            // DATABASE HIT
            var lstUserBet =
                JsonConvert.DeserializeObject<List<UserBets>>(
                    objUsersServiceCleint.GetUserbetsbyUserID(
                        userId,
                        _passwordSettingsService.PasswordForValidate
                    )
                );

            List<UserBets> lstUserBets = lstUserBet
                .Where(x =>
                    x.isMatched == true )
                .ToList();

            // STORE CACHE
            _cache.Set(
                cacheKey,
                lstUserBets,
                new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow =
                        TimeSpan.FromSeconds(10)
                });

            return lstUserBets;
        }

        // ============================================
        // CLEAR CACHE
        // ============================================
        public void RemoveUserBets(int userId)
        {
            string cacheKey = $"USER_BETS_{userId}";

            _cache.Remove(cacheKey);
        }

        // ============================================
        // UPDATE CACHE
        // ============================================
        public void UpdateUserBets(
            int userId,
            List<UserBets> bets)
        {
            string cacheKey = $"USER_BETS_{userId}";

            _cache.Set(
                cacheKey,
                bets,
                new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow =
                        TimeSpan.FromSeconds(10)
                });
        }
    }
}
