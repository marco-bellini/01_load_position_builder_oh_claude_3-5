# Loan Position Builder and Drawdown Calculator

This solution provides a set of APIs for calculating loan positions and drawdown scenarios in a collateral lending system.

## Architecture

The solution is built with:
- .NET 8.0
- ASP.NET Core Web API
- xUnit for testing

The solution consists of three projects:
1. `LoanPositionBuilder.Core` - Core business logic and models
2. `LoanPositionBuilder.Api` - API endpoints
3. `LoanPositionBuilder.Tests` - Unit tests

## Features

### Position Builder
- Calculates maximum borrowable amount based on collateral assets
- Provides detailed breakdown of lending value per asset
- Supports multiple collateral assets
- Real-time recalculation based on input changes

### Drawdown Calculator
- Calculates risk thresholds for loan positions
- Shows Initial Margin (IM), Soft Margin (SM), and Hard Margin (HM) levels
- Displays percentage and absolute value decreases before triggering thresholds
- Provides portfolio-level totals and per-asset breakdowns

## API Endpoints

### 1. Calculate Maximum Loan Amount
```http
POST /api/LoanCalculator/max-loan-amount
```

Request body:
```json
{
  "collateralAssets": [
    {
      "symbol": "BTC",
      "quantity": 5,
      "currentPrice": 60000,
      "initialMarginLtv": 0.7,
      "softMarginLtv": 0.65,
      "hardMarginLtv": 0.6
    }
  ],
  "loanCurrency": "USD",
  "tenor": "6M"
}
```

### 2. Calculate Drawdown Scenario
```http
POST /api/LoanCalculator/drawdown-scenario
```

Request body:
```json
{
  "collateralAssets": [
    {
      "symbol": "BTC",
      "quantity": 5,
      "currentPrice": 60000,
      "initialMarginLtv": 0.7,
      "softMarginLtv": 0.65,
      "hardMarginLtv": 0.6
    }
  ],
  "loanAmount": 250000,
  "loanCurrency": "USD",
  "tenor": "6M"
}
```

## Example Scenario

For a portfolio with:
- 5 BTC at $60,000 each
- 2,000 SOL at $100 each

With the following LTV ratios:
- BTC: IM at 70%, SM at 65%, HM at 60%
- SOL: IM at 50%, SM at 45%, HM at 40%

The calculator will show:
1. Maximum loan amount: $310,000
   - BTC contribution: $210,000 (5 × $60,000 × 70%)
   - SOL contribution: $100,000 (2,000 × $100 × 50%)

2. For a $250,000 loan:
   - Current collateralization: 200%
   - Margin thresholds showing when IM, SM, and HM are triggered
   - Percentage and absolute value decreases before each threshold

## Running the Solution

1. Clone the repository
2. Ensure .NET 8.0 SDK is installed
3. Run `dotnet restore` to restore dependencies
4. Run `dotnet test` to execute tests
5. Run `dotnet run --project LoanPositionBuilder.Api` to start the API

## Testing

The solution includes unit tests covering:
- Maximum loan amount calculations
- Drawdown scenario calculations
- Edge cases and validation

Run tests with:
```bash
dotnet test
```