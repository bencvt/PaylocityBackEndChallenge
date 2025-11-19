using Api.Models;

namespace Api.Retrievers;

public class DependentRetriever : Retriever<Dependent>
{
    private static List<Dependent> QueryResults => new()
    {
        new()
        {
            Id = 1,
            FirstName = "Spouse",
            LastName = "Morant",
            Relationship = Relationship.Spouse,
            DateOfBirth = new DateTime(1998, 3, 3),
            EmployeeId = 2,
        },
        new()
        {
            Id = 2,
            FirstName = "Child1",
            LastName = "Morant",
            Relationship = Relationship.Child,
            DateOfBirth = new DateTime(2020, 6, 23),
            EmployeeId = 2,
        },
        new()
        {
            Id = 3,
            FirstName = "Child2",
            LastName = "Morant",
            Relationship = Relationship.Child,
            DateOfBirth = new DateTime(2021, 5, 18),
            EmployeeId = 2,
        },
        new()
        {
            Id = 4,
            FirstName = "DP",
            LastName = "Jordan",
            Relationship = Relationship.DomesticPartner,
            DateOfBirth = new DateTime(1974, 1, 2),
            EmployeeId = 3,
        },
    };

    public override IEnumerable<Dependent> RetrieveAll()
    {
        foreach (var item in QueryResults)
        {
            // Employee will be null
            yield return item;
        }
    }
    public void RetrieveForEmployee(Employee employee)
    {
        foreach (var item in QueryResults.Where(x => x.EmployeeId == employee.Id))
        {
            item.Employee = employee;
            employee.Dependents.Add(item);
        }
    }
}
