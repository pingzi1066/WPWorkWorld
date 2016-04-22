/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2016-04-22     WP      Initial version V.1.0 Save int data 
 * 
 * *****************************************************************************/

using UnityEngine;
using System.Collections;
using System;
using SimpleJSON;

/// <summary>
/// 数据保存基类
/// </summary>
abstract public class LocalData<T> where T : LocalData<T>
{
    /// <summary>
    /// 唯一的Key值，用于保存到数据
    /// </summary>
    /// <returns></returns>
    abstract public string Key();
    protected string key { get { return Key(); } }

    /// <summary>
    /// 返回保存枚举的类型 用 typeof(Enum) 
    /// </summary>
    /// <returns></returns>
    abstract public Type EnumType();
    protected Type enumType { get { return EnumType(); } }

    abstract protected void SetInt(Enum eKey, int value);
    abstract public int GetInt(Enum eKey);

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

    /// <summary>
    /// 读取数据
    /// </summary>
    public virtual void LoadData()
    {
        if (PlayerPrefs.HasKey(key))
        {
            string jsonText = PlayerPrefs.GetString(key);

            JSONNode data = JSON.Parse(jsonText);
            JSONClass obj = data.AsObject;

            Type tp = enumType;
            Array arr = Enum.GetValues(tp);
            foreach (var e in arr)
            {
                if (obj.HasKey(e.ToString()))
                {
                    SetInt(e as Enum, obj[e.ToString()].AsInt);
                }
                else SetInt(e as Enum, GetDefaultInt(e as Enum));
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
        Type tp = enumType;
        Array arr = Enum.GetValues(tp);

        JSONClass jsonObj = new JSONClass();

        foreach (var a in arr)
        {
            // add key and jsonData
            jsonObj.Add(a.ToString(), new JSONData(GetInt(a as Enum)));
        }

        string jsonText = jsonObj.ToString();
        PlayerPrefs.SetString(key, jsonObj.ToString());
        return jsonText;
    }

    /// <summary>
    /// 创建默认数据
    /// </summary>
    protected virtual void CreateDefaultData()
    {
        Type tp = enumType;
        Array arr = Enum.GetValues(tp);

        foreach (var a in arr)
        {
            SetInt(a as Enum, 0);
        }
    }

    protected virtual int GetDefaultInt(Enum e)
    {
        return 0;
    }
}