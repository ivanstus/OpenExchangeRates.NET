// ReSharper disable once CheckNamespace
namespace System.Text;

internal static class StringBuilderExtensions
{
    public static StringBuilder AppendNameValueBoolean(this StringBuilder sb, string name, bool? value)
    {
        ArgumentNullException.ThrowIfNull(sb);

#if NET8_0_OR_GREATER
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
#elif NET7_0
        ArgumentException.ThrowIfNullOrEmpty(name);
#else
        ArgumentNullException.ThrowIfNull(name);
#endif

        return value == null ? sb : sb.Append($"&{name}={(value.Value ? "true" : "false")}");
    }
}