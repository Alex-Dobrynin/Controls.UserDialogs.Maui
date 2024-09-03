using Android.OS;

namespace Controls.UserDialogs.Maui;

public class ConfigStore
{
    public string BundleKey { get; set; } = "UserDialogFragmentConfig";

    private long _counter = 0;
    private readonly IDictionary<long, object> _configStore = new Dictionary<long, object>();

    public static ConfigStore Instance { get; } = new ConfigStore();

    public void Store(Bundle bundle, object config)
    {
        _counter++;
        _configStore[_counter] = config;
        bundle.PutLong(BundleKey, _counter);
    }

    public bool Contains(Bundle? bundle) => _configStore.ContainsKey(bundle?.GetLong(BundleKey, -1) ?? -1);

    public T Pop<T>(Bundle bundle) where T : class
    {
        var id = bundle.GetLong(BundleKey);
        var cfg = (T)_configStore[id];
        _configStore.Remove(id);
        return cfg;
    }
}