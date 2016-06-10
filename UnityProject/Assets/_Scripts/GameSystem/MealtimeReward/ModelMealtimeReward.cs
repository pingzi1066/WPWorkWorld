/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2016-06-09     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using System.Collections;

/// <summary>
/// 定时礼物单独数据
/// </summary>
public class ModelMealtimeReward : ModelBase<StaticMealtimeReward> 
{

    /// <summary>
    /// 小时
    /// </summary>
    /// <value>The dis time.</value>
    public int hour { get { return GetInt("hour"); } }

    /// <summary>
    /// 分钟
    /// </summary>
    /// <value>The minute.</value>
    public int minute { get { return GetInt("minute"); } }

    /// <summary>
    /// 礼物类型
    /// </summary>
    /// <value>The type of the gift.</value>
    public int gift_type { get { return GetInt("gift_type"); } }
    /// <summary>
    /// 礼物数量
    /// </summary>
    /// <value>The gift number.</value>
    public int gift_num { get { return GetInt("gift_num"); } }

    /// <summary>
    /// 领取限制时间
    /// </summary>
    /// <value>The last time.</value>
    public int last_time { get { return GetInt("last_time"); } }
   
}