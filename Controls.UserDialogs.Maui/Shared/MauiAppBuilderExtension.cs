namespace Controls.UserDialogs.Maui;

public static class MauiAppBuilderExtension
{
    public static MauiAppBuilder UseUserDialogs(this MauiAppBuilder builder, Action configure = null)
    {
        UserDialogs.Instance = new UserDialogsImplementation();

        configure?.Invoke();

        builder.Services.AddTransient((s) => UserDialogs.Instance);

        return builder;
    }
}
