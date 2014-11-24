using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif


/// <summary>
/// 绘制 Handle 到 Scene 的Mono类
/// 
/// Maintaince Logs:
/// 2014-11-24      WP      Initial version. 
/// </summary>
public class KMHandle : MonoBehaviour
{
#if UNITY_EDITOR
    public enum DrawType
    {
        WireSphere,
        WireCube,
        Arrow,
        Other,
        None
    }

    public DrawType drawType = DrawType.WireCube;

    public Color color = Color.white;

    public Vector3 center = Vector3.zero;

    public float size = 1;

    public bool isSelectedDraw = true;

    public Vector3 drawCenter { get { return transform.position + center; } }

    void Start() { }

    protected void OnDrawGizmos()
    {
        if (!isSelectedDraw)
        {
            DrawAll();
        }
    }

    protected void OnDrawGizmosSelected()
    {
        if (isSelectedDraw)
        {
            DrawAll();
        }
    }

    void DrawAll()
    {
        Handles.color = color;
        DrawLabel();
        DrawGizmos();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(this);
        }
    }

    void DrawGizmos()
    {
        switch (drawType)
        {
            case KMHandle.DrawType.WireCube:
                DrawWireCube();
                break;
            case KMHandle.DrawType.WireSphere:
                DrawWireSphere();
                break;
            case KMHandle.DrawType.Arrow:
                DrawArrow();
                break;
            case KMHandle.DrawType.Other:
                DrawOther();
                break;
        }
    }

    void DrawLabel()
    {
        Handles.Label(transform.position, name);


    }

    protected virtual void DrawWireCube()
    {
        Handles.RectangleCap(0,
            drawCenter,
            transform.rotation,
            size);
    }

    protected virtual void DrawWireSphere()
    {
        Handles.CircleCap(0,
            drawCenter,
            transform.rotation,
            size);
    }

    protected virtual void DrawArrow()
    {
        Handles.ArrowCap(0,
            drawCenter,
            transform.rotation,
            size);
    }

    protected virtual void DrawOther()
    {

    }
#endif
}
