using GTExCore.Models;
using Microsoft.AspNetCore.SignalR;

public class UserBetsHub : Hub
{
    private readonly IHubContext<UserBetsHub> _hubContext;

    public UserBetsHub(IHubContext<UserBetsHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task SendUserBets(List<UserBets> userBets)
    {
        await Clients.All.SendAsync("ReceiveUserBets", userBets);
    }

    //public async Task SendMessages(List<UserBets> userBets)
    //{
    //    await _hubContext.Clients.All.SendAsync("ReceiveUserBets", userBets);
    //}
}

