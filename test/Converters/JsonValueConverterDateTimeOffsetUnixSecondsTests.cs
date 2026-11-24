using System.Text.Json;

using OpenExchangeRates.Converters;

namespace OpenExchangeRates.Tests.Converters;

public class JsonValueConverterDateTimeOffsetUnixSecondsTests
{
    private static readonly JsonSerializerOptions Options = new()
    {
        Converters = { new JsonValueConverterDateTimeOffsetUnixSeconds() }
    };

    [Theory]
    [InlineData("0", 1970, 1, 1, 0, 0, 0)]
    [InlineData("1700000000", 2023, 11, 14, 22, 13, 20)]
    public void Read_ReturnsExpectedDateTimeOffset(string json, int year, int month, int day, int hour, int minute,
        int second)
    {
        var result = JsonSerializer.Deserialize<DateTimeOffset>(json, Options);

        Assert.Equal(new DateTimeOffset(year, month, day, hour, minute, second, TimeSpan.Zero), result);
    }

    [Fact]
    public void Write_ThrowsNotImplementedException()
    {
        Assert.Throws<NotImplementedException>(() => JsonSerializer.Serialize(DateTimeOffset.UnixEpoch, Options));
    }
}