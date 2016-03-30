using UnityEngine;
using UnityEditor;
using System.Collections;

/// <summary>
/// Maintaince Logs:
/// 2014-12-05  WP      Initial version. 
/// </summary>
[CanEditMultipleObjects]
[CustomEditor(typeof(WayController))]
public class InsWayController : Editor
{

    private WayController animator;
    private WayBezier bezier;

    private RenderTexture pointPreviewTexture = null;
    private float aspect = 1.7777f;
    private Camera sceneCamera = null;
    private Skybox sceneCameraSkybox = null;

    private Vector3 inScenePos;
    private Quaternion inSceneRot;

    void OnEnable()
    {
        animator = (WayController)target;
        bezier = animator.GetComponent<WayBezier>();
    }

    public void OnSceneGUI()
    {

        if (animator.showScenePreview)
        {
            float handleSize = HandleUtility.GetHandleSize(inScenePos * 0.5f);
            Handles.color = (Color.white - bezier.lineColour) + new Color(0, 0, 0, 1);
            Handles.DrawLine(inScenePos, inScenePos + Vector3.up * 0.5f);
            Handles.DrawLine(inScenePos, inScenePos + Vector3.down * 0.5f);
            Handles.DrawLine(inScenePos, inScenePos + Vector3.left * 0.5f);
            Handles.DrawLine(inScenePos, inScenePos + Vector3.right * 0.5f);
            Handles.DrawLine(inScenePos, inScenePos + Vector3.forward * 0.5f);
            Handles.DrawLine(inScenePos, inScenePos + Vector3.back * 0.5f);

            Handles.ArrowCap(0, inScenePos, inSceneRot, handleSize);
            Handles.Label(inScenePos, "LookAt");
        }

        if (GUI.changed)
        {
            bezier.RecalculateStoredValues();
            EditorUtility.SetDirty(animator);
            EditorUtility.SetDirty(bezier);
        }
    }

