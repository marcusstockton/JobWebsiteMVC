using JobWebsiteMVC.Models.Job;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JobWebsiteMVC.ViewModels.Job
{
    public class JobEditViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        [Display(Name = "Job Title")]
        public string JobTitle { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "Draft?")]
        public bool IsDraft { get; set; }

        [Display(Name = "Min Salary")]
        public decimal MinSalary { get; set; }

        [Display(Name = "Max Salary")]
        public decimal MaxSalary { get; set; }

        [Display(Name = "Start Time")]
        [DataType(DataType.Time)]
        public TimeSpan WorkingHoursStart { get; set; }

        [Display(Name = "End Time")]
        [DataType(DataType.Time)]
        public TimeSpan WorkingHoursEnd { get; set; }

        [Display(Name = "Hours Per Week")]
        public decimal HoursPerWeek { get; set; }

        [Display(Name = "Holiday Allowance")]
        public decimal HolidayEntitlement { get; set; }

        [Display(Name = "Active?")]
        public bool IsActive { get; set; }

        [Display(Name = "Closing Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ClosingDate { get; set; }

        [Display(Name = "Publish Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime PublishDate { get; set; }

        [Display(Name = "Job Benefits")]
        public virtual ICollection<Job_JobBenefit> Job_JobBenefits { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CreatedDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? UpdatedDate { get; set; }

        [Display(Name = "Job Benefits")]
        public List<Guid> JobBenefitsIds { get; set; }

        [Display(Name = "Job Type")]
        public Guid JobTypeId { get; set; }

        public virtual List<SelectListItem> JobTypesList { get; set; }
    }
}