/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2015-11-30     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using System.Collections;

/// <summary>
/// 单例
/// </summary>
public class DemoSingleton : Singleton<DemoSingleton>
{
    public string desc = "Hello,This is singleton!";


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}