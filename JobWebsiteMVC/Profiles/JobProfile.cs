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
                .ReverseMap();
            CreateMap<Job, JobEditViewModel>()
                .ForMember(dto => dto.Job_JobBenefits, opt => opt.MapFrom(src => src.JobBenefits))
                .ReverseMap();
            CreateMap<Job, JobDeleteViewModel>()
                .ForMember(dto => dto.Title, opt => opt.MapFrom(src => src.JobTitle))
                .ReverseMap();
        }
    }
}