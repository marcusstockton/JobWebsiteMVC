using JobWebsiteMVC.Models.Job;
using System;
using System.ComponentModel.DataAnnotations;

namespace JobWebsiteMVC.ViewModels.Job
{
    public class JobListViewModel
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

        [Display(Name = "Holiday")]
        public decimal HolidayEntitlement { get; set; }

        [Display(Name = "Job Type")]
        public virtual JobType JobType { get; set; }
    }
}