using UnityEngine;
using UnityEditor;
using System.Collections;

/// <summary>
/// Maintaince Logs:
/// 2014-12-05  WP      Initial version. 
/// </summary>

public class WayPointMenu : EditorWindow
{

    [MenuItem("GameObject/Create New Way Point", false, 3)]
    public static void CreatePath()
    {
        GameObject newPath = new GameObject("New Way");
        newPath.AddComponent<WayController>();
        newPath.AddComponent<WayPointBezier>();
    }
}
