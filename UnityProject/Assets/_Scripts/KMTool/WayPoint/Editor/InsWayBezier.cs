using UnityEditor;
using UnityEngine;
using System.Collections;

namespace KMTool
{
    /// <summary>
    /// Maintaince Logs:
    /// 2014-12-05  WP      Initial version. 
    ///         07  WP      加入Smooth All，修改一些注释
    /// </summary>
    [CustomEditor(typeof(WayBezier))]
    public class InsWayBezier : Editor
    {

        Vector2 scrollPosition;
        private WayPoint bezierControlPoint;

        public void OnSceneGUI()
        {
            WayBezier bezier = (WayBezier)target;

            if (GUI.changed)
            {
                bezier.RecalculateStoredValues();
                EditorUtility.SetDirty(bezier);
            }
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.indentLevel = 0;

            WayBezier bezier = (WayBezier)target;
            int numberOfControlPoints = bezier.numberOfControlPoints;

            if (numberOfControlPoints > 0)
            {
                if (numberOfControlPoints > 1)
                {
                    GUILayout.Space(10);
                    bezier.lineColour = EditorGUILayout.ColorField("Line Colour", bezier.lineColour);
                    GUILayout.Space(5);

                    bezier.mode = (WayBezier.viewmodes)EditorGUILayout.EnumPopup("Mode:", bezier.mode);
                    GUILayout.Space(5);

                    if (bezier.mode == WayBezier.viewmodes.target)
                    {
                        if (bezier.target == null)
                            EditorGUILayout.HelpBox("No target has been specified in the bezier path", MessageType.Warning);
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Look at Target");
                        bezier.target = (Transform)EditorGUILayout.ObjectField(bezier.target, typeof(Transform), true);
                        EditorGUILayout.EndHorizontal();
                        GUILayout.Space(5);
                    }

                    bool newValue = EditorGUILayout.Toggle("Loop", bezier.loop);
                    if (newValue != bezier.loop)
                    {
                        Undo.RecordObject(bezier, "Set Loop");
                        bezier.loop = newValue;
                        bezier.RecalculateStoredValues();
                    }

                }
                else
                {
                    if (numberOfControlPoints == 1)
                        EditorGUILayout.HelpBox("Click Add New Point to add additional points, you need at least two points to make a path.", MessageType.Info);
                }

                GUILayout.Space(5);

                GUILayout.BeginHorizontal();

                if (GUILayout.Button("Smooth All"))
                {
                    if (EditorUtility.DisplayDialog("圆滑处理？", "圆滑所有的点的旋转", "确定", "取消"))
                    {
                        foreach (WayPoint wp in bezier.GetPoints())
                        {
                            wp.SetRotationToCurve();
                        }

                        Undo.RegisterCompleteObjectUndo(bezier.GetPoints(), "Smooth Path");
                    }
                }

                if (GUILayout.Button("Reset Path"))
                {
                    if (EditorUtility.DisplayDialog("Resetting path?", "Are you sure you want to delete all control points?", "Delete", "Cancel"))
                    {
                        Undo.RegisterCompleteObjectUndo(bezier.GetPoints(), "Reset Path");
                        bezier.ResetPath();
                        return;
                    }
                }

                GUILayout.EndHorizontal();

                KMGUI.DarwLine(10, 3);

                if (KMGUI.Button("Add New Point At Head", Color.green)) //if (GUILayout.Button("Add New Point At End"))
                {
                    Undo.RecordObject(bezier.AddNewPoint(0), "Create a new way point");

                    EditorUtility.SetDirty(bezier);
//                    Debug.Log(Screen.width - 20);
                }

                EditorGUIUtility.labelWidth = 50;

                for (int i = 0; i < numberOfControlPoints; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    WayPoint go = bezier.controlPoints[i];

                    if (go == null)
                        go = (WayPoint)EditorGUILayout.ObjectField(null, typeof(WayPoint), true);
                    else
                        go = (WayPoint)EditorGUILayout.ObjectField("No." + (i + 1), go, typeof(WayPoint), true);


                    //if (GUILayout.Button("Delete"))
                    if (KMGUI.Button("Delete", Color.red, GUILayout.MaxWidth(50)))
                    {
                        Undo.RecordObject(bezier.GetPoints()[i], "Delete way point");
                        bezier.DeletePoint(i, true);
                        numberOfControlPoints = bezier.numberOfControlPoints;
                        EditorUtility.SetDirty(bezier);
                        return;
                    }

                    EditorGUILayout.EndHorizontal();

                    string btnText = "Add New Point At End";
                    if (i != numberOfControlPoints - 1)
                    {
                        btnText = "Add New Point Between";
                    }

                    //if (GUILayout.Button())
                    if (KMGUI.Button(btnText, Color.green))
                    {
                        Undo.RecordObject(bezier.AddNewPoint(i + 1), "Create a new way point");
                        //bezier.AddNewPoint(i + 1);
                        EditorUtility.SetDirty(bezier);
                    }


                    KMGUI.DarwLine();
                }
                //EditorGUILayout.EndScrollView();
            }
            else
            {
                if (KMGUI.Button("Add New Point At End", Color.green)) //if (GUILayout.Button("Add New Point At End"))
                {
                    Undo.RecordObject(bezier.AddNewPoint(), "Create a way point");

                    EditorUtility.SetDirty(bezier);
                }
                EditorGUILayout.HelpBox("Click Add New Point to add points and begin the path, you will need two points to create a path.", MessageType.Info);
            }

            if (GUI.changed)
            {
                bezier.RecalculateStoredValues();
                EditorUtility.SetDirty(bezier);
            }
        }

    }
}