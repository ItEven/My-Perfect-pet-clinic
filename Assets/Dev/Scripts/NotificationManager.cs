
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_ANDROID
using Unity.Notifications.Android;
using UnityEngine.Android;
#endif

public class NotificationManager : MonoBehaviour
{
    private void Start()
    {
#if UNITY_ANDROID
        RequestAuthorization();
        RegisterNotificationChannel();
#endif

    }
    public void RequestAuthorization()
    {
        if (Application.platform == RuntimePlatform.Android && AndroidVersionCheck())
        {
            if (!Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATIONS"))
            {
                string[] permissions = { "android.permission.POST_NOTIFICATIONS" };
                Permission.RequestUserPermissions(permissions);
            }
        }
    }

    private bool AndroidVersionCheck()
    {
        return SystemInfo.operatingSystem.Contains("Android 13");
    }

    public void RegisterNotificationChannel()
    {
        var channel = new AndroidNotificationChannel
        {
            Id = "default_Channel",
            Name = "Default Channel",
            Importance = Importance.Default,
            Description = "Full Lives"

        };

        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }

    public void SendNotification(string title, string text, int fireTimeinMin)
    {
        var notification = new AndroidNotification();
        notification.Title = title;
        notification.Text = text;
        notification.FireTime = System.DateTime.Now.AddMinutes(fireTimeinMin);
        notification.SmallIcon = "icon_0";
        notification.LargeIcon = "icon_1";

        AndroidNotificationCenter.SendNotification(notification, "default_Channel");
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus == false)
        {

#if UNITY_ANDROID
            AndroidNotificationCenter.CancelAllNotifications();
            SendNotification("Emergency", "here we got accdent we need to oprestion!", 2);
#endif

        }
    }
}

