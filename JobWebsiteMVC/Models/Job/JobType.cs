using System.ComponentModel.DataAnnotations;

namespace JobWebsiteMVC.Models.Job
{
    public class JobType : Base
    {
        [MaxLength(50)]
        public string Description { get; set; }
    }
}