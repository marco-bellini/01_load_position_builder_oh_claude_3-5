using Microsoft.AspNetCore.Mvc;
using LoanPositionBuilder.Core.Models;
using LoanPositionBuilder.Core.Services;

namespace LoanPositionBuilder.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoanCalculatorController : ControllerBase
{
    private readonly ILoanPositionBuilder _loanPositionBuilder;

    public LoanCalculatorController(ILoanPositionBuilder loanPositionBuilder)
    {
        _loanPositionBuilder = loanPositionBuilder;
    }

    [HttpPost("max-loan-amount")]
    public async Task<ActionResult<LoanCalculationResult>> CalculateMaxLoanAmount(
        [FromBody] MaxLoanAmountRequest request)
    {
        try
        {
            var result = await _loanPositionBuilder.CalculateMaxLoanAmount(
                request.CollateralAssets,
                request.LoanCurrency,
                request.Tenor);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("drawdown-scenario")]
    public async Task<ActionResult<LoanCalculationResult>> CalculateDrawdownScenario(
        [FromBody] DrawdownScenarioRequest request)
    {
        try
        {
            var result = await _loanPositionBuilder.CalculateDrawdownScenario(
                request.CollateralAssets,
                request.LoanAmount,
                request.LoanCurrency,
                request.Tenor);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}

public class MaxLoanAmountRequest
{
    public List<CollateralAsset> CollateralAssets { get; set; } = new();
    public string LoanCurrency { get; set; } = string.Empty;
    public string Tenor { get; set; } = string.Empty;
}

public class DrawdownScenarioRequest : MaxLoanAmountRequest
{
    public decimal LoanAmount { get; set; }
}