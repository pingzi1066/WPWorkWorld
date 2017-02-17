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

        private ModelAvatar curBuyModel;

        public enum BuyFailType
        {
            Busy,
            NoMoney,
            AvatarError,
        }

        public delegate void DelOnBuy(ModelAvatar model);
        private DelOnBuy eventOnBuySuccess; 
        public delegate void DelOnFail(BuyFailType tp);
        private DelOnFail eventOnBuyFail;


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

        public void BuyAvatar(ModelAvatar model,DelOnBuy success = null, DelOnFail fail = null)
        {
            if(curBuyModel != null)
            {
                //Don't set 
                if(fail != null)
                    fail(BuyFailType.Busy);
                return;
            }

            if(model != null)
            {
                curBuyModel = model;
                eventOnBuySuccess = success;
                eventOnBuyFail = fail;

                if(CanBuy(model))
                {
                    BuySuccess(model);
                }
                else
                {
                    BuyFail(BuyFailType.NoMoney);
                }
            }
        }

        public bool CanBuy(ModelAvatar model)
        {
            //TODO you code

            return true;
        }

        private void BuySuccess(ModelAvatar model)
        {
            //Todo you code
            AvatarListData.instance.AddItem(E_AvatarList.UnlockIds,model.templateID);
            AvatarListData.instance.SaveData();

            if(eventOnBuySuccess != null)
            {
                eventOnBuySuccess(curBuyModel);
            }
            curBuyModel = null;
            eventOnBuySuccess = null;
        }

        private void BuyFail(BuyFailType tp)
        {
            if(eventOnBuyFail  != null)
                eventOnBuyFail(tp);

            eventOnBuyFail = null;
            curBuyModel = null;
        }

        public void SelectAvatar(ModelAvatar model)
        {
            if(model != null)
            {
                AvatarData.instance.SetData(E_AvatarData.curAvatarId,model.templateID);
                AvatarData.instance.SaveData();
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