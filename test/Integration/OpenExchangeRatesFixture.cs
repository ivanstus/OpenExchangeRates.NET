namespace OpenExchangeRates.Tests.Integration;

public sealed class OpenExchangeRatesFixture : IDisposable
{
    public OpenExchangeRatesFixture()
    {
        AppId = IntegrationTestConfiguration.GetAppId()
                ?? throw new InvalidOperationException($"'{IntegrationTestConfiguration.AppIdKey}' is not configured.");

        Client = new OpenExchangeRatesClient(AppId);
    }

    public OpenExchangeRatesClient Client { get; }

    public string AppId { get; }

    public void Dispose()
    {
        Client.Dispose();
    }
}