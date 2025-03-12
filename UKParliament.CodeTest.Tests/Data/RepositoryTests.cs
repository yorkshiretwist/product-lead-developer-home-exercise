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
    public class RepositoryTests : TestBase
    {
        private readonly Mock<PersonManagerContext> _mockPersonManagerContext;
        private readonly Repository _repository;

        public RepositoryTests()
        {
            _mockPersonManagerContext = new Mock<PersonManagerContext>();

            _repository = new Repository(_mockPersonManagerContext.Object);
        }

        [Fact]
        public async Task GetDepartmentsAsync_ShouldReturnDepartments()
        {
            // Arrange
            var testDepartments = GetTestDepartments();
            _mockPersonManagerContext.Setup(m => m.Departments).ReturnsDbSet(testDepartments);

            // Act
            var result = await _repository.GetDepartmentsAsync();

            // Assert
            result.Should().BeEquivalentTo(testDepartments);
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
            person.FirstName = "Newname";

            var mockDbSet = new Mock<DbSet<Person>>();
            mockDbSet.Setup(m => m.Update(It.IsAny<Person>()));
            
            _mockPersonManagerContext.Setup(m => m.People).ReturnsDbSet(testPeople, mockDbSet);

            // Act
            await _repository.UpdatePersonAsync(person);

            // Assert
            _mockPersonManagerContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task SearchPeopleAsync_EmptyQuery_Page1_ShouldReturnPagedResponseModelOfAllPeople()
        {
            // Arrange
            var testPeople = GetTestPeople();
            _mockPersonManagerContext.Setup(m => m.People).ReturnsDbSet(testPeople);

            var searchPeopleModel = new SearchPeopleParams
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

        // TODO: add tests for SearchPeopleAsync with text query and department id, GetDepartmentsAsync, and GetDepartmentByIdAsync
    }
}
