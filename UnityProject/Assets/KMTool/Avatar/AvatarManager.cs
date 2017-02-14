/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2017-02-14     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using System.Collections.Generic;

namespace KMTool
{

    /// <summary>
    /// 角色全局管理器
    /// </summary>
    public class AvatarManager : Singleton<AvatarManager>
    {
        /// <summary>
        /// 所有的模板的数组
        /// </summary>
        public List<ModelAvatar> models = new List<ModelAvatar>();

        protected override void Init()
        {
            int[] ids = StaticAvatar.Instance().allID;

            for (int i = 0; i < ids.Length; i++)
            {
                ModelAvatar m = new ModelAvatar();
                m.SetTemplateID(ids[i]);
                models.Add(m);
            }
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

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