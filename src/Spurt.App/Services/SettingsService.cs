using System.IO;
using System.Text.Json;
using Spurt.App.Models;

namespace Spurt.App.Services;

public sealed class SettingsService
{
    private readonly string _path;
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };
    private static readonly AppSettings DefaultSettings = new();

    public SettingsService(string path) => _path = path;

    public static SettingsService CreateForTests(string path) => new(path);

    public AppSettings Load()
    {
        if (!File.Exists(_path))
        {
            return CloneDefaults();
        }

        try
        {
            var json = File.ReadAllText(_path);
            return JsonSerializer.Deserialize<AppSettings>(json) ?? CloneDefaults();
        }
        catch (JsonException)
        {
            // Malformed or incompatible JSON should fall back to defaults.
            return CloneDefaults();
        }
        catch (IOException)
        {
            return CloneDefaults();
        }
        catch (UnauthorizedAccessException)
        {
            return CloneDefaults();
        }
    }

    public void Save(AppSettings settings)
    {
        var directory = Path.GetDirectoryName(_path);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.WriteAllText(_path, JsonSerializer.Serialize(settings, JsonOptions));
    }

    public AppSettings Update(Func<AppSettings, AppSettings> update)
    {
        var updated = update(Load());
        Save(updated);
        return updated;
    }

    private static AppSettings CloneDefaults()
    {
        return new AppSettings
        {
            Hotkey = DefaultSettings.Hotkey,
            LaunchOnStartup = DefaultSettings.LaunchOnStartup,
            CloseOnUnfocus = DefaultSettings.CloseOnUnfocus,
            SearchEngineTemplate = DefaultSettings.SearchEngineTemplate,
            ThemeMode = DefaultSettings.ThemeMode
        };
    }
}
