namespace OpenExchangeRates;

/// <summary>
///     Represents a request to convert a specific amount from one currency to another
///     using the Open Exchange Rates API.
/// </summary>
/// <remarks>
///     This class encapsulates the details of a currency conversion request,
///     including the amount to be converted, the source currency, the target currency,
///     and the query string used for the API call.
/// </remarks>
public sealed class ConvertRequest
{
    /// <summary>
    ///     Gets the query string used for the API call to convert a specific amount
    ///     from one currency to another.
    /// </summary>
    /// <remarks>
    ///     The query string is constructed based on the source currency, target currency,
    ///     and the amount to be converted. It is used to communicate with the Open Exchange Rates API.
    /// </remarks>
    public required string Query { get; init; }

    /// <summary>
    ///     Gets or initializes the amount to be converted in the currency conversion request.
    /// </summary>
    /// <value>
    ///     A <see cref="decimal" /> representing the monetary value to be converted.
    /// </value>
    /// <remarks>
    ///     This property specifies the amount of money to be converted from the source currency
    ///     to the target currency as part of the conversion request.
    /// </remarks>
    public decimal Amount { get; init; }

    /// <summary>
    ///     Gets or sets the ISO 4217 currency code of the source currency
    ///     for the conversion request.
    /// </summary>
    /// <remarks>
    ///     This property specifies the currency from which the amount will be converted.
    ///     It must be a valid three-letter currency code, such as "USD" or "EUR".
    /// </remarks>
    public required string From { get; init; }

    /// <summary>
    ///     Gets or sets the target currency code for the conversion request.
    /// </summary>
    /// <remarks>
    ///     This property specifies the ISO 4217 currency code of the currency
    ///     to which the amount will be converted.
    /// </remarks>
    public required string To { get; init; }
}