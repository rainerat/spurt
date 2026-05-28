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

        var targetUri = new Uri(uri, UriKind.Absolute);
        SetWebViewSource(targetUri);

        LastNavigatedUri = targetUri.AbsoluteUri;
        Activate();
    }

    private void SetWebViewSource(Uri uri)
    {
        var webView = GetType().GetField("WebView")?.GetValue(this)
            ?? GetType().GetProperty("WebView")?.GetValue(this);
        var sourceProperty = webView?.GetType().GetProperty("Source");
        if (sourceProperty?.CanWrite == true)
        {
            sourceProperty.SetValue(webView, uri);
        }
    }
}
