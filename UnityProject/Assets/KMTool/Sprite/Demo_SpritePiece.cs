/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2017-05-11     WP      Initial version
 * 
 * *****************************************************************************/

using KMTool;
using UnityEngine;
using System.Collections;

namespace KMToolDemos
{

    /// <summary>
    /// the demo of sprite joint
    /// </summary>
    public class Demo_SpritePiece : MonoBehaviour
    {
        public SpritePiece target;

        public SpritePiece mover;

        public SpritePiece.JointPos joType;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnClick()
        {
            mover.Joint(target, joType);
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
}