    public override void OnInspectorGUI()
    {
        Camera[] cams = Camera.allCameras;
        bool sceneHasCamera = cams.Length > 0;
        if (Camera.main)
        {
            sceneCamera = Camera.main;
        }
        else if (sceneHasCamera)
        {
            sceneCamera = cams[0];
        }

        if (sceneCamera != null)
            if (sceneCameraSkybox == null)
                sceneCameraSkybox = sceneCamera.GetComponent<Skybox>();

        if (pointPreviewTexture == null)
            pointPreviewTexture = new RenderTexture(400, Mathf.RoundToInt(400 / aspect), 24);

        if (bezier.numberOfCurves > 0 && pointPreviewTexture != null)
        {

            bool cameraPreview = EditorPrefs.GetBool("CameraPreview");
            GUILayout.Space(7);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Animation Preview");
            if (cameraPreview)
            {
                if (GUILayout.Button("Hide Look Preview", GUILayout.Width(150)))
                    EditorPrefs.SetBool("CameraPreview", false);
            }
            else
            {
                if (GUILayout.Button("Show Look Preview", GUILayout.Width(150)))
                    EditorPrefs.SetBool("CameraPreview", true);
            }
            EditorGUILayout.Space();
            EditorGUILayout.EndHorizontal();

            if (!Application.isPlaying && cameraPreview)
            {
                float usePercentage = animator.normalised ? animator.RecalculatePercentage(animator.editorTime) : animator.editorTime;

                //Get animation values and apply them to the preview camera
                inScenePos = bezier.GetPathPosition(usePercentage);
                inSceneRot = Quaternion.identity;

                //Assign rotation to preview camera
                Vector3 plusPoint, minusPoint;
                switch (bezier.mode)
                {
                    case WayBezier.viewmodes.usercontrolled:
                        inSceneRot = bezier.GetPathRotation(usePercentage);
                        break;

                    case WayBezier.viewmodes.target:

                        if (bezier.target != null)
                        {
                            inSceneRot = Quaternion.LookRotation(bezier.target.transform.position - inScenePos);
                        }
                        else
                        {
                            EditorGUILayout.HelpBox("No target has been specified in the bezier path", MessageType.Warning);
                            inSceneRot = Quaternion.identity;
                        }
                        break;

                    case WayBezier.viewmodes.followpath:

                        minusPoint = bezier.GetPathPosition(Mathf.Clamp01(usePercentage - 0.05f));
                        plusPoint = bezier.GetPathPosition(Mathf.Clamp01(usePercentage + 0.05f));
                        inSceneRot = Quaternion.LookRotation(plusPoint - minusPoint);
                        break;

                    case WayBezier.viewmodes.reverseFollowpath:

                        minusPoint = bezier.GetPathPosition(Mathf.Clamp01(usePercentage - 0.05f));
                        plusPoint = bezier.GetPathPosition(Mathf.Clamp01(usePercentage + 0.05f));
                        inSceneRot = Quaternion.LookRotation(minusPoint - plusPoint);
                        break;

                }

                //Render the camera preview
                GameObject go = new GameObject("Point Preview");
                go.transform.parent = bezier.transform;
                go.AddComponent<Camera>();
                //Retreive camera settings from the main camera
                if (sceneCamera != null)
                {
                    go.GetComponent<Camera>().backgroundColor = sceneCamera.backgroundColor;
                    if (sceneCameraSkybox != null)
                        go.AddComponent<Skybox>().material = sceneCameraSkybox.material;
                    else
                        if (RenderSettings.skybox != null)
                            go.AddComponent<Skybox>().material = RenderSettings.skybox;
                }
                go.transform.position = inScenePos;
                go.transform.rotation = inSceneRot;

                go.GetComponent<Camera>().targetTexture = pointPreviewTexture;
                go.GetComponent<Camera>().Render();
                go.GetComponent<Camera>().targetTexture = null;
                DestroyImmediate(go);

                //Display the camera preview

                Rect previewRect = new Rect(0, 0, Screen.width, Screen.width / aspect);
                Rect layoutRect = EditorGUILayout.BeginVertical();
                previewRect.x = layoutRect.x;
                previewRect.y = layoutRect.y + 5;
                EditorGUI.DrawPreviewTexture(previewRect, pointPreviewTexture);
                GUILayout.Space(previewRect.height + 10);
                pointPreviewTexture.Release();

                EditorGUILayout.BeginHorizontal();
                float time = EditorGUILayout.Slider(animator.editorTime * animator.pathTime, 0, animator.pathTime);
                animator.editorTime = time / animator.pathTime;
                EditorGUILayout.LabelField("sec", GUILayout.Width(25));
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
            }
        }


        animator.showScenePreview = EditorGUILayout.Toggle("Show Scene Preview Info", animator.showScenePreview);

        EditorGUILayout.BeginHorizontal();
        animator.pathTime = EditorGUILayout.FloatField("Animation Time", animator.pathTime);
        EditorGUILayout.LabelField("sec", GUILayout.Width(25));
        EditorGUILayout.EndHorizontal();

        bool noPath = bezier.numberOfControlPoints < 2;
        EditorGUI.BeginDisabledGroup(noPath);
        EditorGUILayout.BeginHorizontal();
        float newPathSpeed = EditorGUILayout.FloatField("Animation Speed", animator.pathSpeed);
        if (!noPath)
            animator.pathSpeed = newPathSpeed;
        EditorGUILayout.LabelField("m/sec", GUILayout.Width(25));
        EditorGUILayout.EndHorizontal();
        EditorGUI.EndDisabledGroup();

        animator.pathTime = Mathf.Max(animator.pathTime, 0.001f);//ensure it's a real number

        animator.normalised = EditorGUILayout.Toggle("Normalised Path", animator.normalised);

        if (GUI.changed)
        {
            bezier.RecalculateStoredValues();
            EditorUtility.SetDirty(animator);
            EditorUtility.SetDirty(bezier);
        }
    }
}
