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

        return TrySubmitQuery(QueryText);
    }

    private bool TrySubmitQuery(string? rawQuery)
    {
        if (string.IsNullOrWhiteSpace(rawQuery))
        {
            return false;
        }

        var uri = _router.BuildSearchUri(_template, rawQuery);
        _wrapperManager.Navigate(uri);
        return true;
    }

    private void QueryInput_KeyDown(object sender, object e)
    {
        var key = GetKey(e);
        if (key is null)
        {
            return;
        }

        var currentText = GetText(sender);
        SetQuery(currentText ?? string.Empty);
        var handled = TryHandleKeyDown(key.Value);
        SetHandled(e, handled);
    }

    private static global::Windows.System.VirtualKey? GetKey(object args)
    {
        var keyProperty = args.GetType().GetProperty("Key");
        if (keyProperty?.GetValue(args) is global::Windows.System.VirtualKey key)
        {
            return key;
        }

        return null;
    }

    private static string? GetText(object sender)
    {
        var textProperty = sender.GetType().GetProperty("Text");
        return textProperty?.GetValue(sender) as string;
    }

    private static void SetHandled(object args, bool handled)
    {
        var handledProperty = args.GetType().GetProperty("Handled");
        if (handledProperty?.CanWrite == true)
        {
            handledProperty.SetValue(args, handled);
        }
    }
}
