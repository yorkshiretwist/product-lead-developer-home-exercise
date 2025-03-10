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
        var person = await _personService.GetPersonByIdAsync(id);
        if (person == null)
        {
            return NotFound();
        }

        return Ok(person);
    }

    [Route("{id:int}")]
    [HttpPut]
    public async Task<ActionResult<PersonViewModel>> UpdateAsync(PersonViewModel personViewModel)
    {
        var updatePersonResult = await _personService.UpdatePersonAsync(personViewModel);

        if (updatePersonResult.ValidationErrors != null && updatePersonResult.ValidationErrors.Any())
        {
            return BadRequest(updatePersonResult.ValidationErrors);
        }

        if (updatePersonResult.Person == null)
        {
            return StatusCode(500);
        }

        return Ok(updatePersonResult.Person);
    }

    [HttpPost]
    public async Task<ActionResult<PersonViewModel>> CreateAsync(PersonViewModel personViewModel)
    {
        var createPersonResult = await _personService.UpdatePersonAsync(personViewModel);

        if (createPersonResult.ValidationErrors != null && createPersonResult.ValidationErrors.Any())
        {
            return BadRequest(createPersonResult.ValidationErrors);
        }

        if (createPersonResult.Person == null)
        {
            return StatusCode(500);
        }

        return Ok(createPersonResult.Person);
    }

    [HttpPost]
    public async Task<ActionResult<PersonViewModel>> SearchAsync(SearchPeopleQueryViewModel searchPeopleViewModel)
    {
        var pagedResponse = await _personService.SearchPeopleAsync(searchPeopleViewModel);
        if (pagedResponse.TotalCount == 0)
        {
            return NotFound();
        }

        return Ok(pagedResponse);
    }
}