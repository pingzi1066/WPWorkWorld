/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2016-06-09     WP      Initial version
 * 
 * *****************************************************************************/

using System;
using UnityEngine;
using System.Collections;

/// <summary>
/// 每天定时奖励的管理器
/// </summary>
public class MealtimeRewardManager : MonoBehaviour 
{
    public MealtimeRewardData data { get { return MealtimeRewardData.instance; } }

    public delegate void DelOnTime(float time);

    /// <summary>
    /// 领取时间倒计时
    /// </summary>
    static public DelOnTime eventOnTime;

    public delegate void DelFinished();

    /// <summary>
    /// 当前有已经完成的
    /// </summary>
    static public DelFinished eventOnFinished;

    /// <summary>
    /// 当前可以领取的Model
    /// </summary>
    private ModelMealtimeReward curModel = new ModelMealtimeReward();

    /// <summary>
    /// 等于0的时候代表没有礼物可以领取
    /// </summary>
    static private int curId = 0;

    /// <summary>
    /// 离得最近的一个领取的时间 
    /// </summary>
    static private float timeCount = 999999;

    /// <summary>
    /// 是否已经加入推送
    /// </summary>
    private bool isAddPush = true;

    /// <summary>
    /// 计时器用参数 
    /// </summary>
    private float timeParam = 0;

    private static MealtimeRewardManager mInstance;
    public static MealtimeRewardManager instance
    {
        get
        { 
            if (mInstance == null)
            {
                GameObject go = new GameObject(typeof(MealtimeRewardManager).Name);
                mInstance = go.AddComponent<MealtimeRewardManager>();
                //全局管理不需要被销毁
                DontDestroyOnLoad(go);
            }
            return mInstance;
        }
    }

    static public void Begin()
    {
        if (instance)
        {
            instance.Check();
        }
    }

    void Update()
    {
        timeParam += KMTime.deltaTime;
        if (timeParam > 1)
        {
            timeParam--;
            //计时结束
            if (timeCount <= 0)
            {
                Check();
                return;
            }

            timeCount--;

            if (curId == 0 && eventOnTime != null)
                eventOnTime(timeCount);


        }
    }

    private void Check()
    {
        int[] allId = StaticMealtimeReward.Instance().allID;
        DateTime now = DateTime.Now;

        int year = now.Year;
        int month = now.Month;
        int day= now.Day;

        bool hasLast = false;
        DateTime last = now;
        if (data.GetData(ED_MealtimeReward.lastYear) != 0)
        {
            last = new DateTime(data.GetData(ED_MealtimeReward.lastYear), data.GetData(ED_MealtimeReward.lastMonth),
                            data.GetData(ED_MealtimeReward.lastDay), data.GetData(ED_MealtimeReward.lastHour), 0, 0);
        }

        //防修改时间 
        if(hasLast && last > now) return;

        ModelMealtimeReward temp = new ModelMealtimeReward();

        // 检察是否在时间内
        for (int i = 0; i < allId.Length; i++)
        {
            temp.SetTemplateID(allId[i]);

            DateTime newDate = new DateTime(year, month, day, temp.hour, temp.minute, 0);
            if (hasLast)
            {
                //是否已经领取
                if (last > newDate && last < newDate.AddMinutes(temp.last_time))
                {
                    continue;
                }
            }

            //是否超出领取时间
            if (now > newDate.AddMinutes(temp.last_time))
            {
                continue;
            }

            //在当前领取时间内
            if (now > newDate)
            {
                curId = allId[i];
                curModel.SetTemplateID(curId);

                if (eventOnFinished != null)
                    eventOnFinished();
            }
            else
            { 
                TimeSpan ts = newDate - now;

                if (ts.TotalSeconds < timeCount)
                {
                    timeCount = (float)ts.TotalSeconds;
                }

                if (isAddPush)
                {
                    //加入推送
                    IOS_Notification.instance.AddNotificationMessage(temp.desc, temp.hour, temp.minute, true);
                }
            }
        }

        isAddPush = false;

        //当天没有礼物可领的情况下
        if (timeCount == 999999)
        {
            temp.SetTemplateID(allId[0]);
            DateTime dt = new DateTime(year, month, day + 1, temp.hour, temp.minute, 0);
            timeCount = (float)((dt - now).TotalSeconds);
        }
    }

    /// <summary>
    /// 返回当前礼物
    /// </summary>
    /// <returns>The gift.</returns>
    public ModelMealtimeReward GetGift()
    {
        if (curId == 0)
            return null;

        return curModel;
    }

    /// <summary>
    /// 领取奖励
    /// </summary>
    public void ReceiveAward()
    {
        Debug.Log("TODO Receive award!! award "  + curModel.desc);
//        UserData.instance.AddInt((E_UserData)curModel.gift_type, curModel.gift_num);
//        UserData.instance.SaveData();

        curId = 0;
        //记录时间：
        DateTime now = DateTime.Now;

        int year = now.Year;
        int month = now.Month;
        int day= now.Day;
        int hour = now.Hour;
        data.SetData(ED_MealtimeReward.lastYear, year);
        data.SetData(ED_MealtimeReward.lastMonth, month);
        data.SetData(ED_MealtimeReward.lastDay, day);
        data.SetData(ED_MealtimeReward.lastHour, hour);
        data.SaveData();
    }

    void KMDebug()
    {
        DateTime dt = DateTime.Now;
        Debug.Log("year : " + dt.Year + "  mouth " + dt.Month + " day " + dt.Day + dt.ToString());

        Debug.Log(dt.ToString());
        dt.AddMonths(1);
        dt.AddDays(29);
        Debug.Log(dt.ToString());
        dt = dt.AddSeconds(-1);
        Debug.Log(dt.ToString());

        TimeSpan tp = new TimeSpan(0, 0, 40);
        TimeSpan tp2 = new TimeSpan(0, 0, -2);
        tp += tp2;
        Debug.Log(tp.ToString());

    }

    static public void RefreshEvent()
    {
        if (curId == 0)
        {
            if (eventOnTime != null)
            {
                eventOnTime(timeCount);
            }
        }
        else
        {
            if (eventOnFinished != null)
                eventOnFinished();
        }
    }
}

public enum ED_MealtimeReward
{
    /// <summary>
    /// 最后一次领取年
    /// </summary>
    lastYear,
    lastMonth,
    lastDay,
    lastHour,
}

public class MealtimeRewardData : LocalInt<MealtimeRewardData,ED_MealtimeReward>
{
    public override string Key()
    {
        return "MealtimeRewardData";
    }
}