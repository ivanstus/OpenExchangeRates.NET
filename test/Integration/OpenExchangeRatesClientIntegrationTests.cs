namespace OpenExchangeRates.Tests.Integration;

[Trait("Category", "Integration")]
public class OpenExchangeRatesClientIntegrationTests(OpenExchangeRatesFixture fixture)
    : IClassFixture<OpenExchangeRatesFixture>
{
    [IntegrationFact]
    public async Task GetCurrenciesAsync_ReturnsCurrenciesContainingUsdAndEur()
    {
        var currencies = await fixture.Client.GetCurrenciesAsync();

        Assert.NotNull(currencies);
        Assert.NotEmpty(currencies);
        Assert.True(currencies.ContainsKey("USD"));
        Assert.True(currencies.ContainsKey("EUR"));
    }

    [IntegrationFact]
    public async Task GetLatestRatesAsync_ReturnsValidRatesResponse()
    {
        var rates = await fixture.Client.GetLatestRatesAsync();

        Assert.NotNull(rates);
        Assert.Equal("USD", rates.BaseCurrency);
        Assert.NotEqual(default, rates.Timestamp);
        Assert.NotNull(rates.Rates);
        Assert.NotEmpty(rates.Rates);
        Assert.True(rates.Rates.ContainsKey("EUR"));
    }

    [IntegrationFact]
    public async Task GetHistoricalRatesAsync_DateOnly_ReturnsHistoricalRates()
    {
        var date = new DateOnly(2024, 1, 15);

        var rates = await fixture.Client.GetHistoricalRatesAsync(date, currencies: ["EUR", "GBP"]);

        Assert.NotNull(rates);
        Assert.Equal("USD", rates.BaseCurrency);
        Assert.NotNull(rates.Rates);
        Assert.NotEmpty(rates.Rates);
        Assert.True(rates.Rates.ContainsKey("EUR"));
    }

    [IntegrationFact]
    public async Task GetHistoricalRatesAsync_DateTime_ReturnsHistoricalRates()
    {
        var date = new DateTime(2024, 1, 15);

        var rates = await fixture.Client.GetHistoricalRatesAsync(date, currencies: ["EUR"]);

        Assert.NotNull(rates);
        Assert.Equal("USD", rates.BaseCurrency);
        Assert.NotNull(rates.Rates);
        Assert.True(rates.Rates.ContainsKey("EUR"));
    }

    [IntegrationFact]
    public async Task GetUsageDataAsync_ReturnsUsageDataForCurrentAppId()
    {
        var usageData = await fixture.Client.GetUsageDataAsync();

        Assert.NotNull(usageData);
        Assert.Equal(fixture.AppId, usageData.AppId);
        Assert.Equal(ApiStatus.Active, usageData.Status);
        Assert.NotNull(usageData.Plan);
        Assert.False(string.IsNullOrWhiteSpace(usageData.Plan.Name));
        Assert.NotNull(usageData.Usage);
        Assert.True(usageData.Usage.RequestsQuota > 0);
    }

    [IntegrationFact]
    public async Task GetLatestRatesAsync_WithInvalidAppId_ThrowsOpenExchangeRatesException()
    {
        using var invalidClient = new OpenExchangeRatesClient("invalid-app-id-for-integration-test");

        await Assert.ThrowsAsync<OpenExchangeRatesException>(() => invalidClient.GetLatestRatesAsync());
    }
}