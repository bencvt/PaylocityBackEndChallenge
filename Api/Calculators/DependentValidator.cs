using Api.Models;

namespace Api.Calculators;

public static class DependentValidator
{
    public static void ValidateDependents(CalculatorConfiguration settings, Employee employee)
    {
        if (!settings.EnforceMaxOneSpouseOrDomesticPartner)
        {
            return;
        }

        int count = employee.Dependents
            .Count(x => x.Relationship == Relationship.Spouse || x.Relationship == Relationship.DomesticPartner);

        if (count > 1)
        {
            throw new ArgumentException($"Employee {employee.Id} has {count} spouses and/or domestic partners; a maximum of one is allowed");
        }
    }
}
