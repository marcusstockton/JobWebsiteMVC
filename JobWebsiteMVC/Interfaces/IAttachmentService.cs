using System.Threading.Tasks;
using JobWebsiteMVC.Models;
using Microsoft.AspNetCore.Http;

namespace JobWebsiteMVC.Interfaces
{
    public interface IAttachmentService
    {
        Task<Attachment> SaveAvatar(IFormFile file, ApplicationUser user);
    }
}