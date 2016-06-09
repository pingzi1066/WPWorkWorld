/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2016-06-05     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using System.Collections;

/// <summary>
/// 时间奖励取表模版
/// </summary>
public class ModelTimeCountReward : ModelBase<StaticTimeCountReward> 
{
    /// <summary>
    /// 总时间
    /// </summary>
    /// <value>The dis time.</value>
    public int dis_time { get { return GetInt("dis_time"); } }
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

}