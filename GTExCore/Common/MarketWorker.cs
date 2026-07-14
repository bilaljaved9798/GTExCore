using Microsoft.AspNetCore.SignalR;

namespace GTExCore.Common
{
    public class MarketWorker: BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IHubContext<MarketHub> _hubContext;
        private readonly IMarketCacheService _cache;

        public MarketWorker(
            IServiceScopeFactory scopeFactory,
            IHubContext<MarketHub> hubContext,
            IMarketCacheService cache)
        {
            _scopeFactory = scopeFactory;
            _hubContext = hubContext;
            _cache = cache;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope =
                        _scopeFactory.CreateScope();

                    var marketService =
                        scope.ServiceProvider
                        .GetRequiredService<IMarketService>();

                    var activeMarkets = new[] { 234, 546, 3232 };

                    //marketService.GetActiveMarketIds();

                    foreach (var marketId in activeMarkets)
                    {
                        var data =
                            await marketService
                            .GetMarketDataAsync("marketId");

                        _cache.Markets.AddOrUpdate(
                            "marketId",
                            data,
                            (k, v) => data);

                        await _hubContext
                            .Clients
                            .Group("marketId")
                            .SendAsync(
                                "ReceiveMarketUpdate",
                                data,
                                stoppingToken);
                    }
                }
                catch (Exception ex)
                {
                    // log
                }

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
