using Bogus;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Models;
using Xunit;

// clear up ambiguity between UKParliament.CodeTest.Models.Person and Bogus.Person
using Person = UKParliament.CodeTest.Models.Person;

namespace UKParliament.CodeTest.Tests
{
    public class RepositoryTests
    {
        private readonly Mock<PersonManagerContext> _mockPersonManagerContext;
        private readonly Repository _repository;

        public RepositoryTests()
        {
            _mockPersonManagerContext = new Mock<PersonManagerContext>();

            _repository = new Repository(_mockPersonManagerContext.Object);
        }

        private ICollection<Department> GetTestDepartments()
        {
            return new List<Department>
            {
                new Department { Id = 1, Name = "Sales" },
                new Department { Id = 2, Name = "Marketing" },
                new Department { Id = 3, Name = "Finance" },
                new Department { Id = 4, Name = "HR" }
            };
        }

        private ICollection<Person> GetTestPeople()
        {
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
                    EmailAddress = faker.Internet.Email()
                });
            }
            return list;
        }

        [Fact]
        public async Task CreatePersonAsync_ShouldAddPersonToDbContext()
        {
            // Arrange
            var testPeople = GetTestPeople();
            var newPerson = testPeople.FirstOrDefault(p => p.Id == 17);
            // reset the db id
            newPerson.Id = 0;

            var expectedSavedPerson = testPeople.FirstOrDefault(p => p.Id == 17);

            var mockDbSet = new Mock<DbSet<Person>>();
            mockDbSet.Setup(m => m.Add(It.IsAny<Person>()));

            _mockPersonManagerContext.Setup(m => m.People).ReturnsDbSet(testPeople, mockDbSet);

            // Act
            await _repository.CreatePersonAsync(newPerson);

            // Assert
            mockDbSet.Verify(m => m.Add(newPerson), Times.Once);
            _mockPersonManagerContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task GetPersonByIdAsync_PersonNotFound_ShouldReturnPersonFromDbContext()
        {
            // Arrange
            var testPeople = GetTestPeople();
            _mockPersonManagerContext.Setup(m => m.People).ReturnsDbSet(testPeople);

            // Act
            var result = await _repository.GetPersonByIdAsync(999);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetPersonByIdAsync_ShouldReturnPersonFromDbContext()
        {
            // Arrange
            var testPeople = GetTestPeople();
            var personId = 17;
            var expectedPerson = testPeople.FirstOrDefault(p => p.Id == personId);
            _mockPersonManagerContext.Setup(m => m.People).ReturnsDbSet(testPeople);

            // Act
            var result = await _repository.GetPersonByIdAsync(personId);

            // Assert
            result.Should().Be(expectedPerson);
        }

        [Fact]
        public async Task UpdatePersonAsync_PersonNotFound_ShouldThrowException()
        {
            // Arrange
            var testPeople = GetTestPeople();
            var person = testPeople.FirstOrDefault(p => p.Id == 17);
            // set an ID that does not exist
            person.Id = 999;

            var mockDbSet = new Mock<DbSet<Person>>();
            mockDbSet.Setup(m => m.Update(It.IsAny<Person>()));

            _mockPersonManagerContext.Setup(m => m.People).ReturnsDbSet(testPeople, mockDbSet);

            // Act
            Func<Task> action = async () => _repository.UpdatePersonAsync(person);

            // Assert
            mockDbSet.Verify(m => m.Update(person), Times.Never);
            _mockPersonManagerContext.Verify(m => m.SaveChangesAsync(default), Times.Never);
            action.Should().ThrowAsync<InvalidOperationException>($"Person with id {person.Id} not found");
        }

        [Fact]
        public async Task UpdatePersonAsync_ShouldUpdatePersonInDbContext()
        {
            // Arrange
            var testPeople = GetTestPeople();
            var person = testPeople.FirstOrDefault(p => p.Id == 17);

            var mockDbSet = new Mock<DbSet<Person>>();
            mockDbSet.Setup(m => m.Update(It.IsAny<Person>()));
            
            _mockPersonManagerContext.Setup(m => m.People).ReturnsDbSet(testPeople, mockDbSet);

            // Act
            await _repository.UpdatePersonAsync(person);

            // Assert
            mockDbSet.Verify(m => m.Update(person), Times.Once);
            _mockPersonManagerContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task SearchPeopleAsync_EmptyQuery_Page1_ShouldReturnPagedResponseModelOfAllPeople()
        {
            // Arrange
            var testPeople = GetTestPeople();
            _mockPersonManagerContext.Setup(m => m.People).ReturnsDbSet(testPeople);

            var searchPeopleModel = new SearchPeopleModel
            {
                Page = 1,
                PageSize = 10
            };
            var expectedPeople = testPeople
                .OrderBy(x => x.Id)
                .Skip(0)
                .Take(searchPeopleModel.PageSize);

            // Act
            var result = await _repository.SearchPeopleAsync(searchPeopleModel);

            // Assert
            result.Should().NotBeNull();
            result.TotalCount.Should().Be(testPeople.Count);
            result.TotalPages.Should().Be((int)Math.Ceiling((decimal)testPeople.Count / searchPeopleModel.PageSize));
            result.PageSize.Should().Be(searchPeopleModel.PageSize);
            result.Page.Should().Be(searchPeopleModel.Page);
            result.Items.Should().BeEquivalentTo(expectedPeople);
        }

        // TODO: further tests required
        // - Searching by text query
        // - Searching by department id
    }
}
