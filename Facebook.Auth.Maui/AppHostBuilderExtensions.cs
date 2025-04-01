#if ANDROID
using Facebook.Auth.Android;
#elif IOS
using Facebook.Auth.iOS;
#endif
using Microsoft.Maui.LifecycleEvents;

// ReSharper disable all

namespace Google.Auth;

public static class AppHostBuilderExtensions
{
    public static MauiAppBuilder UseGoogleAuth(this MauiAppBuilder builder)
    {
        builder.ConfigureLifecycleEvents(events =>
        {
#if ANDROID
            events.AddAndroid(android => android
                .OnActivityResult((activity, requestCode, resultCode, data) => FacebookAuthImplementation.OnActivityResult(requestCode, resultCode, data!))
                .OnCreate((activity, bundle) => FacebookAuthImplementation.Initialize()));
#elif IOS
            events.AddiOS(ios => ios
                .WillFinishLaunching((app, launchOptions) => FacebookAuthImplementation.Initialize()));
            events.AddiOS(ios => ios.OpenUrl((app, url, options) =>
            {
                try
                {
                    if (FacebookAuthImplementation.OpenUrl(app, url, options))
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
