using System.Collections.Concurrent;

namespace GTExCore.Common
{
    public class MarketCacheService: IMarketCacheService
    {
        public ConcurrentDictionary<string, object> Markets { get; } = new();
    }
    public interface IMarketCacheService
    {
        ConcurrentDictionary<string, object> Markets { get; }
    }
}
