using Bogus;
using Microsoft.EntityFrameworkCore;
using UKParliament.CodeTest.Models;

// clear up ambiguity between UKParliament.CodeTest.Models.Person and Bogus.Person
using Person = UKParliament.CodeTest.Models.Person;

namespace UKParliament.CodeTest.Data;

public class PersonManagerContext : DbContext
{
    public PersonManagerContext(DbContextOptions<PersonManagerContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // add the departments to the database
        modelBuilder.Entity<Department>().HasData(
            new Department { Id = 1, Name = "Sales" },
            new Department { Id = 2, Name = "Marketing" },
            new Department { Id = 3, Name = "Finance" },
            new Department { Id = 4, Name = "HR" });

        // add 99 fake people to the database
        // adding this many people allows us to implement pagination
        var faker = new Faker();
        for (var i = 1; i < 100; i++)
        {
            modelBuilder.Entity<Person>().HasData(new Person
            {
                Id = i,
                FirstName = faker.Name.FirstName(),
                LastName = faker.Name.LastName(),
                DepartmentId = faker.Random.Number(1, 4),
                EmailAddress = faker.Internet.Email()
            });
        }
    }

    public DbSet<Person> People { get; set; }

    public DbSet<Department> Departments { get; set; }
}