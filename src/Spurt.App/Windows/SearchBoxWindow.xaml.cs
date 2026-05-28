namespace Spurt.App.Windows;

public sealed partial class SearchBoxWindow
{
    public string QueryText { get; private set; } = string.Empty;

    public SearchBoxWindow()
    {
    }

    public void SetQuery(string value)
    {
        QueryText = value ?? string.Empty;
    }
}
