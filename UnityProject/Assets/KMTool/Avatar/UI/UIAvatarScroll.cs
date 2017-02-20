/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2017-02-20     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace KMTool
{

    /// <summary>
    /// 用来判断是否还在拖动列表
    /// </summary>
    public class UIAvatarScroll : ScrollRect
    {
        public bool isDraging { private set; get; }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);

            isDraging = true;
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);

            isDraging = false;

            float dis = Vector2.Distance(Vector2.zero, velocity);
            UIAvatarCtrl.instance.OnCenterByDragEnd(dis);
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