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
    /// 实际上是用于Ren Texture 渲染的要摄像机
    /// </summary>
    public partial class UIRTAvatarCtrl : MonoBehaviour
    {
        /// <summary>
        /// 所有物品的父对象
        /// </summary>
        [SerializeField]
        private Transform parent;

        [SerializeField]
        private float xPind = 5;

        [SerializeField]
        [DisableEdit]
        private int curSelectIndex;
        public Vector3 centerPos
        {
            get
            {
                return parent.position;
            }
        }

        public static UIRTAvatarCtrl instance;

        void Awake()
        {
            instance = this;
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