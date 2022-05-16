// ReSharper disable once CheckNamespace
namespace System.Text;

internal static class StringBuilderExtensions
{
    public static StringBuilder AppendNameValueBoolean(this StringBuilder sb, string name, bool? value)
    {
        ArgumentNullException.ThrowIfNull(sb);
        ArgumentNullException.ThrowIfNull(name);

        return value == null ? sb : sb.Append($"&{name}={(value.Value ? "true" : "false")}");
    }
}