using UKParliament.CodeTest.Models;

namespace UKParliament.CodeTest.Data
{
    public interface IRepository
    {
        Task<Person?> GetPersonByIdAsync(int id);

        Task<Person> UpdatePersonAsync(Person person);

        Task<Person> CreatePersonAsync(Person person);

        Task<PagedResult<Person>> SearchPeopleAsync(SearchPeopleQuery searchPeopleModel);

        Task<ICollection<Department>> GetDepartmentsAsync();

        Task<Department?> GetDepartmentByIdAsync(int id);
    }
}
