// ReSharper disable All

namespace Plugin.PushNotifiation.Service;

public interface IPushNotificationService
{
    //void OnError(string error);
    //void OnOpened(NotificationResponse response);
    //void OnReceived(IDictionary<string, object> parameters);


    //void OnNotificationReceived(object source, FirebasePushNotificationDataEventArgs e);
    //void OnNotificationOpened(object source, FirebasePushNotificationResponseEventArgs e);
    //void OnNotificationAction(object source, FirebasePushNotificationResponseEventArgs e);
    //void OnNotificationDeleted(object source, FirebasePushNotificationDataEventArgs e);


    void DeleteNotificationFromNotificationCenter(string notificationID);
}
