/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2016-11-27     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using System.Collections;

/// <summary>
/// 描述
/// </summary>
public class DemoReourcesPrefabMono : MonoBehaviour 
{
    // Use this for initialization
    void Start()
    {
        if (DemoReourcesPrefab.instance)
        {
            Debug.Log("instance is success load \n" + DemoReourcesPrefab.instance.desc);
        }
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