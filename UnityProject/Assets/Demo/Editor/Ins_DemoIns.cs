/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2016-03-30     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using System.Collections;
using KMTool;

using UnityEditor;
/// <summary>
/// 描述
/// </summary>

[CustomEditor(typeof(Demo_Inspector))]
public class Ins_DemoIns : Editor
{
    void OnEnable()
    {
        labelWidth = 120;
        indentLevel= 0;
    }

    private int intNum = 0;
    private bool isEditor = false;
    private int toolBar = 0;
    private string textCJK = "苍井空，1983年11月11日出生于日本东京。\n日本AV女演员、成人模特，兼电视、电影演员。\n日本女子组合惠比寿麝香葡萄的初代首领，现成员、OG首领。\n2010年3月毕业并将组合首领之位交托给麻美由真，同年10月复归。入行前曾是泳装写真女星，2002年进入AliceJapan公司，开始性感影片的拍摄生涯。因为其“童颜巨乳”的特色，开始获得人气\n并连续在2003年及2004年蝉联日本《VideoBoy》杂志年度性感女艺人第一名。\n从2003年起，开始参加一般电视戏剧及综艺节目中演出，\n2004年11月移籍到S1，成功转型，大牌杂志模特及电影演员。";
    private string textB = "";
    private string textM = "";

    private float labelWidth;
    private int indentLevel;

    public override void OnInspectorGUI()
    {
        //设置 key 与 value 之间，key的显示长度
        EditorGUIUtility.labelWidth = EditorGUILayout.FloatField("key的显示长度", labelWidth);
         
        //缩进，数值越大，缩进越前 ,默认的缩进是0
        EditorGUI.indentLevel = EditorGUILayout.IntField("缩进", indentLevel);

        if (EditorGUIUtility.labelWidth != labelWidth)
            labelWidth = EditorGUIUtility.labelWidth;
        if (EditorGUI.indentLevel != indentLevel)
            indentLevel = EditorGUI.indentLevel;

        //添加树向的背景方框
        EditorGUILayout.BeginVertical("Box");
        //这中间添加显示的内容
        EditorGUILayout.LabelField("内容1");
        //一个按扭
        GUILayout.Button(EditorGUIUtility.ObjectContent(null, typeof(Rigidbody)), GUILayout.Height(40), GUILayout.Width(200));
        EditorGUILayout.LabelField("内容2");
//        GUILayout.Button(EditorGUIUtility.ObjectContent(null, typeof(Rigidbody)));
        EditorGUILayout.EndVertical();

        //求签页
        toolBar = GUILayout.Toolbar(toolBar, new GUIContent[]
        {
            new GUIContent("麻美由真"),
            new GUIContent("苍井空"),
            new GUIContent("波多野结衣"),
        });

        //横向排列
        EditorGUILayout.BeginHorizontal();

        switch (toolBar)
        {
            case 0:
                EditorGUILayout.LabelField(textM);
                break;
            case 1:
                //图片显示
                GUILayout.Label((Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Demo/Images/demo001.jpg", typeof(Texture2D)));
                EditorGUILayout.TextArea(textCJK, GUILayout.Width(200), GUILayout.ExpandHeight(true));
                break;
            case 2:
                EditorGUILayout.LabelField(textB);
                break;
        }

        EditorGUILayout.EndHorizontal();

        //提示
        EditorGUILayout.HelpBox("提示内容", MessageType.Info);

        //开关
        isEditor = EditorGUILayout.Toggle("开关", isEditor);

        //禁用编辑 
        EditorGUI.BeginDisabledGroup(isEditor);
        intNum = EditorGUILayout.IntField("编辑值", intNum);
        if (KMGUI.Button("按钮")) { }
        EditorGUI.EndDisabledGroup();

        KMGUI.BeginContents();
        //提示
        EditorGUILayout.HelpBox("KMGUI 区域", MessageType.Warning);

        if (KMGUI.DrawHeader("展开区间"))
        {
            KMGUI.DarwLine(5, 3);
            EditorGUILayout.LabelField("中隔线1");
            KMGUI.DarwLine(3, 3);
            EditorGUILayout.LabelField("中隔线2");
            KMGUI.DarwLine(3, 3);
            EditorGUILayout.LabelField("中隔线3");
            KMGUI.DarwLine(3, 5);

        }

        KMGUI.EndContents();


        //当编辑器改变时一定要调用此函数，此函数保存你所有的改变，此参数的作用域只有一个方法内。target 为改变的脚本
        if (GUI.changed)
            EditorUtility.SetDirty(target);

//        


    }
}