using Spurt.App.Windows;
using Spurt.App.Services;
using System;
using System.IO;

namespace Spurt.App;

public sealed partial class App : Microsoft.UI.Xaml.Application
{
    private Microsoft.UI.Xaml.Window? _window;
    private readonly SettingsService _settingsService;
    private readonly StartupService _startupService;
    private readonly HotkeyService _hotkeyService;
    private readonly ThemeService _themeService;

    public App()
    {
        _settingsService = new SettingsService(GetSettingsPath());
        _startupService = new StartupService();
        _hotkeyService = new HotkeyService();
        _themeService = new ThemeService();
    }

    protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        var settings = _settingsService.Load();
        _startupService.SetEnabled(settings.LaunchOnStartup);
        _ = _hotkeyService.TryRegister(settings.Hotkey);
        _ = _themeService.Normalize(settings.ThemeMode);

        _window = new SearchBoxWindow();
        _window.Activate();
    }

    private static string GetSettingsPath()
    {
        var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        return Path.Combine(basePath, "Spurt", "settings.json");
    }
}
