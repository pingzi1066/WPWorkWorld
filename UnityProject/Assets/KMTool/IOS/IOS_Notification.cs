/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2016-06-09     WP      Initial version
 * 
 * *****************************************************************************/

using System;
using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR || UNITY_IOS

/// <summary>
/// 负责 IOS 端的推送工作
/// </summary>
public class IOS_Notification : MonoBehaviour 
{
    public class Notification
    {
        public string message;
        public DateTime newDate;
        public bool isRepeatDay;

        public Notification(string m, DateTime dt , bool r){ message = m; newDate = dt; isRepeatDay = r; }
    }

    static private List<Notification> listNotifi = new List<Notification>();

    private static IOS_Notification mInstance;
    public static IOS_Notification instance
    {
        get
        { 
            if (mInstance == null)
            {
                GameObject go = new GameObject(typeof(IOS_Notification).Name);
                mInstance = go.AddComponent<IOS_Notification>();
                //全局管理不需要被销毁
                DontDestroyOnLoad(go);
            }
            return mInstance;
        }
    }

    //本地推送
    public void AddNotificationMessage(string message,int hour ,int minute,bool isRe)
    {
        int year = DateTime.Now.Year;
        int month = DateTime.Now.Month;
        int day= DateTime.Now.Day;
        DateTime newDate = new DateTime(year,month,day,hour,0,0);
        Notification n = new Notification(message, newDate, false);

        //加入列表 
        listNotifi.Add(n);
    }
    //本地推送 你可以传入一个固定的推送时间
    private void NotificationMessage(Notification notifi)
    {
        DateTime newDate = notifi.newDate;
        string message = notifi.message;
        bool isRepeatDay = notifi.isRepeatDay;

        //推送时间需要大于当前时间
        if(newDate > DateTime.Now)
        {
            UnityEngine.iOS.LocalNotification localNotification = new UnityEngine.iOS.LocalNotification();
            localNotification.fireDate =newDate;    
            localNotification.alertBody = message;
            localNotification.applicationIconBadgeNumber = 1;
            localNotification.hasAction = true;
            if(isRepeatDay)
            {
                //是否每天定期循环
                localNotification.repeatCalendar = UnityEngine.iOS.CalendarIdentifier.ChineseCalendar;
                localNotification.repeatInterval = UnityEngine.iOS.CalendarUnit.Day;
            }
            localNotification.soundName = UnityEngine.iOS.LocalNotification.defaultSoundName;
            UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(localNotification);
        }
    }

    void Awake()
    {
        //第一次进入游戏的时候清空，有可能用户自己把游戏冲后台杀死，这里强制清空
        CleanNotification();
        UnityEngine.iOS.NotificationServices.RegisterForNotifications(
            UnityEngine.iOS.NotificationType.Badge | 
            UnityEngine.iOS.NotificationType.Alert | 
            UnityEngine.iOS.NotificationType.Sound);
    }

    void OnApplicationPause(bool paused)
    {
        //程序进入后台时
        if(paused)
        {
            for (int i = 0; i < listNotifi.Count; i++)
            {
                if (listNotifi[i] != null)
                    NotificationMessage(listNotifi[i]);
            }
        }
        else
        {
            //程序从后台进入前台时
            CleanNotification();
        }
    }

    //清空所有本地消息
    void CleanNotification()
    {
        UnityEngine.iOS.LocalNotification l = new UnityEngine.iOS.LocalNotification (); 
        l.applicationIconBadgeNumber = -1; 
        UnityEngine.iOS.NotificationServices.PresentLocalNotificationNow (l); 
        UnityEngine.iOS.NotificationServices.CancelAllLocalNotifications (); 
        UnityEngine.iOS.NotificationServices.ClearLocalNotifications (); 
    }

    void Test()
    {
        //10秒后发送
//        NotificationMessage(new Notification("P : 20秒后发送",DateTime.Now.AddSeconds(20)));
        //每天中午12点推送
//        NotificationMessage("P : 每天中午12点推送",12,0,true);

    }
}

#endif