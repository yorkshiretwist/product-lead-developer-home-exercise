namespace UKParliament.CodeTest.Web.ViewModels
{
    public class PagedResponseViewModel<T>
    {
        public int Page { get; set; }
        
        public int PageSize { get; set; }
        
        public int TotalCount { get; set; }
        
        public int TotalPages { get; set; }

        public ICollection<T> Items { get; set; }
    }
}
