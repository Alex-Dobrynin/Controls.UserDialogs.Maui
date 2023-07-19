using Android.OS;

namespace Controls.UserDialogs.Maui;

public class ConfigStore
{
    public string BundleKey { get; set; } = "UserDialogFragmentConfig";

    long _counter = 0;
    readonly IDictionary<long, object> _configStore = new Dictionary<long, object>();

    public static ConfigStore Instance { get; } = new ConfigStore();

    public void Store(Bundle bundle, object config)
    {
        this._counter++;
        this._configStore[this._counter] = config;
        bundle.PutLong(this.BundleKey, this._counter);
    }

    public bool Contains(Bundle bundle) => _configStore.ContainsKey(bundle?.GetLong(BundleKey, -1) ?? -1);

    public T Pop<T>(Bundle bundle) where T : class
    {
        var id = bundle.GetLong(this.BundleKey);
        var cfg = (T)this._configStore[id];
        this._configStore.Remove(id);
        return cfg;
    }
}