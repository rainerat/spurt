using Spurt.App.Services;

namespace Spurt.App.Windows;

public sealed partial class SearchBoxWindow : Microsoft.UI.Xaml.Window
{
    private readonly SearchRouter _router;
    private readonly WrapperWindowManager _wrapperManager;
    private readonly string _template;

    public string QueryText { get; private set; } = string.Empty;

    public SearchBoxWindow()
        : this(new SearchRouter(), new WrapperWindowManager(), "https://www.google.com/search?q={query}")
    {
    }

    internal SearchBoxWindow(SearchRouter router, WrapperWindowManager wrapperManager, string template)
    {
        _router = router ?? throw new ArgumentNullException(nameof(router));
        _wrapperManager = wrapperManager ?? throw new ArgumentNullException(nameof(wrapperManager));
        _template = template ?? throw new ArgumentNullException(nameof(template));
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

    public bool TryHandleKeyDown(global::Windows.System.VirtualKey key)
    {
        if (key != global::Windows.System.VirtualKey.Enter)
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(QueryText))
        {
            return false;
        }

        var uri = _router.BuildSearchUri(_template, QueryText);
        _wrapperManager.Navigate(uri);
        return true;
    }
}
