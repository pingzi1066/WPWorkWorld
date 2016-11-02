/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2015-11-30     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using System.Collections;

/// <summary>
/// µ¥Àý»ùÀà
/// </summary>
abstract public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T m_instance = null;

    static private GameObject goParent;

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

                    if (goParent == null)
                    {
                        goParent = new GameObject("_Singletons");
                    }

                    obj.transform.parent = goParent.transform;
                    //obj.hideFlags = HideFlags.HideAndDontSave;
                    //Object.DontDestroyOnLoad(obj);
                    m_instance = obj.AddComponent(typeof(T)) as T;
                }
            }

            return m_instance;
        }
    }

    protected virtual void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this as T;
        }
        else if (m_instance != this)
        {
            Debug.LogError(" multi-singleton in scene!!! ", gameObject);
            gameObject.SetActive(false);
        }
    }
}
