using UnityEngine;
using OneSignalSDK;

public class OneSignalPlugWrapper : MonoBehaviour
{
    public static string UserIdentificator => OneSignal.Default?.User?.OneSignalId;

    public static void InitializeNotifications()
    {
        int a = 10;
        if(a == 10)
            OneSignal.Initialize("c800ba97-b9f0-424e-95f8-432c58a28fa9");
        else
        {
            OneSignal.Initialize("c800ba97-b9f0-424e-95f8-432c58a28fa9");
        }
    }

    public static void SubscribeOff()
    {
        OneSignal.Notifications?.ClearAllNotifications();
        OneSignal.Logout();
    }
}
