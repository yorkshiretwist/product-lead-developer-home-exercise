using AutoMapper;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Web.ViewModels;

namespace UKParliament.CodeTest.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public DepartmentService(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ICollection<DepartmentViewModel>> GetDepartmentsAsync()
        {
            var departments = await _repository.GetDepartmentsAsync();
            if (departments == null || !departments.Any())
            {
                return null;
            }

            return departments.Select(x => _mapper.Map<DepartmentViewModel>(x)).ToList();
        }
    }
}
