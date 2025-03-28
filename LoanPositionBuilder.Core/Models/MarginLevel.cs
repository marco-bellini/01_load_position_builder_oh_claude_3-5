namespace LoanPositionBuilder.Core.Models;

public class MarginLevel
{
    public string AssetSymbol { get; set; } = string.Empty;
    public decimal InitialMarginPercentage { get; set; }
    public decimal SoftMarginPercentage { get; set; }
    public decimal HardMarginPercentage { get; set; }
}