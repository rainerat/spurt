using System;
using Spurt.App.Interop;

namespace Spurt.App.Services;

public sealed class HotkeyService : IDisposable
{
    private const int DefaultHotkeyId = 1;

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

        _currentHotkey = hotkey.Trim();

        // Task 9 shell: map current settings hotkey token to a fixed OS hotkey.
        _isRegistered = NativeMethods.RegisterHotKey(IntPtr.Zero, DefaultHotkeyId, NativeMethods.ModControl | NativeMethods.ModAlt, NativeMethods.VkSpace);
        return _isRegistered;
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
