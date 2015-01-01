using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

/// <summary>
/// 提供  窗口编辑器 unity navigation 设置 
/// 
/// Maintaince Logs:
/// 2015-01-01  WP      Initial version
/// </summary>
public class EW_Navigation : EditorWindow
{
    private StaticEditorFlags flags = StaticEditorFlags.BatchingStatic;

    private static bool[] userSels = new bool[Enum.GetValues(typeof(StaticEditorFlags)).Length];

    private const string MENU_LABEL = "Tools/" + "寻路";

    void OnGUI()
    {
        Array values = Enum.GetValues(typeof(StaticEditorFlags));

        GameObject[] selects = Selection.gameObjects;

        //EditorGUILayout.PropertyField(, new GUIContent("Event Mask"));
        int index = 0;
        StaticEditorFlags newFlags = 0;
        foreach (StaticEditorFlags s in values)
        {
            userSels[index] = EditorGUILayout.Toggle(s.ToString(), userSels[index]);
            if (newFlags == 0 && userSels[index])
            {
                newFlags = s;
            }
            else if (userSels[index])
            {
                newFlags |= s;
            }

            index++;
        }

        if (selects != null && selects.Length > 0)
        {
            if (GUILayout.Button("应用到对象"))

                foreach (GameObject go in selects)
                {
                    go.isStatic = true;
                    GameObjectUtility.SetStaticEditorFlags(go, newFlags);
                }


        }
    }

    [MenuItem(MENU_LABEL)]
    static void OpenWindow()
    {
        EditorWindow.GetWindow<EW_Navigation>(false, "Navigation Editor", true).Show();
    }
}
