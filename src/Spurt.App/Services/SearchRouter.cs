namespace Spurt.App.Services;

public sealed class SearchRouter
{
    public string BuildSearchUri(string template, string rawQuery)
    {
        if (string.IsNullOrWhiteSpace(template))
        {
            throw new ArgumentException("Template is required.", nameof(template));
        }

        if (string.IsNullOrWhiteSpace(rawQuery))
        {
            throw new ArgumentException("Query is required.", nameof(rawQuery));
        }

        if (!template.Contains("{query}", StringComparison.Ordinal))
        {
            throw new ArgumentException("Template must include {query}.", nameof(template));
        }

        var encoded = Uri.EscapeDataString(rawQuery.Trim());
        return template.Replace("{query}", encoded, StringComparison.Ordinal);
    }
}
