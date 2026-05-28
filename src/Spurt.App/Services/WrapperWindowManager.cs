using Spurt.App.Windows;

namespace Spurt.App.Services;

public sealed class WrapperWindowManager
{
    private readonly Func<WrapperWindow> _windowFactory;
    private WrapperWindow? _wrapperWindow;

    public WrapperWindowManager()
        : this(() => new WrapperWindow())
    {
    }

    internal WrapperWindowManager(Func<WrapperWindow> windowFactory)
    {
        _windowFactory = windowFactory ?? throw new ArgumentNullException(nameof(windowFactory));
    }

    public int CreatedCount { get; private set; }

    public string? LastUri { get; private set; }

    public void Navigate(string uri)
    {
        if (string.IsNullOrWhiteSpace(uri))
        {
            throw new ArgumentException("URI is required.", nameof(uri));
        }

        if (_wrapperWindow is null)
        {
            _wrapperWindow = _windowFactory();
            CreatedCount++;
        }

        _wrapperWindow.Navigate(uri);
        LastUri = uri;
    }

    public void Close()
    {
        _wrapperWindow = null;
        LastUri = null;
    }
}
