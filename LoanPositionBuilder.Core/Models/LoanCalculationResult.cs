namespace LoanPositionBuilder.Core.Models;

public class LoanCalculationResult
{
    public decimal MaxLoanAmount { get; set; }
    public string LoanCurrency { get; set; } = string.Empty;
    public List<CollateralContribution> CollateralContributions { get; set; } = new();
    public decimal TotalPortfolioValue { get; set; }
    public decimal CollateralValueAtInitialMargin { get; set; }
    public decimal CollateralValueAtSoftMargin { get; set; }
    public decimal CollateralValueAtHardMargin { get; set; }
    public decimal CurrentCollateralization { get; set; }
    public decimal PercentageToInitialMargin { get; set; }
    public decimal PercentageToSoftMargin { get; set; }
    public decimal PercentageToHardMargin { get; set; }
    public decimal AbsoluteAmountToInitialMargin { get; set; }
    public decimal AbsoluteAmountToSoftMargin { get; set; }
    public decimal AbsoluteAmountToHardMargin { get; set; }
}

public class CollateralContribution
{
    public string Symbol { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal MarketValue { get; set; }
    public decimal LendingValue { get; set; }
    public decimal InitialMarginLtv { get; set; }
    public string Currency { get; set; } = string.Empty;
}