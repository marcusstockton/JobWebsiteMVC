using AutoMapper;
using JobWebsiteMVC.Models.Job;
using JobWebsiteMVC.ViewModels.Job;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobWebsiteMVC.Profiles
{
    public class JobProfile : Profile
    {
        public JobProfile()
        {
            CreateMap<Job, JobCreateViewModel>().ReverseMap();
            CreateMap<Job, JobDetailsViewModel>().ReverseMap();
            CreateMap<Job, JobEditViewModel>().ReverseMap();
        }
    }
}
