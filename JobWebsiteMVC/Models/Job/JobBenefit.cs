using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JobWebsiteMVC.Models.Job
{
    public class JobBenefit : Base
    {
        [Required, MinLength(5), MaxLength(100)]
        public string Description { get; set; }

        public virtual ICollection<Job_JobBenefit> Job_JobBenefits { get; set; }
    }
}