using UnityEngine;
using System.Collections;

using UnityEditor;
using KMTool;

[CustomEditor(typeof(Test))]
public class Ins_Test : Editor
{

    Test test;

    void OnEnable()
    {
        test = target as Test;
    }

    public override void OnInspectorGUI()
    {
        Ins_Default();
    }

    void Ins_Default()
    {


        if (KMGUI.DrawHeader("DrawHeader"))
        {

            KMGUI.BeginContents();

            GUILayout.Label("in contents");

            KMGUI.EndContents();
            GUILayout.Label("DrawHeader");
        }

        GUIStyle style = new GUIStyle();
        style.name = "Float";
        Texture2D selectedBoxColour = new Texture2D(1, 1);
        selectedBoxColour.SetPixel(0, 0, test.color);
        selectedBoxColour.Apply();


        style.normal.background = selectedBoxColour;

        EditorGUILayout.BeginVertical(style, GUILayout.MinHeight(10f));

        GUILayout.Label("AS TextArea");
        GUILayout.Label("TextArea");

        EditorGUILayout.EndVertical();


        GUILayout.Label("DrawSeparator");

        KMGUI.DrawSeparator();

        if (KMGUI.Button("Add Button", Color.green))
        {
            Debug.Log("Add");
        }

        if (KMGUI.BtnDelete())
        {
            Debug.Log("del");
        }
    }

    void OnSceneGUI()
    {
        if (test.target)
            KMSceneEditor.DrawAxis(test.target);
    }
}
