/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2017-03-20     WP      Initial version
 * 
 * *****************************************************************************/


using UnityEngine;
using System.Collections.Generic;
using System;
using SimpleJSON;

namespace KMTool
{
    /// <summary>
    /// int数据保存基类 T 为此继承函数，U为枚举
    /// </summary>
    public abstract class ListLocalInt<T, U>
       where T : ListLocalInt<T, U>
    {

        public delegate void DelOnValue(int id, U key, int value);
        static public DelOnValue eventOnValue;

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

        /// <summary>
        /// 一个Item里面的属性
        /// </summary>
        public class LocalIntByList
        {
            public Dictionary<U, int> dict = new Dictionary<U, int>();

            public delegate void DelOnValue(U key, int value);

            public LocalIntByList()
            {
                CreateDefaultData();
            }

            /// <summary>
            /// 当值改变的方法监听
            /// </summary>
            public DelOnValue eventOnValue;

            /// <summary>
            /// 设置值 全局唯一设置值的接口
            /// </summary>
            /// <param name="eKey">E key.</param>
            /// <param name="value">Value.</param>
            /// <typeparam name="K">The 1st type parameter.</typeparam>
            public virtual void SetData(U eKey, int value)
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
            /// 添加值
            /// </summary>
            /// <param name="e">E.</param>
            /// <param name="addValue">Add value.</param>
            public virtual void AddData(U e, int addValue)
            {
                SetData(e, dict[e] + addValue);
            }

            /// <summary>
            /// 取值
            /// </summary>
            /// <returns>The int.</returns>
            /// <param name="eKey">E key.</param>
            /// <typeparam name="K">The 1st type parameter.</typeparam>
            public int GetData(U eKey)
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

            protected virtual int GetDefaultValue(U e)
            {
                return 0;
            }
        }

        /// <summary>
        /// 所有数据字典
        /// </summary>
        protected Dictionary<int, LocalIntByList> dict = new Dictionary<int, LocalIntByList>();

        /// <summary>
        /// 唯一的Key值，用于保存到数据
        /// </summary>
        /// <returns></returns>
        public virtual string Key() { return typeof(T).Name; }
        protected string key { get { return Key(); } }

        abstract protected U HeadKey();

        /// <summary>
        /// 读取数据
        /// </summary>
        public virtual void LoadData()
        {
            if (LocalTools.HasKey(key))
            {
                string jsonText = LocalTools.GetString(key);

                JSONNode data = JSON.Parse(jsonText);
                JSONArray array = data.AsArray;
                if (array != null)
                {
                    for (int i = 0; i < array.Count; i++)
                    {
                        JSONClass obj = array[i].AsObject;

                        Type tp = typeof(U);
                        Array arr = Enum.GetValues(tp);

                        LocalIntByList localInt = new LocalIntByList();
                        foreach (U e in arr)
                        {
                            localInt.SetData(e, obj[e.ToString()].AsInt);
                        }
                        dict.Add(localInt.GetData(HeadKey()), localInt);
                    }
                }
            }
            else
            {
                CreateDefaultData();
                SaveData();
            }
        }

        /// <summary>
        /// 设置值 全局唯一设置值的接口
        /// </summary>
        /// <param name="eKey">E key.</param>
        /// <param name="value">Value.</param>
        /// <typeparam name="K">The 1st type parameter.</typeparam>
    	public virtual void SetData(int id, U key, int value)
        {
            if (dict.ContainsKey(id))
            {
                dict[id].dict[key] = value;
            }
            else
            {
                //创建数据
                dict.Add(id, CreateDefaultData());
                //设置ID
                dict[id].SetData(HeadKey(), id);
                //设置值
                dict[id].SetData(key, value);
            }

            if (isInit && eventOnValue != null)
                eventOnValue(id, key, value);
        }

        public virtual void AddValueToData(int id, U key, int addValue)
        {
            if (dict.ContainsKey(id))
            {
                int per = dict[id].GetData(key);
                SetData(id, key, addValue + per);
            }
            else SetData(id, key, addValue);
        }

        public virtual int GetData(int id, U key)
        {
            if (dict.ContainsKey(id))
            {
                return dict[id].GetData(key);
            }
            Debug.Log("don't find this id");
            return 0;
        }

        /// <summary>
        /// 取当前数据类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual LocalIntByList GetItem(int id)
        {
            if (dict.ContainsKey(id))
            {
                return dict[id];
            }
            return null;
        }

        /// <summary>
        /// 是否包含id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Contains(int id)
        {
            return dict.ContainsKey(id);
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
            JSONArray jsonObj = new JSONArray();

            foreach (var localDataClass in dict)
            {
                JSONClass jcInOne = new JSONClass();
                // add key and jsonData
                foreach (var v in localDataClass.Value.dict)
                {
                    jcInOne.Add(v.Key.ToString(), new JSONData(v.Value).Value);
                }
                //添加一条
                jsonObj.Add(jcInOne);
                //Debug.Log(jcInOne.ToString() + "\n" + jsonObj.ToString());
            }

            string jsonText = jsonObj.ToString();
            return jsonText;
        }

        /// <summary>
        /// 创建默认数据
        /// </summary>
        protected virtual LocalIntByList CreateDefaultData()
        {
            LocalIntByList local = new LocalIntByList();
            return local;
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        public void ClearData()
        {
            CreateDefaultData();
            SaveData();
#if UNITY_EDITOR
            Debug.Log("Clear Data " + typeof(T).Name + "\n new Data :" + ToDebug());
#endif
        }

        public string ToDebug()
        {
            string text = "The Key :  " + key + "\n";

            text += ConvertDictToJson() + "\n";

            return text;
        }
    }
}