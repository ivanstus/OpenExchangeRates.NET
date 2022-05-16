namespace OpenExchangeRates;

public sealed class OpenExchangeRatesException : Exception
{
    public OpenExchangeRatesException(string? message) : base(message)
    {
    }

    public OpenExchangeRatesException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}