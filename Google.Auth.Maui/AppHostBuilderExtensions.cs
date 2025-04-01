#if ANDROID
using Google.Auth.Android;
#elif IOS
using Foundation;
using Google.Auth.iOS;
#endif
using Microsoft.Maui.Hosting;
using Microsoft.Maui.LifecycleEvents;

// ReSharper disable all

namespace Google.Auth;

public static class AppHostBuilderExtensions
{
    public static MauiAppBuilder UseGoogleAuth(this MauiAppBuilder builder, string googleRequestIdToken)
    {
        builder.ConfigureLifecycleEvents(events =>
        {
#if ANDROID
            events.AddAndroid(android => android
                .OnActivityResult((activity, requestCode, resultCode, data) => GoogleAuthImplementation.OnActivityResult(requestCode, resultCode, data!))
                .OnCreate((activity, bundle) => GoogleAuthImplementation.Initialize(googleRequestIdToken)));
#elif IOS
            events.AddiOS(ios => ios
                .WillFinishLaunching((app, launchOptions) => GoogleAuthImplementation.Initialize()));
            events.AddiOS(ios => ios.OpenUrl((app, url, options) =>
            {
                try
                {
                    if (GoogleAuthImplementation.OpenUrl(app, url, options))
                        return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                return false;
            }));
#endif
        });
        
        
        return builder;
    }
}
