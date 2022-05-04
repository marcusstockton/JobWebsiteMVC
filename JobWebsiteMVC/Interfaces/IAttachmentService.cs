using JobWebsiteMVC.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace JobWebsiteMVC.Interfaces
{
    public interface IAttachmentService
    {
        Task<Attachment> SaveAvatar(IFormFile file, ApplicationUser user);
    }
}