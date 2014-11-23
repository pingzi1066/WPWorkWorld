/******************************************************************************
 *
 * Maintaince Logs:
 * 2013-08-01   WP      Initial version. 
 *
 * *****************************************************************************/

using UnityEngine;
using System.Collections;

/// <summary>
/// 循环播放某一个动画
/// </summary>
public class LoopPlayAnimation : MonoBehaviour
{

    public string nameAnim = "idle";

    // Use this for initialization
    void Start()
    {
        if (animation == null)
        {
            enabled = false;
            return;
        }

        animation[nameAnim].wrapMode = WrapMode.Loop;
        animation.Play(nameAnim);
    }


}
