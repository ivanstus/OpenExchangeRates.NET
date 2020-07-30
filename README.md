# OpenExchangeRates.NET

.NET client for Open Exchange Rates API

![GitHub](https://img.shields.io/github/license/ivanstus/OpenExchangeRates.NET)
![GitHub Workflow Status](https://img.shields.io/github/workflow/status/ivanstus/OpenExchangeRates.NET/.NET%20Core)
[![Nuget](https://img.shields.io/nuget/v/OpenExchangeRates.NET)](https://www.nuget.org/packages/OpenExchangeRates.NET/)

# Usage
```
using var client = new OpenExchangeRatesClient("YOUR_APP_ID");

// convert 1 USD to EUR
var convertResponse = await client.ConvertAsync("USD", "EUR", 1m);

// get all cureencies
var currencies = await client.GetCurrenciesAsync();

// get historical rates
var historicalRatesResponse = await client.GetHistoricalRatesAsync(new DateTime(2020, 06, 01));

// get latest rates
var latestRatesResponse = await client.GetLatestRatesAsync();

// get usage data
var usageData = await client.GetUsageDataAsync();
```
