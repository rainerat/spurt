using Spurt.App.Models;
using Spurt.App.Services;

namespace Spurt.App.Windows;

public sealed partial class SettingsWindow : Microsoft.UI.Xaml.Window
{
    private readonly SettingsService _settingsService;
    private readonly StartupService _startupService;
    private readonly ThemeService _themeService;
    private readonly AppSettings _settings;

    public SettingsWindow(SettingsService settingsService, StartupService startupService, ThemeService? themeService = null)
    {
        _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
        _startupService = startupService ?? throw new ArgumentNullException(nameof(startupService));
        _themeService = themeService ?? new ThemeService();
        InitializeComponent();

        _settings = _settingsService.Load();
        LoadControlsFromSettings(_settings);
    }

    public AppSettings CurrentSettings => _settings;

    public bool StartupEnabled => StartupToggle.IsOn;
    public bool CloseOnUnfocusEnabled => CloseOnUnfocusToggle.IsOn;
    public string ThemeMode => ThemeModeCombo.SelectedItem ?? "System";
    public string EngineTemplate => EngineTemplateInput.Text ?? string.Empty;

    public void ApplyAndSave()
    {
        _settings.LaunchOnStartup = StartupToggle.IsOn;
        _settings.CloseOnUnfocus = CloseOnUnfocusToggle.IsOn;
        _settings.ThemeMode = _themeService.Normalize(ThemeModeCombo.SelectedItem);
        _settings.SearchEngineTemplate = string.IsNullOrWhiteSpace(EngineTemplateInput.Text)
            ? AppSettingsDefaults.SearchEngineTemplate
            : EngineTemplateInput.Text.Trim();

        _settingsService.Save(_settings);
        _startupService.SetEnabled(_settings.LaunchOnStartup);
    }

    private void LoadControlsFromSettings(AppSettings settings)
    {
        StartupToggle.IsOn = settings.LaunchOnStartup;
        CloseOnUnfocusToggle.IsOn = settings.CloseOnUnfocus;
        ThemeModeCombo.SelectedItem = _themeService.Normalize(settings.ThemeMode);
        EngineTemplateInput.Text = settings.SearchEngineTemplate;
        _startupService.SetEnabled(settings.LaunchOnStartup);
    }

    private void InitializeComponent()
    {
        // XAML code generation is not enabled in this project configuration.
    }

    private static class AppSettingsDefaults
    {
        public const string SearchEngineTemplate = "https://www.google.com/search?q={query}";
    }

    private sealed class ToggleSwitchAdapter
    {
        public bool IsOn { get; set; }
    }

    private sealed class ComboBoxAdapter
    {
        public string? SelectedItem { get; set; } = "System";
    }

    private sealed class TextBoxAdapter
    {
        public string? Text { get; set; } = string.Empty;
    }

    private ToggleSwitchAdapter StartupToggle { get; } = new();
    private ToggleSwitchAdapter CloseOnUnfocusToggle { get; } = new();
    private ComboBoxAdapter ThemeModeCombo { get; } = new();
    private TextBoxAdapter EngineTemplateInput { get; } = new();
}
