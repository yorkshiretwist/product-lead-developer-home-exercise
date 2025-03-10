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

        public async Task<PagedResponseModel<Person>> SearchPeopleAsync(SearchPeopleModel searchPeopleModel)
        {
            var skip = searchPeopleModel.PageSize * (searchPeopleModel.Page - 1);
            var query = _personManagerContext.People;

            if (!string.IsNullOrWhiteSpace(searchPeopleModel.Query))
            {
                query.Where(x => x.EmailAddress.Contains(searchPeopleModel.Query)
                    || x.FirstName.Contains(searchPeopleModel.Query)
                    || x.LastName.Contains(searchPeopleModel.Query)
                    || $"{x.FirstName}{x.LastName}".Contains(Regex.Replace(searchPeopleModel.Query, @"[\s\t\n\r]+", "")));
            };

            if (searchPeopleModel.DepartmentId.HasValue)
            {
                query.Where(x => x.DepartmentId == searchPeopleModel.DepartmentId.Value);
            }

            var totalPeopleFound = query.Count();

            var totalPages = (int)Math.Ceiling((decimal)totalPeopleFound / searchPeopleModel.PageSize);

            var pagedPeople = query
                .OrderBy(x => x.Id)
                .Skip(skip)
                .Take(searchPeopleModel.PageSize);

            return new PagedResponseModel<Person>
            {
                TotalCount = totalPeopleFound,
                Items = pagedPeople.ToList(),
                Page = searchPeopleModel.Page,
                PageSize = searchPeopleModel.PageSize,
                TotalPages = totalPages
            };
        }
    }
}
