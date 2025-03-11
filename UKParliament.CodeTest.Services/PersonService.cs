using AutoMapper;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Models;
using UKParliament.CodeTest.Web.ViewModels;

namespace UKParliament.CodeTest.Services;

public class PersonService : IPersonService
{
    private readonly IRepository _repository;
    private readonly IPersonValidationService _personValidationService;
    private readonly IMapper _mapper;

    public PersonService(IRepository repository, IPersonValidationService personValidationService, IMapper mapper)
    {
        _repository = repository;
        _personValidationService = personValidationService;
        _mapper = mapper;
    }

    public async Task<CreateOrUpdatePersonResult> CreatePersonAsync(PersonViewModel personViewModel)
    {
        var validationErrors = await _personValidationService.ValidatePersonAsync(personViewModel, true);
        if (validationErrors != null && validationErrors.Any())
        {
            return new CreateOrUpdatePersonResult
            {
                ValidationErrors = validationErrors.Select(x => _mapper.Map<ValidationErrorViewModel>(x)).ToList()
            };
        }

        var personToCreate = _mapper.Map<Person>(personViewModel);

        var createdPerson = await _repository.CreatePersonAsync(personToCreate);
        if (createdPerson == null)
        {
            return new CreateOrUpdatePersonResult();
        }

        var createdPersonViewModel = _mapper.Map<PersonViewModel>(createdPerson);
        var createdPersonDepartment = await _repository.GetDepartmentByIdAsync(createdPerson.DepartmentId);
        createdPersonViewModel.DepartmentId = createdPersonDepartment.Id;
        createdPersonViewModel.DepartmentName = createdPersonDepartment.Name;

        return new CreateOrUpdatePersonResult
        {
            Person = createdPersonViewModel
        };
    }

    public async Task<PersonViewModel?> GetPersonByIdAsync(int id)
    {
        var foundPerson = await _repository.GetPersonByIdAsync(id);
        if (foundPerson == null)
        {
            return null;
        }

        return _mapper.Map<PersonViewModel>(foundPerson);
    }

    public async Task<PagedResponseViewModel<PersonViewModel>> SearchPeopleAsync(SearchPeopleParamsViewModel searchPeopleQueryViewModel)
    {
        var searchModel = _mapper.Map<SearchPeopleParams>(searchPeopleQueryViewModel);
        var pagedResult = await _repository.SearchPeopleAsync(searchModel);
        if (pagedResult == null)
        {
            return null;
        }

        return _mapper.Map<PagedResponseViewModel<PersonViewModel>>(pagedResult);
    }

    public async Task<CreateOrUpdatePersonResult> UpdatePersonAsync(PersonViewModel personViewModel)
    {
        var validationErrors = await _personValidationService.ValidatePersonAsync(personViewModel);
        if (validationErrors != null && validationErrors.Any())
        {
            return new CreateOrUpdatePersonResult
            {
                ValidationErrors = validationErrors.Select(x => _mapper.Map<ValidationErrorViewModel>(x)).ToList()
            };
        }

        var personToUpdate = _mapper.Map<Person>(personViewModel);

        var updatedPerson = await _repository.UpdatePersonAsync(personToUpdate);
        if (updatedPerson == null)
        {
            return new CreateOrUpdatePersonResult();
        }

        return new CreateOrUpdatePersonResult
        {
            Person = _mapper.Map<PersonViewModel>(updatedPerson)
        };
    }
}