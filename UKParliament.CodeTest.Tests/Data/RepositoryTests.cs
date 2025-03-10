using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Models;
using Xunit;

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

        [Fact]
        public async Task CreatePersonAsync_ShouldAddPersonToDbContext()
        {
            // Arrange
            var person = new Person { Id = 1, FirstName = "John", LastName = "Doe", EmailAddress = "john.doe@example.com", DepartmentId = 1, IsActive = true };
            var mockDbSet = new Mock<DbSet<Person>>();
            _mockPersonManagerContext.Setup(m => m.Set<Person>()).Returns(mockDbSet.Object);

            // Act
            await _repository.CreatePersonAsync(person);

            // Assert
            mockDbSet.Verify(m => m.AddAsync(person, default), Times.Once);
            _mockPersonManagerContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task GetPersonByIdAsync_ShouldReturnPersonFromDbContext()
        {
            // Arrange
            var person = new Person { Id = 1, FirstName = "John", LastName = "Doe", EmailAddress = "john.doe@example.com", DepartmentId = 1, IsActive = true };
            var mockDbSet = new Mock<DbSet<Person>>();
            mockDbSet.Setup(m => m.FindAsync(1)).ReturnsAsync(person);
            _mockPersonManagerContext.Setup(m => m.Set<Person>()).Returns(mockDbSet.Object);

            // Act
            var result = await _repository.GetPersonByIdAsync(1);

            // Assert
            Assert.Equal(person, result);
        }

        [Fact]
        public async Task UpdatePersonAsync_ShouldUpdatePersonInDbContext()
        {
            // Arrange
            var person = new Person { Id = 1, FirstName = "John", LastName = "Doe", EmailAddress = "john.doe@example.com", DepartmentId = 1, IsActive = true };
            var mockDbSet = new Mock<DbSet<Person>>();
            _mockPersonManagerContext.Setup(m => m.Set<Person>()).Returns(mockDbSet.Object);

            // Act
            await _repository.UpdatePersonAsync(person);

            // Assert
            mockDbSet.Verify(m => m.Update(person), Times.Once);
            _mockPersonManagerContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task SearchPeopleAsync_ShouldReturnPagedResponseModel()
        {
            // Arrange
            var searchPeopleModel = new SearchPeopleModel { Query = "John", Page = 1, PageSize = 10, DepartmentId = 1 };
            var people = new List<Person>
            {
                new Person { Id = 1, FirstName = "John", LastName = "Doe", EmailAddress = "john.doe@example.com", DepartmentId = 1, IsActive = true }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Person>>();
            mockDbSet.As<IQueryable<Person>>().Setup(m => m.Provider).Returns(people.Provider);
            mockDbSet.As<IQueryable<Person>>().Setup(m => m.Expression).Returns(people.Expression);
            mockDbSet.As<IQueryable<Person>>().Setup(m => m.ElementType).Returns(people.ElementType);
            mockDbSet.As<IQueryable<Person>>().Setup(m => m.GetEnumerator()).Returns(people.GetEnumerator());
            _mockPersonManagerContext.Setup(m => m.Set<Person>()).Returns(mockDbSet.Object);

            // Act
            var result = await _repository.SearchPeopleAsync(searchPeopleModel);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Items);
            Assert.Equal(1, result.TotalCount);
            Assert.Equal(1, result.TotalPages);
        }
    }
}
