using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JobWebsiteMVC.Models.Job;

public class JobDetailsDTO
{
    public Guid Id { get; set; }
    public string JobTitle { get; set; }
    public string Description { get; set; }
    public bool IsDraft { get; set; }
    public decimal? MinSalary { get; set; }
    public decimal? MaxSalary { get; set; }
    public decimal? HoursPerWeek { get; set; }

    [Display(Name = "Start Time")]
    [DataType(DataType.Time)]
    [DisplayFormat(DataFormatString = "{0:HH\\:mm}", ApplyFormatInEditMode = true)]
    public TimeOnly WorkingHoursStart { get; set; }

    [Display(Name = "End Time")]
    [DataType(DataType.Time)]
    [DisplayFormat(DataFormatString = "{0:HH\\:mm}", ApplyFormatInEditMode = true)]
    public TimeOnly WorkingHoursEnd { get; set; }

    public decimal? HolidayEntitlement { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
    public DateTimeOffset ClosingDate { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
    public DateTimeOffset PublishDate { get; set; }

    public virtual ICollection<JobBenefit> JobBenefits { get; set; }

    public Guid JobTypeId { get; set; }

    public virtual JobType JobType { get; set; }

    public virtual ICollection<JobCategory> JobCategories { get; set; }

}