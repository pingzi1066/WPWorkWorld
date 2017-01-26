/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2016-04-15     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using System.Collections.Generic;
using KMTool;

namespace KMToolDemo
{
    /// <summary>
    /// WayPoint 的Demo演示
    /// </summary>
    public class Demo_WayPoint : MonoBehaviour
    {
        [SerializeField]
        private GameObject goAnimTarget;

        [SerializeField]
        private List<WayController> ways;

        private WayAnimator target;

        void OnGUI()
        {
            if (GUI.Button(new Rect(100, 100, 200, 60), "Play Way To Cube"))
            {
                PlayWay();
            }

            if (target && GUI.Button(new Rect(100, 200, 100, 60), "Pause"))
            {
                Pause();
            }

            if (target && GUI.Button(new Rect(100, 300, 100, 60), "Stop"))
            {
                Stop();
            }
        }

        void PlayWay()
        {
            if (target == null)
            {
                target = goAnimTarget.AddComponent<WayAnimator>();
                target.SetParms(ways, WayAnimator.modes.once);
            }

            target.Play();
        }

        void Pause()
        {
            target.Pause();
        }

        void Stop()
        {
            target.Stop();
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
}