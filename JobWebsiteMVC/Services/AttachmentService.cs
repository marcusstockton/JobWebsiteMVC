using JobWebsiteMVC.Data;
using JobWebsiteMVC.Interfaces;

namespace JobWebsiteMVC.Services
{
    public class AttachmentService : IAttachmentService
    {
        private ApplicationDbContext _context;

        public AttachmentService(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}