/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2016-06-09     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// 计时奖励的显示层
/// </summary>
public class ViewTimeCountReward : MonoBehaviour 
{
    /// <summary>
    /// 剩余等待时间
    /// </summary>
    protected int surplusTime = 0;

	// Use this for initialization
	void Start() 
	{
        //FIX:
        TimeCountRewardManager.Begin();
	}
	
    void OnEnable()
    {
        TimeCountRewardManager.eventTimeCount += Show;
    }

    void OnDisable()
    {
        TimeCountRewardManager.eventTimeCount += Show;
    }

    protected virtual void Show(int curTime,int maxTime)
    {
        surplusTime = maxTime - curTime;

        TimeSpan time = new TimeSpan(0, 0, surplusTime);

        //FIX:
        Debug.Log(time);
    }

    /// <summary>
    /// 领取按钮
    /// </summary>
    public void BtnReceive()
    {
        if (surplusTime == 0)
        {
            ModelTimeCountReward mode = TimeCountRewardManager.instance.model;

            Debug.Log("gift_type is  " + mode.gift_type + "gift_num is " + mode.gift_num + "  " + mode.dis_time);

            TimeCountRewardManager.instance.ReceiveAwardAndNextGift();

            Debug.Log("Reward is finished!");
        }
    }

    void KMDebug()
    {
        BtnReceive();
    }

    void KMEditor()
    {
        TimingRewardData.instance.ClearData();
    }
}