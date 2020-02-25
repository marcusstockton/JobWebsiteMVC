using System.ComponentModel.DataAnnotations;

namespace JobWebsiteMVC.Models.Job
{
    public class JobType : Base
    {
        [Required, MinLength(5), MaxLength(50)]
        public string Description { get; set; }
    }
}