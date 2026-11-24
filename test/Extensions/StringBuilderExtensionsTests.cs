using System.Text;

namespace OpenExchangeRates.Tests.Extensions;

public class StringBuilderExtensionsTests
{
    [Fact]
    public void AppendNameValueBoolean_AppendsTrue_WhenValueIsTrue()
    {
        var sb = new StringBuilder("app_id=123");

        sb.AppendNameValueBoolean("prettyprint", true);

        Assert.Equal("app_id=123&prettyprint=true", sb.ToString());
    }

    [Fact]
    public void AppendNameValueBoolean_AppendsFalse_WhenValueIsFalse()
    {
        var sb = new StringBuilder("app_id=123");

        sb.AppendNameValueBoolean("prettyprint", false);

        Assert.Equal("app_id=123&prettyprint=false", sb.ToString());
    }

    [Fact]
    public void AppendNameValueBoolean_DoesNotAppend_WhenValueIsNull()
    {
        var sb = new StringBuilder("app_id=123");

        sb.AppendNameValueBoolean("prettyprint", null);

        Assert.Equal("app_id=123", sb.ToString());
    }

    [Fact]
    public void AppendNameValueBoolean_ThrowsArgumentNullException_WhenStringBuilderIsNull()
    {
        StringBuilder sb = null!;

        Assert.Throws<ArgumentNullException>(() => sb.AppendNameValueBoolean("prettyprint", true));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void AppendNameValueBoolean_ThrowsArgumentException_WhenNameIsInvalid(string? name)
    {
        var sb = new StringBuilder();

        Assert.ThrowsAny<ArgumentException>(() => sb.AppendNameValueBoolean(name!, true));
    }
}