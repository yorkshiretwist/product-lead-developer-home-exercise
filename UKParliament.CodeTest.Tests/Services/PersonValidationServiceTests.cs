using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Moq;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Models;
using UKParliament.CodeTest.Services;
using UKParliament.CodeTest.Web.ViewModels;
using Xunit;

namespace UKParliament.CodeTest.Tests
{
    public class PersonValidationServiceTests : TestBase
    {
        private readonly Mock<IRepository> _mockRepository;
        private readonly PersonValidationService _personValidationService;

        public PersonValidationServiceTests()
        {
            _mockRepository = new Mock<IRepository>();
            _personValidationService = new PersonValidationService(_mockRepository.Object);
        }

        private void SetupValidDepartment(PersonViewModel personViewModel)
        {
            var department = GetTestDepartments().First(x => x.Id == personViewModel.Department.Id);
            _mockRepository.Setup(repo => repo.GetDepartmentByIdAsync(personViewModel.Department.Id)).ReturnsAsync(department);
        }

        [Fact]
        public async Task ValidatePersonAsync_ShouldReturnNullPersonError_WhenPersonIsNull()
        {
            // Act
            var result = await _personValidationService.ValidatePersonAsync(null);

            // Assert
            Assert.Single(result);
            Assert.Equal(PersonValidationService.NullPersonError, result[0]);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public async Task ValidatePersonAsync_ShouldReturnInvalidIdError_WhenIdIsInvalid(int id)
        {
            // Arrange
            var personViewModel = GetTestPersonViewModel();
            personViewModel.Id = id;
            SetupValidDepartment(personViewModel);

            // Act
            var result = await _personValidationService.ValidatePersonAsync(personViewModel);

            // Assert
            Assert.Contains(result, e => e.Code == PersonValidationService.InvalidIdError.Code);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ValidatePersonAsync_ShouldReturnInvalidFirstNameError_WhenFirstNameIsNullOrEmpty(string firstName)
        {
            // Arrange
            var personViewModel = GetTestPersonViewModel();
            personViewModel.FirstName = firstName;
            SetupValidDepartment(personViewModel);

            // Act
            var result = await _personValidationService.ValidatePersonAsync(personViewModel);

            // Assert
            Assert.Contains(result, e => e.Code == PersonValidationService.InvalidFirstNameError.Code);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ValidatePersonAsync_ShouldReturnInvalidLastNameError_WhenLastNameIsNullOrEmpty(string lastName)
        {
            // Arrange
            var personViewModel = GetTestPersonViewModel();
            personViewModel.LastName = lastName;
            SetupValidDepartment(personViewModel);

            // Act
            var result = await _personValidationService.ValidatePersonAsync(personViewModel);

            // Assert
            Assert.Contains(result, e => e.Code == PersonValidationService.InvalidLastNameError.Code);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("invalid")]
        [InlineData("with spaces")]
        [InlineData("name@")]
        [InlineData("@domain")]
        public async Task ValidatePersonAsync_ShouldReturnInvalidEmailAddressError_WhenEmailAddressIsInvalid(string emailAddress)
        {
            // Arrange
            var personViewModel = GetTestPersonViewModel();
            personViewModel.EmailAddress = emailAddress;
            SetupValidDepartment(personViewModel);

            // Act
            var result = await _personValidationService.ValidatePersonAsync(personViewModel);

            // Assert
            Assert.Contains(result, e => e.Code == PersonValidationService.InvalidEmailAddressError.Code);
        }

        [Fact]
        public async Task ValidatePersonAsync_ShouldReturnInvalidDepartmentIdError_WhenDepartmentIsNull()
        {
            // Arrange
            var personViewModel = GetTestPersonViewModel();
            personViewModel.Department = null;

            // Act
            var result = await _personValidationService.ValidatePersonAsync(personViewModel);

            // Assert
            Assert.Contains(result, e => e.Code == PersonValidationService.InvalidDepartmentIdError.Code);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public async Task ValidatePersonAsync_ShouldReturnInvalidDepartmentIdError_WhenDepartmentIdIsInvalid(int departmentId)
        {
            // Arrange
            var personViewModel = GetTestPersonViewModel();
            personViewModel.Department.Id = departmentId;

            // Act
            var result = await _personValidationService.ValidatePersonAsync(personViewModel);

            // Assert
            Assert.Contains(result, e => e.Code == PersonValidationService.InvalidDepartmentIdError.Code);
        }

        [Fact]
        public async Task ValidatePersonAsync_ShouldReturnInvalidDepartmentError_WhenDepartmentDoesNotExist()
        {
            // Arrange
            var personViewModel = GetTestPersonViewModel();
            _mockRepository.Setup(repo => repo.GetDepartmentByIdAsync(1)).ReturnsAsync((Department)null);

            // Act
            var result = await _personValidationService.ValidatePersonAsync(personViewModel);

            // Assert
            Assert.Contains(result, e => e.Code == PersonValidationService.InvalidDepartmentError.Code);
        }

        [Fact]
        public async Task ValidatePersonAsync_ShouldReturnNoErrors_WhenPersonIsValid()
        {
            // Arrange
            var personViewModel = GetTestPersonViewModel();
            SetupValidDepartment(personViewModel);

            // Act
            var result = await _personValidationService.ValidatePersonAsync(personViewModel);

            // Assert
            Assert.Empty(result);
        }
    }
}
