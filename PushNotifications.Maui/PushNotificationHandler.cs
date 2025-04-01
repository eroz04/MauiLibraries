using System.Diagnostics;
using Plugin.Firebase.CloudMessaging;
using Plugin.Firebase.CloudMessaging.EventArgs;
// ReSharper disable All

namespace Plugin.PushNotifiation.Service;


public static partial class PushNotificationHandler
{
    public static async void TokenChanged(object sender, FCMTokenChangedEventArgs e)
    {
        await Task.Run(async () =>
        {
            Debug.WriteLine($"TOKEN: {e.Token}");
            await CrossFirebaseCloudMessaging.Current.SubscribeToTopicAsync("cielito_general");
            if (DeviceInfo.Current.Platform == DevicePlatform.iOS)
                await CrossFirebaseCloudMessaging.Current.SubscribeToTopicAsync("cielito_ios");
            else if (DeviceInfo.Current.Platform == DevicePlatform.Android)
                await CrossFirebaseCloudMessaging.Current.SubscribeToTopicAsync("cielito_android");

            Preferences.Default.Set("DeviceToken", e.Token);
            await SubscribeUser();
        });
    }

    public static Task SubscribeUser()
    {
        return Task.Run(async () =>
        {
            var userId = Preferences.Default.Get("UserId", string.Empty);
            var deviceToken = Preferences.Default.Get("DeviceToken", string.Empty);
            if (!Preferences.Default.Get("IsSubscribedToUserTopic", false)
                && !string.IsNullOrWhiteSpace(deviceToken)
                && !string.IsNullOrWhiteSpace(userId))
            {
                var userFPNID = string.Format("cielito_{0}", Preferences.Default.Get("UserId", string.Empty);
                Debug.WriteLine($"subscribe as: {userFPNID}");
                await CrossFirebaseCloudMessaging.Current.SubscribeToTopicAsync(userFPNID);
                Preferences.Default.Set("IsSubscribedToUserTopic", true);
                // await SessionManager.Instance.SetDeciveToken(userId, deviceToken);
            }
        });
    }

    public static async void UnSubscribeUser()
    {
        if (Preferences.Default.Get("IsSubscribedToUserTopic", false))
        {
            var userFPNID = string.Format("cielito_{0}", Preferences.Default.Get("UserId", string.Empty);
            Debug.WriteLine($"unsubscribe as: {userFPNID}");
            await CrossFirebaseCloudMessaging.Current.UnsubscribeFromTopicAsync(userFPNID);
            // await SessionManager.Instance.SetDeciveToken(AppMobileSettings.UserId, "");
        }
    }

    public static void NotificationReceived(object source, FCMNotificationReceivedEventArgs e)
    {
        Debug.WriteLine("Received");

        string title = e?.Notification?.Title ?? string.Empty;
        string message = e?.Notification?.Body ?? string.Empty;
        string categoryPN = string.Empty;
        string ID = string.Empty;
        string extra = string.Empty;
        string iconnumber = string.Empty;
        string itemid = string.Empty;
        bool isSilentInForeground = e?.Notification?.IsSilentInForeground ?? false;

        foreach (var data in e.Notification.Data)
        {
            Debug.WriteLine($"{data.Key} : {data.Value}");

            switch (data.Key)
            {
                case "title":
                case "aps.alert.title":
                    if (string.IsNullOrWhiteSpace(title))
                        title = data.Value.ToString();
                    break;
                case "message":
                case "aps.alert.body":
                    if (string.IsNullOrWhiteSpace(message))
                        message = data.Value.ToString();
                    break;
                case "category":
                case "aps.category":
                    categoryPN = data.Value.ToString();
                    break;
                case "ID":
                case "aps.ID":
                    ID = data.Value.ToString();
                    break;
                case "extra":
                case "aps.extra":
                    extra = data.Value.ToString();
                    break;
                case "iconnumber":
                case "aps.iconnumber":
                    iconnumber = data.Value.ToString();
                    break;
                case "itemid":
                case "aps.itemid":
                    itemid = data.Value.ToString();
                    break;
            }
        }

        if (int.TryParse(categoryPN, out int categoryInt))
        {
            if (Enum.IsDefined(typeof(PushNotificationCategory), categoryInt))
            {
                Preferences.Default.Set("PushNotificationID", ID);
                var category = (PushNotificationCategory)categoryInt;
                ProcessPushNotification(category, ID, title, message, iconnumber, itemid, extra, true);
            }
        }
    }

    public static void NotificationTapped(object source, FCMNotificationTappedEventArgs e)
    {
        Debug.WriteLine("Opened");

        string categoryPN = string.Empty;
        string ID = string.Empty;
        string title = string.Empty;
        string message = string.Empty;
        string extra = string.Empty;
        string itemId = string.Empty;
        string iconnumber = string.Empty;

        foreach (var data in e.Notification.Data)
        {
            Debug.WriteLine($"{data.Key} : {data.Value}");

            if (data.Key == "category")
            {
                categoryPN = data.Value.ToString();
            }

            if (data.Key == "extra")
            {
                extra = data.Value.ToString();
            }

            switch (data.Key)
            {
                case "aps.category":
                    categoryPN = data.Value.ToString();
                    break;
                case "aps.ID":
                case "ID":
                    ID = data.Value.ToString();
                    break;
                case "aps.alert.title":
                    title = data.Value.ToString();
                    break;
                case "aps.alert.body":
                    message = data.Value.ToString();
                    break;
                case "aps.extra":
                    extra = data.Value.ToString();
                    break;
                case "aps.iconnumber":
                    iconnumber = data.Value.ToString();
                    break;
                case "itemid":
                case "apps.itemid":
                    itemId = data.Value.ToString();
                    break;
            }
        }
        
        Preferences.Default.Set("PushNotificationID", ID);
        Preferences.Default.Set("PushNotificationCategory", categoryPN);
        Preferences.Default.Set("PushNotificationTitle", title);
        Preferences.Default.Set("PushNotificationBody", message);
        Preferences.Default.Set("PushNotificationIconNumber", iconnumber);
        Preferences.Default.Set("PushNotificationItemID", itemId);
        Preferences.Default.Set("PushNotificationExtra", extra);
        Preferences.Default.Set("PushNotificationNavigatePush", true);


        if (int.TryParse(categoryPN, out int categoryInt))
        {
            if (Enum.IsDefined(typeof(PushNotificationCategory), categoryInt))
            {
                var category = (PushNotificationCategory)categoryInt;
                ProcessPushNotification(category, ID, title, message, iconnumber, itemId, extra);
            }
        }
    }

    public static void Error(object source, FCMErrorEventArgs e)
    {
        Debug.WriteLine("error " + e.Message);
    }


    public static void ProcessPushNotification(PushNotificationCategory pushNotificationCategory, string ID, string title, string message, string iconnumber, string itemid, string extra, bool displayalert = false)
    {
        //if (!AppMobileSettings.IsLogin)
        //    return;
        displayalert = true;
        if (displayalert)
            ReceiveNotificationOnForeground(pushNotificationCategory, ID, title, message, iconnumber, itemid, extra);
        else
            TapOnNotification(pushNotificationCategory, ID, title, message, iconnumber, itemid, extra);
    }

    public static void TapOnNotification(PushNotificationCategory pushNotificationCategory, string ID, string title, string message, string iconnumber, string itemid, string extra, bool displayalert = false)
    {
        PNCategory(pushNotificationCategory, ID, title, message, iconnumber, itemid, extra);
    }

    public static void ReceiveNotificationOnForeground(PushNotificationCategory pushNotificationCategory, string ID, string title, string message, string iconnumber, string itemid, string extra, bool displayalert = false)
    {
        if (string.IsNullOrWhiteSpace(message))
            return;

        Preferences.Set("PushNotificationNavigatePush", false);
        UpdateIconBadgeValue(iconnumber, changeOnNavigationBar: true);
        PNCategory(pushNotificationCategory, ID, title, message, iconnumber, itemid, extra);
    }

    private static async void PNCategory(PushNotificationCategory pushNotificationCategory, string ID, string title, string message, string iconnumber, string itemid, string extra, bool fromAppStart = false)
    {
        switch (pushNotificationCategory)
        {
            case PushNotificationCategory.MyLevel:
                UpdateIconBadgeValue(iconnumber);
                await MainThread.InvokeOnMainThreadAsync(async () => await Shell.Current.GoToAsync("///micielito"));
                if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(message))
                    await AlertManager.ShowAlertAsync(title, message, AppResources.OkButtonTitle);
                break;

            case PushNotificationCategory.Stores:
                await MainThread.InvokeOnMainThreadAsync(async () => await Shell.Current.GoToAsync("///stores"));
                if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(message))
                    await AlertManager.ShowAlertAsync(title, message, AppResources.OkButtonTitle);
                // --- DeletNotification(ID);
                break;

            case PushNotificationCategory.StoresTerrazas:
                AppMobileSettings.IsStoresTerrazas = true;
                await MainThread.InvokeOnMainThreadAsync(async () => await Shell.Current.GoToAsync("///stores"));
                if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(message))
                    await AlertManager.ShowAlertAsync(title, message, AppResources.OkButtonTitle);
                //---DeletNotification(ID);
                break;

            case PushNotificationCategory.MyOfferts:
                await MainThread.InvokeOnMainThreadAsync(async () => await Shell.Current.GoToAsync("///promotions"));
                if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(message))
                    await AlertManager.ShowAlertAsync(title, message, AppResources.OkButtonTitle);
                //---DeletNotification(ID);
                break;

