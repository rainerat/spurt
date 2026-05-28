namespace Spurt.App.Services;

public sealed class WrapperWindowManager
{
    private bool _created;

    public int CreatedCount { get; private set; }

    public string? LastUri { get; private set; }

    public void Navigate(string uri)
    {
        if (!_created)
        {
            _created = true;
            CreatedCount++;
        }

        LastUri = uri;
    }

    public void Close()
    {
        _created = false;
        LastUri = null;
    }
}
