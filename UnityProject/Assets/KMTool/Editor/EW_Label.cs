using UnityEngine;
using UnityEditor;
using System.Collections;

namespace KMTool
{
    /// <summary>
    /// 提供  窗口编辑器 unity gameobject label 
    /// 
    /// Maintaince Logs:
    /// 2014-12-02  WP      Initial version
    /// </summary>
    public class EW_Label : EditorWindow
    {

        private const string TIP_SELECT_OBJ = "请选择一个对象";
        private const string TIP_SELECT_PROJECT_OBJ = "请选择工程里面的游戏对象";
        private const string BTN_SetLabel = "设置到物品";
        private const string BTN_ClearLabel = "清空物品标签";

        private const string MENU_LABEL = "Tools/其它/" + "标签设置";
        private const string DESC_LABEL = "标签名字：";

        private const string saveKey = "KMLabel";

        /// <summary>
        /// 系统保存值
        /// </summary>
        private static string value = "";

        void OnGUI()
        {
            Object[] selects = Selection.objects;


            if (selects.Length == 0)
            {
                GUILayout.Label(TIP_SELECT_OBJ);
                return;
            }
            else if (!AssetDatabase.Contains(selects[0]))
            {
                GUILayout.Label(TIP_SELECT_PROJECT_OBJ);
                return;
            }

            GUILayout.Label(DESC_LABEL);
            value = GUILayout.TextField(value);

            GUILayout.Space(10f);

            if (GUILayout.Button(BTN_SetLabel) && !string.IsNullOrEmpty(value))
            {
                string[] labels = new string[] { value };
                AddLabelToPrefab(labels);
                Save();
            }

            if (GUILayout.Button(BTN_ClearLabel))
            {
                ClearLabelsForPrefab();
            }
        }

        static void Save()
        {
            EditorPrefs.SetString(saveKey, value);
        }

        static void Load()
        {
            value = EditorPrefs.GetString(saveKey, "");
        }

        [MenuItem(MENU_LABEL)]
        static void OpenWindow()
        {
            EditorWindow.GetWindow<EW_Label>(false, "Label Editor", true).Show();
            Load();
        }

        static void ClearLabelsForPrefab()
        {
            Object[] selects = Selection.objects;
            foreach (Object obj in selects)
            {
                AssetDatabase.ClearLabels(obj);
            }
            AssetDatabase.Refresh();
        }

        static void AddLabelToPrefab(string[] labels)
        {
            Object[] selects = Selection.objects;
            foreach (Object obj in selects)
            {
                AssetDatabase.SetLabels(obj, labels);
            }
            AssetDatabase.Refresh();
        }
    }
}