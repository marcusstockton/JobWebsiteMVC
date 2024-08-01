using System.Threading.Tasks;

namespace JobWebsiteMVC.Interfaces
{
    public interface INotificationsHub
    {
        Task SendMessage(string userId, string message);
    }
}
