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
    public async Task WhenAskedForAnEmployeePaycheck_ShouldReturnCorrectEmployeePaycheck()
    {
        var response = await HttpClient.GetAsync("/api/v1/paychecks/1/2025-12-25");
        var paycheck = new GetPaycheckDto
        {
            GrossPay = 2900.8073076923076923076923077m,
            NetPay = 2862.3457692307692307692307692m,
            CheckDate = DateTime.Parse("2025-12-31"),
            PayPeriod = 26,
            Deductions = new List<GetDeductionDto>()
            {
                new()
                {
                    Amount = 38.461538461538461538461538462m,
                    DeductionReason = DeductionReason.BaseBenefits,
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
