using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenExchangeRates.Converters
{
    internal sealed class JsonValueConverterApiStatus : JsonConverter<ApiStatus>
    {
        public override ApiStatus Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();

            if (string.Equals(value, "active", StringComparison.OrdinalIgnoreCase))
                return ApiStatus.Active;

            if (string.Equals(value, "access_restricted", StringComparison.OrdinalIgnoreCase))
                return ApiStatus.AccessRestricted;

            return ApiStatus.Unknown;
        }

        public override void Write(Utf8JsonWriter writer, ApiStatus value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}