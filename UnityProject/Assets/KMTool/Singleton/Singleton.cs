/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2015-11-30       WP      Initial version
 * 2016-11-16       WP      fixed: mutil parent!
 * 
 * *****************************************************************************/

using UnityEngine;
using System.Collections;

namespace KMTool
{

    static public class _Singleton
    {
        static private GameObject go;
        static public GameObject goParent
        {
            get
            {
                if (go == null)
                {
                    go = new GameObject("_Singletons");
                    //obj.hideFlags = HideFlags.HideAndDontSave;
                    //Object.DontDestroyOnLoad(obj);
                }
                return go;
            }
        }
    }

    /// <summary>
    /// µ¥Àý»ùÀà
    /// </summary>
    abstract public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static T m_instance = null;

        private bool isInit = false;
        public static T instance
        {
            get
            {
                if (m_instance == null)
                {
                    Object go = GameObject.FindObjectOfType(typeof(T));
                    if (go != null)
                    {
                        m_instance = go as T;
                    }

                    if (m_instance == null)
                    {
                        GameObject obj = new GameObject(typeof(T).FullName);

                        GameObject goParent = _Singleton.goParent;

                        obj.transform.parent = goParent.transform;
                        m_instance = obj.AddComponent(typeof(T)) as T;
                    }
                    if (!m_instance.isInit)
                    {
                        m_instance.isInit = true;
                        m_instance.Init();
                    }
                }

                return m_instance;
            }
        }

        protected virtual void Init()
        {
        }

        protected virtual void Awake()
        {
            if (m_instance == null)
            {
                m_instance = this as T;
                if (!isInit)
                {
                    Init();
                    isInit = true;
                }
            }
            else if (m_instance != this)
            {
                Debug.LogError(" multi-singleton in scene!!! ", gameObject);
                gameObject.SetActive(false);
            }
        }
    }
}