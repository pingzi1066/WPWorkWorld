/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2015-11-30     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using System.Collections;

/// <summary>
/// 描述
/// </summary>
public class DemoSingetonItem : MonoBehaviour 
{

    void Awake()
    {
        Debug.Log(DemoSingleton.instance.desc);
    }

	// Use this for initialization
	void Start() 
	{
	
	}
	
	// Update is called once per frame
	void Update() 
	{
	
	}
}