using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JobWebsiteMVC.Models.Job;

namespace JobWebsiteMVC.ViewModels.Job
{
    public class JobDetailsViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

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

        [Display(Name = "Start Time")]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Time)]
        public TimeSpan WorkingHoursStart { get; set; }

        [Display(Name = "End Time")]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Time)]
        public TimeSpan WorkingHoursEnd { get; set; }

        [Display(Name = "Hours per Week")]
        public decimal HoursPerWeek { get; set; }

        [Display(Name = "Holiday")]
        public decimal HolidayEntitlement { get; set; }

        [Display(Name = "Active?")]
        public bool IsActive { get; set; }

        [Display(Name = "Closing Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime ClosingDate { get; set; }

        [Display(Name = "Publish Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime PublishDate { get; set; }

        [Display(Name = "Job Benefits")]
        public virtual ICollection<Job_JobBenefit> Job_JobBenefits { get; set; }

        [Display(Name = "Created Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Updated Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? UpdatedDate { get; set; }

        [Display(Name = "Job Type")]
        public virtual JobType JobType { get; set; }
    }
}