/******************************************************************************
 *
 * Maintaince Logs:
 * 2012-11-15    WP   Initial version. 
 *
 * *****************************************************************************/

using UnityEngine;

/// <summary>
/// ipad 1024*768 (4:3) iphone 1152*768 (3:2) iphone5 1356*768 (113:64) 
/// </summary>
public class IOSGizmos : MonoBehaviour
{
#if UNITY_EDITOR

    public bool isIPAD = true;
    public bool isIphone = true;
    public bool isIphone5 = true;

    void OnDrawGizmos()
    {
        if (isIPAD)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(gameObject.transform.position, new Vector3(2.6666f, 2.0f, 0));
        }

        if (isIphone)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(gameObject.transform.position, new Vector3(3, 2.0f, 0));
        }

        if (isIphone5)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(gameObject.transform.position, new Vector3(3.53f, 2.0f, 0));
        }

    }
#endif
}
