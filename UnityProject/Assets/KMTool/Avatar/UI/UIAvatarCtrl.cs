/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2017-02-14     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

namespace KMTool
{
    /// <summary>
    /// 用于角色的UI展示、解锁、选择等入口
    /// </summary>
    public class UIAvatarCtrl : MonoBehaviour
    {
        [SerializeField]
        private ScrollRect scroll;

        private RectTransform parentContent
        {
            get
            {
                if (scroll == null)
                {
                    Debug.LogError("scroll is null");
                    return transform as RectTransform;
                }
                return scroll.content;
            }
        }

        [SerializeField]
        private UIAvatarItem prefabItem;

        private List<UIAvatarItem> items = new List<UIAvatarItem>();

        [SerializeField]
        [DisableEdit]
        private UIAvatarItem mItem;

        [SerializeField]
        private AnimationCurve scorllKey;
        
        [SerializeField]
        [DisableEdit]
        private Vector3 fromPos;
        [SerializeField]
        [DisableEdit]
        private Vector3 toPos;
        [SerializeField]
        [DisableEdit]
        private float keyParam = 0;

        /// <summary>
        /// 当前所展示的物品
        /// </summary>
        private UIAvatarItem curItem
        {
            get { return mItem; }
            set
            {
                if (mItem != value)
                {
                    if (mItem != null) mItem.ResetCurrent();

                    mItem = value;
                    mItem.SetCurrent();
                }
            }
        }

        public static UIAvatarCtrl instance;

        void Awake()
        {
            instance = this;

            List<ModelAvatar> models = AvatarManager.instance.models;
            int curId = AvatarData.instance.GetData(E_AvatarData.curAvatarId);

            if (prefabItem)
            {
                for (int i = 0; i < models.Count; i++)
                {
                    UIAvatarItem item = KMTools.AddChild(parentContent.gameObject, prefabItem);
                    item.Init(models[i]);
                    items.Add(item);

                    //scroll view to this model
                    if (curId == models[i].templateID)
                    {
                        curItem = item;
                        //Scorll to item
                    }
                }

                prefabItem.gameObject.SetActive(false);
                ScorllToItem(curItem);
            }
        }

        public void ScorllToItem(UIAvatarItem item)
        {
            keyParam = 0;
            fromPos = parentContent.position;
            toPos = fromPos;
            toPos.x += scroll.transform.position.x - item.transform.position.x;

            //Debug.Log("start x " + fromPos.x + "  topos x " + toPos.x);

            //if(scroll) scroll.
            StartCoroutine(ScorllToPos());
        }

        private IEnumerator ScorllToPos()
        {
            yield return new WaitForEndOfFrame();
            int length = scorllKey.length;
            float time = scorllKey[length - 1].time;
            while (keyParam < time)
            {
                yield return new WaitForEndOfFrame();
                keyParam += Time.deltaTime;
                float factor = scorllKey.Evaluate(keyParam);

                parentContent.transform.position = Vector3.Lerp(fromPos, toPos, factor);
            }

        }

        // Use this for initialization
        void Start()
        {

        }

        #region 测试

        public void KMDebug()
        {
            Debug.Log(" ---------KMDebug----------     " + parentContent.position.x, gameObject);

        }

        public void KMEditor()
        {
            Debug.Log(" ---------KMEditor----------", gameObject);
        }

        #endregion
    }
}