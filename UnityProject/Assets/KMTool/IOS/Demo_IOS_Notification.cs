/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2016-06-09     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using System.Collections;

namespace KMToolDemo
{
    /// <summary>
    /// 描述
    /// </summary>
    public class Demo_IOS_Notification : MonoBehaviour 
    {

    	// Use this for initialization
    	void Start()
        {
#if UNITY_EDITOR || UNITY_IOS
            //10秒后发送
            IOS_Notification.instance.AddNotificationMessage("P : 60秒后发送", 0,1, false);
            //每天中午12点推送
            IOS_Notification.instance.AddNotificationMessage("P : 每天中午12点推送", 12,0, true);
#endif
        }
    	
    	// Update is called once per frame
    	void Update() 
    	{
    	
    	}
    }
}