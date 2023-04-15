using Microsoft.AspNetCore.SignalR;
namespace BookShopAsp.Hubs
{
    public class ChatHub:Hub
    {
        public async Task Send (string message,string userName)
        {
            await Clients.All.SendAsync("Send", message, userName);
        }
    }
}
