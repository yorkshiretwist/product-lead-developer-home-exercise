using System.Text.RegularExpressions;
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

        public async Task<Person> CreatePersonAsync(Person person)
        {
            var entity = _personManagerContext.People.Add(person);
            await _personManagerContext.SaveChangesAsync();
            return person;
        }

        public async Task<Person?> GetPersonByIdAsync(int id)
        {
            return _personManagerContext.People.FirstOrDefault(p => p.Id == id);
        }

        public async Task<Person> UpdatePersonAsync(Person person)
        {
            var currentPerson = await GetPersonByIdAsync(person.Id);
            if (currentPerson == null)
            {
                throw new InvalidOperationException($"Person with id {person.Id} not found");
            }

            _personManagerContext.People.Update(person);
            await _personManagerContext.SaveChangesAsync();

            return person;
        }

        public async Task<PagedResult<Person>> SearchPeopleAsync(SearchPeopleParams searchPeopleModel)
        {
            var skip = searchPeopleModel.PageSize * (searchPeopleModel.Page - 1);
            // TODO: check that this doesn't fetch every user from the database
            // and then filter them in memory - filtering should be done in the database
            var people = _personManagerContext.People.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(searchPeopleModel.Query))
            {
                // TODO: tests are required to ensure this regex is working as expected
                var fullNameQuery = Regex.Replace(searchPeopleModel.Query, @"[\s\t\n\r]+", "");
                people = people.Where(x => x.EmailAddress.Contains(searchPeopleModel.Query, StringComparison.InvariantCultureIgnoreCase)
                    || x.FirstName.Contains(searchPeopleModel.Query, StringComparison.InvariantCultureIgnoreCase)
                    || x.LastName.Contains(searchPeopleModel.Query, StringComparison.InvariantCultureIgnoreCase)
                    || $"{x.FirstName}{x.LastName}".Contains(fullNameQuery, StringComparison.InvariantCultureIgnoreCase));
            };

            if (searchPeopleModel.DepartmentId.HasValue)
            {
                people = people.Where(x => x.DepartmentId == searchPeopleModel.DepartmentId.Value);
            }

            if (searchPeopleModel.OnlyActive)
            {
                people = people.Where(x => x.IsActive);
            }

            var totalPeopleFound = people.Count();

            var totalPages = (int)Math.Ceiling((decimal)totalPeopleFound / searchPeopleModel.PageSize);

            var pagedPeople = people
                .OrderBy(x => x.Id)
                .Skip(skip)
                .Take(searchPeopleModel.PageSize)
                .ToList();

            return new PagedResult<Person>
            {
                TotalCount = totalPeopleFound,
                Items = pagedPeople,
                Page = searchPeopleModel.Page,
                PageSize = searchPeopleModel.PageSize,
                TotalPages = totalPages
            };
        }

        public async Task<ICollection<Department>> GetDepartmentsAsync()
        {
            return _personManagerContext.Departments.ToList();
        }

        public async Task<Department?> GetDepartmentByIdAsync(int id)
        {
            return _personManagerContext.Departments.FirstOrDefault(p => p.Id == id);
        }
    }
}
