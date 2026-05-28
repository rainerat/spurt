namespace Spurt.App.Models;

public sealed class AppSettings
{
    public string Hotkey { get; set; } = "Ctrl+Alt";
    public bool LaunchOnStartup { get; set; } = true;
    public bool CloseOnUnfocus { get; set; } = false;
    public string SearchEngineTemplate { get; set; } = "https://www.google.com/search?q={query}";
    public string ThemeMode { get; set; } = "System";
}
