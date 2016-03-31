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

        GUI.DrawTexture(selectionRect.H_CR(), GetTexture2D("eye"));
        KMGUI.xMiniButton(selectionRect.H_CR(2, 10).H_Size(24), "11", true, .5f, false);

        //string text = " \nx: " + selectionRect.x + " y: " + selectionRect.y + " width: " + selectionRect.width + " height: " + selectionRect.height;
        //Debug.Log(go.name + selectionRect.position + "  " + selectionRect.center + "   " + selectionRect.size + text, go);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public static Texture2D GetTexture2D(string id)
    {
        Texture2D result;
        //var dirList = Directory.GetDirectories("Assets", "_Script", SearchOption.AllDirectories);
        string path = "Assets/_Scripts/Editor/Images/" + id + ".png";

        var ba = File.ReadAllBytes(path);
        result = new Texture2D(4, 4, TextureFormat.ARGB32, false) { hideFlags = HideFlags.HideAndDontSave };
        result.LoadImage(ba);

        return result;
    }
}