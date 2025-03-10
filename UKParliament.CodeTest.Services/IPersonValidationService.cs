using UKParliament.CodeTest.Models;
using UKParliament.CodeTest.Web.ViewModels;

namespace UKParliament.CodeTest.Services
{
    public interface IPersonValidationService
    {
        Task<List<ValidationError>> ValidatePersonAsync(PersonViewModel personViewModel, bool isNewPerson = false);
    }
}
