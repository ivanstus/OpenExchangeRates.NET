using System.Text.Json.Serialization;

namespace OpenExchangeRates;

/// <summary>
///     Represents the response from a currency conversion request made to the Open Exchange Rates API.
/// </summary>
/// <remarks>
///     This class encapsulates the details of a conversion operation, including the original request,
///     the converted amount, and metadata about the conversion process.
/// </remarks>
public sealed class ConvertResponse
{
    /// <summary>
    ///     Gets the details of the original currency conversion request.
    /// </summary>
    /// <remarks>
    ///     This property provides information about the conversion request, including the source currency,
    ///     target currency, amount to be converted, and the query string used for the API call.
    /// </remarks>
    public required ConvertRequest Request { get; init; }

    /// <summary>
    ///     Gets the converted amount resulting from the currency conversion request.
    /// </summary>
    /// <value>
    ///     A <see cref="decimal" /> representing the amount in the target currency after conversion.
    /// </value>
    /// <remarks>
    ///     The value is calculated based on the exchange rate and the amount specified in the request.
    /// </remarks>
    [JsonPropertyName("response")]
    public decimal Amount { get; init; }

    /// <summary>
    ///     Gets metadata related to the currency conversion operation.
    /// </summary>
    /// <remarks>
    ///     This metadata includes details such as the timestamp of the conversion
    ///     and the exchange rate used.
    /// </remarks>
    [JsonPropertyName("meta")]
    public required ConversionMetadata Metadata { get; init; }
}