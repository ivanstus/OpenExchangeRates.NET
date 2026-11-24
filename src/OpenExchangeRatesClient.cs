using System.Net;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json;

using OpenExchangeRates.Converters;
#if !NET11_0_OR_GREATER
using System.Net.Http.Json;
#endif

namespace OpenExchangeRates;

/// <summary>
///     Provides a client for interacting with the Open Exchange Rates API, enabling currency conversion,
///     retrieval of exchange rates, and access to usage data.
/// </summary>
/// <remarks>
///     This class is designed to simplify integration with the Open Exchange Rates API by providing
///     methods for common operations such as fetching the latest exchange rates, historical rates,
///     currency conversion, and usage statistics. It uses an <see cref="HttpClient" /> for making HTTP
///     requests and requires an API key for authentication.
/// </remarks>
/// <example>
///     Example usage:
///     <code>
/// var client = new OpenExchangeRatesClient("your-app-id");
/// var rates = await client.GetLatestRatesAsync();
/// </code>
/// </example>
public sealed class OpenExchangeRatesClient : IDisposable
{
    private const string ApiBaseUrl = "https://openexchangerates.org/api/";
    private const string DefaultCurrency = "USD";

    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonValueConverterApiStatus(), new JsonValueConverterDateTimeOffsetUnixSeconds() },
        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase
    };

    private readonly string _appId;

    private readonly HttpClient _httpClient = new()
    {
        BaseAddress = new Uri(ApiBaseUrl),
        DefaultRequestHeaders =
        {
            UserAgent =
            {
                new ProductInfoHeaderValue("OpenExchangeRates.NET",
                    Assembly.GetExecutingAssembly().GetName().Version?.ToString())
            }
        }
    };

    /// <summary>
    ///     Initializes a new instance of the <see cref="OpenExchangeRatesClient" /> class with the specified API key.
    /// </summary>
    /// <param name="appId">
    ///     The application ID (API key) required for authenticating requests to the Open Exchange Rates API.
    /// </param>
    /// <exception cref="ArgumentException">
    ///     Thrown when the <paramref name="appId" /> is null, empty, or consists only of white-space characters.
    /// </exception>
    public OpenExchangeRatesClient(string appId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(appId);

        _appId = appId;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _httpClient.Dispose();
    }

    /// <summary>
    ///     Converts a specified amount from one currency to another asynchronously.
    /// </summary>
    /// <param name="from">The ISO 4217 currency code of the source currency. Cannot be null, empty, or whitespace.</param>
    /// <param name="to">The ISO 4217 currency code of the target currency. Cannot be null, empty, or whitespace.</param>
    /// <param name="amount">The amount to convert. Must be greater than zero.</param>
    /// <param name="prettyPrint">A boolean value indicating whether the response should be formatted for readability.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a <see cref="ConvertResponse" /> object
    ///     representing the conversion result, or <c>null</c> if the response is empty.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     Thrown when <paramref name="from" />, <paramref name="to" />, or <paramref name="amount" /> is invalid.
    /// </exception>
    /// <exception cref="OpenExchangeRatesException">
    ///     Thrown when the API response indicates an error.
    /// </exception>
    public async Task<ConvertResponse?> ConvertAsync(string from, string to, decimal amount, bool prettyPrint = false,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(from);
        ArgumentException.ThrowIfNullOrWhiteSpace(to);

        if (string.Equals(from.Trim(), string.Empty, StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException(null, nameof(from));

        if (string.Equals(to.Trim(), string.Empty, StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException(null, nameof(to));

        if (amount <= decimal.Zero)
            throw new ArgumentException(null, nameof(amount));

        var response =
            await _httpClient.GetAsync($"convert/{amount}/{from}/{to}?" + BuildQuery(prettyPrint: prettyPrint),
                cancellationToken);

        if (!response.IsSuccessStatusCode)
            throw new OpenExchangeRatesException(response.ReasonPhrase);

        return await response.Content.ReadFromJsonAsync<ConvertResponse>(JsonOptions, cancellationToken);
    }

    /// <summary>
    ///     Retrieves a dictionary of available currencies and their corresponding codes from the Open Exchange Rates API.
    /// </summary>
    /// <param name="prettyPrint">
    ///     Specifies whether the response should be formatted for readability. Defaults to <c>false</c>.
    /// </param>
    /// <param name="alternative">
    ///     Indicates whether to include alternative currency codes in the response. Defaults to <c>false</c>.
    /// </param>
    /// <param name="inactive">
    ///     Determines whether to include inactive currencies in the response. Defaults to <c>false</c>.
    /// </param>
    /// <param name="cancellationToken">
    ///     A token to monitor for cancellation requests.
    /// </param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a read-only dictionary
    ///     where the keys are currency codes and the values are currency names, or <c>null</c> if the response is empty.
    /// </returns>
    /// <exception cref="OpenExchangeRatesException">
    ///     Thrown when the API response indicates an error.
    /// </exception>
    public async Task<IReadOnlyDictionary<string, string>?> GetCurrenciesAsync(bool prettyPrint = false,
        bool alternative = false, bool inactive = false, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(
            "currencies.json?" + BuildQuery(prettyPrint: prettyPrint, alternative: alternative, inactive: inactive),
            cancellationToken);

        if (!response.IsSuccessStatusCode)
            throw new OpenExchangeRatesException(response.ReasonPhrase);

        return await response.Content.ReadFromJsonAsync<IReadOnlyDictionary<string, string>?>(JsonOptions,
            cancellationToken);
    }

    /// <summary>
    ///     Retrieves historical exchange rates for a specific date from the Open Exchange Rates API.
    /// </summary>
    /// <param name="date">
    ///     The date for which to retrieve historical exchange rates.
    /// </param>
    /// <param name="baseCurrency">
    ///     The base currency against which exchange rates are calculated. Defaults to "USD".
    /// </param>
    /// <param name="currencies">
    ///     A collection of specific currencies to include in the response. If null, all available currencies are included.
    /// </param>
    /// <param name="prettyPrint">
    ///     A boolean value indicating whether the response should be formatted for readability.
    /// </param>
    /// <param name="alternative">
    ///     A boolean value indicating whether to use alternative data sources for the response.
    /// </param>
    /// <param name="cancellationToken">
    ///     A token to monitor for cancellation requests.
    /// </param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a <see cref="RatesResponse" /> object
    ///     with the historical exchange rates, or null if the operation fails.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     Thrown when the <paramref name="baseCurrency" /> is null, empty, or consists only of white-space characters.
    /// </exception>
    public async Task<RatesResponse?> GetHistoricalRatesAsync(DateOnly date, string baseCurrency = DefaultCurrency,
        IEnumerable<string>? currencies = null, bool prettyPrint = false, bool alternative = false,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(baseCurrency);

        return await GetHistoricalRatesAsync(date.ToDateTime(TimeOnly.MinValue), baseCurrency, currencies, prettyPrint,
            alternative, cancellationToken);
    }

    /// <summary>
    ///     Retrieves historical exchange rates for a specific date.
    /// </summary>
    /// <param name="date">
    ///     The date for which to retrieve historical exchange rates.
    /// </param>
    /// <param name="baseCurrency">
    ///     The base currency to use for the exchange rates. Defaults to "USD".
    /// </param>
    /// <param name="currencies">
    ///     A collection of specific currencies to include in the response. If null, all available currencies are included.
    /// </param>
    /// <param name="prettyPrint">
    ///     A boolean value indicating whether the response should be formatted for readability.
    /// </param>
    /// <param name="alternative">
    ///     A boolean value indicating whether to use alternative data sources.
    /// </param>
    /// <param name="cancellationToken">
    ///     A token to monitor for cancellation requests.
    /// </param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a <see cref="RatesResponse" /> object
    ///     with the historical exchange rates, or null if the operation fails.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     Thrown when the <paramref name="baseCurrency" /> is null, empty, or consists only of white-space characters.
    /// </exception>
    /// <exception cref="OpenExchangeRatesException">
    ///     Thrown when the API request fails with an error response.
    /// </exception>
    public async Task<RatesResponse?> GetHistoricalRatesAsync(DateTime date, string baseCurrency = DefaultCurrency,
        IEnumerable<string>? currencies = null, bool prettyPrint = false, bool alternative = false,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(baseCurrency);

        if (string.Equals(baseCurrency.Trim(), string.Empty, StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException(null, nameof(baseCurrency));

        var response = await _httpClient.GetAsync(
            $"historical/{date:yyyy-MM-dd}.json?" + BuildQuery(baseCurrency, currencies, prettyPrint, alternative),
            cancellationToken);

        if (!response.IsSuccessStatusCode)
            throw new OpenExchangeRatesException(response.ReasonPhrase);

        return await response.Content.ReadFromJsonAsync<RatesResponse>(JsonOptions, cancellationToken);
    }

    /// <summary>
    ///     Retrieves the latest exchange rates from the Open Exchange Rates API.
    /// </summary>
    /// <param name="baseCurrency">
    ///     The base currency for the exchange rates. If <c>null</c>, the default currency is used.
    /// </param>
    /// <param name="currencies">
    ///     A collection of specific currencies to retrieve rates for. If <c>null</c>, rates for all available currencies are
    ///     returned.
    /// </param>
    /// <param name="prettyPrint">
    ///     A flag indicating whether the response should be formatted for readability.
    /// </param>
    /// <param name="alternative">
    ///     A flag indicating whether to include alternative currencies in the response.
    /// </param>
    /// <param name="cancellationToken">
    ///     A token to monitor for cancellation requests.
    /// </param>
    /// <returns>
    ///     A <see cref="RatesResponse" /> object containing the latest exchange rates, or <c>null</c> if the response is
    ///     empty.
    /// </returns>
    /// <exception cref="OpenExchangeRatesException">
    ///     Thrown when the API response indicates an error.
    /// </exception>
    public async Task<RatesResponse?> GetLatestRatesAsync(string? baseCurrency = null,
        IEnumerable<string>? currencies = null, bool prettyPrint = false, bool alternative = false,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(
            "latest.json?" + BuildQuery(baseCurrency, currencies, prettyPrint, alternative), cancellationToken);

        if (!response.IsSuccessStatusCode)
            throw new OpenExchangeRatesException(response.ReasonPhrase);

        return await response.Content.ReadFromJsonAsync<RatesResponse>(JsonOptions, cancellationToken);
    }

    /// <summary>
    ///     Retrieves the usage data for the current API key, including information about the API plan,
    ///     usage limits, and remaining requests.
    /// </summary>
    /// <param name="prettyPrint">
    ///     A boolean value indicating whether the response should be formatted for readability.
    ///     Defaults to <c>false</c>.
    /// </param>
    /// <param name="cancellationToken">
    ///     A <see cref="CancellationToken" /> that can be used to cancel the asynchronous operation.
    /// </param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains an instance of
    ///     <see cref="UsageData" /> with details about the API usage, or <c>null</c> if the data could not be retrieved.
    /// </returns>
    /// <exception cref="OpenExchangeRatesException">
    ///     Thrown when the API response indicates an error, such as an invalid API key or a server issue.
    /// </exception>
    public async Task<UsageData?> GetUsageDataAsync(bool prettyPrint = false,
        CancellationToken cancellationToken = default)
    {
        var response =
            await _httpClient.GetAsync("usage.json?" + BuildQuery(prettyPrint: prettyPrint), cancellationToken);

        if (!response.IsSuccessStatusCode)
            throw new OpenExchangeRatesException(response.ReasonPhrase);

        var usageResponse = await response.Content.ReadFromJsonAsync<UsageResponse>(JsonOptions, cancellationToken);

        return usageResponse?.Data;
    }

    private string BuildQuery(string? baseCurrency = null, IEnumerable<string>? currencies = null,
        bool? prettyPrint = null, bool? alternative = null, bool? inactive = null)
    {
        if (string.IsNullOrWhiteSpace(_appId))
            throw new InvalidOperationException("API key is required.");

        var sb = new StringBuilder();
        sb.Append($"app_id={_appId}");

        if (baseCurrency != null)
            sb.Append($"&base={baseCurrency}");

        if (currencies != null)
            sb.Append($"&symbols={WebUtility.UrlEncode(string.Join(",", currencies))}");

        return sb.AppendNameValueBoolean("prettyprint", prettyPrint)
            .AppendNameValueBoolean("show_alternative", alternative)
            .AppendNameValueBoolean("show_inactive", inactive)
            .ToString();
    }
}