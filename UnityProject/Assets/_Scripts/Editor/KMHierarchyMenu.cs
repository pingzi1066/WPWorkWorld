/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2016-05-05     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// 层级菜单
/// </summary>
public partial class KMHierarchy
{
    /// <summary>
    /// 菜单绘制
    /// </summary>
    private static void DrawMenu(GameObject go, Rect selectRect)
    {
        ////menu.AddSeparator("");

        var e = Event.current;

        if (e.type == EventType.mouseDown && e.button == 1 && selectRect.Contains(e.mousePosition))
        {
            Debug.Log("Right click me ", go);
            var menu = new GenericMenu();
            menu.AddItem(new GUIContent("Test"), false, Test);
            //menu.ShowAsContext();
        }
    }

    [MenuItem("GameObject/TransForm/Copy Position", false, 3)]
    static void Test() { }
}