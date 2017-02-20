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
        public Vector3 worldPos { get { return transform.position; } }

        [SerializeField] protected Text textName;
        [SerializeField] protected Text textInfo;
        [SerializeField] protected bool showRes = true;
        [SerializeField] protected GameObject mParentRes;

        protected GameObject parentRes
        {
            get
            {
                if (mParentRes != null) return mParentRes;
                return gameObject;
            }
        }

        [SerializeField] protected Button button;

        /// <summary>
        /// 3D资源或者2D资源，用于角色的展示
        /// </summary>
        [SerializeField][DisableEdit] protected GameObject curRes;
        [SerializeField][DisableEdit] protected Texture resTexture;
        [SerializeField][DisableEdit] protected Renderer mRedRend;
        protected Renderer resRend
        {
            get
            {
                if(mRedRend) return mRedRend;
                if (curRes)
                {
                    mRedRend = curRes.GetComponentInChildren<Renderer>();
                    if (mRedRend)
                    {
                        resTexture = mRedRend.material.mainTexture;
                    }
                }
                return mRedRend;
            }
        }

        public ModelAvatar mModel;
        const string DefaultShader = "Standard";
        const string GrayShader = "Custom/Gray";

        public delegate void DelOnClick(UIAvatarItem item);
        protected DelOnClick eventOnClick;

        public virtual void Init(ModelAvatar model, DelOnClick onClick = null)
        {
            mModel = model;
            eventOnClick = onClick;

            if (mModel != null)
            {
                gameObject.name = "Avatar_" + model.templateID + model.Name;

                if (curRes) Destroy(curRes);

                if (showRes)
                {
                    GameObject prefab = model.GetUIPrefab();
                    if (prefab)
                    {
                        curRes = KMTools.AddGameObj(parentRes, prefab, true);
                    }
                }
                KMUITools.SetText(textName, model.Name);
                KMUITools.SetText(textInfo, model.Info);
                Refresh();
            }
            else
            {
                SetNull();
            }
        }

        /// <summary>
        /// 这刷新 一般和本地持久数据挂接
        /// </summary>
        public virtual void Refresh()
        {
            if(mModel != null)
                SetUnlock(mModel.IsUnlock());
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
            if (UIAvatarCtrl.instance.isUnlockEffect && resRend)
            {
                resRend.material.shader = Shader.Find(isUnlock ? DefaultShader : GrayShader);
                if(resRend.material.mainTexture == null && resTexture)
                    resRend.material.mainTexture = resTexture;
            }
        }

        /// <summary>
        /// 设置为当前
        /// </summary>
        public virtual void SetCurrent()
        {
            //set to current 
            //Debug.Log("todo set current ", gameObject);
        }

        /// <summary>
        /// 重置
        /// </summary>
        public virtual void ResetCurrent()
        {
            //reset to current
            //Debug.Log("todo reset current ", gameObject);
        }

        public virtual void BtnEvent()
        {
            if(eventOnClick!= null) eventOnClick(this);
        }

        public void SetResScale(float size)
        {
            if(parentRes)
            {
                parentRes.transform.localScale = new Vector3(size,size, size);
            }
        }

        /// <summary>
        /// 是否已经解锁
        /// </summary>
        /// <returns></returns>
        public virtual bool IsUnlock()
        {
            if (mModel != null)
                return mModel.IsUnlock();

            return false;
        }

        /// <summary>
        /// 是否当前选择
        /// </summary>
        /// <returns></returns>
        public virtual bool IsCurrentSelect()
        {
            if(mModel != null)
            {
                return AvatarData.instance.GetData(E_AvatarData.curAvatarId) == mModel.templateID;
            }

            return false;
        }

        // Use this for initialization
        protected virtual void Start()
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