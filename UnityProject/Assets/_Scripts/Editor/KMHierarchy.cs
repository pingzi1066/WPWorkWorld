/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2016-03-30     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

/// <summary>
/// Hierarchy 外观控制显示
/// </summary>
[InitializeOnLoad]
public class KMHierarchy
{

    static KMHierarchy()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HierarchyItemCB;
    }

    private static void HierarchyItemCB(int instanceID, Rect selectionRect)
    {
        var go = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
        if (go == null) return;

        //Texture2D tex = KMGUI.blankTexture; //GetTexture2D("eye");
        Rect eyeRect = selectionRect.H_CR();

        GUI.DrawTexture(eyeRect, GetTexture2D("eye"));
        if (GUI.Button(eyeRect, "", EditorStyles.label))
        {
            Debug.Log("Eye to me!");
        }

        Rect childRect = selectionRect.H_CR(2, 0).H_Size(24);
        GUI.Label(childRect, "" + go.transform.childCount, EditorStyles.label);
        if (GUI.Button(childRect, "", EditorStyles.label))
        {
            Debug.Log("child count for button");
        }
    }

    public static Texture2D GetTexture2D(string id)
    {
        Texture2D result;

        string path = "Assets/_Scripts/Editor/Images/" + id + ".png";

        var ba = File.ReadAllBytes(path);
        result = new Texture2D(4, 4, TextureFormat.ARGB32, false) { hideFlags = HideFlags.HideAndDontSave };
        result.LoadImage(ba);

        return result;
    }
}