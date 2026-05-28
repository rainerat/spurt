namespace Spurt.App.Windows;

public sealed partial class WrapperWindow : Microsoft.UI.Xaml.Window
{
    public string? LastNavigatedUri { get; private set; }

    public WrapperWindow()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        // XAML code generation is not enabled in this project configuration.
    }

    public void Navigate(string uri)
    {
        if (string.IsNullOrWhiteSpace(uri))
        {
            throw new ArgumentException("URI is required.", nameof(uri));
        }

        LastNavigatedUri = uri;
        Activate();
    }
}
