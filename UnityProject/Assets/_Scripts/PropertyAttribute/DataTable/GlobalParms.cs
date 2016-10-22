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
    private List<ScriptableObjectIntParms> intParms = new List<ScriptableObjectIntParms>();
    [SerializeField]
    private List<ScriptableObjectFloatParms> floatParms = new List<ScriptableObjectFloatParms>();


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
                }
            }
            return mInstance;
        }
    }

    static public void AddIntObj(ScriptableObjectIntParms so)
    {
        if (instance)
            instance.AddIntFile(so);   
    }

    private void AddIntFile(ScriptableObjectIntParms so)
    {
        if (!intParms.Contains(so))
        {
            intParms.Add(so);
        }
    }

    static public void AddFloatObj(ScriptableObjectFloatParms so)
    {
        if (instance)
            instance.AddFloatFile(so);   
    }

    private void AddFloatFile(ScriptableObjectFloatParms so)
    {
        if (!floatParms.Contains(so))
        {
            floatParms.Add(so);
        }
    }

    static public int GetInt(string name)
    {
        if (instance)
            return instance.GetIntParm(name);

        Debug.Log("Don't find indtance");
        return -1;
    }

    private int GetIntParm(string name)
    {
        for (int i = 0; i < intParms.Count; i++)
        {
            if (intParms[i] != null && intParms[i].listParms.Contains(name))
            {
                return intParms[i].listParms.Get(name);
            }
        }

        Debug.Log("Don't find int with " + name);
        return -1;
    }

    static public float GetFloat(string name)
    {
        if (instance)
            return instance.GetFloatParm(name);

        Debug.Log("Don't find indtance");
        return -1;
    }

    private float GetFloatParm(string name)
    {
        for (int i = 0; i < intParms.Count; i++)
        {
            if (intParms[i] != null && intParms[i].listParms.Contains(name))
            {
                return intParms[i].listParms.Get(name);
            }
        }
        Debug.Log("Don't find float with " + name);
        return -1;
    }
}
