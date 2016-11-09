/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * #CREATIONDATE#     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using System.Collections;
using System;
using SimpleJSON;

/// <summary>
/// 描述
/// </summary>
public class JsonTest : MonoBehaviour 
{

    public bool debug = true;
    public string value = "";

    public bool getValue = false;
    public int intID = 1;
    public string intKey ="";
    public int intValue = 0;
    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (debug)
        {
            debug = false;
            Debug.Log("value is --- " + GetType(value).ToString());
        }

        if (getValue)
        {
            getValue = false;

            intValue = ModelTest3.GetData(intID).GetInt(intKey);

            Debug.Log("get int value is " + intValue);
        }


    }

    /// <summary>
    /// 判断是否是字符串
    /// </summary>
    /// <returns><c>true</c> if is string the specified s; otherwise, <c>false</c>.</returns>
    /// <param name="s">S.</param>
    public static System.Type GetType(string s)
    {
        //System.Single
        float f = 0;
        bool result = float.TryParse(s, out f);
        if (s.Contains("."))
        {
            if (result)
                return typeof(float);
        }

        // System.Int32
        int i = 0;
        result = int.TryParse(s, out i);
        if (result)
            return typeof(int);
        
        //System.String          
        return typeof(string);
    }

    #region 测试

    public void KMDebug()
    {
        Debug.Log(" ---------KMDebug----------", gameObject);
    }

    public void KMEditor()
    {
        Debug.Log(" ---------KMEditor----------", gameObject);
    }

    #endregion
}