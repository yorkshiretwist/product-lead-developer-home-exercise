using System;
using UKParliament.CodeTest.Models;

namespace UKParliament.CodeTest.Data
{
    public interface IRepository
    {
        Task<Person> GetPersonByIdAsync(int id);

        Task<Person> UpdatePersonAsync(Person person);

        Task<Person> CreatePersonAsync(Person person);

        Task<PagedResponseModel<Person>> SearchPeopleAsync(SearchPeopleModel searchPeopleModel);
    }
}
