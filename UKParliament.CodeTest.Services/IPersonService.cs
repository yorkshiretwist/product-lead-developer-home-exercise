using UKParliament.CodeTest.Web.ViewModels;

namespace UKParliament.CodeTest.Services;

public interface IPersonService
{
    Task<PersonViewModel?> GetPersonByIdAsync(int id);

    Task<CreateOrUpdatePersonResult> UpdatePersonAsync(PersonViewModel personViewModel);

    Task<CreateOrUpdatePersonResult> CreatePersonAsync(PersonViewModel personViewModel);

    Task<PagedResponseViewModel<PersonViewModel>> SearchPeopleAsync(SearchPeopleParamsViewModel searchPeopleViewModel);
}