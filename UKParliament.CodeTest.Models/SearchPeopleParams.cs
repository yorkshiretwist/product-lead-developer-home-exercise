﻿namespace UKParliament.CodeTest.Models
{
    public class SearchPeopleParams
    {
        /// <summary>
        /// The text query, which searches email addresses, first names, and last names
        /// </summary>
        public string? Query { get; set; } = null;

        /// <summary>
        /// The page number to return; default is 1
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// The number of people to return; defauult is 25
        /// </summary>
        public int PageSize { get; set; } = 25;

        /// <summary>
        /// The department id for which to fetch people; default is null (all departments)
        /// </summary>
        public int? DepartmentId { get; set; } = null;

        /// <summary>
        /// A boolean indicating whether to only fetch active people or not; default is false (all people, irrespective of their active status)
        /// </summary>
        public bool OnlyActive { get; set; }
    }
}
