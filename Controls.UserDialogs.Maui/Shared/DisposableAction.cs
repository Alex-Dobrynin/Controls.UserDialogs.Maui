namespace Controls.UserDialogs.Maui;

public class DisposableAction : IDisposable
{
    readonly Action _action;

    public DisposableAction(Action action)
    {
        _action = action;
    }

    public void Dispose()
    {
        _action();
        GC.SuppressFinalize(this);
    }
}