namespace UKParliament.CodeTest.Web.ViewModels
{
    public class CreateOrUpdatePersonResult
    {
        public PersonViewModel? Person { get; set; }

        public ICollection<ValidationErrorViewModel>? ValidationErrors { get; set; }
    }
}
