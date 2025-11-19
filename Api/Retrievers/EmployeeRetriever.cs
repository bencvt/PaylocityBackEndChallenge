using Api.Models;

namespace Api.Retrievers;

public class EmployeeRetriever : Retriever<Employee>
{
    private static List<Employee> QueryResults => new()
    {
        new()
        {
            Id = 1,
            FirstName = "LeBron",
            LastName = "James",
            Salary = 75420.99m,
            DateOfBirth = new DateTime(1984, 12, 30),
        },
        new()
        {
            Id = 2,
            FirstName = "Ja",
            LastName = "Morant",
            Salary = 92365.22m,
            DateOfBirth = new DateTime(1999, 8, 10),
        },
        new()
        {
            Id = 3,
            FirstName = "Michael",
            LastName = "Jordan",
            Salary = 143211.12m,
            DateOfBirth = new DateTime(1963, 2, 17),
        },
    };

    public override IEnumerable<Employee> RetrieveAll()
    {
        foreach (var item in QueryResults)
        {
            var retriever = new DependentRetriever();
            retriever.RetrieveForEmployee(item);
            yield return item;
        }
    }
}
