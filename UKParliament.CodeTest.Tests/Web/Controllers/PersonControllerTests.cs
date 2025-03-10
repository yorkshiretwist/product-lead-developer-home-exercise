using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UKParliament.CodeTest.Models;
using UKParliament.CodeTest.Services;
using UKParliament.CodeTest.Web.Controllers;
using UKParliament.CodeTest.Web.ViewModels;
using Xunit;

namespace UKParliament.CodeTest.Tests
{
    public class PersonControllerTests
    {
        private readonly Mock<IPersonService> _mockPersonService;
        private readonly PersonController _personController;

        public PersonControllerTests()
        {
            _mockPersonService = new Mock<IPersonService>();
            _personController = new PersonController(_mockPersonService.Object);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnPersonViewModel()
        {
            // Arrange
            var person = new Person { Id = 1, FirstName = "John", LastName = "Doe", EmailAddress = "john.doe@example.com", DepartmentId = 1, IsActive = true };
            _mockPersonService.Setup(service => service.GetPersonByIdAsync(1)).ReturnsAsync(person);

            // Act
            var result = await _personController.GetByIdAsync(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<PersonViewModel>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<PersonViewModel>(okResult.Value);
            Assert.Equal(person.Id, returnValue.Id);
            Assert.Equal(person.FirstName, returnValue.FirstName);
            Assert.Equal(person.LastName, returnValue.LastName);
            Assert.Equal(person.EmailAddress, returnValue.EmailAddress);
            Assert.Equal(person.IsActive, returnValue.IsActive);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnUpdatedPersonViewModel()
        {
            // Arrange
            var personViewModel = new PersonViewModel { Id = 1, FirstName = "John", LastName = "Doe", EmailAddress = "john.doe@example.com", IsActive = true };
            var person = new Person { Id = 1, FirstName = "John", LastName = "Doe", EmailAddress = "john.doe@example.com", DepartmentId = 1, IsActive = true };
            _mockPersonService.Setup(service => service.UpdatePersonAsync(It.IsAny<Person>())).ReturnsAsync(person);

            // Act
            var result = await _personController.UpdateAsync(personViewModel);

            // Assert
            var actionResult = Assert.IsType<ActionResult<PersonViewModel>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<PersonViewModel>(okResult.Value);
            Assert.Equal(person.Id, returnValue.Id);
            Assert.Equal(person.FirstName, returnValue.FirstName);
            Assert.Equal(person.LastName, returnValue.LastName);
            Assert.Equal(person.EmailAddress, returnValue.EmailAddress);
            Assert.Equal(person.IsActive, returnValue.IsActive);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnCreatedPersonViewModel()
        {
            // Arrange
            var personViewModel = new PersonViewModel { Id = 1, FirstName = "John", LastName = "Doe", EmailAddress = "john.doe@example.com", IsActive = true };
            var person = new Person { Id = 1, FirstName = "John", LastName = "Doe", EmailAddress = "john.doe@example.com", DepartmentId = 1, IsActive = true };
            _mockPersonService.Setup(service => service.CreatePersonAsync(It.IsAny<Person>())).ReturnsAsync(person);

            // Act
            var result = await _personController.CreateAsync(personViewModel);

            // Assert
            var actionResult = Assert.IsType<ActionResult<PersonViewModel>>(result);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var returnValue = Assert.IsType<PersonViewModel>(createdAtActionResult.Value);
            Assert.Equal(person.Id, returnValue.Id);
            Assert.Equal(person.FirstName, returnValue.FirstName);
            Assert.Equal(person.LastName, returnValue.LastName);
            Assert.Equal(person.EmailAddress, returnValue.EmailAddress);
            Assert.Equal(person.IsActive, returnValue.IsActive);
        }

        [Fact]
        public async Task SearchAsync_ShouldReturnPagedResponseModel()
        {
            // Arrange
            var searchPeopleViewModel = new SearchPeopleViewModel { Query = "John", Page = 1, PageSize = 10, DepartmentId = 1 };
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
            _mockPersonService.Setup(service => service.SearchPeopleAsync(It.IsAny<SearchPeopleModel>())).ReturnsAsync(pagedResponse);

            // Act
            var result = await _personController.SearchAsync(searchPeopleViewModel);

            // Assert
            var actionResult = Assert.IsType<ActionResult<PagedResponseModel<PersonViewModel>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<PagedResponseModel<PersonViewModel>>(okResult.Value);
            Assert.Single(returnValue.Items);
            Assert.Equal(1, returnValue.TotalCount);
            Assert.Equal(1, returnValue.TotalPages);
        }
    }
}
