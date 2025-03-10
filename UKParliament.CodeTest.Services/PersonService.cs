using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Models;

namespace UKParliament.CodeTest.Services;

public class PersonService : IPersonService
{
    private readonly IRepository _repository;

    public PersonService(IRepository repository)
    {
        _repository = repository;
    }

    public Task<Person> CreatePersonAsync(Person person)
    {
        throw new NotImplementedException();
    }

    public Task<Person> GetPersonByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<PagedResponseModel<Person>> SearchPeopleAsync(SearchPeopleModel searchPeopleModel)
    {
        throw new NotImplementedException();
    }

    public Task<Person> UpdatePersonAsync(Person person)
    {
        throw new NotImplementedException();
    }
}