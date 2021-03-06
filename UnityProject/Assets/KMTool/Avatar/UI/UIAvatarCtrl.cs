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
    /// 由于UGUI Scroll Rect （ScrollView） 的一此限制，需要手动的设置中一些参数，来达到最佳的体验效果
    /// 也必须遵守一些规则，来适应此脚本
    /// 1、Scroll View 和 View Point 默认中心点，（如果没有特定的去改变这个值，默认就是正确的）。
    /// 2、Content物品上加入了两个脚本：Horizontal Layer Group 和 Content Size Fitter 两个
    ///     前者是用来自动对齐物品的坐标，并且预留滑动条两侧的距离（如果没有这个距离，最边上的两个物品将无法在中间显示）
    ///     后者是用来自动适应Content的大小，这对动态加载物品很有帮助
    /// </summary>
    public class UIAvatarCtrl : MonoBehaviour
    {
        [SerializeField]private UIAvatarScroll scroll;
        [SerializeField]private UIAvatarItem prefabItem;
        [SerializeField]private AnimationCurve scrollKey;
        /// <summary>
        /// 当启用时选择到当前
        /// </summary>
        [SerializeField]private bool isSelectOnEnable = true;

        [SerializeField]private float minDelta = 0.05f;
        [SerializeField][Range(0.005f, 1)]private float minScrollDis = 0.01f;
        [SerializeField][DisableEdit]private bool isScrolling;
        [SerializeField][DisableEdit]private float moveDelta;
        [SerializeField][DisableEdit]private Vector2 preScrollPos;

        [SerializeField][DisableEdit]private UIAvatarItem mItem;
        [SerializeField][DisableEdit]private float fromPosX;
        [SerializeField][DisableEdit]private float toPosX;
        [SerializeField][DisableEdit]private float keyParam = 0;
        [SerializeField][DisableEdit]private Vector3 centerPos;
        private List<UIAvatarItem> items = new List<UIAvatarItem>();

        [SerializeField]private bool isCenterEffect = true;
        [SerializeField][Range(1, 5)]private float itemEffectSize = 1.4f;
        [SerializeField][Range(0.1f, 20)]private float minDis = 5;
        public bool isUnlockEffect = true;

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

        [SerializeField] private UIAvatarItem templateItem;

        /// <summary>
        /// 当前所展示的物品
        /// </summary>
        private UIAvatarItem curShowItem
        {
            get { return mItem; }
            set
            {
                if (mItem != value)
                {
                    if (mItem != null) mItem.ResetCurrent();

                    mItem = value;
                    mItem.SetCurrent();

                    RefreshByCurrItem();
                    if(templateItem != null) templateItem.Init(mItem.mModel);
                }
            }
        }
        /// <summary>
        /// 当前所选择的对象
        /// </summary>
        [SerializeField][DisableEdit]private UIAvatarItem curSelectedItem;

        [SerializeField]private Button btnBuy;
        [SerializeField]private Button btnSelect;
        [SerializeField]private Button btnCurrent;

        public static UIAvatarCtrl instance;

        void Awake()
        {
            instance = this;

            List<ModelAvatar> models = AvatarManager.instance.models;
            int curId = AvatarData.instance.GetData(E_AvatarData.curAvatarId);

            for (int i = 0; i < models.Count; i++)
            {
                UIAvatarItem item = KMTools.AddChild(parentContent.gameObject, prefabItem, false);
                item.Init(models[i],SetShowItem);
                items.Add(item);

                //scroll view to this model
                if (curId == models[i].templateID)
                {
                    curShowItem = item;
                }
            }

            prefabItem.gameObject.SetActive(false);
            //Scroll to item
            Invoke("ScrollToCurItem", .1f);

            scroll.onValueChanged.AddListener(OnScroll);
            centerPos = scroll.transform.position;

            if (btnBuy) btnBuy.onClick.AddListener(BtnBuy);
            if (btnSelect) btnSelect.onClick.AddListener(BtnSelect);
            if (btnCurrent) btnCurrent.onClick.AddListener(BtnCurrent);

            //scroll.movementType = ScrollRect.MovementType.Unrestricted;
        }

        void OnEnable()
        {
            if(isSelectOnEnable && curSelectedItem)
            {
                SetShowItem(curSelectedItem);
            }
        }

        private void OnScroll(Vector2 pos)
        {
            if (isCenterEffect)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    UIAvatarItem item = items[i];

                    float dis = Mathf.Abs(centerPos.x - item.worldPos.x);

                    float size = Mathf.Lerp(1, itemEffectSize, 1 - dis / minDis);
                    item.SetResScale(size);
                }
            }

            //move speed 
            if(!isScrolling && !scroll.isDraging)
            {
                moveDelta = Vector3.Distance(preScrollPos, pos);
                preScrollPos = pos;

                if(moveDelta < minDelta)
                {
                    ToCenter();
                }
            }
        }

        public virtual void OnCenterByDragEnd(float dis)
        {
            if(dis < 1.5f && !isScrolling)
            {
                ToCenter();
            }
        }

        /// <summary>
        /// 吸附
        /// </summary>
        protected virtual void ToCenter()
        {
            float minDis = 1000;
            UIAvatarItem item = null;
            for (int i = 0; i < items.Count; i++)
            {
                float curDis = Vector3.Distance(items[i].worldPos, centerPos);
                if (curDis < minDis)
                {
                    minDis = curDis;
                    item = items[i];
                }
            }
            if (item != null)
            {
                SetShowItem(item);
            }
        }

        public void SetShowItem(UIAvatarItem item)
        {
            if(isScrolling) return;
            curShowItem = item;
            ScrollToCurItem();
        }

        private void ScrollToCurItem()
        {
            ScrollToItem(curShowItem);
        }

        private void ScrollToItem(UIAvatarItem item)
        {
            keyParam = 0;
            fromPosX = parentContent.position.x;
            toPosX = fromPosX + centerPos.x - item.transform.position.x;
            
            float moveDis = toPosX - fromPosX;
            if (Mathf.Abs(moveDis) < minScrollDis)
            {
                parentContent.Translate(moveDis, 0, 0);
                scroll.StopMovement();
            }
            else
            {
                StartCoroutine(ScrollToPos());
            }
        }

        private IEnumerator ScrollToPos()
        {
            scroll.StopMovement();
            moveDelta = 100;
            
            isScrolling  = true;
            //当前Curve 的 value 如果超过了1或者低于0，有相应数对应
            yield return new WaitForEndOfFrame();
            int length = scrollKey.length;
            float time = scrollKey[length - 1].time;

            float moveDis  = toPosX - fromPosX;
            //记录开始的位置 移动的方向
            Vector3 startPos = parentContent.position;
            Vector3 dir = parentContent.right;

            while (keyParam < time)
            {
                keyParam += Time.deltaTime;
                float factor = scrollKey.Evaluate(keyParam);
                Vector3 toPos = startPos;
                toPos += (moveDis * factor) * dir;

                parentContent.transform.position = toPos;//Vector3.Lerp(fromPos, toPos, factor);
                yield return new WaitForEndOfFrame();
            }

            //强制移动
            fromPosX = parentContent.position.x;
            toPosX = fromPosX + centerPos.x - curShowItem.transform.position.x;
            moveDis = toPosX - fromPosX;
            parentContent.Translate(moveDis, 0, 0);

            isScrolling = false;
            scroll.StopMovement();
        }

        private void RefreshByCurrItem()
        {
            if (mItem.IsCurrentSelect())
            {
                curSelectedItem = mItem;

                if (btnCurrent) btnCurrent.gameObject.SetActive(true);
                if (btnBuy) btnBuy.gameObject.SetActive(false);
                if (btnSelect) btnSelect.gameObject.SetActive(false);
            }
            else if (mItem.IsUnlock())
            {
                if (btnCurrent) btnCurrent.gameObject.SetActive(false);
                if (btnBuy) btnBuy.gameObject.SetActive(false);
                if (btnSelect) btnSelect.gameObject.SetActive(true);
            }
            else
            {
                if (btnCurrent) btnCurrent.gameObject.SetActive(false);
                if (btnBuy) btnBuy.gameObject.SetActive(true);
                if (btnSelect) btnSelect.gameObject.SetActive(false);
            }
        }

        public void BtnNextItem()
        {
            int index = items.IndexOf(curShowItem);

            if (index + 1 < items.Count)
            {
                SetShowItem(items[index + 1]);
            }
        }

        public void BtnPrevItem()
        {
            int index = items.IndexOf(curShowItem);

            if (index > 0)
            {
                SetShowItem(items[index - 1]);
            }
        }

        public void BtnBuy()
        {
            if (curShowItem)
            {
                AvatarManager.instance.BuyAvatar(curShowItem.mModel,BuySuccess,BuyFail);
            }
        }

        public void BtnSelect()
        {
            if (curShowItem)
            {
                AvatarManager.instance.SelectAvatar(curShowItem.mModel);

                RefreshByCurrItem();
            }
        }

        public void BtnCurrent()
        {
            //TODO: you code

        }

        private void BuySuccess(ModelAvatar model)
        {
            Debug.Log("Buy Success!!!");
            RefreshByCurrItem();
            curShowItem.Refresh();
        }

        private void BuyFail(AvatarManager.BuyFailType tp)
        {
            Debug.Log("buy is fail " + tp);
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