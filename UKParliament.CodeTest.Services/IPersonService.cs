using UKParliament.CodeTest.Models;

namespace UKParliament.CodeTest.Services;

public interface IPersonService
{
    Task<Person> GetPersonByIdAsync(int id);

    Task<Person> UpdatePersonAsync(Person person);

    Task<Person> CreatePersonAsync(Person person);

    Task<PagedResponseModel<Person>> SearchPeopleAsync(SearchPeopleModel searchPeopleModel);
}