/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2016-06-07     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using System.Collections;

/// <summary>
/// 计时礼物管理器
/// </summary>
public class TimeCountRewardManager : MonoBehaviour 
{
    //礼物开始ID 和结束ID，需要手动设置。详细可以访问 StaticTimingReward
    static public int startId
    {
        get
        {
            return StaticTimeCountReward.Instance().allID[0];
        }
    }
    const int lastId = 5;

    /// <summary>
    /// 计时委托 每一秒更新一次
    /// </summary>
    public delegate void OnTimeCount(int curTime,int endTime);

    /// <summary>
    /// 每一秒更新一次
    /// </summary>
    static public OnTimeCount eventTimeCount;
    
    static private ModelTimeCountReward mModel;
    /// <summary>
    /// 表里数据
    /// </summary>
    static public ModelTimeCountReward model
    {
        get
        { 
            if (mModel == null)
            {
                mModel = new ModelTimeCountReward();
                mModel.SetTemplateID(data.GetInt(ED_TimingReward.curId));
            }
            return mModel;
        }
    }

    /// <summary>
    /// 当前数据
    /// </summary>
    private static TimingRewardData data { get { return TimingRewardData.instance; } }

    private static TimeCountRewardManager mInstance;
    public static TimeCountRewardManager instance
    {
        get
        { 
            if (mInstance == null)
            {
                GameObject go = new GameObject(typeof(TimeCountRewardManager).Name);
                mInstance = go.AddComponent<TimeCountRewardManager>();
                //全局管理不需要被销毁
                DontDestroyOnLoad(go);
            }
            return mInstance;
        }
    }

    /// <summary>
    /// 计时用
    /// </summary>
    private float timeParam = 0;

    /// <summary>
    /// 当前计进是否完成
    /// </summary>
    private bool isFinishByCurGift = false;

    /// <summary>
    /// 所有的礼物是否已经领取完毕，领取完毕代表最后一个礼物已经领取，而不是计时完成
    /// </summary>
    private bool isOver = false;

    /// <summary>
    /// 开始计时工作
    /// </summary>
    public static bool Begin()
    {
        if (data.GetInt(ED_TimingReward.isFinished) == 0)
        {
            if (instance)
            {
                return true;
            }
        }
        return false;
    }

	// Use this for initialization
	void Start() 
	{

        if (data.GetInt(ED_TimingReward.isFinished) != 0)
        {
            isOver = true;
        }
	}
	
	// Update is called once per frame
	void Update() 
	{
        if (!isFinishByCurGift && !isOver)
            Timing();
	}

    void OnApplicationQuit()
    {
        data.SaveData();
    }

    /// <summary>
    /// 计时
    /// </summary>
    private void Timing()
    {
        //加入实时时间
        timeParam += KMTime.deltaTime;

        if (timeParam > 1)
        {
            timeParam--;
            data.AddInt(ED_TimingReward.curTime, 1);

            //时间已经完成
            if (model.dis_time <= data.GetInt(ED_TimingReward.curTime))
            {
                //时间总数不超过
                data.SetInt(ED_TimingReward.curTime, model.dis_time);
                isFinishByCurGift = true;
            }

            //事件更新
            RefreshEvent();
        }
    }

    /// <summary>
    /// 领取当前礼物 并 开启下一个礼物的计时
    /// </summary>
    public void ReceiveAwardAndNextGift()
    {
        if (isFinishByCurGift)
        {
            ReceiveAward();

            if (model.templateID >= lastId)
            {
                //全完成
                data.SetInt(ED_TimingReward.isFinished, 1);
                isOver = true;
            }
            else
            {
                data.AddInt(ED_TimingReward.curId, 1);
                data.SetInt(ED_TimingReward.curTime, 0);
                isFinishByCurGift = false;

                RefreshEvent();
            }

            data.SaveData();
        }
    }

    /// <summary>
    /// 领取当时计时礼物的奖励 取表里面的奖励可以取model变量
    /// </summary>
    protected void ReceiveAward()
    {
        //加入数据：TODO
//        UserData.instance.AddInt((E_UserData)model.gift_type, model.gift_num);
    }

    /// <summary>
    /// 提供给公共参数的目标是当注册方法的时候没有即时调用更新，所以就用了
    /// </summary>
    static public void RefreshEvent()
    {
        //事件更新
        if (eventTimeCount != null)
        {
            eventTimeCount(data.GetInt(ED_TimingReward.curTime), model.dis_time);
        }
    }
}

/// <summary>
/// 计时数据保存类型
/// </summary>
public enum ED_TimingReward
{
    /// <summary>
    /// 当前表里面的ID
    /// </summary>
    curId,
    /// <summary>
    /// 当前礼物已经经过的时间
    /// </summary>
    curTime,
    /// <summary>
    /// 是否已经全部完成，0为未完成。
    /// </summary>
    isFinished,
}
/// <summary>
/// 计时数据保存类型
/// </summary>
public class TimingRewardData :LocalInt<TimingRewardData,ED_TimingReward>
{
    public override string Key()
    {
        return "TimingRewardData";
    }

    protected override int GetDefaultInt(ED_TimingReward e)
    {
        if (e == ED_TimingReward.curId)
            return TimeCountRewardManager.startId;
        return 0;
    }
}