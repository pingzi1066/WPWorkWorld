/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2016-07-19       WP      Initial version
 * 2017-01-26       WP      reset '' added eventSaveData
 * 
 * *****************************************************************************/

using UnityEngine;
using System.Collections;

namespace KMTool
{
    /// <summary>
    /// 给于单独的数据保存 提供接口
    /// </summary>
    public static class LocalTools
    {
        public delegate void DelSetData();
        static public DelSetData eventSaveData;
        static public DelSetData eventDelData;

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

        static public void SaveAllData()
        {
            if (eventSaveData != null)
                eventSaveData();
        }

        static public void ResetAllData()
        {
            if (eventDelData != null)
                eventDelData();
        }
    }
}