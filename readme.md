# <img src="userdialogs_maui_icon.png" width="70" height="70"/> Controls.Userdialogs.Maui

#### A cross platform library that allows you to call for native user dialogs, which can by styled from your maui application anywhere anytime.

Inspired by [Allan Ritchie](https://github.com/aritchie)'s Acr.UserDialogs

##### Since the original (Acr.UserDialogs) repo is out of support, this will give new breath to UserDialogs. It is more flexible to style your dialogs as you want. 

## Supported Platforms

* .NET7 for Android (min 7.0)(major target 13.0)
* .NET7 for iOS (min 14.2)

### Features

* Alert
* Confirm
* Action Sheets
* Loading/Progress
* Toast
* Snackbar
* [Sample](https://github.com/Alex-Dobrynin/Maui.Controls.UserDialogs/tree/develop/Sample)

### As for now it supports only Android and iOS. I don't have in plans to add new platforms. You are welcome to submit PR's for issues you may be having or for features you need and they will be reviewed.

## Setup

To use, make sure you are using the latest version of .NET MAUI

Add ```UseUserDialogs(() => { })``` to your MauiProgram.cs file

```csharp
builder
    .UseMauiApp<App>()
    .UseUserDialogs(() =>
    {
        //setup your default styles for dialogs
        AlertConfig.DefaultBackgroundColor = Colors.Purple;
#if ANDROID
        AlertConfig.DefaultFontFamily = "OpenSans-Regular.ttf";
#else
        AlertConfig.DefaultFontFamily = "OpenSans-Regular";
#endif

        ToastConfig.DefaultCornerRadius = 15;
    })
    .ConfigureFonts(fonts =>
    {
        fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
        fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
    });
```

##### Note: there are some properties available only for Android or only for iOS

## Powered By:

* Android - Progress/Loading uses Redth's [AndHUD](https://github.com/Redth/AndHUD)
* iOS - Progress/Loading uses Nic Wise's [BTProgressHUD](https://github.com/nicwise/BTProgressHUD)

# Frequently Asked Questions

1. I'm getting a nullreferenceexception when using loading.
    * This happens when you run loading (or almost any dialog) from the constructor of your page or viewmodel.  The view hasn't been rendered yet, therefore there is nothing to render to.

2. Navigating while inside of a loading/progress dialog causes exceptions or the progress no longer appears properly
    * Hide the progress dialog before navigating

3. I don't like the way X method works on platform Y
    * No problems. Override the implementation like below. Note: this is a partial class which has shared and platform specific realizations


    ```csharp
    public class MyCustomUserDialogs : Controls.UserDialogs.Maui.UserDialogImplementation 
    {
            public override ..
    }
    ```

    then in you MauiProgram.cs add this

    ```csharp
    builder
        .UseMauiApp<App>()
        .UseUserDialogs(() =>
        {
    #if ANDROID
            Controls.UserDialogs.Maui.UserDialogs.Instance = new MyCustomUserDialogs(); //Android realization
    #else
            Controls.UserDialogs.Maui.UserDialogs.Instance = new MyCustomUserDialogs(); //iOS realization
    #endif

            //setup your default styles for dialogs
        })
        .ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
        });
    ```

4. Why don't you cancel a dialog when the app goes to the background (AND) why do I get an exception when I call for a dialog?

    * USER DIALOGS DOES NOT SOLVE WORLD PEACE! Guess what - most android API version and iOS don't call this.  This library is not a window state manager, if you call for a dialog, 
        it will try to present one. If your app goes to the background and you call for a dialog, iOS & Android are tossing you the exception. The library isn't here to save you from bad design choices.  
        Call us an anti-pattern if you want, we present dialogs!

5. Why does the library allow me to open multiple windows?

    * Similar to #4 - the library does not manage windows. It opens dialogs - SURPRISE
    
6. I'd like to customize the dialogs in native way (e.g. in Android in styles or themes)

    * The library wasn't really designed or meant for this. It was meant for using native dialogs with programmatically styling. That's it. If you need something more you are free to contribute here or to use Acr.UserDialogs which is out of support.
