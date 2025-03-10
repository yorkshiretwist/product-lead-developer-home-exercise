﻿namespace UKParliament.CodeTest.Models
{
    public class PagedResult<T>
    {
        public int Page { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public int TotalPages { get; set; }

        public ICollection<T> Items { get; set; }
    }
}
