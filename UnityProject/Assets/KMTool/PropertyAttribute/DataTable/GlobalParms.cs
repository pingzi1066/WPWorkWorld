/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2016-10-22     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using System.Collections.Generic;
using KMTool;

public class GlobalParms : MonoBehaviour 
{
    private const string PATH = "GlobalParms/GlobalParms";

    [SerializeField]
    private List<ScriptableObjectIntParms> intScriptableObjects = new List<ScriptableObjectIntParms>();
    [SerializeField]
    private List<ScriptableObjectFloatParms> floatScriptableObjects = new List<ScriptableObjectFloatParms>();
    [SerializeField]
    private List<ScriptableObjectStringParms> stringScriptableObjects = new List<ScriptableObjectStringParms>();

    private Dictionary<string,float> floatParms = new Dictionary<string, float>();
    private Dictionary<string,int> intParms = new Dictionary<string, int>();
    private Dictionary<string,string> stringParms = new Dictionary<string, string>();


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
        if (!Application.isPlaying)
            return;

        floatParms.Clear();
        intParms.Clear();
        stringParms.Clear();

        for (int i = 0; i < intScriptableObjects.Count; i++)
        {
            ScriptableObjectIntParms so = intScriptableObjects[i];

            if (so == null)
                continue;
            if (so.listParms == null)
                continue;

            for (int j = 0; j < so.listParms.Count; j++)
            {
                if (!intParms.ContainsKey(so.listParms[j].name))
                    intParms.Add(so.listParms[j].name, so.listParms[j].value);
                else
                    Debug.Log("has key " + so.listParms[j].name);
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
                if (!floatParms.ContainsKey(so.listParms[j].name))
                    floatParms.Add(so.listParms[j].name,so.listParms[j].value);
                else
                    Debug.Log("has key " + so.listParms[j].name);
            }
        }

        for (int i = 0; i < stringScriptableObjects.Count; i++)
        {
            ScriptableObjectStringParms so = stringScriptableObjects[i];

            if (so == null)
                continue;
            if (so.listParms == null)
                continue;

            for (int j = 0; j < so.listParms.Count; j++)
            {
                if (!stringParms.ContainsKey(so.listParms[j].name))
                    stringParms.Add(so.listParms[j].name,so.listParms[j].value);
                else
                    Debug.Log("has key " + so.listParms[j].name);
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

    static public void AddStringObj(ScriptableObjectStringParms so)
    {
        if (instance)
            instance.AddStringFile(so);   
    }

    private void AddStringFile(ScriptableObjectStringParms so)
    {
        if (!stringScriptableObjects.Contains(so))
        {
            stringScriptableObjects.Add(so);
        }
    }

    static public void RemoveStringObj(ScriptableObjectStringParms so)
    {
        if (instance)
            instance.RemoveStringFile(so);   
    }

    private void RemoveStringFile(ScriptableObjectStringParms so)
    {
        if (stringScriptableObjects.Contains(so))
        {
            stringScriptableObjects.Remove(so);
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

    static public string GetString(string name)
    {
        if (instance)
            return instance.GetStringParm(name);

        Debug.Log("Don't find instance");
        return "";
    }

    private string GetStringParm(string name)
    {
        if (stringParms.ContainsKey(name))
        {
            return stringParms[name];
        }
        Debug.Log("Don't find str with " + name);
        return "";
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

        for (int i = 0; i < stringScriptableObjects.Count;)
        {
            if (stringScriptableObjects[i] == null)
            {
                stringScriptableObjects.RemoveAt(i);
                continue;
            }
            i++;
        }
    }

    static public double GetDouble(string name)
    {
        double d = 0;
        string str = GetString(name);
        double.TryParse(str, out d);

        return d;
    }
}
