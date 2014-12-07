using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// List Demo
/// 
/// Maintaince Logs:
/// 2014-12-07  WP      Initial version. 
/// </summary>
[CustomEditor(typeof(Demo_List))]
public class Demo_InsList : Editor
{
    /// <summary>
    /// 所有元素展开开关
    /// </summary>
    public bool expand = true;

    public override void OnInspectorGUI()
    {
        var script = (Demo_List)target;

        this.expand = InsList.SerializedObjFoldOutList<Demo_Test_Ins_List>
            ("List的名字",
            script.demoList,
            this.expand,
            ref script._editorListItemStates,
            true
            );

        GUILayout.Button(new GUIContent("+", "click add "));

        // Flag Unity to save the changes to to the prefab to disk
        if (GUI.changed)
            EditorUtility.SetDirty(target);
    }
}
