/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2016-12-10     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using System.Collections;

/// <summary>
/// 描述
/// </summary>
public class Demo_Local : MonoBehaviour 
{
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
        Debug.Log(" ---------KMDebug----------" + LocalDataInt.ToDebug(), gameObject);

    }

    public void KMEditor()
    {
        Debug.Log(" ---------KMEditor----------" + LocalDataString.ToDebug(), gameObject);
    }

    #endregion
}