/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2016-07-22     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Collections.Generic;
using UnityEditorInternal;
using System.Reflection;

/// <summary>
/// 用于编辑器中 ReorderableList 的数据保存与绘制提供
/// </summary>
public static partial class KMGUI
{
    #region Color control
    /// <summary>
    /// The temporal storage of original color
    /// </summary>
    private static Stack<Color> originColors = new Stack<Color>();

    /// <summary>
    /// The type colors. key = type name, value = color.
    /// </summary>
    private static Dictionary<string, Color> _typeColors = new Dictionary<string, Color>() 
    {
        {"Boolean"   , new Color(0.588f, 0.275f, 0.588f, 0.784f)   },   
        {"Int32"	 , new Color(0.502f, 0.635f, 1.000f, 0.784f)   },   
        {"Single"	 , new Color(0.467f, 0.745f, 0.467f, 0.784f)   },   
        {"String"	 , new Color(0.957f, 0.604f, 0.761f, 0.784f)   },   
        {"Enum"	     , new Color(1.000f, 0.702f, 0.278f, 0.784f)   },   
        {"Object"	 , new Color(0.992f, 0.992f, 0.588f, 0.784f)   },   
        {"Vector2"   , new Color(1.000f, 0.412f, 0.380f, 0.784f)   },   
        {"Vector3"   , new Color(1.000f, 0.412f, 0.380f, 0.784f)   },   
        {"Vector4"   , new Color(1.000f, 0.412f, 0.380f, 0.784f)   },   
        {"Color"     , new Color(0.706f, 0.878f, 1.000f, 0.784f)   },

    }
        ;

    /// <summary>
    /// Begins the color of the GUI. It should be paired with <see cref="EndGUIColor"/>.
    /// </summary>
    /// <param name="color">The color.</param>
    public static void BeginGUIColor(Color color)
    {
        originColors.Push(GUI.color);
        GUI.color = color;
    }

    /// <summary>
    /// Ends the color of the GUI. It should be paired with <see cref="BeginGUIColor(Color)"/>.
    /// </summary>
    public static void EndGUIColor()
    {
        GUI.color = originColors.Pop();
    }

    /// <summary>
    /// Gets the color of the type. 
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>Corresponding Color. if there is no match, then return white color.</returns>
    public static Color GetTypeColor(Type type)
    {
        string typeName = type.Name;

        if (type.IsSubclassOf(typeof(UnityEngine.Object)))
        {
            typeName = typeof(UnityEngine.Object).Name;
        }
        else if (type.IsSubclassOf(typeof(Enum)))
        {
            typeName = typeof(Enum).Name;
        }

        if (_typeColors.ContainsKey(typeName))
        {
            return _typeColors[typeName];
        }
        else
        {
            return Color.white;
        }
    }

    #endregion

    #region Layout
    /// <summary>
    /// The rect scope
    /// </summary>
    private static Stack<Rect> _rectScope = new Stack<Rect>();

    /// <summary>
    /// How many getters called.
    /// </summary>
    private static Stack<int> _getterCallCount = new Stack<int>();

    /// <summary>
    /// Begins the horizontal layout. It should be paired with <see cref="EndHorizontal"/>
    /// </summary>
    /// <param name="scope">The scope.</param>
    public static void BeginHorizontal(Rect scope)
    {
        _rectScope.Push(scope);
        _getterCallCount.Push(0);
    }

    /// <summary>
    /// Gets the horizontal rect for horizontal layout. It should be used only between <see cref="BeginHorizontal(Rect)"/> and <see cref="EndHorizontal"/>
    /// </summary>
    /// <param name="width">The width.</param>
    /// <returns></returns>
    public static Rect GetHorizontalRect(float width)
    {
        Rect result = _rectScope.Pop();
        int callCount = _getterCallCount.Pop();

        if (callCount == 0)
        {
            result.width = width;
        }
        else
        {
            result.position = new Vector2(result.position.x + result.width, result.position.y);
            result.width = width;
        }

        callCount++;

        _rectScope.Push(result);
        _getterCallCount.Push(callCount);
        return result;
    }

    /// <summary>
    /// Ends the horizontal layout. It should be paired with <see cref="BeginHorizontal(Rect)"/>
    /// </summary>
    public static void EndHorizontal()
    {
        _rectScope.Pop();
        _getterCallCount.Pop();
    }

