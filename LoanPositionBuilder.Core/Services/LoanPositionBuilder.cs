using LoanPositionBuilder.Core.Models;

namespace LoanPositionBuilder.Core.Services;

public class LoanPositionBuilder : ILoanPositionBuilder
{
    public async Task<LoanCalculationResult> CalculateMaxLoanAmount(
        IEnumerable<CollateralAsset> collateralAssets,
        string loanCurrency,
        string tenor)
    {
        var result = new LoanCalculationResult
        {
            LoanCurrency = loanCurrency,
            CollateralContributions = new List<CollateralContribution>()
        };

        foreach (var asset in collateralAssets)
        {
            var marketValue = asset.GetMarketValue();
            var lendingValue = asset.GetLendingValue();

            result.CollateralContributions.Add(new CollateralContribution
            {
                Symbol = asset.Symbol,
                Quantity = asset.Quantity,
                MarketValue = marketValue,
                LendingValue = lendingValue,
                InitialMarginLtv = asset.InitialMarginLtv,
                Currency = asset.PriceCurrency
            });

            result.MaxLoanAmount += lendingValue;
            result.TotalPortfolioValue += marketValue;
        }

        result.CollateralValueAtInitialMargin = result.MaxLoanAmount;
        result.CollateralValueAtSoftMargin = CalculateSoftMarginValue(collateralAssets);
        result.CollateralValueAtHardMargin = CalculateHardMarginValue(collateralAssets);

        return result;
    }

    public async Task<LoanCalculationResult> CalculateDrawdownScenario(
        IEnumerable<CollateralAsset> collateralAssets,
        decimal loanAmount,
        string loanCurrency,
        string tenor)
    {
        var result = await CalculateMaxLoanAmount(collateralAssets, loanCurrency, tenor);
        
        if (loanAmount > result.MaxLoanAmount)
        {
            throw new InvalidOperationException("Loan amount exceeds maximum borrowable amount");
        }

        result.CurrentCollateralization = result.TotalPortfolioValue / loanAmount;

        var valueAtIM = result.CollateralValueAtInitialMargin;
        var valueAtSM = result.CollateralValueAtSoftMargin;
        var valueAtHM = result.CollateralValueAtHardMargin;

        result.PercentageToInitialMargin = CalculatePercentageDecrease(result.TotalPortfolioValue, valueAtIM);
        result.PercentageToSoftMargin = CalculatePercentageDecrease(result.TotalPortfolioValue, valueAtSM);
        result.PercentageToHardMargin = CalculatePercentageDecrease(result.TotalPortfolioValue, valueAtHM);

        result.AbsoluteAmountToInitialMargin = result.TotalPortfolioValue - valueAtIM;
        result.AbsoluteAmountToSoftMargin = result.TotalPortfolioValue - valueAtSM;
        result.AbsoluteAmountToHardMargin = result.TotalPortfolioValue - valueAtHM;

        return result;
    }

    private decimal CalculateSoftMarginValue(IEnumerable<CollateralAsset> assets)
    {
        return assets.Sum(asset => asset.GetMarketValue() * asset.SoftMarginLtv);
    }

    private decimal CalculateHardMarginValue(IEnumerable<CollateralAsset> assets)
    {
        return assets.Sum(asset => asset.GetMarketValue() * asset.HardMarginLtv);
    }

    private decimal CalculatePercentageDecrease(decimal currentValue, decimal targetValue)
    {
        if (currentValue == 0) return 0;
        var decrease = ((currentValue - targetValue) / currentValue) * 100;
        return decrease > 0 ? decrease : 0;
    }
}