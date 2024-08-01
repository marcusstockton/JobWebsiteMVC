using JobWebsiteMVC.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace JobWebsiteMVC.Hubs
{
    public class NotificationsHub : Hub<INotificationsHub>
    {
        public async Task SendMessage(string userId, string message)
            => await Clients.All.SendMessage(userId, message);
    }
}