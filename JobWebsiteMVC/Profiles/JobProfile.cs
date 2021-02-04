using AutoMapper;
using JobWebsiteMVC.Models.Job;
using JobWebsiteMVC.ViewModels.Job;

namespace JobWebsiteMVC.Profiles
{
    public class JobProfile : Profile
    {
        public JobProfile()
        {
            CreateMap<Job, JobCreateViewModel>().ReverseMap();
            CreateMap<Job, JobDetailsViewModel>()
                .ForMember(dto => dto.JobType, opt => opt.MapFrom(src => src.JobType)).ReverseMap();
            CreateMap<Job, JobEditViewModel>().ReverseMap();
            CreateMap<Job, JobDeleteViewModel>().ReverseMap();
        }
    }
}