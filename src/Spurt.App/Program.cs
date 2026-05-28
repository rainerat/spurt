namespace Spurt.App;

public static class Program
{
    [STAThread]
    private static void Main(string[] args)
    {
        Microsoft.UI.Xaml.Application.Start(_ => new App());
    }
}
