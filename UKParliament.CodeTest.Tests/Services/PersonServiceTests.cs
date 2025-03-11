using AutoMapper;
using FluentAssertions;
using Moq;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Models;
using UKParliament.CodeTest.Services;
using Xunit;

namespace UKParliament.CodeTest.Tests
{
    public class PersonServiceTests : TestBase
    {
        private readonly Mock<IRepository> _mockRepository;
        private readonly Mock<IPersonValidationService> _mockPersonValidationService;
        private readonly PersonService _personService;

        public PersonServiceTests()
        {
            _mockRepository = new Mock<IRepository>();
            _mockPersonValidationService = new Mock<IPersonValidationService>();
            IMapper mapper = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile())).CreateMapper();
            _personService = new PersonService(_mockRepository.Object, _mockPersonValidationService.Object, mapper);
        }

        [Fact]
        public async Task CreatePersonAsync_WithValidModel_ShouldCallRepositoryCreatePersonAsync()
        {
            // Arrange
            var personToCreateViewModel = GetTestPersonViewModel(true);
            var personToCreate = GetTestPerson();
            personToCreate.Id = 0;
            var createdPerson = GetTestPerson();
            var createdPersonViewModel = GetTestPersonViewModel();
            var departments = GetTestDepartments();

            _mockRepository.Setup(repo => repo.CreatePersonAsync(It.IsAny<Person>())).ReturnsAsync(createdPerson);
            _mockRepository.Setup(repo => repo.GetDepartmentByIdAsync(personToCreateViewModel.DepartmentId)).ReturnsAsync(departments.First(x => x.Id == personToCreateViewModel.DepartmentId));
            _mockPersonValidationService.Setup(x => x.ValidatePersonAsync(personToCreateViewModel, true)).ReturnsAsync(new List<ValidationError>());

            // Act
            var result = await _personService.CreatePersonAsync(personToCreateViewModel);

            // Assert
            result.Should().NotBeNull();
            result.Person.Should().BeEquivalentTo(createdPersonViewModel);
            result.ValidationErrors.Should().BeNullOrEmpty();
            _mockRepository.Verify(repo => repo.CreatePersonAsync(It.Is<Person>(x => x.Should().BeEquivalentTo(personToCreate, "") != null)), Times.Once);
            _mockRepository.Verify(repo => repo.GetDepartmentByIdAsync(personToCreateViewModel.DepartmentId), Times.Once);
            _mockPersonValidationService.Verify(x => x.ValidatePersonAsync(personToCreateViewModel, true), Times.Once);
        }

        [Fact]
        public async Task CreatePersonAsync_WithInvalidModel_ShouldReturnValidationErrors()
        {
            // Arrange
            var personToCreateViewModel = GetTestPersonViewModel(true);
            var personToCreate = GetTestPerson();
            personToCreate.Id = 0;

            _mockRepository.Setup(repo => repo.CreatePersonAsync(It.IsAny<Person>())).ReturnsAsync(personToCreate);

            var validationErrors = new List<ValidationError>
            {
                PersonValidationService.InvalidEmailAddressError,
                PersonValidationService.InvalidDepartmentError
            };
            _mockPersonValidationService.Setup(x => x.ValidatePersonAsync(personToCreateViewModel, true)).ReturnsAsync(validationErrors);

            // Act
            var result = await _personService.CreatePersonAsync(personToCreateViewModel);

            // Assert
            result.Should().NotBeNull();
            result.Person.Should().BeNull();
            result.ValidationErrors.Should().BeEquivalentTo(validationErrors);
            _mockRepository.Verify(repo => repo.CreatePersonAsync(It.IsAny<Person>()), Times.Never);
            _mockRepository.Verify(repo => repo.GetDepartmentByIdAsync(It.IsAny<int>()), Times.Never);
            _mockPersonValidationService.Verify(x => x.ValidatePersonAsync(personToCreateViewModel, true), Times.Once);
        }

        // TODO: add tests for GetPersonByIdAsync, SearchPeopleAsync, and UpdatePersonAsync
    }
}
