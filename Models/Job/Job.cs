using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobWebsiteMVC.Models.Job
{
    public class Job : Base
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public bool IsDraft { get; set; }
        public decimal MinSalary { get; set; }
        public decimal MaxSalary { get; set; }
        public TimeSpan WorkingHoursStart { get; set; }
        public TimeSpan WorkingHoursEnd { get; set; }
        public decimal HoursPerWeek { get; set; }
        public decimal HolidayEntitlement { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ClosingDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime PublishDate { get; set; }

        public virtual ICollection<Job_JobBenefit> Job_JobBenefits { get; set; }

        public Guid JobTypeId { get; set; }

        [ForeignKey("JobTypeId")]
        public virtual JobType JobType{get;set;}
    }
}