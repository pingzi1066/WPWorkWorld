/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2016-07-19     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using System.Collections;

/// <summary>
/// 给于单独的数据保存 提供接口
/// </summary>
public static class LocalTools
{

    static public void SetString(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
    }

    static public string GetString(string key, string defaultValue = "")
    {
        return PlayerPrefs.GetString(key, defaultValue);
    }

    static public bool HasKey(string key)
    {
        return PlayerPrefs.HasKey(key);
    }
}