using Controls.UserDialogs.Maui;

namespace Sample;

public partial class ActionSheetTestPage : ContentPage
{
    private readonly IUserDialogs _userDialogs;
    
    public ActionSheetTestPage(IUserDialogs userDialogs)
    {
        _userDialogs = userDialogs;
        
        InitializeComponent();
    }
    
    private void Button_Clicked_1(object sender, EventArgs e)
    {
#if MACCATALYST
            UserDialogs.Instance.Alert("Bottom Action sheet is not supported on mac catalyst", "Warning", "Understand", "dotnet_bot.png");
#else
        var config = new ActionSheetConfig()
        {
            UseBottomSheet = true,
            Destructive = new ActionSheetOption("Destroy", () => { }, "dotnet_bot.png"),
            Cancel = new ActionSheetOption("Cancel", () => 
            { 
                
            }, "dotnet_bot.png"),
            Title = "Bottom Action sheet",
            Message = "This is Bottom Action sheet",
            Icon = "dotnet_bot.png",
            Options = new ActionSheetOption[]
            {
                new ActionSheetOption("First option", () => { }, "dotnet_bot.png"),
                new ActionSheetOption("Second option", () => { }, "dotnet_bot.png"),
                new ActionSheetOption("Third option", () => { }, "dotnet_bot.png"),
            }
        };

        _userDialogs.ActionSheet(config);
#endif
    }

    private async void Button_Clicked_2(object sender, EventArgs e)
    {
#if MACCATALYST
            UserDialogs.Instance.Alert("Async Bottom Action sheet is not supported on mac catalyst", "Warning", "Understand", "dotnet_bot.png");
#else
        var res = await _userDialogs.ActionSheetAsync(
            "This is Async Bottom Action sheet",
            "Async Bottom Action sheet",
            "Cancel",
            "Destroy",
            "dotnet_bot.png",
            true,
            default,
            "First option",
            "Second option",
            "Third option"
        );

        var r = res;
#endif
    }
    
    private void Button_Clicked_3(object sender, EventArgs e)
        {
#if MACCATALYST
            UserDialogs.Instance.Alert("Bottom Action sheet is not supported on mac catalyst", "Warning", "Understand", "dotnet_bot.png");
#else
            var config = new ActionSheetConfig()
            {
                UseBottomSheet = true,
                Destructive = new ActionSheetOption("Destroy", () => { }, "dotnet_bot.png"),
                Cancel = new ActionSheetOption("Cancel", () => 
                { 
                
                }, "dotnet_bot.png"),
                Title = null,
                Message = "This is Bottom Action sheet",
                Icon = "dotnet_bot.png",
                Options = new ActionSheetOption[]
                {
                    new ActionSheetOption("First option", () => { }, "dotnet_bot.png"),
                    new ActionSheetOption("Second option", () => { }, "dotnet_bot.png"),
                    new ActionSheetOption("Third option", () => { }, "dotnet_bot.png"),
                }
            };

            _userDialogs.ActionSheet(config);
#endif
        }
        
        private void Button_Clicked_5(object sender, EventArgs e)
        {
#if MACCATALYST
            UserDialogs.Instance.Alert("Bottom Action sheet is not supported on mac catalyst", "Warning", "Understand", "dotnet_bot.png");
#else
            var config = new ActionSheetConfig()
            {
                UseBottomSheet = true,
                Destructive = new ActionSheetOption("Destroy", () => { }, "dotnet_bot.png"),
                Cancel = new ActionSheetOption("Cancel", () => 
                { 
                
                }, "dotnet_bot.png"),
                Title = null,
                Message = null,
                Icon = "dotnet_bot.png",
                Options = new ActionSheetOption[]
                {
                    new ActionSheetOption("First option", () => { }, "dotnet_bot.png"),
                    new ActionSheetOption("Second option", () => { }, "dotnet_bot.png"),
                    new ActionSheetOption("Third option", () => { }, "dotnet_bot.png"),
                }
            };

            _userDialogs.ActionSheet(config);
#endif
        }
    
    private async void Button_Clicked_4(object sender, EventArgs e)
    {
#if MACCATALYST
            UserDialogs.Instance.Alert("Async Bottom Action sheet is not supported on mac catalyst", "Warning", "Understand", "dotnet_bot.png");
#else
        var res = await _userDialogs.ActionSheetAsync(
            "This is Async Bottom Action sheet",
            null,
            "Cancel",
            "Destroy",
            "dotnet_bot.png",
            true,
            default,
            "First option",
            "Second option",
            "Third option"
        );

        var r = res;
#endif
    }
        
    private async void Button_Clicked_6(object sender, EventArgs e)
    {
#if MACCATALYST
            UserDialogs.Instance.Alert("Async Bottom Action sheet is not supported on mac catalyst", "Warning", "Understand", "dotnet_bot.png");
#else
        var res = await _userDialogs.ActionSheetAsync(
            null,
            null,
            "Cancel",
            "Destroy",
            "dotnet_bot.png",
            true,
            default,
            "First option",
            "Second option",
            "Third option"
        );

        var r = res;
#endif
    }
}