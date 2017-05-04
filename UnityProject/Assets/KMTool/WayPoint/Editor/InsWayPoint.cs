using UnityEngine;
using UnityEditor;
using System.Collections;

namespace KMTool
{
    /// <summary>
    /// Maintaince Logs:
    /// 2014-12-05  WP      Initial version. 
    /// </summary>
    [CanEditMultipleObjects]
    [CustomEditor(typeof(WayPoint))]
    public class InsWayPoint : Editor
    {

        private WayPoint bezierControlPoint;
        private WayController animator;
        private WayBezier bezier;

        private Vector3 previewCamPos;
        private Quaternion previewCamRot;

        void OnEnable()
        {
            bezierControlPoint = (WayPoint)target;
            bezier = bezierControlPoint.bezier;
            animator = bezier.GetComponent<WayController>();
        }

        void OnSceneGUI()
        {

            Vector3 newPosition = bezierControlPoint.worldControlPoint;// bezierControlPoint.controlPoint + bezierControlPoint.transform.position;
            float handlesize = HandleUtility.GetHandleSize(newPosition) * 0.6f;

            Handles.color = new Color(0.99f, 0.50f, 0.35f);
            newPosition = Handles.Slider(newPosition, Vector3.right, handlesize, Handles.ArrowHandleCap, 1);
            Handles.color = new Color(0.30f, 0.85f, 0.99f);
            newPosition = Handles.Slider(newPosition, Vector3.forward, handlesize, Handles.ArrowHandleCap, 1);
            Handles.color = new Color(0.85f, 0.95f, 0.30f);
            newPosition = Handles.Slider(newPosition, Vector3.up, handlesize, Handles.ArrowHandleCap, 1);

            //Tilting draw function for follow path
            if (bezier.mode == WayBezier.viewmodes.followpath || bezier.mode == WayBezier.viewmodes.reverseFollowpath)
            {
                Handles.color = Color.yellow;
                float tiltScale = HandleUtility.GetHandleSize(bezierControlPoint.transform.position) * 1.25f;
                Vector3 forward = bezierControlPoint.GetPathDirection().normalized * tiltScale;
                Quaternion fwdRot = Quaternion.LookRotation(forward);
                Vector3 tilt = Quaternion.Euler(0, 0, -bezierControlPoint.tilt) * (Vector3.up * tiltScale);
                Handles.DrawWireDisc(bezierControlPoint.transform.position, forward, tiltScale);
                Handles.DrawLine(bezierControlPoint.transform.position, bezierControlPoint.transform.position + fwdRot * tilt);
            }


            Handles.color = Color.red;
            Handles.DrawLine(bezierControlPoint.transform.position, bezierControlPoint.worldControlPoint);

            if (bezierControlPoint.worldControlPoint != newPosition)
            {
                GUI.changed = true;
                bezierControlPoint.worldControlPoint = newPosition;
            }

            string handleName = bezierControlPoint.name;
            float center = HandleUtility.GetHandleSize(newPosition) * 1.25f;
            Handles.Label(bezierControlPoint.transform.position + Vector3.up * center
                , handleName);

            Handles.color = (Color.white - bezier.lineColour) + new Color(0, 0, 0, 1);
            Handles.ArrowHandleCap(0, previewCamPos, previewCamRot, handlesize * 1.5f, EventType.Repaint);

            if (GUI.changed)
            {
                bezier.RecalculateStoredValues();
                Undo.RecordObject(bezierControlPoint, "Move Control Point");
                EditorUtility.SetDirty(bezierControlPoint);
                EditorUtility.SetDirty(bezier);
                EditorUtility.SetDirty(animator);
            }
        }

