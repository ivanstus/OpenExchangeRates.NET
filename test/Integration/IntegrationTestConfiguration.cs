using System.Net;

using Microsoft.Extensions.Configuration;

namespace OpenExchangeRates.Tests.Integration;

internal static class IntegrationTestConfiguration
{
    public const string AppIdKey = "OPENEXCHANGERATES_APP_ID";

    private static readonly IConfiguration Configuration = new ConfigurationBuilder().AddEnvironmentVariables()
        .AddUserSecrets<OpenExchangeRatesFixture>(true)
        .Build();

    static IntegrationTestConfiguration()
    {
        var httpProxy = Configuration["HTTP_PROXY"];

        if (!string.IsNullOrWhiteSpace(httpProxy))
            HttpClient.DefaultProxy = new WebProxy(httpProxy, true);
    }

    public static string? GetAppId()
    {
        var appId = Configuration[AppIdKey];

        return string.IsNullOrWhiteSpace(appId) ? null : appId;
    }
}