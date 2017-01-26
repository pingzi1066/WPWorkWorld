/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2016-07-06     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using System.Collections;
using KMTool;

namespace KMToolDemo
{
    public enum DemoListEnum
    {
        CharIds,
        CIds,
    }
    /// <summary>
    /// 描述
    /// </summary>
    public class Demo_ListIntData : LocalListInt<Demo_ListIntData, DemoListEnum> 
    {

    }
}