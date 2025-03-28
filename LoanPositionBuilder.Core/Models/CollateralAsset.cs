namespace LoanPositionBuilder.Core.Models;

public class CollateralAsset
{
    public string Symbol { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal CurrentPrice { get; set; }
    public string PriceCurrency { get; set; } = "USD";
    public decimal InitialMarginLtv { get; set; }
    public decimal SoftMarginLtv { get; set; }
    public decimal HardMarginLtv { get; set; }

    public decimal GetMarketValue() => Quantity * CurrentPrice;
    
    public decimal GetLendingValue() => GetMarketValue() * InitialMarginLtv;
}