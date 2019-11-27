﻿using JobWebsiteMVC.Models.Job;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JobWebsiteMVC.ViewModels.Job
{
    public class JobCreateViewModel
    {
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
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ClosingDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime PublishDate { get; set; }

        public DateTime CreatedDate { get; set; }

        [Display(Name="Job Benefits")]
        public virtual List<Guid> JobBenefitsIds { get; set; }

        [Display(Name="Job Type")]
        public Guid JobTypeId { get; set; }
        public virtual List<SelectListItem> JobTypesList{get;set;}
    }
}
