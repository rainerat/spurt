using Spurt.App.Services;

namespace Spurt.App.Windows;

public sealed partial class SearchBoxWindow : Microsoft.UI.Xaml.Window
{
    private readonly SearchRouter _router;
    private readonly WrapperWindowManager _wrapperManager;
    private readonly SettingsService _settingsService;
    private readonly StartupService _startupService;
    private readonly ThemeService _themeService;
    private SettingsWindow? _settingsWindow;
    private string _template = string.Empty;

    public string QueryText { get; private set; } = string.Empty;

    public SearchBoxWindow()
        : this(
            new SearchRouter(),
            new WrapperWindowManager(),
            new SettingsService(Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Spurt",
                "settings.json")),
            new StartupService(),
            new ThemeService())
    {
    }

    internal SearchBoxWindow(
        SearchRouter router,
        WrapperWindowManager wrapperManager,
        SettingsService settingsService,
        StartupService startupService,
        ThemeService themeService)
    {
        _router = router ?? throw new ArgumentNullException(nameof(router));
        _wrapperManager = wrapperManager ?? throw new ArgumentNullException(nameof(wrapperManager));
        _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
        _startupService = startupService ?? throw new ArgumentNullException(nameof(startupService));
        _themeService = themeService ?? throw new ArgumentNullException(nameof(themeService));
        InitializeComponent();
        ApplySettings(_settingsService.Load());
    }

    private void InitializeComponent()
    {
        // XAML code generation is not enabled in this project configuration.
    }

    public void SetQuery(string value)
    {
        QueryText = value ?? string.Empty;
    }

    public bool TryHandleKeyDown(global::Windows.System.VirtualKey key)
    {
        if (key != global::Windows.System.VirtualKey.Enter)
        {
            return false;
        }

        return TrySubmitQuery(QueryText);
    }

    private bool TrySubmitQuery(string? rawQuery)
    {
        if (string.IsNullOrWhiteSpace(rawQuery))
        {
            return false;
        }

        var uri = _router.BuildSearchUri(_template, rawQuery);
        _wrapperManager.Navigate(uri);
        return true;
    }

    public void OpenSettingsWindow()
    {
        _settingsWindow ??= new SettingsWindow(_settingsService, _startupService, _themeService);
        _settingsWindow.Activate();
    }

    public void ApplySettingsFromWindow()
    {
        if (_settingsWindow is null)
        {
            return;
        }

        _settingsWindow.ApplyAndSave();
        ApplySettings(_settingsWindow.CurrentSettings);
    }

    private void ApplySettings(Models.AppSettings settings)
    {
        _template = settings.SearchEngineTemplate;
        _startupService.SetEnabled(settings.LaunchOnStartup);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1144:Unused private types or members should be removed", Justification = "Bound from XAML KeyDown event")]
    private void QueryInput_KeyDown(object sender, object e)
    {
        var key = GetKey(e);
        if (key is null)
        {
            return;
        }

        var currentText = GetText(sender);
        SetQuery(currentText ?? string.Empty);
        var handled = TryHandleKeyDown(key.Value);
        SetHandled(e, handled);
    }

    private static global::Windows.System.VirtualKey? GetKey(object args)
    {
        var keyProperty = args.GetType().GetProperty("Key");
        if (keyProperty?.GetValue(args) is global::Windows.System.VirtualKey key)
        {
            return key;
        }

        return null;
    }

    private static string? GetText(object sender)
    {
        var textProperty = sender.GetType().GetProperty("Text");
        return textProperty?.GetValue(sender) as string;
    }

    private static void SetHandled(object args, bool handled)
    {
        var handledProperty = args.GetType().GetProperty("Handled");
        if (handledProperty?.CanWrite == true)
        {
            handledProperty.SetValue(args, handled);
        }
    }
}
