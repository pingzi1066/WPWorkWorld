/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2016-10-22     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using System.Collections.Generic;

public class GlobalParms : MonoBehaviour 
{
    private const string PATH = "GlobalParms/GlobalParms";

    [SerializeField]
    private List<ScriptableObjectIntParms> intScriptableObjects = new List<ScriptableObjectIntParms>();
    [SerializeField]
    private List<ScriptableObjectFloatParms> floatScriptableObjects = new List<ScriptableObjectFloatParms>();

    private Dictionary<string,float> floatParms = new Dictionary<string, float>();
    private Dictionary<string,int> intParms = new Dictionary<string, int>();


    static private GlobalParms mInstance;
    static public GlobalParms instance
    {
        get
        {
            if (mInstance == null)
            {
                Object obj = Resources.Load(PATH, typeof(GameObject));
                if (obj != null)
                {
                    GameObject go = obj as GameObject;

                    mInstance = go.GetComponent<GlobalParms>();

                    if (mInstance)
                    {
                        mInstance.Init();
                    }
                }
            }
            return mInstance;
        }
    }

    /// <summary>
    /// 首次取实例的时候，会进行添加
    /// </summary>
    private void Init()
    {
        floatParms.Clear();
        intParms.Clear();

        for (int i = 0; i < intScriptableObjects.Count; i++)
        {
            ScriptableObjectIntParms so = intScriptableObjects[i];

            if (so == null)
                continue;
            if (so.listParms == null)
                continue;

            for (int j = 0; j < so.listParms.Count; j++)
            {
                intParms.Add(so.listParms[j].name,so.listParms[j].value);
            }
        }

        for (int i = 0; i < floatScriptableObjects.Count; i++)
        {
            ScriptableObjectFloatParms so = floatScriptableObjects[i];

            if (so == null)
                continue;
            if (so.listParms == null)
                continue;

            for (int j = 0; j < so.listParms.Count; j++)
            {
                floatParms.Add(so.listParms[j].name,so.listParms[j].value);
            }
        }
    }

    static public void AddIntObj(ScriptableObjectIntParms so)
    {
        if (instance)
            instance.AddIntFile(so);   
    }

    private void AddIntFile(ScriptableObjectIntParms so)
    {
        if (!intScriptableObjects.Contains(so))
        {
            intScriptableObjects.Add(so);
        }
    }

    static public void RemoveIntObj(ScriptableObjectIntParms so)
    {
        if (instance)
            instance.RemoveIntFile(so);
    }

    private void RemoveIntFile(ScriptableObjectIntParms so)
    {
        Debug.Log("remove");
        if (intScriptableObjects.Contains(so))
        {
            intScriptableObjects.Remove(so);
        }
    }

    static public void AddFloatObj(ScriptableObjectFloatParms so)
    {
        if (instance)
            instance.AddFloatFile(so);   
    }

    private void AddFloatFile(ScriptableObjectFloatParms so)
    {
        if (!floatScriptableObjects.Contains(so))
        {
            floatScriptableObjects.Add(so);
        }
    }

    static public void RemoveFloatObj(ScriptableObjectFloatParms so)
    {
        if (instance)
            instance.RemoveFloatFile(so);   
    }

    private void RemoveFloatFile(ScriptableObjectFloatParms so)
    {
        if (floatScriptableObjects.Contains(so))
        {
            floatScriptableObjects.Remove(so);
        }
    }

    static public int GetInt(string name)
    {
        if (instance)
            return instance.GetIntParm(name);

        Debug.Log("Don't find instance");
        return -1;
    }

    private int GetIntParm(string name)
    {
        if (intParms.ContainsKey(name))
        {
            return intParms[name];
        }

        Debug.Log("Don't find int with " + name);
        return -1;
    }

    static public float GetFloat(string name)
    {
        if (instance)
            return instance.GetFloatParm(name);

        Debug.Log("Don't find instance");
        return -1;
    }

    private float GetFloatParm(string name)
    {
        if (floatParms.ContainsKey(name))
        {
            return floatParms[name];
        }
        Debug.Log("Don't find float with " + name);
        return -1;
    }

    static public void RefreshRes()
    {
        if (instance)
            instance.Refresh();
    }

    private void Refresh()
    {
        for (int i = 0; i < intScriptableObjects.Count;)
        {
            if (intScriptableObjects[i] == null)
            {
                intScriptableObjects.RemoveAt(i);
                continue;
            }
            i++;
        }

        for (int i = 0; i < floatScriptableObjects.Count;)
        {
            if (floatScriptableObjects[i] == null)
            {
                floatScriptableObjects.RemoveAt(i);
                continue;
            }
            i++;
        }
    }
}
