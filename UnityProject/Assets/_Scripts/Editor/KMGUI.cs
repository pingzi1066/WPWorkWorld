using UnityEngine;
using UnityEditor;
using System.Collections;

/// <summary>
/// 提供一些Inspector 或 window 绘制工具
/// 
/// Maintaince Logs:
/// 2014-11-01      WP      Initial version
/// 2015-01-10      WP      加入有色按钮,类改名为KMGUI
/// </summary>
public static class KMGUI
{

    /// <summary>
    /// Returns a blank usable 1x1 white texture.
    /// </summary>

    static public Texture2D blankTexture
    {
        get
        {
            return EditorGUIUtility.whiteTexture;
        }
    }

    /// <summary>
    /// Draw a distinctly different looking header label
    /// </summary>

    static public bool DrawHeader(string text) { return DrawHeader(text, text, false); }

    /// <summary>
    /// Draw a distinctly different looking header label
    /// </summary>

    static public bool DrawHeader(string text, string key, bool forceOn)
    {
        bool state = EditorPrefs.GetBool(key, true);

        GUILayout.Space(3f);
        if (!forceOn && !state) GUI.backgroundColor = new Color(0.8f, 0.8f, 0.8f);
        GUILayout.BeginHorizontal();
        GUILayout.Space(3f);

        GUI.changed = false;
#if UNITY_3_5
		if (state) text = "\u25B2 " + text;
		else text = "\u25BC " + text;
		if (!GUILayout.Toggle(true, text, "dragtab", GUILayout.MinWidth(20f))) state = !state;
#else
        text = "<b><size=11>" + text + "</size></b>";
        if (state) text = "\u25B2 " + text;
        else text = "\u25BC " + text;
        if (!GUILayout.Toggle(true, text, "dragtab", GUILayout.MinWidth(20f))) state = !state;
#endif
        if (GUI.changed) EditorPrefs.SetBool(key, state);

        GUILayout.Space(2f);
        GUILayout.EndHorizontal();
        GUI.backgroundColor = Color.white;
        if (!forceOn && !state) GUILayout.Space(3f);
        return state;
    }

    /// <summary>
    /// Begin drawing the content area.
    /// </summary>

    static public void BeginContents()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(4f);
        EditorGUILayout.BeginHorizontal("AS TextArea", GUILayout.MinHeight(10f));
        GUILayout.BeginVertical();
        GUILayout.Space(2f);
    }

    /// <summary>
    /// End drawing the content area.
    /// </summary>

    static public void EndContents()
    {
        GUILayout.Space(3f);
        GUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(3f);
        GUILayout.EndHorizontal();
        GUILayout.Space(3f);
    }

    /// <summary>
    /// Draw a visible separator in addition to adding some padding.
    /// </summary>

    static public void DrawSeparator()
    {
        GUILayout.Space(12f);

        if (Event.current.type == EventType.Repaint)
        {
            Texture2D tex = blankTexture;
            Rect rect = GUILayoutUtility.GetLastRect();
            GUI.color = new Color(0f, 0f, 0f, 0.25f);
            GUI.DrawTexture(new Rect(0f, rect.yMin + 6f, Screen.width, 4f), tex);
            GUI.DrawTexture(new Rect(0f, rect.yMin + 6f, Screen.width, 1f), tex);
            GUI.DrawTexture(new Rect(0f, rect.yMin + 9f, Screen.width, 1f), tex);
            GUI.color = Color.white;
        }
    }

    #region 按钮

    /// <summary>
    /// 绘制按钮
    /// </summary>
    /// <param name="content">内容</param>
    /// <param name="color">颜色</param>
    /// <param name="options">设置</param>

    static public bool Button(GUIContent content, Color color, params GUILayoutOption[] options)
    {
        Color oldCol = GUI.backgroundColor;

        GUI.backgroundColor = color;

        bool isTap = GUILayout.Button(content, options);

        GUI.backgroundColor = oldCol;

        return isTap;
    }

    /// <summary>
    /// 绘制按钮
    /// </summary>
    /// <param name="text">内容</param>
    /// <param name="color">颜色</param>
    /// <param name="options">设置</param>

    static public bool Button(string text, Color color, params GUILayoutOption[] options)
    {
        return Button(new GUIContent(text), color, options);
    }

    static public bool Button(string text, params GUILayoutOption[] options)
    {
        return Button(new GUIContent(text), GUI.backgroundColor, options);
    }

    /// <summary>
    /// 删除按钮
    /// </summary>

    static public bool BtnDelete(params GUILayoutOption[] options)
    {
        if (options.Length == 0) options = new GUILayoutOption[1] { GUILayout.Width(45) };
        return Button("删除", Color.red, options);
    }

    public static bool xMiniButton(this Rect r, string lb, bool autoSize = true, float lbAlign = 0.5f, bool drawButton = true)
    {
        //lb = "999+";
        var style = EditorStyles.miniLabel;
        var lbRect = style.CalcSize(new GUIContent(lb));
        var rr = r;//.wh((autoSize ? lbRect.x : r.width), 14f);

        lbRect = EditorStyles.label.CalcSize(new GUIContent(lb));
        var isClicked = drawButton && GUI.Button(rr, "", EditorStyles.miniButton);
        GUI.Label(rr,//rr.dx((rr.width - lbRect.x) * lbAlign).dy(-1f), lb,
            lb, drawButton ? EditorStyles.miniLabel : EditorStyles.label);

        return isClicked;
    }

    #endregion
}
