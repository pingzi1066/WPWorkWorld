/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2016-11-27     WP      Initial version
 * 
 * *****************************************************************************/

using KMTool;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace KMToolDemo
{
    /// <summary>
    /// 描述
    /// </summary>
    public class Demo_CameraShake : MonoBehaviour 
    {
        public CameraShakeCtrl.ShakeParams parms;

        [SerializeField] private Text textInfo;

        // Use this for initialization
        void Start()
        {
            parms = new CameraShakeCtrl.ShakeParams();
            textInfo.text = "";
        }

        public void BtnShake()
        {
            CameraShakeCtrl.Shake(parms,ShakeFinished);
            textInfo.text = "Shaking " + parms.name;
        }

        private void ShakeFinished()
        {
            textInfo.text = "Shake finished!!!";
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