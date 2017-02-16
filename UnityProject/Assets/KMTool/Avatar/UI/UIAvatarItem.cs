/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2017-02-16     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace KMTool
{
    /// <summary>
    /// 一个角色的UI对象
    /// </summary>
    public class UIAvatarItem : MonoBehaviour
    {
        //public enum ShowType
        //{
        //    ModelInUI,
        //    UI,
        //}

        //[SerializeField]
        //private ShowType showType = ShowType.UI;

        [SerializeField]
        private Text textName;
        [SerializeField]
        private Text textInfo;
        [SerializeField]
        private GameObject mParentRes;
        private GameObject parentRes
        {
            get
            {
                if (mParentRes != null) return mParentRes;
                return gameObject;
            }
        }

        [SerializeField]
        private Button button;

        /// <summary>
        /// 3D资源或者2D资源，用于角色的展示
        /// </summary>
        [SerializeField]
        [DisableEdit]
        private GameObject curRes;

        private ModelAvatar mModel;

        public void Init(ModelAvatar model)
        {
            mModel = model;

            if (mModel != null)
            {
                gameObject.name = "Avatar_" + model.templateID + model.Name;

                if (curRes) Destroy(curRes);
                GameObject prefab = model.GetUIPrefab();
                if (prefab)
                {
                    curRes = KMTools.AddGameObj(parentRes, prefab, true);
                }
                KMUITools.SetText(textName, model.Name);
                KMUITools.SetText(textInfo, model.Info);
                SetUnlock(model.IsUnlock());
            }
            else
            {
                SetNull();
            }
        }

        protected virtual void SetNull()
        {
            gameObject.name = "Avatar_Null";
            KMUITools.SetText(textName, "");
            KMUITools.SetText(textInfo, "");

            if (curRes) Destroy(curRes);
        }

        protected virtual void SetUnlock(bool isUnlock)
        {
            Debug.Log("set unlock todo this!");
        }

        /// <summary>
        /// 设置为当前
        /// </summary>
        public virtual void SetCurrent()
        {
            //set to current 
            Debug.Log("todo set current ", gameObject);
        }

        /// <summary>
        /// 重置
        /// </summary>
        public virtual void ResetCurrent()
        {
            //reset to current
            Debug.Log("todo reset current ", gameObject);
        }

        public virtual void BtnEvent()
        {
            if (mModel != null)
            {
                Debug.Log("click me!!", gameObject);
                UIAvatarCtrl.instance.ScorllToItem(this);
            }
        }

        // Use this for initialization
        void Start()
        {
            if (button) button.onClick.AddListener(BtnEvent);
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