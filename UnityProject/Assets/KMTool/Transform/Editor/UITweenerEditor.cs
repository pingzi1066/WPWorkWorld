
using KMTool;
using UnityEngine;
using UnityEditor;

namespace KMTool
{

    [CustomEditor(typeof(KMTweener), true)]
    public class UITweenerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            GUILayout.Space(6f);
            EditorGUIUtility.labelWidth = 110f;
            base.OnInspectorGUI();
            DrawCommonProperties();
        }

        protected void DrawCommonProperties()
        {
            KMTweener tw = target as KMTweener;

            if (KMGUI.DrawHeader("Tweener"))
            {
                KMGUI.BeginContents();
                EditorGUIUtility.labelWidth = 110f;

                GUI.changed = false;

                KMTweener.Style style = (KMTweener.Style)EditorGUILayout.EnumPopup("Play Style", tw.style);
                AnimationCurve curve = EditorGUILayout.CurveField("Animation Curve", tw.animationCurve, GUILayout.Width(170f), GUILayout.Height(62f));
                //UITweener.Method method = (UITweener.Method)EditorGUILayout.EnumPopup("Play Method", tw.method);

                GUILayout.BeginHorizontal();
                float dur = EditorGUILayout.FloatField("Duration", tw.duration, GUILayout.Width(170f));
                GUILayout.Label("seconds");
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                float del = EditorGUILayout.FloatField("Start Delay", tw.delay, GUILayout.Width(170f));
                GUILayout.Label("seconds");
                GUILayout.EndHorizontal();

                int tg = EditorGUILayout.IntField("Tween Group", tw.tweenGroup, GUILayout.Width(170f));
                bool ts = EditorGUILayout.Toggle("Ignore TimeScale", tw.ignoreTimeScale);
                bool fx = EditorGUILayout.Toggle("Use Fixed Update", tw.useFixedUpdate);

                if (GUI.changed)
                {
                    KMEditorTools.RegisterUndo("Tween Change", tw);
                    tw.animationCurve = curve;
                    //tw.method = method;
                    tw.style = style;
                    tw.ignoreTimeScale = ts;
                    tw.tweenGroup = tg;
                    tw.duration = dur;
                    tw.delay = del;
                    tw.useFixedUpdate = fx;
                    EditorUtility.SetDirty(tw);
                }
                KMGUI.EndContents();
            }

            EditorGUIUtility.labelWidth = 80;

            //draw events
            DrawProp("onFinished");
            serializedObject.ApplyModifiedProperties();
            //NGUIEditorTools.DrawEvents("On Finished", tw, tw.onFinished);
        }

        protected void DrawProp(string name)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty(name));
        }
    }
}