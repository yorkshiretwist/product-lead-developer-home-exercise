using Microsoft.AspNetCore.Mvc;
using UKParliament.CodeTest.Services;
using UKParliament.CodeTest.Web.ViewModels;

namespace UKParliament.CodeTest.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonController : ControllerBase
{
    private readonly IPersonService _personService;

    public PersonController(IPersonService personService)
    {
        _personService = personService;
    }

    [Route("{id:int}")]
    [HttpGet]
    public async Task<ActionResult<PersonViewModel>> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    [Route("{id:int}")]
    [HttpPut]
    public async Task<ActionResult<PersonViewModel>> UpdateAsync(PersonViewModel personViewModel)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    public async Task<ActionResult<PersonViewModel>> CreateAsync(PersonViewModel personViewModel)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    public async Task<ActionResult<PersonViewModel>> SearchAsync(SearchPeopleViewModel searchPeopleViewModel)
    {
        throw new NotImplementedException();
    }
}