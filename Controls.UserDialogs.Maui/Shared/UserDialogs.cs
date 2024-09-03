namespace Controls.UserDialogs.Maui;

public static class UserDialogs
{
    private static IUserDialogs? _currentInstance;

    public static IUserDialogs Instance
    {
        get
        {
            if (_currentInstance is null)
                throw new ArgumentException("[Controls.UserDialogs.Maui] You must call UseUserDialogs() in your MauiProgram for initialization");

            return _currentInstance;
        }
        set => _currentInstance = value;
    }

    public static MauiAppBuilder UseUserDialogs(this MauiAppBuilder builder, Action? configure = null)
    {
        return UseUserDialogs(builder, false, configure);
    }

    public static MauiAppBuilder UseUserDialogs(this MauiAppBuilder builder, bool registerInterface, Action? configure = null)
    {
        Instance = new UserDialogsImplementation();

        configure?.Invoke();

        if (registerInterface)
        {
            builder.Services.AddTransient((s) => Instance);
        }

        return builder;
    }
}