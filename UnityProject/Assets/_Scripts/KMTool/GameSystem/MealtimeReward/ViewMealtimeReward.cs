/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2016-06-10     WP      Initial version
 * 
 * *****************************************************************************/

using System;
using UnityEngine;
using System.Collections;

/// <summary>
/// 定时礼物的视图
/// </summary>
public class ViewMealtimeReward : MonoBehaviour 
{
    private bool isFinished = false;

    void OnEnable()
    {
        MealtimeRewardManager.eventOnTime += EventOnTime;
        MealtimeRewardManager.eventOnFinished += EventOnFinished;
        MealtimeRewardManager.RefreshEvent();
    }

    void OnDisable()
    {
        MealtimeRewardManager.eventOnTime -= EventOnTime;
        MealtimeRewardManager.eventOnFinished -= EventOnFinished;
    }

	// Use this for initialization
	void Start() 
	{
	}
	
	// Update is called once per frame
	void Update() 
	{
	
	}

    private void EventOnFinished()
    {
        isFinished = true;
    }

    private void EventOnTime(float time)
    {
        TimeSpan ts = new TimeSpan(0, 0, (int)time);
        Debug.Log("TODO:  " + ts.ToString());


        if (time > 0)
            isFinished = false;

    }

    public void BtnReceive()
    {
        if (isFinished)
        {
            ModelMealtimeReward model = MealtimeRewardManager.instance.GetGift();
            if (model != null)
            {
                Debug.Log("Btn ");
            }
        }
        else
        {
            Debug.Log("Don't finished");
        }
    }

    void KMDebug()
    {
        BtnReceive();
    }
}