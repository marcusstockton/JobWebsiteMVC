using JobWebsiteMVC.Models.Job;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JobWebsiteMVC.ViewModels.Job
{
    public class JobDetailsViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Job Title")]
        public string JobTitle { get; set; }

        public string Description { get; set; }

        public string DescriptionShort
        {
            get
            {
                if (Description.Length > 100)
                {
                    return Description.Substring(0, 100) + "...";
                }
                return Description;
            }
        }

        [Display(Name = "Draft?")]
        public bool IsDraft { get; set; }

        [Display(Name = "Min Salary")]
        [DataType(DataType.Currency)]
        public decimal MinSalary { get; set; }

        [Display(Name = "Max Salary")]
        [DataType(DataType.Currency)]
        public decimal MaxSalary { get; set; }

        [Display(Name = "Hours per Week")]
        public decimal HoursPerWeek { get; set; }

        [Display(Name = "Start Time")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH\\:mm}", ApplyFormatInEditMode = true)]
        public TimeOnly WorkingHoursStart { get; set; }

        [Display(Name = "End Time")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH\\:mm}", ApplyFormatInEditMode = true)]
        public TimeOnly WorkingHoursEnd { get; set; }

        [Display(Name = "Holiday")]
        public decimal HolidayEntitlement { get; set; }

        [Display(Name = "Active?")]
        public bool IsActive { get; set; }

        [Display(Name = "Closing Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTimeOffset ClosingDate { get; set; }

        [Display(Name = "Publish Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTimeOffset PublishDate { get; set; }

        [Display(Name = "Job Benefits")]
        public virtual ICollection<JobBenefit> JobBenefits { get; set; }

        [Display(Name = "Created Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTimeOffset CreatedDate { get; set; }

        [Display(Name = "Updated Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTimeOffset? UpdatedDate { get; set; }

        [Display(Name = "Job Type")]
        public virtual JobType JobType { get; set; }
    }
}