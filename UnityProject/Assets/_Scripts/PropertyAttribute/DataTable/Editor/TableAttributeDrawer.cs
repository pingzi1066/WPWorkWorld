using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Custom inspector for <see cref="TableAttribute"/>
/// </summary>
/// <seealso cref="UnityEditor.PropertyDrawer" />
[CustomPropertyDrawer(typeof(TableAttribute))]
public class TableAttributeDrawer : PropertyDrawer
{
    private const string PropertyName = "Rows";

    private string search = "";

    /// <summary>
    /// The bool value for fold out.
    /// </summary>
    private bool _showTable = true;

    /// <summary>
    /// The table attribute
    /// </summary>
    private TableAttribute _attr;

    /// <summary>
    /// The data table
    /// </summary>
    private SerializedProperty _dataTable;

    /// <summary>
    /// The label width
    /// </summary>
    private float _labelWidth = 80.0f;

    /// <summary>
    /// The prefix label width
    /// </summary>
    private float _prefixLabelWidth = 40.0f;

    /// <summary>
    /// The single height
    /// </summary>
    private float _singleHeight = EditorGUIUtility.singleLineHeight;

    /// <summary>
    /// Override this method to make your own GUI for the property.
    /// </summary>
    /// <param name="rect">Rectangle on the screen to use for the property GUI.</param>
    /// <param name="property">The SerializedProperty to make the custom GUI for.</param>
    /// <param name="label">The label of this property.</param>
    public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
    {
        _attr = attribute as TableAttribute;
        _dataTable = property.FindPropertyRelative(PropertyName);

        EditorGUI.BeginChangeCheck();

        _showTable = EditorGUI.Foldout(new Rect(rect.x, rect.y, _labelWidth, _singleHeight), _showTable, label);

        var sizePrefixLabelRect = new Rect(rect.width - _labelWidth - _prefixLabelWidth + 12.0f, rect.y, _prefixLabelWidth, _singleHeight);
        var sizeFieldRect = new Rect(sizePrefixLabelRect.position.x + sizePrefixLabelRect.width, rect.y, _labelWidth, _singleHeight);
        var totalSizeFieldRect = new Rect(sizePrefixLabelRect.position.x, sizePrefixLabelRect.position.y,
                                          sizePrefixLabelRect.width + sizePrefixLabelRect.y,
                                          _singleHeight);
        
        //将查找内容进行比对，并选择配对的项目，然后转换为数组，加入到显示的 _dataTable 中，玩家在个性显示的数组时，要求同时修改源数据
        List<int> searList = new List<int>();
        if (!string.IsNullOrEmpty(search))
        {
            FieldInfo[] fields = _attr.RowType.GetFields(BindingFlags.Public | BindingFlags.Instance);

            //添加stirng型字段名字
            List<string> searchStr = new List<string>();
            for (int i = 0; i < fields.Length; i++)
            {
                if (string.Compare(fields[i].FieldType.ToString(), "System.String") == 0)
                {
                    searchStr.Add(fields[i].Name);
                }

            }

            //将符合要求的发送过去
            for (int i = 0; i < _dataTable.arraySize; i++)
            {
                for (int j = 0; j < searchStr.Count; j++)
                {
                    string curValue = _dataTable.GetArrayElementAtIndex(i).FindPropertyRelative(searchStr[j]).stringValue;
                    if (curValue.Contains(search))
                    {
                        searList.Add(i);
                        break;
                    }
                }
            }
           
        }


        //显示数组总长度
        EditorGUI.HandlePrefixLabel(totalSizeFieldRect, sizePrefixLabelRect, new GUIContent("Size"));
        _dataTable.arraySize = EditorGUI.DelayedIntField(sizeFieldRect, _dataTable.arraySize);

        rect.position = new Vector2(rect.position.x, rect.position.y + _singleHeight);

        if (_showTable)
        {

            var list = KMGUI.GetReorderableList(GetHashCode(), property.serializedObject, _dataTable, _attr.RowType.GetFields(),searList);

            list.DoList(rect);

            //位置 = （数组长度 ） * 单元高度 + 头 + 尾
            rect.y = rect.position.y + (list.elementHeight) * (list.count) + KMGUI.ListFooterHeight + KMGUI.ListHeaderHeight + 10f;

            if (list.count == 0)
            {
                rect.y += list.elementHeight;
            }

            rect.height = _singleHeight;
            search = EditorGUI.TextField(rect, "Search", search);

        }

        EditorGUI.EndChangeCheck();
    }

    /// <summary>
    /// Override this method to specify how tall the GUI for this field is in pixels.
    /// </summary>
    /// <param name="property">The SerializedProperty to make the custom GUI for.</param>
    /// <param name="label">The label of this property.</param>
    /// <returns>
    /// The height in pixels.
    /// </returns>
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (_showTable)
        {
            int rowCount = property.FindPropertyRelative(PropertyName).arraySize;

            rowCount = rowCount == 0 ? 1 : rowCount;

            float totalHeight = _singleHeight
                                + KMGUI.ListHeaderHeight
                                + (KMGUI.ListElementHeight * rowCount)
                                + KMGUI.ListFooterHeight
                                + _singleHeight // search
                                + 10f; //padding

            return totalHeight;
        }
        else
        {
            return _singleHeight;
        }
    }

}
