/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2016-07-19     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using System.Collections.Generic;
using System;
using SimpleJSON;

/// <summary>
/// String数组 数据保存
/// </summary>
public class LocalListString<T, U>
    where T : LocalListString<T, U>
{

    private static T m_instance = null;
    public static T instance
    {
        get
        {
            if (m_instance == null)
            {
                System.Type type = typeof(T);
                object obj = type.Assembly.CreateInstance(type.FullName);
                m_instance = obj as T;
                m_instance.LoadData();
            }
            return m_instance;
        }
    }

    protected Dictionary<U, List<string>> dict = new Dictionary<U, List<string>>();

    /// <summary>
    /// 唯一的Key值，用于保存到数据
    /// </summary>
    /// <returns></returns>
    public virtual string Key() { return typeof(T).Name; }
    protected string key { get { return Key(); } }

    public delegate void DelOnValue(U key, string value);

    /// <summary>
    /// 当值改变的方法监听
    /// </summary>
    static public DelOnValue eventOnValue;

    /// <summary>
    /// 是否允许重复
    /// </summary>
    /// <returns></returns>
    protected bool IsRepeat()
    {
        return false;
    }

    /// <summary>
    /// 增加值
    /// </summary>
    /// <param name="e">E.</param>
    /// <param name="addItem">Add value.</param>
    public virtual void AddItem(U e, string addItem)
    {
        if (!dict.ContainsKey(e))
        {
            Debug.LogError("Don't fount list " + e.ToString());
            return;
        }

        if (dict[e].Contains(addItem) && !IsRepeat())
        {
            return;
        } 
        
        dict[e].Add(addItem);
    }

    public virtual void RemoveItem(U e, string item)
    {
        if (dict.ContainsKey(e) && dict[e].Contains(item))
        {
            dict[e].Remove(item);
        }
        else
        {
            Debug.Log("Don't found item " + item);
        }
    }

    public virtual void Clear(U e)
    {
        if (dict.ContainsKey(e))
        {
            dict[e].Clear();
        }
        else
        {
            Debug.Log("Don't found list " + e.ToString());
        }
    }

    /// <summary>
    /// 取值
    /// </summary>
    /// <returns>The int.</returns>
    /// <param name="eKey">E key.</param>
    /// <typeparam name="K">The 1st type parameter.</typeparam>
    public List<string> GetData(U eKey)
    {
#if UNITY_EDITOR
        if (!dict.ContainsKey(eKey))
        {
            Debug.Log("Don't fount key " + eKey.ToString());
            return null;
        }
#endif

        return dict[eKey];
    }


    /// <summary>
    /// 读取数据
    /// </summary>
    public virtual void LoadData()
    {
        if (LocalTools.HasKey(key))
        {
            string jsonText = LocalTools.GetString(key);

            JSONNode data = JSON.Parse(jsonText);
            JSONClass obj = data.AsObject;

            Type tp = typeof(U);
            Array arr = Enum.GetValues(tp);
            foreach (U e in arr)
            {
                //读取的时候用新数组
                List<string> newList = new List<string>();
                if (obj.HasKey(e.ToString()))
                {
                    JSONArray jsonArray = obj[e.ToString()].AsArray;

                    for (int i = 0; i < jsonArray.Count; i++)
                    {
                        newList.Add(jsonArray[i].ToString());
                    }
                }
                SetData(e, newList);

            }
        }
        else
        {
            CreateDefaultData();
            SaveData();
        }
    }

    /// <summary>
    /// 保存数据
    /// </summary>
    public virtual string SaveData()
    {
        string jsonText = ConvertDictToJson();
        LocalTools.SetString(key, jsonText);
        return jsonText;
    }

    protected string ConvertDictToJson()
    {
        JSONClass jsonObj = new JSONClass();

        foreach (var v in dict)
        {
            List<string> list = v.Value;
            JSONArray array = new JSONArray();
            // add key and jsonData
            for (int i = 0; i < list.Count; i++)
            {
                array.Add(new JSONData(list[i]));
            }
            jsonObj.Add(v.Key.ToString(), array);
        }

        string jsonText = jsonObj.ToString();
        return jsonText;
    }

    /// <summary>
    /// 创建默认数据
    /// </summary>
    protected virtual void CreateDefaultData()
    {
        Type tp = typeof(U);
        Array arr = Enum.GetValues(tp);

        foreach (U a in arr)
        {
            SetData(a, GetDefaultValue(a));
        }
    }

    protected virtual List<string> GetDefaultValue(U e)
    {
        return new List<string>();
    }

    /// <summary>
    /// 取数组：值传递
    /// </summary>
    /// <returns>The dict.</returns>
    public Dictionary<U, List<string>> GetDict()
    {
        return dict;
    }

    protected virtual void SetData(U e, List<string> list)
    {
        if (dict.ContainsKey(e))
        {
            dict[e] = list;
        }
        else dict.Add(e, list);
    }

    /// <summary>
    /// 清空数据
    /// </summary>
    public void ClearData()
    {
        CreateDefaultData();
        SaveData();
#if UNITY_EDITOR
        Debug.Log("Clear Data " + typeof(T).Name);
#endif
    }

    public string ToDebug()
    {
        string text = "The Key :  " + key + "\n";

        text += ConvertDictToJson() + "\n";

        return text;
    }
}