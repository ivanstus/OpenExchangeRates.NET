using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using System.Text.Json;

using OpenExchangeRates.Converters;

namespace OpenExchangeRates;

public sealed class OpenExchangeRatesClient : IDisposable
{
    private const string ApiBaseUrl = "https://openexchangerates.org/api/";
    private const string DefaultCurrency = "USD";

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        Converters = { new JsonValueConverterApiStatus(), new JsonValueConverterDateTimeOffsetUnixSeconds() },
        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private readonly string _appId;

    private readonly HttpClient _httpClient = new()
    {
        BaseAddress = new Uri(ApiBaseUrl),
        DefaultRequestHeaders =
        {
            UserAgent = { new ProductInfoHeaderValue("OpenExchangeRates.NET", Assembly.GetExecutingAssembly().GetName().Version?.ToString()) }
        }
    };

    public OpenExchangeRatesClient(string appId)
    {
#if NET8_0_OR_GREATER
        ArgumentException.ThrowIfNullOrWhiteSpace(appId);
#elif NET7_0
        ArgumentException.ThrowIfNullOrEmpty(appId);
#else
        ArgumentNullException.ThrowIfNull(appId);
#endif

        _appId = appId;
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }

    public async Task<ConvertResponse?> ConvertAsync(string from, string to, decimal amount, bool prettyPrint = false,
        CancellationToken cancellationToken = default)
    {
#if NET8_0_OR_GREATER
        ArgumentException.ThrowIfNullOrWhiteSpace(from);
        ArgumentException.ThrowIfNullOrWhiteSpace(to);
#elif NET7_0
        ArgumentException.ThrowIfNullOrEmpty(from);
        ArgumentException.ThrowIfNullOrEmpty(to);
#else
        ArgumentNullException.ThrowIfNull(from);
        ArgumentNullException.ThrowIfNull(to);
#endif

        if (string.Equals(from.Trim(), string.Empty, StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException(message: null, nameof(from));

        if (string.Equals(to.Trim(), string.Empty, StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException(message: null, nameof(to));

        if (amount <= decimal.Zero)
            throw new ArgumentException(message: null, nameof(amount));

        var response = await _httpClient.GetAsync($"convert/{amount}/{from}/{to}?" + BuildQuery(prettyPrint: prettyPrint), cancellationToken);

        if (!response.IsSuccessStatusCode)
            throw new OpenExchangeRatesException(response.ReasonPhrase);

        return await response.Content.ReadFromJsonAsync<ConvertResponse>(JsonOptions, cancellationToken);
    }

    public async Task<IReadOnlyDictionary<string, string>?> GetCurrenciesAsync(bool prettyPrint = false, bool alternative = false,
        bool inactive = false, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(
            "currencies.json?" + BuildQuery(prettyPrint: prettyPrint, alternative: alternative, inactive: inactive), cancellationToken);

        if (!response.IsSuccessStatusCode)
            throw new OpenExchangeRatesException(response.ReasonPhrase);

        return await response.Content.ReadFromJsonAsync<IReadOnlyDictionary<string, string>?>(JsonOptions, cancellationToken);
    }

    public async Task<RatesResponse?> GetHistoricalRatesAsync(DateOnly date, string baseCurrency = DefaultCurrency,
        IEnumerable<string>? currencies = null, bool prettyPrint = false, bool alternative = false, CancellationToken cancellationToken = default)
    {
#if NET8_0_OR_GREATER
        ArgumentException.ThrowIfNullOrWhiteSpace(baseCurrency);
#elif NET7_0
        ArgumentException.ThrowIfNullOrEmpty(baseCurrency);
#else
        ArgumentNullException.ThrowIfNull(baseCurrency);
#endif

        return await GetHistoricalRatesAsync(date.ToDateTime(TimeOnly.MinValue), baseCurrency, currencies, prettyPrint, alternative,
            cancellationToken);
    }

    public async Task<RatesResponse?> GetHistoricalRatesAsync(DateTime date, string baseCurrency = DefaultCurrency,
        IEnumerable<string>? currencies = null, bool prettyPrint = false, bool alternative = false, CancellationToken cancellationToken = default)
    {
#if NET8_0_OR_GREATER
        ArgumentException.ThrowIfNullOrWhiteSpace(baseCurrency);
#elif NET7_0
        ArgumentException.ThrowIfNullOrEmpty(baseCurrency);
#else
        ArgumentNullException.ThrowIfNull(baseCurrency);
#endif

        if (string.Equals(baseCurrency.Trim(), string.Empty, StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException(message: null, nameof(baseCurrency));

        var response = await _httpClient.GetAsync(
            $"historical/{date:yyyy-MM-dd}.json?" + BuildQuery(baseCurrency, currencies, prettyPrint, alternative), cancellationToken);

        if (!response.IsSuccessStatusCode)
            throw new OpenExchangeRatesException(response.ReasonPhrase);

        return await response.Content.ReadFromJsonAsync<RatesResponse>(JsonOptions, cancellationToken);
    }

    public async Task<RatesResponse?> GetLatestRatesAsync(string? baseCurrency = null, IEnumerable<string>? currencies = null,
        bool prettyPrint = false, bool alternative = false, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync("latest.json?" + BuildQuery(baseCurrency, currencies, prettyPrint, alternative), cancellationToken);

        if (!response.IsSuccessStatusCode)
            throw new OpenExchangeRatesException(response.ReasonPhrase);

        return await response.Content.ReadFromJsonAsync<RatesResponse>(JsonOptions, cancellationToken);
    }

    public async Task<UsageData?> GetUsageDataAsync(bool prettyPrint = false, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync("usage.json?" + BuildQuery(prettyPrint: prettyPrint), cancellationToken);

        if (!response.IsSuccessStatusCode)
            throw new OpenExchangeRatesException(response.ReasonPhrase);

        var usageResponse = await response.Content.ReadFromJsonAsync<UsageResponse>(JsonOptions, cancellationToken);

        return usageResponse?.Data;
    }

    private string BuildQuery(string? baseCurrency = null, IEnumerable<string>? currencies = null, bool? prettyPrint = null, bool? alternative = null,
        bool? inactive = null)
    {
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