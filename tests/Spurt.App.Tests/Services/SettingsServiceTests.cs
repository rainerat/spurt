using System;
using System.IO;
using FluentAssertions;
using Spurt.App.Services;
using Xunit;

namespace Spurt.App.Tests.Services;

public class SettingsServiceTests
{
    [Fact]
    public void Load_WhenFileMissing_ReturnsDefaults()
    {
        var service = SettingsService.CreateForTests(Path.GetTempFileName() + ".missing");
        var settings = service.Load();
        settings.Hotkey.Should().Be("Ctrl+Alt");
        settings.LaunchOnStartup.Should().BeTrue();
        settings.CloseOnUnfocus.Should().BeFalse();
        settings.ThemeMode.Should().Be("System");
    }

    [Fact]
    public void SaveAndLoad_RoundTripsValues()
    {
        var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".json");
        var service = SettingsService.CreateForTests(path);
        service.Save(new()
        {
            Hotkey = "Ctrl+Shift+Space",
            LaunchOnStartup = false,
            CloseOnUnfocus = true,
            SearchEngineTemplate = "https://duckduckgo.com/?q={query}",
            ThemeMode = "Dark"
        });

        var loaded = service.Load();
        loaded.Hotkey.Should().Be("Ctrl+Shift+Space");
        loaded.LaunchOnStartup.Should().BeFalse();
        loaded.CloseOnUnfocus.Should().BeTrue();
        loaded.SearchEngineTemplate.Should().Contain("{query}");
        loaded.ThemeMode.Should().Be("Dark");
    }

    [Fact]
    public void Load_WhenJsonCorrupt_ReturnsDefaults()
    {
        var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".json");
        File.WriteAllText(path, "{broken");
        var service = SettingsService.CreateForTests(path);

        var loaded = service.Load();
        loaded.Hotkey.Should().Be("Ctrl+Alt");
    }

    [Fact]
    public void Save_WithFilenameOnlyPath_WritesFileInCurrentDirectory()
    {
        var originalCurrentDirectory = Directory.GetCurrentDirectory();
        var tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);
        var fileName = $"{Guid.NewGuid():N}.json";

        try
        {
            Directory.SetCurrentDirectory(tempDirectory);
            var service = SettingsService.CreateForTests(fileName);

            service.Save(new());

            File.Exists(Path.Combine(tempDirectory, fileName)).Should().BeTrue();
        }
        finally
        {
            Directory.SetCurrentDirectory(originalCurrentDirectory);
            Directory.Delete(tempDirectory, recursive: true);
        }
    }
}
