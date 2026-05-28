namespace Spurt.App.Windows;

public sealed partial class SearchBoxWindow : Microsoft.UI.Xaml.Window
{
    public string QueryText { get; private set; } = string.Empty;

    public SearchBoxWindow()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        // XAML code generation is not enabled in this project configuration.
    }

    public void SetQuery(string value)
    {
        QueryText = value ?? string.Empty;
    }
}
