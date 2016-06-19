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
        
	}
	
    void OnEnable()
    {
        TimeCountRewardManager.eventTimeCount += Show;
        TimeCountRewardManager.RefreshEvent();
    }

    void OnDisable()
    {
        TimeCountRewardManager.eventTimeCount -= Show;

    }

    protected virtual void Show(int curTime,int maxTime)
    {
        surplusTime = maxTime - curTime;

        TimeSpan time = new TimeSpan(0, 0, surplusTime);

        Debug.Log(time.ToString());
    }

    /// <summary>
    /// 领取按钮
    /// </summary>
    public void BtnReceive()
    {
        if (surplusTime == 0)
        {

//            Debug.Log("gift_type is  " + mode.gift_type + "gift_num is " + mode.gift_num + "  " + mode.dis_time);

            TimeCountRewardManager.instance.ReceiveAwardAndNextGift();

            Debug.Log("Reward is finished!");
        }
    }

    void KMDebug()
    {
        
    }

    void KMEditor()
    {
        TimingRewardData.instance.ClearData();
    }
}