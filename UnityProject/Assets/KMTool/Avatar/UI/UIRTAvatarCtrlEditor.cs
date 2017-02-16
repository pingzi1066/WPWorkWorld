/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2017-02-15     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using System.Collections;

namespace KMTool
{

    /// <summary>
    /// ren tex 的控制器的编辑开发脚本
    /// </summary>
    public partial class UIRTAvatarCtrl
    {
#if UNITY_EDITOR

        private float exPind;

        void OnDrawGizmos()
        {
            if (parent == null) return;

            if (exPind != xPind)
            {
                exPind = xPind;

                Vector3 pos = parent.position;
                for (int i = 0; i < parent.childCount; i++)
                {
                    parent.GetChild(i).position = pos;
                    pos.x += xPind;
                }
            }
        }

#endif

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