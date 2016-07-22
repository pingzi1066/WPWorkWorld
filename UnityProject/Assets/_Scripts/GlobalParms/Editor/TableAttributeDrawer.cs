using System.IO;
using System.Xml;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

/// <summary>
/// Custom inspector for <see cref="TableAttribute"/>
/// </summary>
/// <seealso cref="UnityEditor.PropertyDrawer" />
[CustomPropertyDrawer(typeof(TableAttribute))]
public class TableAttributeDrawer : PropertyDrawer
{
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
    /// <param name="position">Rectangle on the screen to use for the property GUI.</param>
    /// <param name="property">The SerializedProperty to make the custom GUI for.</param>
    /// <param name="label">The label of this property.</param>
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        _attr = attribute as TableAttribute;
        _dataTable = property.FindPropertyRelative("listParms");

        EditorGUI.BeginChangeCheck();

        _showTable = EditorGUI.Foldout(new Rect(position.x, position.y, _labelWidth, _singleHeight), _showTable, label);

        var sizePrefixLabelRect = new Rect(position.width - _labelWidth - _prefixLabelWidth + 12.0f, position.y, _prefixLabelWidth, _singleHeight);
        var sizeFieldRect = new Rect(sizePrefixLabelRect.position.x + sizePrefixLabelRect.width, position.y, _labelWidth, _singleHeight);
        var totalSizeFieldRect = new Rect(sizePrefixLabelRect.position.x, sizePrefixLabelRect.position.y,
                                          sizePrefixLabelRect.width + sizePrefixLabelRect.y,
                                          _singleHeight);

        EditorGUI.HandlePrefixLabel(totalSizeFieldRect, sizePrefixLabelRect, new GUIContent("Size"));

        _dataTable.arraySize = EditorGUI.DelayedIntField(sizeFieldRect, _dataTable.arraySize);

        position.position = new Vector2(position.position.x, position.position.y + _singleHeight);

        if (_showTable)
        {
            var list = KMGUI.GetReorderableList(GetHashCode(), property.serializedObject, _dataTable, _attr.RowType.GetFields());

            list.DoList(position);

            //位置 = （数组长度 + 头 + 尾） * 单元高度
            position.position = new Vector2(position.position.x, position.position.y + (list.elementHeight) * (_dataTable.arraySize + 2));
        }

        EditorGUI.EndChangeCheck();

        search = EditorGUI.TextField(position, "Search", search);
    }


}
