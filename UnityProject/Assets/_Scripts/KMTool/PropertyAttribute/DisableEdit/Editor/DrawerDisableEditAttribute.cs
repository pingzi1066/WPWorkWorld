using UnityEngine;
using UnityEditor;

namespace KMTool
{
    [CustomPropertyDrawer(typeof(DisableEdit))]
    public class DrawerDisableEditAttribute : PropertyDrawer 
    {
    	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
    	{
    		GUI.enabled = false;

    		EditorGUI.PropertyField (position, property);
    	}
    }
}