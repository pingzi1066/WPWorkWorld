/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2017-03-02     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using System.Collections;

namespace KMTool
{
    /// <summary>
    /// 旋转
    /// </summary>
    public class KMRotate : MonoBehaviour
    {
        public bool isLocal = true;
        public Vector3 rotateSpeed = Vector3.zero;
        public bool isAuto = true;
        public bool ignoreTimeScale = false;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if(isAuto)
            {
                Rotate();
            }
        }

        public void Rotate()
        {
            transform.Rotate(rotateSpeed * (ignoreTimeScale ? KMTime.deltaTime : Time.deltaTime), isLocal ? Space.Self : Space.World);
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