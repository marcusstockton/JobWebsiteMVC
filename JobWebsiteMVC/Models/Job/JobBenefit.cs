using System.Collections.Generic;

namespace JobWebsiteMVC.Models.Job
{
    public class JobBenefit : Base  
    {
        public string Description { get; set; }
        public virtual ICollection<Job_JobBenefit> Job_JobBenefits { get; set; }
    }
}