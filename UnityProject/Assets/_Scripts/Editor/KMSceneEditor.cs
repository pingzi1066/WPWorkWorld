/******************************************************************************
 *
 * Maintaince Logs:
 * 2014-11-23   WP      Initial version. 
 * 2015-01-09   WP      加入画可以控制坐标轴
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

    public static void DrawAxis(Transform target)
    {
        if (target == null) return;
        Vector3 newPosition = target.position;
        float handlesize = HandleUtility.GetHandleSize(newPosition) * 0.6f;

        Handles.color = new Color(0.99f, 0.50f, 0.35f);
        newPosition = Handles.Slider(newPosition, Vector3.right, handlesize, Handles.ArrowCap, 1);
        Handles.color = new Color(0.30f, 0.85f, 0.99f);
        newPosition = Handles.Slider(newPosition, Vector3.forward, handlesize, Handles.ArrowCap, 1);
        Handles.color = new Color(168/255.0f, 242/255.0f, 77/255.0f);
        newPosition = Handles.Slider(newPosition, Vector3.up, handlesize, Handles.ArrowCap, 1);

        string handleName = target.name;
        float center = HandleUtility.GetHandleSize(newPosition);

        GUIStyle style = new GUIStyle();
        style.active.textColor = Color.gray;
        style.normal.textColor = Color.green;
        style.onNormal.textColor = Color.yellow;
        style.alignment  = TextAnchor.MiddleCenter;

        Handles.Label(target.position + Vector3.up * center
            , handleName, style);

        if (newPosition != target.position)
        {
            target.position = newPosition;
            EditorUtility.SetDirty(target);
        }
    }

    public static void DrawAxis(params Transform[] trans)
    {
        if (trans == null)
            return;
        
        foreach (Transform target in trans)
        {
            if (target == null)
                continue;
            
            Vector3 newPosition = target.position;
            float handlesize = HandleUtility.GetHandleSize(newPosition) * 0.6f;

            Handles.color = new Color(0.99f, 0.50f, 0.35f);
            newPosition = Handles.Slider(newPosition, Vector3.right, handlesize, Handles.ArrowCap, 1);
            Handles.color = new Color(0.30f, 0.85f, 0.99f);
            newPosition = Handles.Slider(newPosition, Vector3.forward, handlesize, Handles.ArrowCap, 1);
            Handles.color = new Color(168/255.0f, 242/255.0f, 77/255.0f);
            newPosition = Handles.Slider(newPosition, Vector3.up, handlesize, Handles.ArrowCap, 1);

            if (newPosition != target.position)
            {
                target.position = newPosition;
                EditorUtility.SetDirty(target);
            }
        }
    }

    public static void DrawName(params Transform[] trans)
    {
        if (trans == null)
            return;

        foreach (Transform target in trans)
        {
            if (target == null)
                continue;

            Vector3 newPosition = target.position;
            string handleName = target.name;
            float center = HandleUtility.GetHandleSize(newPosition);

            GUIStyle style = new GUIStyle();
            style.active.textColor = Color.gray;
            style.normal.textColor = Color.green;
            style.onNormal.textColor = Color.yellow;
            style.alignment  = TextAnchor.MiddleCenter;

            Handles.Label(target.position + Vector3.up * center, handleName, style);
        }
    }
}
