namespace OpenExchangeRates.Tests;

public class OpenExchangeRatesClientTests
{
    private const string TestAppId = "test-app-id";

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_ThrowsArgumentException_WhenAppIdIsNullOrWhiteSpace(string? appId)
    {
        Assert.ThrowsAny<ArgumentException>(() => new OpenExchangeRatesClient(appId!));
    }

    [Fact]
    public void Constructor_Succeeds_WhenAppIdIsValid()
    {
        using var client = new OpenExchangeRatesClient(TestAppId);

        Assert.NotNull(client);
    }

    [Fact]
    public void Dispose_CanBeCalledMultipleTimes()
    {
        var client = new OpenExchangeRatesClient(TestAppId);

        client.Dispose();
        client.Dispose();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task ConvertAsync_ThrowsArgumentException_WhenFromIsInvalid(string? from)
    {
        using var client = new OpenExchangeRatesClient(TestAppId);

        await Assert.ThrowsAnyAsync<ArgumentException>(() =>
            client.ConvertAsync(from!, "EUR", 10m, cancellationToken: TestContext.Current.CancellationToken));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task ConvertAsync_ThrowsArgumentException_WhenToIsInvalid(string? to)
    {
        using var client = new OpenExchangeRatesClient(TestAppId);

        await Assert.ThrowsAnyAsync<ArgumentException>(() =>
            client.ConvertAsync("USD", to!, 10m, cancellationToken: TestContext.Current.CancellationToken));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100.5)]
    public async Task ConvertAsync_ThrowsArgumentException_WhenAmountIsZeroOrNegative(decimal amount)
    {
        using var client = new OpenExchangeRatesClient(TestAppId);

        await Assert.ThrowsAsync<ArgumentException>(() =>
            client.ConvertAsync("USD", "EUR", amount, cancellationToken: TestContext.Current.CancellationToken));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task GetHistoricalRatesAsync_DateOnly_ThrowsArgumentException_WhenBaseCurrencyIsInvalid(
        string? baseCurrency)
    {
        using var client = new OpenExchangeRatesClient(TestAppId);

        await Assert.ThrowsAnyAsync<ArgumentException>(() => client.GetHistoricalRatesAsync(new DateOnly(2024, 1, 15),
            baseCurrency!, cancellationToken: TestContext.Current.CancellationToken));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task GetHistoricalRatesAsync_DateTime_ThrowsArgumentException_WhenBaseCurrencyIsInvalid(
        string? baseCurrency)
    {
        using var client = new OpenExchangeRatesClient(TestAppId);

        await Assert.ThrowsAnyAsync<ArgumentException>(() => client.GetHistoricalRatesAsync(new DateTime(2024, 1, 15),
            baseCurrency!, cancellationToken: TestContext.Current.CancellationToken));
    }
}