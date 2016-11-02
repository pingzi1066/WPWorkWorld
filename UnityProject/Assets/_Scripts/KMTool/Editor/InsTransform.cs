using UnityEngine;
using UnityEditor;
using System.Collections;
using KMTool;

/// <summary>
/// 用于TransForm的Ins，参考了NGUI的方式
/// 
/// Maintaince Logs:
/// 2015-01-25	WP			Initial version. 
/// </summary>
[CanEditMultipleObjects]
[CustomEditor(typeof(Transform), true)]
public class InsTransform : Editor
{
    SerializedProperty mPos;
    SerializedProperty mRot;
    SerializedProperty mScale;

    static Vector3 copyPosition = Vector3.zero;
    static Vector3 copyRotation = Vector3.zero;
    static Vector3 copyScale = Vector3.one;

    static Vector3 copyLocalPosition = Vector3.zero;

    void OnEnable()
    {
        mPos = serializedObject.FindProperty("m_LocalPosition");
        mRot = serializedObject.FindProperty("m_LocalRotation");
        mScale = serializedObject.FindProperty("m_LocalScale");
    }

    /// <summary>
    /// Draw the inspector widget.
    /// </summary>

    public override void OnInspectorGUI()
    {
        EditorGUIUtility.labelWidth = 15;

        serializedObject.Update();


        DrawPosition();
        DrawRotation();
        DrawScale();

        DrawButtons();

        serializedObject.ApplyModifiedProperties();
    }

    void DrawPosition()
    {
        GUILayout.BeginHorizontal();
        {
            bool reset = GUILayout.Button("P", GUILayout.Width(20f));

            if (GUILayout.Button(new GUIContent("复制", "copy local position"), GUILayout.Width(40)))
            {
                copyLocalPosition = mPos.vector3Value;
            }

            if (GUILayout.Button(new GUIContent("粘贴", "paste local position"), GUILayout.Width(40)))
            {
                mPos.vector3Value = copyLocalPosition;
            }

            EditorGUILayout.PropertyField(mPos.FindPropertyRelative("x"));
            EditorGUILayout.PropertyField(mPos.FindPropertyRelative("y"));
            EditorGUILayout.PropertyField(mPos.FindPropertyRelative("z"));

            if (reset) mPos.vector3Value = Vector3.zero;
        }
        GUILayout.EndHorizontal();
    }

    void DrawScale()
    {
        GUILayout.BeginHorizontal();
        {
            bool reset = GUILayout.Button("S", GUILayout.Width(20f));

            EditorGUILayout.PropertyField(mScale.FindPropertyRelative("x"));
            EditorGUILayout.PropertyField(mScale.FindPropertyRelative("y"));
            EditorGUILayout.PropertyField(mScale.FindPropertyRelative("z"));

            if (reset) mScale.vector3Value = Vector3.one;
        }
        GUILayout.EndHorizontal();
    }

    void DrawButtons()
    {
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Copy Local Transform"))
        {
            copyPosition = mPos.vector3Value;
            copyRotation = (serializedObject.targetObject as Transform).localEulerAngles;
            copyScale = mScale.vector3Value;
        }

        if (KMGUI.Button("Paste Local Transform",Color.white))
        {
            mPos.vector3Value = copyPosition;
            (serializedObject.targetObject as Transform).localEulerAngles = copyRotation;
            mScale.vector3Value = copyScale;
        }

        GUILayout.EndHorizontal();
    }

    #region Rotation is ugly as hell... since there is no native support for quaternion property drawing
    enum Axes : int
    {
        None = 0,
        X = 1,
        Y = 2,
        Z = 4,
        All = 7,
    }

    Axes CheckDifference(Transform t, Vector3 original)
    {
        Vector3 next = t.localEulerAngles;

        Axes axes = Axes.None;

        if (Differs(next.x, original.x)) axes |= Axes.X;
        if (Differs(next.y, original.y)) axes |= Axes.Y;
        if (Differs(next.z, original.z)) axes |= Axes.Z;

        return axes;
    }

    Axes CheckDifference(SerializedProperty property)
    {
        Axes axes = Axes.None;

        if (property.hasMultipleDifferentValues)
        {
            Vector3 original = property.quaternionValue.eulerAngles;

            foreach (Object obj in serializedObject.targetObjects)
            {
                axes |= CheckDifference(obj as Transform, original);
                if (axes == Axes.All) break;
            }
        }
        return axes;
    }

    /// <summary>
    /// Draw an editable float field.
    /// </summary>
    /// <param name="hidden">Whether to replace the value with a dash</param>
    /// <param name="greyedOut">Whether the value should be greyed out or not</param>

    static bool FloatField(string name, ref float value, bool hidden, bool greyedOut, GUILayoutOption opt)
    {
        float newValue = value;
        GUI.changed = false;

        if (!hidden)
        {
            if (greyedOut)
            {
                GUI.color = new Color(0.7f, 0.7f, 0.7f);
                newValue = EditorGUILayout.FloatField(name, newValue, opt);
                GUI.color = Color.white;
            }
            else
            {
                newValue = EditorGUILayout.FloatField(name, newValue, opt);
            }
        }
        else if (greyedOut)
        {
            GUI.color = new Color(0.7f, 0.7f, 0.7f);
            float.TryParse(EditorGUILayout.TextField(name, "--", opt), out newValue);
            GUI.color = Color.white;
        }
        else
        {
            float.TryParse(EditorGUILayout.TextField(name, "--", opt), out newValue);
        }

        if (GUI.changed && Differs(newValue, value))
        {
            value = newValue;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Because Mathf.Approximately is too sensitive.
    /// </summary>

    static bool Differs(float a, float b) { return Mathf.Abs(a - b) > 0.0001f; }

    void DrawRotation()
    {
        GUILayout.BeginHorizontal();
        {
            bool reset = GUILayout.Button("R", GUILayout.Width(20f));

            Vector3 visible = (serializedObject.targetObject as Transform).localEulerAngles;

            Axes changed = CheckDifference(mRot);
            Axes altered = Axes.None;

            GUILayoutOption opt = GUILayout.MinWidth(30f);

            if (FloatField("X", ref visible.x, (changed & Axes.X) != 0, false, opt)) altered |= Axes.X;
            if (FloatField("Y", ref visible.y, (changed & Axes.Y) != 0, false, opt)) altered |= Axes.Y;
            if (FloatField("Z", ref visible.z, (changed & Axes.Z) != 0, false, opt)) altered |= Axes.Z;

            if (reset)
            {
                mRot.quaternionValue = Quaternion.identity;
            }
            else if (altered != Axes.None)
            {
                Undo.RecordObjects(serializedObject.targetObjects, "Change Rotation");

                foreach (Object obj in serializedObject.targetObjects)
                {
                    Transform t = obj as Transform;
                    Vector3 v = t.localEulerAngles;

                    if ((altered & Axes.X) != 0) v.x = visible.x;
                    if ((altered & Axes.Y) != 0) v.y = visible.y;
                    if ((altered & Axes.Z) != 0) v.z = visible.z;

                    t.localEulerAngles = v;
                }
            }
        }
        GUILayout.EndHorizontal();
    }
    #endregion
}
