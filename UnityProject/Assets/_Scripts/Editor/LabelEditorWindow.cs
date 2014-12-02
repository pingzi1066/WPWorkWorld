using UnityEngine;
using UnityEditor;
using System.Collections;

/// <summary>
/// 提供 编辑器 unity gameobject label 
/// 
/// Maintaince Logs:
/// 2014-12-02  WP      Initial version
/// </summary>
public class LabelEditorWindow : EditorWindow
{

    private const string saveKey = "KMLabel";

    /// <summary>
    /// 系统保存值
    /// </summary>
    private static string value = "";

    void OnGUI()
    {
        Object[] selects = Selection.objects;
        

        if (selects.Length == 0)
        {
            GUILayout.Label(StrsEditor.TIP_SELECT_OBJ);
            return;
        }
        else if (!AssetDatabase.Contains(selects[0])) 
        {
            GUILayout.Label(StrsEditor.TIP_SELECT_PROJECT_OBJ);
            return;
        }

        GUILayout.Label(StrsEditor.DESC_LABEL);
        value = GUILayout.TextField(value);

        GUILayout.Space(10f);

        if (GUILayout.Button(StrsEditor.BTN_SetLabel) && !string.IsNullOrEmpty(value))
        {
            string[] labels = new string[] { value };
            AddLabelToPrefab(labels);
            Save();
        }

        if (GUILayout.Button(StrsEditor.BTN_ClearLabel))
        {
            ClearLabelsForPrefab();
        }
    }

    static void Save()
    {
        EditorPrefs.SetString(saveKey, value);
    }

    static void Load()
    {
        value = EditorPrefs.GetString(saveKey, "");
    }

    [MenuItem(StrsEditor.MENU_LABEL)]
    static void OpenWindow()
    {
        EditorWindow.GetWindow<LabelEditorWindow>(false, "Label Editor", true).Show();
        Load();
    }

    static void ClearLabelsForPrefab()
    {
        Object[] selects = Selection.objects;
        foreach (Object obj in selects)
        {
            AssetDatabase.ClearLabels(obj);
        }
        AssetDatabase.Refresh();
    }

    static void AddLabelToPrefab(string[] labels)
    {
        Object[] selects = Selection.objects;
        foreach (Object obj in selects)
        {
            AssetDatabase.SetLabels(obj, labels);
        }
        AssetDatabase.Refresh();
    }
}
