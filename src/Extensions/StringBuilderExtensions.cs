#pragma warning disable IDE0130
namespace System.Text;
#pragma warning restore IDE0130

internal static class StringBuilderExtensions
{
    public static StringBuilder AppendNameValueBoolean(this StringBuilder sb, string name, bool? value)
    {
        ArgumentNullException.ThrowIfNull(sb);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return value == null ? sb : sb.Append($"&{name}={(value.Value ? "true" : "false")}");
    }
}