/******************************************************************************
 *
 * Maintaince Logs:
 * 2014-11-23    WP   Initial version. 
 *
 * *****************************************************************************/

using UnityEngine;
using UnityEditor;
using System.Collections;

public class KMSceneEditor
{
    /// <summary>
    /// 去到屏幕的某一点
    /// </summary>
    /// <param name="position"></param>
    public static void GotoScenePoint(Vector3 position)
    {
        Object[] intialFocus = Selection.objects;
        GameObject tempFocusView = new GameObject("Temp Focus View");
        tempFocusView.transform.position = position;
        try
        {
            Selection.objects = new Object[] { tempFocusView };
            SceneView.lastActiveSceneView.FrameSelected();
            Selection.objects = intialFocus;
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.ToString());
        }
        Object.DestroyImmediate(tempFocusView);
    }
}
