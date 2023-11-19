using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JobWebsiteMVC.ViewModels.Job
{
    public class JobCreateViewModel
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(100)]
        public string JobTitle { get; set; }

        [Required]
        [StringLength(1000)]
        public string Description { get; set; }

        [Display(Name = "Draft?")]
        public bool IsDraft { get; set; }

        [Display(Name = "Min Salary"), Required]
        public decimal MinSalary { get; set; }

        [Display(Name = "Max Salary"), Required]
        public decimal MaxSalary { get; set; }

        [Display(Name = "Start Time"), Required]
        [DataType(DataType.Time)]
        public TimeOnly WorkingHoursStart { get; set; }

        [Display(Name = "End Time"), Required]
        [DataType(DataType.Time)]
        public TimeOnly WorkingHoursEnd { get; set; }

        [Display(Name = "Hours Per Week"), Required]
        public decimal HoursPerWeek { get; set; }

        [Display(Name = "Holiday Entitlement"), Required]
        public decimal HolidayEntitlement { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        [Display(Name = "Closing Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ClosingDate { get; set; }

        [Display(Name = "Publish Date"), Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime PublishDate { get; set; }

        public DateTime CreatedDate { get; set; }

        [Display(Name = "Job Benefits")]
        public virtual List<Guid> JobBenefitsIds { get; set; }

        [Display(Name = "Job Type")]
        [Required]
        public Guid JobTypeId { get; set; }
    }
}