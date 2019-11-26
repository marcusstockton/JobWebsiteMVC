using JobWebsiteMVC.Models.Job;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JobWebsiteMVC.ViewModels.Job
{
    public class JobEditViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public bool IsDraft { get; set; }
        public decimal MinSalary { get; set; }
        public decimal MaxSalary { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan WorkingHoursStart { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan WorkingHoursEnd { get; set; }
        public decimal HoursPerWeek { get; set; }
        public decimal HolidayEntitlement { get; set; }
        public bool IsActive { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ClosingDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime PublishDate { get; set; }

        [Display(Name="Job Benefits")]
        public virtual ICollection<Job_JobBenefit> Job_JobBenefits { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CreatedDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? UpdatedDate { get; set; }

        public List<Guid> JobBenefitsIds { get; set; }
    }
}