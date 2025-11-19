using Api.Calculators;
using Api.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace ApiTests.UnitTests.Calculators;

public class DependentValidatorTests
{
    [Theory]
    [MemberData(nameof(ValidDependentSets))]
    public void WhenValidatingAValidSetOfDependents_ShouldNotThrow(IEnumerable<Relationship> relationships)
    {
        var employee = new Employee();
        foreach (var relationship in relationships)
        {
            employee.Dependents.Add(new()
            {
                Relationship = relationship,
            });
        }

        DependentValidator.ValidateDependents(employee);
    }

    [Theory]
    [MemberData(nameof(InvalidDependentSets))]
    public void WhenValidatingAnInvalidSetOfDependents_ShouldThrowArgumentException(IEnumerable<Relationship> relationships)
    {
        var employee = new Employee();
        foreach (var relationship in relationships)
        {
            employee.Dependents.Add(new()
            {
                Relationship = relationship,
            });
        }

        Assert.Throws<ArgumentException>(() => DependentValidator.ValidateDependents(employee));
    }

    public static IEnumerable<object[]> ValidDependentSets => new List<object[]>
    {
        new object[] { new List<Relationship>() },
        new object[] { new List<Relationship>{ Relationship.Child } },
        new object[] { new List<Relationship>{ Relationship.Child, Relationship.Child } },

        new object[] { new List<Relationship>{ Relationship.Spouse } },
        new object[] { new List<Relationship>{ Relationship.Spouse, Relationship.Child } },
        new object[] { new List<Relationship>{ Relationship.Spouse, Relationship.Child, Relationship.Child } },
        new object[] { new List<Relationship>{ Relationship.Child, Relationship.Spouse } },

        new object[] { new List<Relationship>{ Relationship.DomesticPartner } },
        new object[] { new List<Relationship>{ Relationship.DomesticPartner, Relationship.Child } },
        new object[] { new List<Relationship>{ Relationship.DomesticPartner, Relationship.Child, Relationship.Child } },
        new object[] { new List<Relationship>{ Relationship.Child, Relationship.DomesticPartner } },
    };

    public static IEnumerable<object[]> InvalidDependentSets => new List<object[]>
    {
        new object[] { new List<Relationship>{ Relationship.Spouse, Relationship.Spouse } },
        new object[] { new List<Relationship>{ Relationship.Spouse, Relationship.Spouse, Relationship.Child } },
        new object[] { new List<Relationship>{ Relationship.Spouse, Relationship.Child, Relationship.Spouse } },

        new object[] { new List<Relationship>{ Relationship.DomesticPartner, Relationship.DomesticPartner } },
        new object[] { new List<Relationship>{ Relationship.DomesticPartner, Relationship.DomesticPartner, Relationship.Child } },
        new object[] { new List<Relationship>{ Relationship.DomesticPartner, Relationship.Child, Relationship.DomesticPartner } },

        new object[] { new List<Relationship>{ Relationship.Spouse, Relationship.DomesticPartner } },
        new object[] { new List<Relationship>{ Relationship.Spouse, Relationship.DomesticPartner, Relationship.Child } },
        new object[] { new List<Relationship>{ Relationship.Spouse, Relationship.Child, Relationship.DomesticPartner } },
    };
}
