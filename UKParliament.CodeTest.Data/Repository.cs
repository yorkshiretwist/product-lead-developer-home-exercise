using UKParliament.CodeTest.Models;

namespace UKParliament.CodeTest.Data
{
    public class Repository : IRepository
    {
        private readonly PersonManagerContext _personManagerContext;

        public Repository(PersonManagerContext personManagerContext)
        {
            _personManagerContext = personManagerContext;
        }

        public Task<Person> CreatePersonAsync(Person person)
        {
            throw new NotImplementedException();
        }

        public Task<Person> GetPersonByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Person> UpdatePersonAsync(Person person)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResponseModel<Person>> SearchPeopleAsync(SearchPeopleModel searchPeopleModel)
        {
            throw new NotImplementedException();
        }
    }
}
