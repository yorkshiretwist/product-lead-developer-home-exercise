using Bogus;
using UKParliament.CodeTest.Models;
using UKParliament.CodeTest.Web.ViewModels;

// clear up ambiguity between UKParliament.CodeTest.Models.Person and Bogus.Person
using Person = UKParliament.CodeTest.Models.Person;

namespace UKParliament.CodeTest.Tests
{
    public class TestBase
    {
        public ICollection<Department> GetTestDepartments()
        {
            return new List<Department>
            {
                new Department { Id = 1, Name = "Sales" },
                new Department { Id = 2, Name = "Marketing" },
                new Department { Id = 3, Name = "Finance" },
                new Department { Id = 4, Name = "HR" }
            };
        }

        public ICollection<Person> GetTestPeople()
        {
            if (_testPeople != null)
            {
                return _testPeople;
            }
            var list = new List<Person>();
            var faker = new Faker();
            for (var i = 1; i < 100; i++)
            {
                list.Add(new Person
                {
                    Id = i,
                    FirstName = faker.Name.FirstName(),
                    LastName = faker.Name.LastName(),
                    DepartmentId = faker.Random.Number(1, 4),
                    EmailAddress = faker.Internet.Email(),
                    IsActive = faker.Random.Bool()
                });
            }
            _testPeople = list;
            return _testPeople;
        }
        private ICollection<Person>? _testPeople = null;

        public Person GetTestPerson()
        {
            var person = GetTestPeople().First();
            return person;
        }

        public PersonViewModel GetTestPersonViewModel(bool isNewPerson = false)
        {
            var person = GetTestPerson();
            var department = GetTestDepartments().First(x => x.Id == person.DepartmentId);
            return new PersonViewModel
            {
                Id = isNewPerson ? 0 : person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                EmailAddress = person.EmailAddress,
                IsActive = person.IsActive,
                Department = new DepartmentViewModel
                {
                    Id = department.Id,
                    Name = department.Name
                }
            };
        }
    }
}
