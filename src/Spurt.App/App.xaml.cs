using Spurt.App.Windows;

namespace Spurt.App;

public sealed partial class App
{
    public static void Launch()
    {
        _ = new SearchBoxWindow();
    }
}
