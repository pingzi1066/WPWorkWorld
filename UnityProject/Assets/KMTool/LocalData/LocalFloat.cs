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
    /// float 数据保存
    /// </summary>
    public class LocalFloat<T, U>
        where T : LocalFloat<T, U>
    {

        private bool isInit = false;

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
                    LocalTools.eventSaveData += m_instance.PrivateSaveData;
                    LocalTools.eventDelData += m_instance.ClearData;
                    m_instance.isInit = true;
                }
                return m_instance;
            }
        }

        protected Dictionary<U, float> dict = new Dictionary<U, float>();

        /// <summary>
        /// 唯一的Key值，用于保存到数据
        /// </summary>
        /// <returns></returns>
        public virtual string Key() { return typeof(T).Name; }
        protected string key { get { return Key(); } }

        public delegate void DelOnValue(U key, float value);

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
        public virtual void SetData(U eKey, float value)
        {
            if (dict.ContainsKey(eKey))
            {
                dict[eKey] = value;
            }
            else dict.Add(eKey, value);

            if (isInit && eventOnValue != null)
                eventOnValue(eKey, value);
        }

        /// <summary>
        /// 添加值
        /// </summary>
        /// <param name="e">E.</param>
        /// <param name="addValue">Add value.</param>
        public virtual void AddData(U e, float addValue)
        {
            SetData(e, dict[e] + addValue);
        }

        /// <summary>
        /// 取值
        /// </summary>
        /// <returns>The int.</returns>
        /// <param name="eKey">E key.</param>
        /// <typeparam name="K">The 1st type parameter.</typeparam>
        public float GetData(U eKey)
        {
#if UNITY_EDITOR
            if (!dict.ContainsKey(eKey))
            {
                Debug.Log("Don't fount key " + eKey.ToString());
                return 0;
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

                LoadData(jsonText);
            }
            else
            {
                CreateDefaultData();
                SaveData();
            }
        }

        public virtual void LoadData(string jsonText)
        {
            JSONNode data = JSON.Parse(jsonText);
            JSONClass obj = data.AsObject;

            Type tp = typeof(U);
            Array arr = Enum.GetValues(tp);
            foreach (U e in arr)
            {
                if (obj.HasKey(e.ToString()))
                {
                    SetData(e, obj[e.ToString()].AsFloat);
                }
                else SetData(e, GetDefaultValue(e));
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

        private void PrivateSaveData()
        {
            SaveData();
        }

        protected string ConvertDictToJson()
        {
            JSONClass jsonObj = new JSONClass();

            foreach (var v in dict)
            {
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

        protected virtual float GetDefaultValue(U e)
        {
            return 0;
        }

        /// <summary>
        /// 取数组：值传递
        /// </summary>
        /// <returns>The dict.</returns>
        public Dictionary<U, float> GetDict()
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