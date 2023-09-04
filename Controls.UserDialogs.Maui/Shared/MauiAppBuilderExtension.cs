namespace Controls.UserDialogs.Maui;

public static class MauiAppBuilderExtension
{
    public static MauiAppBuilder UseUserDialogs(this MauiAppBuilder builder, Action configure = null)
    {
        UserDialogs.Instance = new UserDialogsImplementation();

        configure?.Invoke();

        return builder;
    }

    public static MauiAppBuilder UseUserDialogs(this MauiAppBuilder builder, bool registerInterface, Action configure = null)
    {
        UseUserDialogs(builder, configure);

        if (registerInterface)
        {
            builder.Services.AddTransient((s) => UserDialogs.Instance);
        }

        return builder;
    }
}
