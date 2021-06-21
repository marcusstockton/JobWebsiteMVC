using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using JobWebsiteMVC.Data;
using JobWebsiteMVC.Interfaces;
using JobWebsiteMVC.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace JobWebsiteMVC.Services
{
    public class AttachmentService : IAttachmentService
    {
        private ApplicationDbContext _context;
        private IWebHostEnvironment _hostingEnvironment;

        public AttachmentService(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<Attachment> SaveAvatar(IFormFile file, ApplicationUser user)
        {
            var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads");

            if (file.Length > 0)
            {
                var filePath = Path.Combine(uploads, user.Id, file.FileName);
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(Path.Combine(uploads, user.Id));
                }
                try
                {
                    using (var filestream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        await file.CopyToAsync(filestream);
                        var attachment = new Attachment
                        {
                            CreatedDate = DateTime.Now,
                            FileName = file.FileName,
                            Location = "~" + filePath.Split("wwwroot").Last().Replace(@"\", "/"),
                            FileType = file.FileName.Split('.').Last(),
                            IsActive = true
                        };
                        await _context.Attachments.AddAsync(attachment);
                        return attachment;
                    }
                }
                catch
                {
                    throw new Exception();
                }
            }

            return null;
        }
    }
}