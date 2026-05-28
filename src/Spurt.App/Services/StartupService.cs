namespace Spurt.App.Services;

public sealed class StartupService
{
    public bool IsEnabled { get; private set; } = true;

    public void SetEnabled(bool enabled)
    {
        IsEnabled = enabled;
    }
}
