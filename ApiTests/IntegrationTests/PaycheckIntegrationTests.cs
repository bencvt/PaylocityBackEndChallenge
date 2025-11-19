using Api.Dtos.Deduction;
using Api.Dtos.Paycheck;
using Api.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ApiTests.IntegrationTests;

public class PaycheckIntegrationTests : IntegrationTest
{
    [Fact]
    public async Task WhenAskedForAnEmployeeWithoutDependentsPaycheck_ShouldReturnCorrectEmployeePaycheck()
    {
        var response = await HttpClient.GetAsync("/api/v1/paychecks/1/2025-12-25");
        var paycheck = new GetPaycheckDto
        {
            GrossPay = 2900.99m,
            NetPay = 2862.49m,
            CheckDate = DateTime.Parse("2025-12-31"),
            PayPeriod = 26,
            Deductions = new List<GetDeductionDto>()
            {
                new()
                {
                    Amount = 38.50m,
                    DeductionReason = DeductionReason.BaseBenefits,
                },
            },
        };
        await response.ShouldReturn(HttpStatusCode.OK, paycheck);
    }

    [Fact]
    public async Task WhenAskedForAnEmployeeWithDependentsPaycheck_ShouldReturnCorrectEmployeePaycheck()
    {
        var response = await HttpClient.GetAsync("/api/v1/paychecks/2/2025-12-25");
        var paycheck = new GetPaycheckDto
        {
            GrossPay = 3552.72m,
            NetPay = 3373.42m,
            CheckDate = DateTime.Parse("2025-12-31"),
            PayPeriod = 26,
            Deductions = new List<GetDeductionDto>()
            {
                new()
                {
                    Amount = 38.50m,
                    DeductionReason = DeductionReason.BaseBenefits,
                },
                new()
                {
                    Amount = 23.25m,
                    DeductionReason = DeductionReason.DependentBenefits,
                    Dependent = new()
                    {
                        Id = 1,
                        FirstName = "Spouse",
                        LastName = "Morant",
                        DateOfBirth = DateTime.Parse("1998-03-03"),
                        Relationship = Relationship.Spouse,
                    },
                },
                new()
                {
                    Amount = 23.25m,
                    DeductionReason = DeductionReason.DependentBenefits,
                    Dependent = new()
                    {
                        Id = 2,
                        FirstName = "Child1",
                        LastName = "Morant",
                        DateOfBirth = DateTime.Parse("2020-06-23"),
                        Relationship = Relationship.Child,
                    },
                },
                new()
                {
                    Amount = 23.25m,
                    DeductionReason = DeductionReason.DependentBenefits,
                    Dependent = new()
                    {
                        Id = 3,
                        FirstName = "Child2",
                        LastName = "Morant",
                        DateOfBirth = DateTime.Parse("2021-05-18"),
                        Relationship = Relationship.Child,
                    },
                },
                new()
                {
                    Amount = 71.05m,
                    DeductionReason = DeductionReason.HighIncome,
                },
            },
        };
        await response.ShouldReturn(HttpStatusCode.OK, paycheck);
    }

    [Fact]
    public async Task WhenAskedForANonexistentEmployeePaycheck_ShouldReturn404()
    {
        var response = await HttpClient.GetAsync($"/api/v1/paychecks/{int.MinValue}/2025-12-25");
        await response.ShouldReturn(HttpStatusCode.NotFound);
    }
}
