namespace GTExCore.Common
{
    using GTExCore.Controllers;
    using Microsoft.AspNetCore.SignalR;
    using System.Web.Services.Description;

    public class MarketHub : Hub
    {
        MarketApiController _marketApiController;
        public MarketHub(MarketApiController service)
        {
            _marketApiController = service;
        }
        public async Task SubscribeMarket(string id, string sheetname, string category)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, id);

            // 🔁 Start sending updates (every 1 sec or from event)
            _ = Task.Run(async () =>
            {
                while (true)
                {
                    var data = await _marketApiController.MarketBookData(id, sheetname, category);

                    await Clients.Group(id).SendAsync("ReceiveMarketUpdate", data);

                    await Task.Delay(1000); // or trigger-based
                }
            });
        }
    }
}
