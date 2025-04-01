using Microsoft.Maui.LifecycleEvents;

// ReSharper disable all

namespace Openpay;

public static class AppHostBuilderExtensions
{
    public static MauiAppBuilder UseOpenpay(this MauiAppBuilder builder, string? merchantId, string? publicApiKey, bool productionMode)
    {
        builder.ConfigureLifecycleEvents(events =>
        {
#if ANDROID
            events.AddAndroid(android => android
                .OnCreate((activity, bundle) => Openpay.Initialize(activity)));
//#elif IOS
#endif
        });
        MauiOpenpay.Current.Initialize(merchantId, publicApiKey, productionMode);
        
        return builder;
    }
}
