using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

using OpenExchangeRates.Converters;

namespace OpenExchangeRates
{
    public sealed class OpenExchangeRatesClient : IDisposable
    {
        private const string ApiBaseUrl = "https://openexchangerates.org/api/";
        private const string DefaultCurrency = "USD";

        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            Converters = { new JsonValueConverterApiStatus(), new JsonValueConverterDateTimeOffsetUnixSeconds() },
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        private readonly string _appId;

        private readonly HttpClient _httpClient = new HttpClient
        {
            BaseAddress = new Uri(ApiBaseUrl), DefaultRequestHeaders = { UserAgent = { new ProductInfoHeaderValue("OpenExchangeRates.NET", "1.0.0") } }
        };

        public OpenExchangeRatesClient(string appId)
        {
            _appId = appId ?? throw new ArgumentNullException(nameof(appId));
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }

        public async Task<ConvertResponse> ConvertAsync(string from, string to, decimal amount, CancellationToken cancellationToken = default, bool prettyPrint = false)
        {
            if (from is null)
                throw new ArgumentNullException(nameof(from));

            if (string.Equals(from.Trim(), string.Empty, StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException(nameof(from));

            if (to is null)
                throw new ArgumentNullException(nameof(to));

            if (string.Equals(to.Trim(), string.Empty, StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException(nameof(to));

            if (amount <= decimal.Zero)
                throw new InvalidOperationException(nameof(amount));

            var response = await _httpClient.GetAsync($"convert/{amount}/{from}/{to}?" + BuildQuery(prettyPrint: prettyPrint), cancellationToken);
            if (!response.IsSuccessStatusCode)
                throw new OpenExchangeRatesException(response.ReasonPhrase);

            using var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<ConvertResponse>(stream, JsonOptions, cancellationToken);
        }

        public async Task<IReadOnlyDictionary<string, string>> GetCurrenciesAsync(CancellationToken cancellationToken = default, bool prettyPrint = false, bool alternative = false,
            bool inactive = false)
        {
            var response = await _httpClient.GetAsync("currencies.json?" + BuildQuery(prettyPrint: prettyPrint, alternative: alternative, inactive: inactive), cancellationToken);
            if (!response.IsSuccessStatusCode)
                throw new OpenExchangeRatesException(response.ReasonPhrase);

            using var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<IReadOnlyDictionary<string, string>>(stream, JsonOptions, cancellationToken);
        }

        public async Task<RatesResponse> GetHistoricalRatesAsync(DateTime date, CancellationToken cancellationToken = default, string baseCurrency = DefaultCurrency,
            IEnumerable<string> currencies = null, bool prettyPrint = false, bool alternative = false)
        {
            if (string.IsNullOrWhiteSpace(baseCurrency))
                throw new InvalidOperationException(nameof(baseCurrency));

            var response = await _httpClient.GetAsync($"historical/{date:yyyy-MM-dd}.json?" + BuildQuery(baseCurrency, currencies, prettyPrint, alternative), cancellationToken);
            if (!response.IsSuccessStatusCode)
                throw new OpenExchangeRatesException(response.ReasonPhrase);

            using var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<RatesResponse>(stream, JsonOptions, cancellationToken);
        }

        public async Task<RatesResponse> GetLatestRatesAsync(CancellationToken cancellationToken = default, string baseCurrency = DefaultCurrency,
            IEnumerable<string> currencies = null, bool prettyPrint = false, bool alternative = false)
        {
            if (string.IsNullOrWhiteSpace(baseCurrency))
                throw new InvalidOperationException(nameof(baseCurrency));

            var response = await _httpClient.GetAsync("latest.json?" + BuildQuery(baseCurrency, currencies, prettyPrint, alternative), cancellationToken);
            if (!response.IsSuccessStatusCode)
                throw new OpenExchangeRatesException(response.ReasonPhrase);

            using var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<RatesResponse>(stream, JsonOptions, cancellationToken);
        }

        public async Task<UsageData> GetUsageDataAsync(CancellationToken cancellationToken = default, bool prettyPrint = false)
        {
            var response = await _httpClient.GetAsync("usage.json?" + BuildQuery(prettyPrint: prettyPrint), cancellationToken);
            if (!response.IsSuccessStatusCode)
                throw new OpenExchangeRatesException(response.ReasonPhrase);

            using var stream = await response.Content.ReadAsStreamAsync();
            var usageResponse = await JsonSerializer.DeserializeAsync<UsageResponse>(stream, JsonOptions, cancellationToken);
            return usageResponse?.Data;
        }

        private string BuildQuery(string baseCurrency = null, IEnumerable<string> currencies = null, bool prettyPrint = false, bool alternative = false, bool inactive = false)
        {
            var sb = new StringBuilder();
            sb.AppendFormat(CultureInfo.InvariantCulture, "app_id={0}", _appId);

            if (baseCurrency != null)
                sb.AppendFormat(CultureInfo.InvariantCulture, "&base={0}", baseCurrency);

            if (currencies != null)
                sb.AppendFormat(CultureInfo.InvariantCulture, "&symbols={0}", HttpUtility.UrlEncode(string.Join(",", currencies)));

            if (prettyPrint)
                sb.Append("&prettyprint=true");

            if (alternative)
                sb.Append("&show_alternative=true");

            if (inactive)
                sb.Append("&show_inactive=true");

            return sb.ToString();
        }
    }
}