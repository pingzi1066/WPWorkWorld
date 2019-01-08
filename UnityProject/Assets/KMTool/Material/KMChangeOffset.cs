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
    /// 改变UV 的 offset
    /// </summary>
    public class KMChangeOffset : MonoBehaviour
    {
        public Renderer[] rends;
        public Vector2 changeSpeed = Vector2.zero;
        public bool ignoreTimeScale = false;

        // Use this for initialization
        void Start()
        {
            if(rends.Length == 0)
                enabled = false;
        }

        // Update is called once per frame
        void Update()
        {
            if(changeSpeed != Vector2.zero)
            {
                Vector2 add = changeSpeed * (ignoreTimeScale ? KMTime.deltaTime : Time.deltaTime);
                for (int i = 0; i < rends.Length; i++)
                {
                    if(rends[i].material)
                        rends[i].material.mainTextureOffset += add;
                }
            }
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