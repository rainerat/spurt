using System;

namespace Microsoft.UI.Xaml;

public class LaunchActivatedEventArgs : EventArgs
{
    public bool IsStub => true;
}

public class Window
{
    public virtual void Activate()
    {
    }
}

public class Application
{
    public static void Start(Action<object?> callback)
    {
        callback(new object());
    }

    protected virtual void OnLaunched(LaunchActivatedEventArgs args)
    {
    }
}
