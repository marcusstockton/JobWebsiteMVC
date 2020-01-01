using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobWebsiteMVC.Models.Job
{
    public class JobApplication : Base
    {
        public Guid JobId { get; set; }
        public string ApplicantId { get; set; }

        [ForeignKey("JobId")]
        public Job Job { get; set; }

        [ForeignKey("ApplicantId")]
        public ApplicationUser Applicant { get; set; }
    }
}