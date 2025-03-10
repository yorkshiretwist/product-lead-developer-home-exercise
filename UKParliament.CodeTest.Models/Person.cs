﻿namespace UKParliament.CodeTest.Models;

public class Person
{
    public int Id { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public required string EmailAddress { get; set; }

    public int DepartmentId { get; set; }

    public bool IsActive { get; set; }
}