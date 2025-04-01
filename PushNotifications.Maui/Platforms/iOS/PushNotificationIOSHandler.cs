using Foundation;
using Plugin.Firebase.CloudMessaging;
using Plugin.Firebase.CloudMessaging.Platforms.iOS.Extensions;

namespace Plugin.PushNotifiation.Service.iOS;

public static class PushNotificationIOSHandler
{
    public static void OnFinishLaunching(NSDictionary options)
    {
        // Handling Push notification when app is closed if App was opened by Push Notification...
        if (options != null && options.Keys != null && options.Keys.Any() && options.ContainsKey(new NSString("UIApplicationLaunchOptionsRemoteNotificationKey")))
        {
            try
            {
                NSDictionary UIApplicationLaunchOptionsRemoteNotificationKey = options.ObjectForKey(new NSString("UIApplicationLaunchOptionsRemoteNotificationKey")) as NSDictionary;

                var userInfo = UIApplicationLaunchOptionsRemoteNotificationKey;

                Console.WriteLine("* Notification: " + userInfo.ToString());

                NSDictionary aps = userInfo.ObjectForKey(new NSString("aps")) as NSDictionary;

                var alertKey = new NSString("alert");

                if (aps.ContainsKey(alertKey))
                {
                    var alert = (NSDictionary)aps.ObjectForKey(alertKey);

                    Console.WriteLine("Notification: " + alert.ToString());
                }

                string categoryPN = string.Empty;
                string ID = string.Empty;
                string title = string.Empty;
                string message = string.Empty;
                string iconnumber = string.Empty;
                string extra = string.Empty;
                string itemid = string.Empty;

                var categoryKey = new NSString("category");
                if (aps.ContainsKey(categoryKey))
                {
                    var data = (object)aps.ObjectForKey(categoryKey);
                    categoryPN = data.ToString();
                }

                var IDKey = new NSString("ID");
                if (aps.ContainsKey(IDKey))
                {
                    var data = (object)aps.ObjectForKey(IDKey);
                    ID = data.ToString();
                }
                var iconnumberKey = new NSString("iconnumber");
                if (aps.ContainsKey(iconnumberKey))
                {
                    var data = (object)aps.ObjectForKey(iconnumberKey);
                    iconnumber = data.ToString();
                }
                var titleKey = new NSString("title");
                if (aps.ContainsKey(titleKey))
                {
                    var data = (object)aps.ObjectForKey(titleKey);
                    title = data.ToString();
                }
                var messageKey = new NSString("message");
                if (aps.ContainsKey(messageKey))
                {
                    var data = (object)aps.ObjectForKey(messageKey);
                    message = data.ToString();
                }
                var itemidKey = new NSString("itemid");
                if (aps.ContainsKey(itemidKey))
                {
                    var data = (object)aps.ObjectForKey(itemidKey);
                    itemid = data.ToString();
                }
                Console.WriteLine("Notification categoryPN: " + categoryPN);
                Console.WriteLine("Notification ID: " + ID);

                if (!string.IsNullOrWhiteSpace(categoryPN))
                {
                    if (int.TryParse(categoryPN, out int categoryInt))
                    {
                        PushNotificationCategory category = (PushNotificationCategory)categoryInt;
                        if (category == PushNotificationCategory.NPS)
                        {
                            AppMobileSettings.NavigateToPush = true;
                            AppMobileSettings.NavigatetoPushNPS = true;
                            AppMobileSettings.NavigatetoPushTitle = title;
                            AppMobileSettings.NavigatetoPushMessage = message;
                        }
                        else
                        {
                            PushNotificationHandler.TapOnNotification(category, ID, title, message, iconnumber, extra, itemid, true);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }

    public static void CustomizePushNotification()
    {
        //FirebaseCloudMessagingImplementation.NotificationBuilderProvider = notificaion => new NotificationCompat.Builder(context, channelId)
        //.SetSmallIcon(Android.Resource.Drawable.SymDefAppIcon)
        //.SetContentTitle(notification.Title)
        //.SetContentText(notification.Body)
        //.SetPriority(NotificationCompat.PriorityDefault)
        //.SetAutoCancel(true);
    }
    public static void CustomizeLocalPushNotification()
    {
        //FirebaseCloudMessagingImplementation.ShowLocalNotificationAction = notification =>
        //{

        //    var intent = PackageManager.GetLaunchIntentForPackage(AppInfo.PackageName);
        //    intent.PutExtra(FirebaseCloudMessagingImplementation.IntentKeyFCMNotification, notification.ToBundle());
        //    intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);

        //    var pendingIntent = PendingIntent.GetActivity(Application.Context, 0, intent, PendingIntentFlags.Immutable | PendingIntentFlags.UpdateCurrent);
        //    var builder = new NotificationCompat.Builder(context, channelId)
        //        .SetSmallIcon(Android.Resource.Drawable.SymDefAppIcon)
        //        .SetContentTitle(notification.Title)
        //        .SetContentText(notification.Body)
        //        .SetPriority(NotificationCompat.PriorityDefault)
        //        .SetAutoCancel(true);

        //    var notificationManager = (NotificationManager)GetSystemService(NotificationService);
        //    notificationManager.Notify(123, builder.SetContentIntent(pendingIntent).Build());
        //};
    }
}