    /// <summary>
    /// Begins the vertical layout. It should be paired with <see cref="EndVertical"/>
    /// </summary>
    /// <param name="scope">The scope.</param>
    public static void BeginVertical(Rect scope)
    {
        _rectScope.Push(scope);
        _getterCallCount.Push(0);
    }

    /// <summary>
    /// Gets the vertical rect for vertical layout. It should be used only between <see cref="BeginHorizontal(Rect)"/> and <see cref="EndVertical"/>
    /// </summary>
    /// <param name="height">The height.</param>
    /// <returns></returns>
    public static Rect GetVerticalRect(float height)
    {
        Rect result = _rectScope.Pop();
        int callCount = _getterCallCount.Pop();

        if (callCount == 0)
        {
            result.height = height;
        }
        else
        {
            result.position = new Vector2(result.position.x, result.position.y + result.height);
            result.height = height;
        }

        callCount++;

        _rectScope.Push(result);
        _getterCallCount.Push(callCount);
        return result;
    }

    /// <summary>
    /// Ends the vertical layout. It should be paired with <see cref="BeginHorizontal(Rect)"/>
    /// </summary>
    public static void EndVertical()
    {
        _rectScope.Pop();
        _getterCallCount.Pop();


    }
    #endregion

    #region ReorderableList Helper

    /// <summary>
    /// Gets the height of the list header.
    /// </summary>
    /// <value>
    /// The height of the list header.
    /// </value>
    public static float ListHeaderHeight { get { return 18.0f; } }

    /// <summary>
    /// Gets the height of the list element.
    /// </summary>
    /// <value>
    /// The height of the list element.
    /// </value>
    public static float ListElementHeight { get { return 18.0f; } }

    /// <summary>
    /// Gets the height of the list footer.
    /// </summary>
    /// <value>
    /// The height of the list footer.
    /// </value>
    public static float ListFooterHeight { get { return 13.0f; } }

    /// <summary>
    /// Gets the width of the index.
    /// </summary>
    /// <value>
    /// The width of the index.
    /// </value>
    public static float IndexWidth { get { return 50.0f; } }

    /// <summary>
    /// Gets the content padding right.
    /// </summary>
    /// <value>
    /// The content padding right.
    /// </value>
    public static float ContentPaddingRight { get { return 26.0f; } }

    /// <summary>
    /// The reorderable list storage. <para />
    /// This dictionary is needed to draw reorderable list in the property drawers.
    /// </summary>
    private static Dictionary<int, ReorderableList> _reorderableListStorage = new Dictionary<int, ReorderableList>();

    private static List<int> stringSearchByList = new List<int>();

    #endregion

    /// <summary>
    /// Gets the reorderable list.
    /// </summary>
    /// <param name="drawerHashCode">The drawer hash code.</param>
    /// <param name="serializedObject">The serialized object.</param>
    /// <param name="elements">The elements.</param>
    /// <param name="fieldsInfo">The fields information.</param>
    /// <returns></returns>
    public static ReorderableList GetReorderableList(int drawerHashCode, SerializedObject serializedObject, SerializedProperty elements, FieldInfo[] fieldsInfo,List<int> searchList = null)
    {
        stringSearchByList = searchList;

        if (_reorderableListStorage.ContainsKey(drawerHashCode))
        {
            _reorderableListStorage[drawerHashCode].serializedProperty = elements;
            return _reorderableListStorage[drawerHashCode];
        }
        else
        {
            _reorderableListStorage.Add(drawerHashCode, CreateReorderableList(serializedObject, elements, fieldsInfo));
            return _reorderableListStorage[drawerHashCode];
        }
    }

