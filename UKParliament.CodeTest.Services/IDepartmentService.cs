using UKParliament.CodeTest.Web.ViewModels;

namespace UKParliament.CodeTest.Services
{
    public interface IDepartmentService
    {
        Task<ICollection<DepartmentViewModel>> GetDepartmentsAsync();
    }
}
