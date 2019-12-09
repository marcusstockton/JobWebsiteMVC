using JobWebsiteMVC.Models.Job;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JobWebsiteMVC.ViewModels.Job
{
    public class JobDetailsViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public string DescriptionShort
        {
            get { 
                if(Description.Length > 100)
                {
                    return Description.Substring( 0, 100 ) + "...";
                }
                return Description;
            }
                
        }

        [Display(Name="Draft?")]
        public bool IsDraft { get; set; }

        [Display(Name="Min Salary")]
        public decimal MinSalary { get; set; }

        [Display(Name="Max Salary")]
        public decimal MaxSalary { get; set; }

        [Display(Name="Start Time")]
        [DataType(DataType.Time)]
        public TimeSpan WorkingHoursStart { get; set; }

        [Display(Name="End Time")]
        [DataType(DataType.Time)]
        public TimeSpan WorkingHoursEnd { get; set; }

        [Display(Name="Hours per Week")]
        public decimal HoursPerWeek { get; set; }

        [Display(Name="Holiday")]
        public decimal HolidayEntitlement { get; set; }

        [Display(Name="Active?")]
        public bool IsActive { get; set; }

        [Display(Name="Closing Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ClosingDate { get; set; }

        [Display(Name="Publish Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime PublishDate { get; set; }

        [Display(Name="Job Benefits")]
        public virtual ICollection<Job_JobBenefit> Job_JobBenefits { get; set; }

        [Display(Name="Created Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime CreatedDate { get; set; }

        [Display(Name="Updated Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? UpdatedDate { get; set; }

        [Display(Name="Job Type")]
        public virtual JobType JobType{get;set;}
    }
}
