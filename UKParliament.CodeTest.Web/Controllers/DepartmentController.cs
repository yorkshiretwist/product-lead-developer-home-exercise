using Microsoft.AspNetCore.Mvc;
using UKParliament.CodeTest.Services;
using UKParliament.CodeTest.Web.ViewModels;

namespace UKParliament.CodeTest.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [Route("{id:int}")]
        [HttpGet]
        public async Task<ActionResult<DepartmentViewModel>> GetByIdAsync(int id)
        {
            var departments = await _departmentService.GetDepartmentsAsync();
            if (departments == null)
            {
                return NotFound();
            }
            return Ok(departments);
        }
    }
}
