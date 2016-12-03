/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2016-11-27     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using System.Collections;

namespace KMTool
{
    /// <summary>
    /// 当Prefab 有这个脚本时，直接调用instance 就可以生成
    /// </summary>
    abstract public class ResourcesInstance<T> : MonoBehaviour where T : ResourcesInstance<T>
    {
        private static T m_instance = null;
        public static T instance
        {
            get
            {
                if (m_instance == null)
                {
                    Object obj = GameObject.FindObjectOfType(typeof(T));
                    if (obj != null)
                    {
                        m_instance = obj as T;
                    }

                    if (m_instance == null)
                    {
                        // prefab 生成方式
                        string path = ResourcesManager.GetPath(typeof(T).FullName);
                        GameObject go = Resources.Load(path, typeof(GameObject)) as GameObject;
//                        if (go != null)
//                            Debug.Log(go.name, go);
//                        else
//                            Debug.Log("go is null  " + path);
                        T prb = go.GetComponent(typeof(T)) as T;
                        m_instance = KMTools.AddChild(null, prb, false);
                    }
                    m_instance.OnInit();
                }

                return m_instance;
            }
        }

        protected virtual void OnInit()
        {
        }

        protected virtual void Awake()
        {
            if (m_instance == null)
            {
                m_instance = this as T;
                OnInit();
            }
            else if (m_instance != this)
            {
                Debug.LogError(" multi-singleton in scene!!! ", gameObject);
                gameObject.SetActive(false);
            }
        }
    }
}