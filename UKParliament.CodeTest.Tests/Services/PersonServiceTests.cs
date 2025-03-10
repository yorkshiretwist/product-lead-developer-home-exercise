using System.Threading.Tasks;
using Moq;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Models;
using UKParliament.CodeTest.Services;
using Xunit;

namespace UKParliament.CodeTest.Tests
{
    public class PersonServiceTests
    {
        private readonly Mock<IRepository> _mockRepository;
        private readonly PersonService _personService;

        public PersonServiceTests()
        {
            _mockRepository = new Mock<IRepository>();
            _personService = new PersonService(_mockRepository.Object);
        }

        [Fact]
        public async Task CreatePersonAsync_ShouldCallRepositoryCreatePersonAsync()
        {
            // Arrange
            var person = new Person { Id = 1, FirstName = "John", LastName = "Doe", EmailAddress = "john.doe@example.com", DepartmentId = 1, IsActive = true };
            _mockRepository.Setup(repo => repo.CreatePersonAsync(person)).ReturnsAsync(person);

            // Act
            var result = await _personService.CreatePersonAsync(person);

            // Assert
            Assert.Equal(person, result);
            _mockRepository.Verify(repo => repo.CreatePersonAsync(person), Times.Once);
        }

        [Fact]
        public async Task GetPersonByIdAsync_ShouldCallRepositoryGetPersonByIdAsync()
        {
            // Arrange
            var person = new Person { Id = 1, FirstName = "John", LastName = "Doe", EmailAddress = "john.doe@example.com", DepartmentId = 1, IsActive = true };
            _mockRepository.Setup(repo => repo.GetPersonByIdAsync(1)).ReturnsAsync(person);

            // Act
            var result = await _personService.GetPersonByIdAsync(1);

            // Assert
            Assert.Equal(person, result);
            _mockRepository.Verify(repo => repo.GetPersonByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task UpdatePersonAsync_ShouldCallRepositoryUpdatePersonAsync()
        {
            // Arrange
            var person = new Person { Id = 1, FirstName = "John", LastName = "Doe", EmailAddress = "john.doe@example.com", DepartmentId = 1, IsActive = true };
            _mockRepository.Setup(repo => repo.UpdatePersonAsync(person)).ReturnsAsync(person);

            // Act
            var result = await _personService.UpdatePersonAsync(person);

            // Assert
            Assert.Equal(person, result);
            _mockRepository.Verify(repo => repo.UpdatePersonAsync(person), Times.Once);
        }

        [Fact]
        public async Task SearchPeopleAsync_ShouldCallRepositorySearchPeopleAsync()
        {
            // Arrange
            var searchPeopleModel = new SearchPeopleModel { Query = "John", Page = 1, PageSize = 10, DepartmentId = 1 };
            var pagedResponse = new PagedResponseModel<Person>
            {
                Page = 1,
                PageSize = 10,
                TotalCount = 1,
                TotalPages = 1,
                Items = new List<Person>
                {
                    new Person { Id = 1, FirstName = "John", LastName = "Doe", EmailAddress = "john.doe@example.com", DepartmentId = 1, IsActive = true }
                }
            };
            _mockRepository.Setup(repo => repo.SearchPeopleAsync(searchPeopleModel)).ReturnsAsync(pagedResponse);

            // Act
            var result = await _personService.SearchPeopleAsync(searchPeopleModel);

            // Assert
            Assert.Equal(pagedResponse, result);
            _mockRepository.Verify(repo => repo.SearchPeopleAsync(searchPeopleModel), Times.Once);
        }
    }
}
