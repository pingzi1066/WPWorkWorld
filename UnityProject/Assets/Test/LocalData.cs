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


namespace KMTool
{
    /// <summary>
    /// 本地化数据 T : 类名， U：枚举， K：值类型
    /// </summary>
    public abstract class LocalData<T, U , K>
        where T : LocalData<T, U , K>
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

        protected Dictionary<U, K> dict = new Dictionary<U, K>();

        /// <summary>
        /// 唯一的Key值，用于保存到数据
        /// </summary>
        /// <returns></returns>
        public virtual string Key() { return typeof(T).Name; }
        protected string key { get { return Key(); } }

        public delegate void DelOnValue(U key, K value);

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
        public virtual void SetData(U eKey, K value)
        {
            if (dict.ContainsKey(eKey))
            {
                dict[eKey] = value;
            }
            else dict.Add(eKey, value);

            if (eventOnValue != null)
                eventOnValue(eKey, value);
        }

        /// <summary>
        /// 取值
        /// </summary>
        /// <returns>The int.</returns>
        /// <param name="eKey">E key.</param>
        /// <typeparam name="K">The 1st type parameter.</typeparam>
        public K GetData(U eKey)
        {
            #if UNITY_EDITOR
            if (!dict.ContainsKey(eKey))
            {
                Debug.Log("Don't fount key " + eKey.ToString());
                return GetDefaultValue(eKey);
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
                    if (obj.HasKey(e.ToString()))
                    {
                        SetData(e, ConvertFormString(obj[e.ToString()]));
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

        static protected string ConvertDictToJson()
        {
            JSONClass jsonObj = new JSONClass();

            foreach (var v in instance.dict)
            {
                // add key and jsonData
                jsonObj.Add(v.Key.ToString(), new JSONData(v.Value.ToString()).Value);
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

        protected abstract K GetDefaultValue(U e);
        protected abstract K ConvertFormString(string str);

        /// <summary>
        /// 取数组：值传递
        /// </summary>
        /// <returns>The dict.</returns>
        public Dictionary<U, K> GetDict()
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

        static public string ToDebug()
        {
            string text = "The Key :  " + instance.key + "\n";

            text += ConvertDictToJson() + "\n";

            return text;
        }

        static public void RefreshEvent(DelOnValue method)
        {
            if (method != null)
            {
                Type tp = typeof(U);
                Array arr = Enum.GetValues(tp);
                foreach (U e in arr)
                {
                    method(e, instance.GetData(e));
                }
            }
        }
    }
}