        public override void OnInspectorGUI()
        {

            EditorGUIUtility.labelWidth = 50;

            previewCamPos = bezierControlPoint.transform.position;
            previewCamRot = Quaternion.identity;

            Vector3 plusPoint, minusPoint;
            float pointPercentage = bezier.GetPathPercentageAtPoint(bezierControlPoint);
            switch (bezier.mode)
            {
                case WayBezier.viewmodes.usercontrolled:
                    previewCamRot = bezierControlPoint.transform.rotation;
                    break;

                case WayBezier.viewmodes.target:

                    if (bezier.target != null)
                    {
                        previewCamRot = Quaternion.LookRotation(bezier.target.transform.position - bezierControlPoint.transform.position);
                    }
                    else
                    {
                        EditorGUILayout.HelpBox("No target has been specified in the bezier path", MessageType.Warning);
                        previewCamRot = Quaternion.identity;
                    }
                    break;

                case WayBezier.viewmodes.followpath:

                    minusPoint = bezier.GetPathPosition(Mathf.Clamp01(pointPercentage - 0.05f));
                    plusPoint = bezier.GetPathPosition(Mathf.Clamp01(pointPercentage + 0.05f));
                    previewCamRot = Quaternion.LookRotation(plusPoint - minusPoint);
                    break;

                case WayBezier.viewmodes.reverseFollowpath:

                    minusPoint = bezier.GetPathPosition(Mathf.Clamp01(pointPercentage - 0.05f));
                    plusPoint = bezier.GetPathPosition(Mathf.Clamp01(pointPercentage + 0.05f));
                    previewCamRot = Quaternion.LookRotation(minusPoint - plusPoint);
                    break;
            }

            GUILayout.Space(10);
            if (!bezierControlPoint.isLastPoint || bezier.loop)
            {
                //EditorGUILayout.HelpBox(, MessageType.Info);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(new GUIContent("Animation Ease",
                    "This controls the easing applied from this point to the next."), EditorStyles.boldLabel);
                bezierControlPoint.ease = (WayPoint.animationEase)EditorGUILayout.EnumPopup(bezierControlPoint.ease);
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                EditorGUILayout.HelpBox("The last control point does not have easing options as there is no following curve.", MessageType.Info);
            }

            //FOVEditor
            GUILayout.Space(7);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Field of View");
            bezierControlPoint.FOV = EditorGUILayout.Slider(bezierControlPoint.FOV, 1, 180);
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(new GUIContent("Smooth",
                EditorGUIUtility.ObjectContent(null, typeof(Vector3)).image,
                "Make the rotation of the Control Point face the direction the path is going in at this point")
                , GUILayout.Width(60)))
            {
                Undo.RecordObject(bezierControlPoint.gameObject.transform, "Set Control Point Rotation to Path Direction");
                bezierControlPoint.SetRotationToCurve();
                EditorUtility.SetDirty(bezierControlPoint);
            }
            Vector3 locaEuler = EditorGUILayout.Vector3Field("Rotate", bezierControlPoint.transform.localEulerAngles);
            if(locaEuler != bezierControlPoint.transform.localEulerAngles)
                bezierControlPoint.transform.localEulerAngles = locaEuler;
            EditorGUILayout.EndHorizontal();



            GUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(new GUIContent("Reset",
                "The control point modifies the curve. Resetting it will centre it and there will be no curve. Useful if you don't want there to be a curve at this point"), GUILayout.Width(60)))
            {
                Undo.RecordObject(bezierControlPoint.gameObject, "Reset Control Point");
                bezierControlPoint.controlPoint = Vector3.zero;
                EditorUtility.SetDirty(bezierControlPoint);
            }
            bezierControlPoint.controlPoint = EditorGUILayout.Vector3Field("Bezier", bezierControlPoint.controlPoint);
            EditorGUILayout.EndHorizontal();


            GUILayout.Space(7);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Delay");
            bezierControlPoint.delayMode = (WayPoint.DELAY_MODES)EditorGUILayout.EnumPopup(bezierControlPoint.delayMode);
            EditorGUILayout.EndHorizontal();

            if (bezierControlPoint.delayMode == WayPoint.DELAY_MODES.timed)
            {
                GUILayout.Space(7);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Time of delay");
                bezierControlPoint.delayTime = EditorGUILayout.FloatField(bezierControlPoint.delayTime, GUILayout.Width(50));
                EditorGUILayout.LabelField("secs", GUILayout.Width(30));
                EditorGUILayout.EndHorizontal();
            }

            if (bezier.mode == WayBezier.viewmodes.followpath || bezier.mode == WayBezier.viewmodes.reverseFollowpath)
            {
                GUILayout.Space(7);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Tilt", GUILayout.Width(40));
                bezierControlPoint.tilt = EditorGUILayout.Slider(bezierControlPoint.tilt, -180, 180);
                EditorGUILayout.EndHorizontal();
            }

            if (GUI.changed)
            {
                bezier.RecalculateStoredValues();

                EditorUtility.SetDirty(bezierControlPoint);
                EditorUtility.SetDirty(bezier);
                EditorUtility.SetDirty(animator);
            }
        }
    }
}