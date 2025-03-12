using System.ComponentModel.DataAnnotations;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Models;
using UKParliament.CodeTest.Web.ViewModels;

namespace UKParliament.CodeTest.Services
{
    public class PersonValidationService : IPersonValidationService
    {
        public static ValidationError NullPersonError = new ValidationError { Code = 1000, Description = "No details about the person have been submitted" };

        public static ValidationError InvalidIdError = new ValidationError { Code = 1001, Description = "A valid ID must be provided" };

        public static ValidationError InvalidFirstNameError = new ValidationError { Code = 1002, Description = "A valid first name must be provided" };

        public static ValidationError InvalidLastNameError = new ValidationError { Code = 1003, Description = "A valid last name must be provided" };

        public static ValidationError InvalidEmailAddressError = new ValidationError { Code = 1004, Description = "A valid email address must be provided" };

        public static ValidationError InvalidDepartmentIdError = new ValidationError { Code = 1005, Description = "A valid department must be provided" };

        public static ValidationError InvalidDepartmentError = new ValidationError { Code = 1006, Description = "The department you have chosen cannot be found" };

        private readonly IRepository _repository;

        public PersonValidationService(IRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Validate the given person
        /// </summary>
        /// <param name="personViewModel">The object representing the properties of the person</param>
        /// <param name="isNewPerson">A boolean indicating if this is a new person; when this is true the checks for a valid id property are skipped</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<ValidationError>> ValidatePersonAsync(PersonViewModel personViewModel, bool isNewPerson = false)
        {
            var errors = new List<ValidationError>();

            if (personViewModel == null)
            {
                errors.Add(NullPersonError);
                return errors;
            }

            if (!isNewPerson && personViewModel.Id <= 0)
            {
                errors.Add(InvalidIdError);
            }

            if (string.IsNullOrWhiteSpace(personViewModel.FirstName))
            {
                errors.Add(InvalidFirstNameError);
            }

            if (string.IsNullOrWhiteSpace(personViewModel.LastName))
            {
                errors.Add(InvalidLastNameError);
            }

            if (string.IsNullOrWhiteSpace(personViewModel.EmailAddress) || !new EmailAddressAttribute().IsValid(personViewModel.EmailAddress))
            {
                errors.Add(InvalidEmailAddressError);
            }

            if (personViewModel.DepartmentId <= 0)
            {
                errors.Add(InvalidDepartmentIdError);
            }
            else
            {
                var department = await _repository.GetDepartmentByIdAsync(personViewModel.DepartmentId);
                if (department == null)
                {
                    errors.Add(InvalidDepartmentError);
                }
            }

            return errors;
        }
    }
}
