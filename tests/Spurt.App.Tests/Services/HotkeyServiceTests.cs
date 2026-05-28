using FluentAssertions;
using Spurt.App.Services;
using Xunit;

namespace Spurt.App.Tests.Services;

public class HotkeyServiceTests
{
    [Theory]
    [InlineData("Ctrl+Alt+Space", 0x0002 | 0x0001, 0x20)]
    [InlineData("Ctrl+Shift+A", 0x0002 | 0x0004, 0x41)]
    [InlineData("Alt+9", 0x0001, 0x39)]
    [InlineData("Win+Z", 0x0008, 0x5A)]
    public void TryParseHotkey_WithValidHotkeys_ReturnsModifiersAndKey(string hotkey, uint expectedModifiers, uint expectedKey)
    {
        var parsed = HotkeyService.TryParseHotkey(hotkey, out var modifiers, out var key);

        parsed.Should().BeTrue();
        modifiers.Should().Be(expectedModifiers);
        key.Should().Be(expectedKey);
    }

    [Theory]
    [InlineData("A")]
    [InlineData("Ctrl+Hyper+A")]
    [InlineData("Ctrl+A+B")]
    [InlineData("")]
    [InlineData("   ")]
    public void TryParseHotkey_WithInvalidHotkeys_ReturnsFalse(string hotkey)
    {
        var parsed = HotkeyService.TryParseHotkey(hotkey, out _, out _);

        parsed.Should().BeFalse();
    }
}
