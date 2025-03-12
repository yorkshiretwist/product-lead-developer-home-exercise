using AutoMapper;
using FluentAssertions;
using Moq;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Models;
using UKParliament.CodeTest.Services;
using UKParliament.CodeTest.Web.ViewModels;
using Xunit;

namespace UKParliament.CodeTest.Tests
{
    public class DepartmentServiceTests : TestBase
    {
        private readonly Mock<IRepository> _mockRepository;
        private readonly IMapper _mapper;
        private readonly DepartmentService _departmentService;

        public DepartmentServiceTests()
        {
            _mockRepository = new Mock<IRepository>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile())).CreateMapper();
            _departmentService = new DepartmentService(_mockRepository.Object, _mapper);
        }

        [Fact]
        public async Task GetDepartmentsAsync_ShouldCallRepositoryGetDepartmentsAsync()
        {
            // Arrange
            var departments = GetTestDepartments();
            _mockRepository.Setup(repo => repo.GetDepartmentsAsync()).ReturnsAsync(departments);

            // Act
            var result = await _departmentService.GetDepartmentsAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(departments.Select(x => _mapper.Map<DepartmentViewModel>(x)).ToList());
            _mockRepository.Verify(repo => repo.GetDepartmentsAsync(), Times.Once);
        }
    }
}