            case PushNotificationCategory.MyBuys:
                await MainThread.InvokeOnMainThreadAsync(async () => await Shell.Current.GoToAsync("///shopping"));
                if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(message))
                    await AlertManager.ShowAlertAsync(title, message, AppResources.OkButtonTitle);
                //---DeletNotification(ID);
                break;

            case PushNotificationCategory.NewLevel:
                await MainThread.InvokeOnMainThreadAsync(async () => await Shell.Current.GoToAsync("///home"));
                ShowNewLevelPopPup();
                //---DeletNotification(ID);
                break;

            case PushNotificationCategory.Membership:
                if (!AppMobileSettings.IsGuestUser)
                {
                    App.NavigateToMembership(true);
                    if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(message))
                        await AlertManager.ShowAlertAsync(title, message, AppResources.OkButtonTitle);
                    //---DeletNotification(ID);
                }
                break;

            case PushNotificationCategory.NPS:
                try
                {
                    if (AppMobileSettings.IsLogin)
                        await MainThread.InvokeOnMainThreadAsync(async () => await Shell.Current.GoToAsync("///home", false));



                    if (fromAppStart)
                    {
                        AppMobileSettings.NavigateToPush = true;
                        AppMobileSettings.NavigatetoPushNPS = true;


                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(message))
                        {
                            await AlertManager.ShowAlertAsync(title, message, AppResources.OkButtonTitle);
                            await App.CheckNPS();
                        }
                    }

                    //---DeletNotification(ID);
                }
                catch
                { }
                break;
            // #endif

            case PushNotificationCategory.Home:
            default:
                await MainThread.InvokeOnMainThreadAsync(async () => await Shell.Current.GoToAsync("///home"));

                if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(message))
                    await AlertManager.ShowAlertAsync(title, message, AppResources.OkButtonTitle);
                //---DeletNotification(ID);
                break;

        }
    }

    public static bool displayed = false;
    public static async void ShowNewLevelPopPup()
    {
        if (displayed)
        {
            return;
        }

        try
        {
            var responses = await SessionManager.Instance.GetNotificationsMessage();

            if (responses.IsSuccess && !string.IsNullOrWhiteSpace(responses?.SuccessResponse?.FirstOrDefault()?.Imagen) && !displayed)
            {
                MainThread.BeginInvokeOnMainThread(async delegate
                {
                    displayed = true;
                    try
                    {
                        await MopupService.Instance.PushAsync(new ImagePopupPage(responses.SuccessResponse.FirstOrDefault().Imagen));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        return;
    }

    //public static void DeletNotification(string ID)
    //{
    //    CrossFirebaseCloudMessaging.Current.
    //    DependencyService.Get<IPushNotificationService>()?.DeleteNotificationFromNotificationCenter(ID);
    //}

    public static void UpdateIconBadgeValue(string iconnumberString, bool changeOnNavigationBar = false)
    {
        int.TryParse(iconnumberString, out int iconnumber);
        if (iconnumber >= 0)
        {
            AppMobileSettings.IconNotificationBadge = iconnumber;

            //if (changeOnNavigationBar)
            //	NavigationPageHelper.Instance.SetBadgeValue(iconnumber);
        }
    }
}
