/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2016-11-27     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using System.Collections.Generic;

namespace KMTool
{

    /// <summary>
    /// Resources Prefab 的管理器
    /// </summary>
    public class ResourcesManager : MonoBehaviour 
    {
        [System.Serializable]
        public class PrefabItem
        {
            public string name;
            public string path;

            public PrefabItem(string n,string p)
            {
                name = n;
                path = p;
            }
        }

        /// <summary>
        /// key : 类名， value 路径
        /// </summary>
        [SerializeField] private List<PrefabItem> prefabs = new List<PrefabItem>();

        public const string thisPrefab = "KMPrefab/ResourcesManager";

        private bool ContainsKey(string key)
        {
            bool con = false;

            for (int i = 0; i < prefabs.Count; i++)
                if (prefabs[i].name == key)
                {
                    con = true;
                    break;
                }

            return con;
        }

        private PrefabItem GetPrefabItem(string key)
        {
            for (int i = 0; i < prefabs.Count; i++)
                if (prefabs[i].name == key)
                {
                    return prefabs[i];
                }

            return null;
        }

        private void SetPath(string key,string path)
        {
            PrefabItem pi = GetPrefabItem(key);

            if (pi != null)
            {
                pi.path = path;
            }
        }

        public void Add(string key ,string path)
        {
            if (!ContainsKey(key))
            {
                prefabs.Add(new PrefabItem(key,path));
            }
            else
            {
                SetPath(key, path);
            }
        }

        static public void AddItem(string key ,string path)
        {
            GameObject go = Resources.Load(thisPrefab, typeof(GameObject)) as GameObject;
            if (go != null)
            {
                ResourcesManager rm = go.GetComponent<ResourcesManager>();
                if (rm)
                {
                    rm.Add(key, path);
                    Debug.Log("add key " + key + " \npath:" + path);
                }
            }
        }

        static public string GetPath(string key)
        {
            string path = "";

            GameObject go = Resources.Load(thisPrefab, typeof(GameObject)) as GameObject;
            if (go != null)
            {
                ResourcesManager rm = go.GetComponent<ResourcesManager>();
                if (rm && rm.ContainsKey(key))
                {
                    path = rm.GetPrefabItem(key).path;
                }
            }

            return path;
        }

    }
}