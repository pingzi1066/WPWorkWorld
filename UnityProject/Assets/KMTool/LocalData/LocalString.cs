/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2016-07-19       WP      Initial version
 * 2016-11-14       WP      Add get and set the int and float method
 * 
 * *****************************************************************************/

using UnityEngine;
using System.Collections.Generic;
using SimpleJSON;
using System;

namespace KMTool
{
    /// <summary>
    /// string 数据保存基类
    /// </summary>
    public class LocalString<T, U>
        where T : LocalString<T, U>
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

        protected Dictionary<U, string> dict = new Dictionary<U, string>();

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
        /// 设置值 全局唯一设置值的接口
        /// </summary>
        /// <param name="eKey">E key.</param>
        /// <param name="value">Value.</param>
        /// <typeparam name="K">The 1st type parameter.</typeparam>
        public virtual void SetData(U eKey, string value)
        {
            if (dict.ContainsKey(eKey))
            {
                dict[eKey] = value;
            }
            else dict.Add(eKey, value);

            if (eventOnValue != null)
                eventOnValue(eKey, value);
        }

        public virtual void AddInt(U eKey,int value)
        {
            int perv = GetInt(eKey);

            perv += value;
            SetData(eKey, perv.ToString());
        }

        public virtual void AddFloat(U eKey,float value)
        {
            float perv = GetFloat(eKey);
            perv += value;
            SetData(eKey, perv.ToString());
        }

        /// <summary>
        /// 取值
        /// </summary>
        /// <returns>The string.</returns>
        /// <param name="eKey">E key.</param>
        /// <typeparam name="K">The 1st type parameter.</typeparam>
        public string GetData(U eKey)
        {
    #if UNITY_EDITOR
            if (!dict.ContainsKey(eKey))
            {
                Debug.Log("Don't fount key " + eKey.ToString());
                return "";
            }
    #endif

            return dict[eKey];
        }

        //取int 类型
        public int GetInt(U eKey)
        {
            string str = GetData(eKey);

            if (string.IsNullOrEmpty(str))
            {
                return 0;
            }

            int v = 0;
            if (!int.TryParse(str, out v))
            {
                Debug.LogError("Parse error ! " + str);
            }
            
            return v;
        }

        //取float类型
        public float GetFloat(U eKey)
        {
            string str = GetData(eKey);

            if (string.IsNullOrEmpty(str))
            {
                return 0;
            }

            float v = 0;
            if (!float.TryParse(str, out v))
            {
                Debug.LogError("Parse error ! " + str);
            }

            return v;
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
                    if (obj.HasKey(e.ToString()))
                    {
                        SetData(e, obj[e.ToString()].Value);
                    }
                    else SetData(e, GetDefaultValue(e));
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
//                if (!string.IsNullOrEmpty(v.Value))
//                {
//                    Debug.Log(new JSONData(v.Value).ToString() + " --- " + new JSONData(v.Value).Value);
//                }
                // add key and jsonData
                jsonObj.Add(v.Key.ToString(), new JSONData(v.Value).Value);
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

        protected virtual string GetDefaultValue(U e)
        {
            return "";
        }

        /// <summary>
        /// 取数组：值传递
        /// </summary>
        /// <returns>The dict.</returns>
        public Dictionary<U, string> GetDict()
        {
            return dict;
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

        public void RefreshEvent()
        {
            if (eventOnValue != null)
            {
                Type tp = typeof(U);
                Array arr = Enum.GetValues(tp);
                foreach (U e in arr)
                {
                    eventOnValue(e, GetData(e));
                }
            }
        }

        public void RefreshEvent(DelOnValue method)
        {
            if (method != null)
            {
                Type tp = typeof(U);
                Array arr = Enum.GetValues(tp);
                foreach (U e in arr)
                {
                    method(e, GetData(e));
                }
            }
        }
    }
}