using Spurt.App.Windows;

namespace Spurt.App;

public sealed partial class App : Microsoft.UI.Xaml.Application
{
    private Microsoft.UI.Xaml.Window? _window;

    protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        _window = new SearchBoxWindow();
        _window.Activate();
    }
}
