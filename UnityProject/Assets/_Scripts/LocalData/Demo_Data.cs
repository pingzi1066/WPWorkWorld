/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2016-04-22     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 数据保存描述
/// </summary>
public class Demo_Data : LocalData<Demo_Data>
{
    public enum DemoEnum
    { 
        Coin,
        Gem,
    }

    private Dictionary<DemoEnum, int> dict = new Dictionary<DemoEnum, int>();

    override public string Key() { return "DemoKey"; }

    override public System.Type EnumType() { return typeof(DemoEnum); }

    public override int GetInt(System.Enum eKey)
    {
        if (!dict.ContainsKey((DemoEnum)eKey))
        {
            Debug.Log("Don't fount key " + eKey.ToString());
            return 0;
        }

        return dict[(DemoEnum)eKey];
    }

    protected override void SetInt(System.Enum eKey, int value)
    {
        DemoEnum e = (DemoEnum)eKey;

        if (dict.ContainsKey(e))
        {
            dict[e] = value;
        }
        else dict.Add(e, value);
    }

    public void AddInt(DemoEnum e, int addValue)
    {
        if (dict.ContainsKey(e))
        {
            dict[e] += addValue;
        }
        else
        {
            dict.Add(e, addValue);
        }
    }
}