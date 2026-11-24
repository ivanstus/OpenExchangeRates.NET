namespace OpenExchangeRates.Tests;

public class OpenExchangeRatesExceptionTests
{
    [Fact]
    public void Constructor_WithMessage_SetsMessage()
    {
        var exception = new OpenExchangeRatesException("API error");

        Assert.Equal("API error", exception.Message);
        Assert.Null(exception.InnerException);
    }

    [Fact]
    public void Constructor_WithMessageAndInnerException_SetsProperties()
    {
        var inner = new InvalidOperationException("Inner");
        var exception = new OpenExchangeRatesException("API error", inner);

        Assert.Equal("API error", exception.Message);
        Assert.Same(inner, exception.InnerException);
    }
}