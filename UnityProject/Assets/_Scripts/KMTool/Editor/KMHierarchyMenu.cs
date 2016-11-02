/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2016-05-05     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using System.Collections;
using UnityEditor;

namespace KMTool
{
    /// <summary>
    /// 层级菜单
    /// </summary>
    public partial class KMHierarchy
    {
        /// <summary>
        /// 当前所选对象
        /// </summary>
        static private GameObject curSelectObj;

        /// <summary>
        /// 复制的坐标
        /// </summary>
        static private Vector3 position = Vector3.zero;

        /// <summary>
        /// 菜单绘制
        /// </summary>
        private static void DrawMenu(GameObject go, Rect selectRect)
        {
            var e = Event.current;

            if (e.type == EventType.mouseDown && e.button == 1 && selectRect.Contains(e.mousePosition))
            {
                //Debug.Log("Right click me ", go);
                //var menu = new GenericMenu();
                //menu.AddItem(new GUIContent("Test"), false, Test);
                //menu.ShowAsContext();
                // 设置当前右键选择的对象
                curSelectObj = go;
            }
        }

        [MenuItem("GameObject/TransForm/Copy Position", false, 3)]
        static void CopyPosition()
        {
            if (curSelectObj)
            {
                position = curSelectObj.transform.position;
            }
        }

        [MenuItem("GameObject/TransForm/Paste Position", false, 3)]
        static void PausePosition()
        {
            if (curSelectObj)
            {
                curSelectObj.transform.position = position;
            }
        }
    }
}