using System;

namespace JobWebsiteMVC.Models.Job
{
    public class Job_JobBenefit
    {
        public Guid JobId { get; set; }
        public virtual Job Job { get; set; }
        public Guid JobBenefitId { get; set; }
        public virtual JobBenefit JobBenefit { get; set; }
    }
}