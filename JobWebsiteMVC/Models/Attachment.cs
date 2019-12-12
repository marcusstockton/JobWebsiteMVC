using System;

namespace JobWebsiteMVC.Models
{
    public class Attachment : Base
    {
        public string FileName { get; set; }
        public string Location { get; set; }
        public string FileType { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}