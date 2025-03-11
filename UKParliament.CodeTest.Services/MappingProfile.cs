using AutoMapper;
using UKParliament.CodeTest.Models;
using UKParliament.CodeTest.Web.ViewModels;

namespace UKParliament.CodeTest.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Person, PersonViewModel>().ReverseMap();
            CreateMap<Department, DepartmentViewModel>().ReverseMap();
            CreateMap<SearchPeopleParamsViewModel, SearchPeopleParams>();
            CreateMap<ValidationError, ValidationErrorViewModel>();
            CreateMap<PagedResult<Person>, PagedResponseViewModel<PersonViewModel>>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
        }
    }
}
