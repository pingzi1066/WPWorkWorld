/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2016-08-05     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using System.Collections;

/// <summary>
/// 描述
/// </summary>

public class Demo_MethodAttribute : MonoBehaviour
{
    [ContextMenuItemAttribute("11", "Start")]
    public int ff;
    [Multiline(2)]
    public string playerBiography = "";
    // Use this for initialization


    //[Header("kkkk")]
    public float ffff;

    //[Method]
    void Start()
    {
        Debug.Log(ff);
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