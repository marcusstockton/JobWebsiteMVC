using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JobWebsiteMVC.Models.Job
{
    public class Benefit : Base
    {
        [Required, MinLength(5), MaxLength(100)]
        public string Description { get; set; }

        public ICollection<JobBenefit> Job_JobBenefits { get; set; }
    }
}