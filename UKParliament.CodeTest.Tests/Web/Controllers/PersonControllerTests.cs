using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UKParliament.CodeTest.Services;
using UKParliament.CodeTest.Web.Controllers;
using UKParliament.CodeTest.Web.ViewModels;
using Xunit;

namespace UKParliament.CodeTest.Tests
{
    public class PersonControllerTests : TestBase
    {
        private readonly Mock<IPersonService> _mockPersonService;
        private readonly PersonController _personController;

        public PersonControllerTests()
        {
            _mockPersonService = new Mock<IPersonService>();
            _personController = new PersonController(_mockPersonService.Object);
        }

        [Fact]
        public async Task GetByIdAsync_PersonFound_ShouldReturn200()
        {
            // Arrange
            var person = GetTestPersonViewModel();
            _mockPersonService.Setup(service => service.GetPersonByIdAsync(person.Id)).ReturnsAsync(person);

            // Act
            var result = await _personController.GetByIdAsync(person.Id);

            // Assert
            _mockPersonService.Verify(service => service.GetPersonByIdAsync(person.Id), Times.Once);
            result.Should().NotBeNull();
            var actionResult = Assert.IsType<ActionResult<PersonViewModel>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var foundPerson = Assert.IsType<PersonViewModel>(okResult.Value);
            foundPerson.Should().NotBeNull();
            foundPerson.Should().BeEquivalentTo(person);
        }

        [Fact]
        public async Task GetByIdAsync_PersonNotFound_ShouldReturn404()
        {
            // Arrange
            var person = GetTestPersonViewModel();
            _mockPersonService.Setup(service => service.GetPersonByIdAsync(person.Id)).ReturnsAsync(default(PersonViewModel));

            // Act
            var result = await _personController.GetByIdAsync(person.Id);

            // Assert
            _mockPersonService.Verify(service => service.GetPersonByIdAsync(person.Id), Times.Once);
            result.Should().NotBeNull();
            var actionResult = Assert.IsType<ActionResult<PersonViewModel>>(result);
            var okResult = Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        // TODO: Add tests for UpdateAsync, CreateAsync, and SearchAsync
    }
}
