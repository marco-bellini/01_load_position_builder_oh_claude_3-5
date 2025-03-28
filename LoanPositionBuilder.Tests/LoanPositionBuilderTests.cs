using LoanPositionBuilder.Core.Models;
using LoanPositionBuilder.Core.Services;

namespace LoanPositionBuilder.Tests;

public class LoanPositionBuilderTests
{
    private readonly ILoanPositionBuilder _loanPositionBuilder;

    public LoanPositionBuilderTests()
    {
        _loanPositionBuilder = new Core.Services.LoanPositionBuilder();
    }

    [Fact]
    public async Task CalculateMaxLoanAmount_WithValidCollateral_ReturnsCorrectAmount()
    {
        // Arrange
        var collateral = new List<CollateralAsset>
        {
            new()
            {
                Symbol = "BTC",
                Quantity = 5,
                CurrentPrice = 60000,
                InitialMarginLtv = 0.7m,
                SoftMarginLtv = 0.65m,
                HardMarginLtv = 0.6m
            },
            new()
            {
                Symbol = "SOL",
                Quantity = 2000,
                CurrentPrice = 100,
                InitialMarginLtv = 0.5m,
                SoftMarginLtv = 0.45m,
                HardMarginLtv = 0.4m
            }
        };

        // Act
        var result = await _loanPositionBuilder.CalculateMaxLoanAmount(collateral, "USD", "6M");

        // Assert
        Assert.Equal(310000m, result.MaxLoanAmount);
        Assert.Equal(500000m, result.TotalPortfolioValue);
        Assert.Equal(2, result.CollateralContributions.Count);
        
        var btcContribution = result.CollateralContributions.First(c => c.Symbol == "BTC");
        Assert.Equal(300000m, btcContribution.MarketValue);
        Assert.Equal(210000m, btcContribution.LendingValue);
        
        var solContribution = result.CollateralContributions.First(c => c.Symbol == "SOL");
        Assert.Equal(200000m, solContribution.MarketValue);
        Assert.Equal(100000m, solContribution.LendingValue);
    }

    [Fact]
    public async Task CalculateDrawdownScenario_WithValidLoanAmount_ReturnsCorrectThresholds()
    {
        // Arrange
        var collateral = new List<CollateralAsset>
        {
            new()
            {
                Symbol = "BTC",
                Quantity = 5,
                CurrentPrice = 60000,
                InitialMarginLtv = 0.7m,
                SoftMarginLtv = 0.65m,
                HardMarginLtv = 0.6m
            },
            new()
            {
                Symbol = "SOL",
                Quantity = 2000,
                CurrentPrice = 100,
                InitialMarginLtv = 0.5m,
                SoftMarginLtv = 0.45m,
                HardMarginLtv = 0.4m
            }
        };

        var loanAmount = 250000m;

        // Act
        var result = await _loanPositionBuilder.CalculateDrawdownScenario(collateral, loanAmount, "USD", "6M");

        // Assert
        Assert.Equal(2m, Math.Round(result.CurrentCollateralization, 2));
        Assert.True(result.PercentageToInitialMargin > 0);
        Assert.True(result.PercentageToSoftMargin > result.PercentageToInitialMargin);
        Assert.True(result.PercentageToHardMargin > result.PercentageToSoftMargin);
    }

    [Fact]
    public async Task CalculateDrawdownScenario_WithExcessiveLoanAmount_ThrowsException()
    {
        // Arrange
        var collateral = new List<CollateralAsset>
        {
            new()
            {
                Symbol = "BTC",
                Quantity = 1,
                CurrentPrice = 60000,
                InitialMarginLtv = 0.7m,
                SoftMarginLtv = 0.65m,
                HardMarginLtv = 0.6m
            }
        };

        var loanAmount = 100000m;

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _loanPositionBuilder.CalculateDrawdownScenario(collateral, loanAmount, "USD", "6M"));
    }
}