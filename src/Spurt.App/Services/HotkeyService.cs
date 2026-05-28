using System;
using Spurt.App.Interop;

namespace Spurt.App.Services;

public sealed class HotkeyService : IDisposable
{
    private const int DefaultHotkeyId = 1;
    private const uint Vk0 = 0x30;
    private const uint VkA = 0x41;

    private bool _isRegistered;
    private string _currentHotkey = string.Empty;

    public string CurrentHotkey => _currentHotkey;

    public bool TryRegister(string? hotkey)
    {
        Unregister();

        if (string.IsNullOrWhiteSpace(hotkey))
        {
            _currentHotkey = string.Empty;
            return false;
        }

        var trimmedHotkey = hotkey.Trim();
        if (!TryParseHotkey(trimmedHotkey, out var modifiers, out var key))
        {
            _currentHotkey = string.Empty;
            return false;
        }

        _currentHotkey = trimmedHotkey;
        _isRegistered = NativeMethods.RegisterHotKey(IntPtr.Zero, DefaultHotkeyId, modifiers, key);
        return _isRegistered;
    }

    internal static bool TryParseHotkey(string hotkey, out uint modifiers, out uint key)
    {
        modifiers = 0;
        key = 0;

        if (string.IsNullOrWhiteSpace(hotkey))
        {
            return false;
        }

        var tokens = hotkey.Split('+', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        if (tokens.Length == 0)
        {
            return false;
        }

        var hasKey = false;
        foreach (var token in tokens)
        {
            if (TryApplyModifier(token, ref modifiers))
            {
                continue;
            }

            if (!TryParseTerminalKey(token, out var parsedKey))
            {
                return false;
            }

            if (hasKey)
            {
                return false;
            }

            key = parsedKey;
            hasKey = true;
        }

        return modifiers != 0 && hasKey;
    }

    private static bool TryApplyModifier(string token, ref uint modifiers)
    {
        switch (token.ToUpperInvariant())
        {
            case "CTRL":
            case "CONTROL":
                modifiers |= NativeMethods.ModControl;
                return true;
            case "ALT":
                modifiers |= NativeMethods.ModAlt;
                return true;
            case "SHIFT":
                modifiers |= NativeMethods.ModShift;
                return true;
            case "WIN":
            case "WINDOWS":
                modifiers |= NativeMethods.ModWin;
                return true;
            default:
                return false;
        }
    }

    private static bool TryParseTerminalKey(string token, out uint key)
    {
        key = 0;

        if (token.Equals("SPACE", StringComparison.OrdinalIgnoreCase))
        {
            key = NativeMethods.VkSpace;
            return true;
        }

        if (token.Length != 1)
        {
            return false;
        }

        var c = char.ToUpperInvariant(token[0]);
        if (c >= 'A' && c <= 'Z')
        {
            key = VkA + (uint)(c - 'A');
            return true;
        }

        if (c >= '0' && c <= '9')
        {
            key = Vk0 + (uint)(c - '0');
            return true;
        }

        return false;
    }

    public void Unregister()
    {
        if (!_isRegistered)
        {
            return;
        }

        NativeMethods.UnregisterHotKey(IntPtr.Zero, DefaultHotkeyId);
        _isRegistered = false;
    }

    public void Dispose()
    {
        Unregister();
        GC.SuppressFinalize(this);
    }
}
