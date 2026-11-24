using System.Text.Json;

using OpenExchangeRates.Converters;

namespace OpenExchangeRates.Tests.Converters;

public class JsonValueConverterApiStatusTests
{
    private static readonly JsonSerializerOptions Options = new()
    {
        Converters = { new JsonValueConverterApiStatus() }
    };

    [Theory]
    [InlineData("\"active\"", ApiStatus.Active)]
    [InlineData("\"ACTIVE\"", ApiStatus.Active)]
    [InlineData("\"access_restricted\"", ApiStatus.AccessRestricted)]
    [InlineData("\"ACCESS_RESTRICTED\"", ApiStatus.AccessRestricted)]
    [InlineData("\"other\"", ApiStatus.Unknown)]
    [InlineData("\"\"", ApiStatus.Unknown)]
    [InlineData("null", ApiStatus.Unknown)]
    public void Read_ReturnsExpectedApiStatus(string json, ApiStatus expected)
    {
        var result = JsonSerializer.Deserialize<ApiStatus>(json, Options);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void Write_ThrowsNotImplementedException()
    {
        Assert.Throws<NotImplementedException>(() => JsonSerializer.Serialize(ApiStatus.Active, Options));
    }
}