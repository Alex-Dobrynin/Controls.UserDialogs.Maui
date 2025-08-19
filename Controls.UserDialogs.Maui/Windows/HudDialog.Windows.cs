using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Platform;

namespace Controls.UserDialogs.Maui
{
    // Minimal IHudDialog implementation for Windows target.
    // Provides non-blocking, MainThread-safe stubs that satisfy the interface
    // so library compiles and basic HUD APIs no-op safely on Windows.
    public class WindowsHudDialog : IHudDialog
    {
        CancellationTokenSource? _cts;
        HudDialogConfig _config;

        public WindowsHudDialog(HudDialogConfig config)
        {
            _config = config;
            _cts = new CancellationTokenSource();

            if (config.AutoShow)
            {
                Show();
            }
        }

        public void Update(string? message = null, int percentComplete = -1, string? image = null, string? cancelText = null, bool show = true, MaskType? maskType = null, Action? cancel = null)
        {
            if (message is not null) _config.Message = message;
            if (percentComplete >= 0) _config.PercentComplete = percentComplete;
            if (image is not null) _config.Image = image;
            if (cancelText is not null) _config.CancelText = cancelText;
            _config.AutoShow = show;
            if (cancel is not null) _config.Cancel = cancel;
            // No visual HUD for now on Windows; could be extended with WinUI popup later.
        }

        public void Update(HudDialogConfig config)
        {
            _config = config;
        }

        public void Show()
        {
            try
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    // If a message exists, show a lightweight notification using MainPage.
                    var page = Application.Current?.MainPage;
                    if (page is not null && !string.IsNullOrEmpty(_config.Message))
                    {
                        // Non-blocking: show a transient alert as fallback.
                        _ = page.DisplayAlert(string.Empty, _config.Message, _config.CancelText ?? "OK");
                    }

                    // Simulate progress if percent provided (fire-and-forget).
                    if (_config.PercentComplete >= 0)
                    {
                        await Task.Delay(500);
                    }
                });
            }
            catch
            {
                // swallow
            }
        }

        public void Hide()
        {
            // No-op for now.
        }

        public void Dispose()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }
    }
}