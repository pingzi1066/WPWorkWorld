/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2016-04-22     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using System.Collections.Generic;
using KMTool;

public enum DemoEnum
{ 
    Coin,
    Gem,
}

/// <summary>
/// 数据保存描述
/// </summary>
public class Demo_IntData : LocalInt<Demo_IntData,DemoEnum>
{
    
}