using Microsoft.AspNetCore.SignalR;

namespace GTExCore.Common
{
    public class CustomUserIdProvider: IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.GetHttpContext().Request.Query["userId"];
        }
    }
}
