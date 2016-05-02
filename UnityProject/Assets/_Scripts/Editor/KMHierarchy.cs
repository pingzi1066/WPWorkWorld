/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2016-03-30     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
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

        DrawChildInfo(go, selectionRect);
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

//    public static void xSetFlag(this Object go, HideFlags flag, bool value, string undoName = null) {
//        if (go == null)
//            return;
//        if (!string.IsNullOrEmpty(undoName))
//            Undo.RecordObject(go, undoName);
//        if (value)
//            go.hideFlags |= flag;
//        else
//            go.hideFlags &= ~flag;
//    }

    public static void DrawChildInfo(GameObject go,Rect selectRect)
    {
        int childCount = go.transform.childCount;
        if (childCount > 0)
        {

            Rect childRect = selectRect.H_CR(2, 6).H_Size(24);
//            GUI.Label(childRect, "" + childCount, EditorStyles.label);

            //取子对象是否隐藏
            bool isShow = (int)(go.transform.GetChild(0).hideFlags & HideFlags.HideInHierarchy) > 0;

            if (isShow)
                childRect = selectRect.H_CR(2, 8).H_Size(22);

            if (GUI.Button(childRect, childCount.ToString(), isShow ? EditorStyles.miniButton : EditorStyles.label))
            {
                bool isAdd = !isShow;
                SetChildrenFlag(go, HideFlags.HideInHierarchy, isAdd);

                Debug.Log("the gameobje is " + (isShow ? "show" : "hide") + " and set is ");
            }
        }
        else
        {
            //
        }
    }

    private static void SetChildrenFlag(GameObject go,HideFlags flag,bool isAdd)
    {
        foreach (Transform t in go.transform)
        {
            if (isAdd)
                t.gameObject.hideFlags |= flag;
            else
                t.gameObject.hideFlags &= ~flag;
        }

        //Work on 
        bool old = go.activeSelf;

        go.SetActive(!old);
        go.SetActive(old);
    }
}