using LoanPositionBuilder.Core.Models;

namespace LoanPositionBuilder.Core.Services;

public interface ILoanPositionBuilder
{
    Task<LoanCalculationResult> CalculateMaxLoanAmount(
        IEnumerable<CollateralAsset> collateralAssets,
        string loanCurrency,
        string tenor);

    Task<LoanCalculationResult> CalculateDrawdownScenario(
        IEnumerable<CollateralAsset> collateralAssets,
        decimal loanAmount,
        string loanCurrency,
        string tenor);
}