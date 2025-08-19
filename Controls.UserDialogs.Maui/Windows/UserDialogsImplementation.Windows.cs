using System;
using System.Linq;
using Microsoft.Maui.Controls;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Platform;

namespace Controls.UserDialogs.Maui
{
    // Windows-specific partial implementation.
    // This provides minimal, non-invasive platform stubs so the library can target Windows.
    // Platform-specific UI (dialogs) can be improved later; for now we use simple MainThread displays.
    public partial class UserDialogsImplementation
    {
        public virtual partial IDisposable Alert(AlertConfig config)
        {
            try
            {
                // Use MainThread to ensure UI thread invocation
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    // Use a simple DisplayAlert from the current Application MainPage if available.
                    var page = Application.Current?.MainPage;
                    if (page is not null)
                    {
                        _ = page.DisplayAlert(config.Title ?? string.Empty, config.Message ?? string.Empty, config.OkText ?? AlertConfig.DefaultOkText);
                        config.Action?.Invoke();
                    }
                });
            }
            catch
            {
                // swallow - best-effort on platforms that may not have UI present during tests
            }

            return (IDisposable)this;
        }

        public virtual partial IDisposable Confirm(ConfirmConfig config)
        {
            try
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    var page = Application.Current?.MainPage;
                    if (page is not null)
                    {
                        bool result = await page.DisplayAlert(config.Title ?? string.Empty, config.Message ?? string.Empty, config.OkText ?? ConfirmConfig.DefaultOkText, config.CancelText ?? ConfirmConfig.DefaultCancelText);
                        config.Action?.Invoke(result);
                    }
                });
            }
            catch
            {
            }

            return (IDisposable)this;
        }

        public virtual partial IDisposable ActionSheet(ActionSheetConfig config)
        {
            try
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    var page = Application.Current?.MainPage;
                    if (page is null) return;

                    // Build choices list from config (cancel/destructive/options)
                    var optionTexts = config.Options?.Select(o => o.Text).ToList() ?? new System.Collections.Generic.List<string>();
                    string? cancelText = config.Cancel?.Text;
                    string? destructiveText = config.Destructive?.Text;

                    // DisplayActionSheet expects: title, cancel, destruction, params string[] buttons
                    string? result = await page.DisplayActionSheet(config.Title ?? string.Empty, cancelText, destructiveText, optionTexts.ToArray());

                    if (result is null) return;

                    // If result matches cancel
                    if (config.Cancel is not null && result == config.Cancel.Text)
                    {
                        config.Cancel.Action?.Invoke();
                        return;
                    }

                    // If result matches destructive
                    if (config.Destructive is not null && result == config.Destructive.Text)
                    {
                        config.Destructive.Action?.Invoke();
                        return;
                    }

                    // Otherwise find matching option
                    var opt = config.Options?.FirstOrDefault(o => o.Text == result);
                    opt?.Action?.Invoke();
                });
            }
            catch
            {
            }

            return (IDisposable)this;
        }

        public virtual partial IDisposable ShowToast(ToastConfig config)
        {
            // MAUI does not have a built-in toast; fallback to a short DisplayAlert-less notification via MainPage.
            try
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    var page = Application.Current?.MainPage;
                    if (page is not null)
                    {
                        // Non-blocking: show a brief alert as a fallback
                        _ = page.DisplayAlert(string.Empty, config.Message ?? string.Empty, "OK");
                    }
                });
            }
            catch
            {
            }

            return (IDisposable)this;
        }

        public virtual partial IDisposable ShowSnackbar(SnackbarConfig config)
        {
            // No native snackbar in basic MAUI Windows host; fallback to toast behavior.
            return ShowToast(new ToastConfig { Message = config.Message, Duration = config.Duration });
        }

        protected virtual partial IHudDialog? CreateHudInstance(HudDialogConfig config)
        {
            // No HUD library wired for Windows yet; return null to let shared code handle absence.
            return null;
        }
    }
}