using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Telephony.Data;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using Plugin.Firebase.CloudMessaging;
using Plugin.Firebase.CloudMessaging.Platforms.Android.Extensions;

namespace Plugin.PushNotifiation.Service.Android;

public static class PushNotificationAndroidHandler
{
    public static void HandleIntent(Intent intent)
    {
        FirebaseCloudMessagingImplementation.OnNewIntent(intent);
    }
    public static void RequestPushNotificationsPermission(Activity activity)
    {
        if (OperatingSystem.IsAndroidVersionAtLeast(33))
        {
            if (ContextCompat.CheckSelfPermission(activity, Manifest.Permission.PostNotifications) != Permission.Granted)
            {
                ActivityCompat.RequestPermissions(activity, new[] { Manifest.Permission.PostNotifications }, 0); ;
            }
        }
    }
    public static void CreateNotificationChannelIfNeeded(Activity activity)
    {
        if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
        {
            CreateNotificationChannel(activity);
        }
    }

    private static void CreateNotificationChannel(Activity activity)
    {
        var channelId = $"{activity.PackageName}.general";
        var notificationManager = (NotificationManager)activity.GetSystemService(Context.NotificationService);
        if (OperatingSystem.IsAndroidVersionAtLeast(26))
        {
            var channel = new NotificationChannel(channelId, "General", NotificationImportance.Default);
            notificationManager.CreateNotificationChannel(channel);
            FirebaseCloudMessagingImplementation.ChannelId = channelId;
            FirebaseCloudMessagingImplementation.SmallIconRef = Cielito.Resource.Drawable.notification_alfa;
        }
    }
}