    /// <summary>
    /// Creates the reorderable list.
    /// </summary>
    /// <param name="serializedObject">The serialized object.</param>
    /// <param name="elements">The elements.</param>
    /// <param name="fieldsInfo">The fields information.</param>
    /// <returns></returns>
    private static ReorderableList CreateReorderableList(SerializedObject serializedObject, SerializedProperty elements, FieldInfo[] fieldsInfo)
    {
        ReorderableList reorderableList = new ReorderableList(serializedObject, elements)
        {
            headerHeight = ListHeaderHeight,
            elementHeight = ListElementHeight,
            footerHeight = ListFooterHeight
        };

        reorderableList.drawHeaderCallback += rect => DrawTableHeader(rect, fieldsInfo);
        reorderableList.drawElementCallback += (Rect rect, int index, bool isActive, bool isFocused) =>
        {
                DrawTableElement(rect, index, isActive, isFocused, reorderableList, fieldsInfo );
        };

        return reorderableList;
    }
    /// <summary>
    /// Draws the table header.
    /// </summary>
    /// <param name="rect">The rect.</param>
    /// <param name="fieldsInfo">The fields information.</param>
    private static void DrawTableHeader(Rect rect, FieldInfo[] fieldsInfo)
    {
        if (fieldsInfo.Length == 0)
        {
            Debug.LogWarning("Field names are missing.");
            return;
        }

        rect.position = new Vector2(rect.position.x - 5.0f, rect.position.y);

        BeginHorizontal(rect);

        EditorGUI.LabelField(GetHorizontalRect(IndexWidth + 3.0f), "Index", KMGUIStyles.HeaderButton);

        float headerFieldWidth = (rect.width - IndexWidth - ContentPaddingRight + 1.0f) / fieldsInfo.Length;

        for (int i = 0; i < fieldsInfo.Length; i++)
        {
            BeginGUIColor(GetTypeColor(fieldsInfo[i].FieldType));

            Rect headerRect = GetHorizontalRect(headerFieldWidth);

            if (GUI.Button(headerRect, fieldsInfo[i].Name, KMGUIStyles.HeaderButton))
            {

            }

            EndGUIColor();
        }

        EndHorizontal();
    }

    /// <summary>
    /// Draws the table element.
    /// </summary>
    /// <param name="rect">The rect.</param>
    /// <param name="index">The index.</param>
    /// <param name="isActive">if set to <c>true</c> [is active].</param>
    /// <param name="isFocused">if set to <c>true</c> [is focused].</param>
    /// <param name="reorderableList">The reorderable list.</param>
    /// <param name="fieldsInfo">The fields information.</param>
    private static void DrawTableElement(Rect rect, int index, bool isActive, bool isFocused, ReorderableList reorderableList, FieldInfo[] fieldsInfo)
    {
        if (index >= reorderableList.count)
        {
            return;
        }

        bool isSearch = false;
        if (stringSearchByList != null && stringSearchByList.Contains(index))
        {
            isSearch = true;
        }

        BeginHorizontal(rect);

        Color col = GUI.backgroundColor;
        if (isSearch)
        {
            GUI.backgroundColor = Color.green;
        }

        float prefixLabelWidth = IndexWidth - EditorGUIUtility.singleLineHeight;

        EditorGUI.HandlePrefixLabel(rect, GetHorizontalRect(prefixLabelWidth), new GUIContent(index.ToString()));

        float columnWidth = (rect.width - prefixLabelWidth - ContentPaddingRight) / fieldsInfo.Length;

        for (int i = 0; i < fieldsInfo.Length; i++)
        {
            EditorGUI.PropertyField(GetHorizontalRect(columnWidth),
                reorderableList.serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative(fieldsInfo[i].Name),
                GUIContent.none);

        }

        if (GUI.Button(GetHorizontalRect(ContentPaddingRight), "X", KMGUIStyles.MidBoldLabel))
        {
            reorderableList.serializedProperty.DeleteArrayElementAtIndex(index);
        }

        if (isSearch)
        {
            GUI.backgroundColor = col;
        }

        EndHorizontal();
    }

    /// <summary>
    /// Collection of short-cut of GUI styles used in KM.
    /// </summary>
    public static class KMGUIStyles
    {
        /// <summary>
        /// Gets the mid bold label style.
        /// </summary>
        /// <value>
        /// The mid bold label.
        /// </value>
        public static GUIStyle MidBoldLabel { get; private set; }

        /// <summary>
        /// Gets the header button style.
        /// </summary>
        /// <value>
        /// The header button.
        /// </value>
        public static GUIStyle HeaderButton { get; private set; }

        /// <summary>
        /// Initializes the <see cref="KMGUIStyles"/> class.
        /// </summary>
        static KMGUIStyles()
        {
            MidBoldLabel = new GUIStyle("Label");
            MidBoldLabel.alignment = TextAnchor.MiddleCenter;
            MidBoldLabel.fontStyle = FontStyle.Bold;

            HeaderButton = new GUIStyle("toolbarbutton");
            HeaderButton.alignment = TextAnchor.UpperCenter;
            HeaderButton.fontSize = 11;
        }
    }
}