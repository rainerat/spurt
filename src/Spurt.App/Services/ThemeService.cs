using System;

namespace Spurt.App.Services;

public sealed class ThemeService
{
    public string Normalize(string? mode)
    {
        if (string.Equals(mode, "Light", StringComparison.OrdinalIgnoreCase))
        {
            return "Light";
        }

        if (string.Equals(mode, "Dark", StringComparison.OrdinalIgnoreCase))
        {
            return "Dark";
        }

        return "System";
    }
}
