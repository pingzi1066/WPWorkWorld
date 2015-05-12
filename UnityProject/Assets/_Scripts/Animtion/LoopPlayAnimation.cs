/******************************************************************************
 *
 * Maintaince Logs:
 * 2013-08-01   WP      Initial version. 
 * 2015-05-11   WP      added play on enable 
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

    public bool playOnEnable = true;

    // Use this for initialization
    void Start()
    {
        if (playOnEnable) return;
        Play();
    }

    void OnEnable()
    {
        if (playOnEnable) Play();
    }

    void Play()
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
