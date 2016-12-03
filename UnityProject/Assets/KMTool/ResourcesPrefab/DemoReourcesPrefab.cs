/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2016-11-27     WP      Initial version
 * 
 * *****************************************************************************/
using KMTool;
using UnityEngine;
using System.Collections;

/// <summary>
/// 描述
/// </summary>
public class DemoReourcesPrefab :  ResourcesInstance<DemoReourcesPrefab>
{
    public string desc = "";

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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