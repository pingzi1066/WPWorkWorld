
using UnityEngine;
using UnityEditor;

namespace KMTool
{

    [CustomEditor(typeof(TweenTransform))]
    public class TweenTransformEditor : UITweenerEditor
    {
        public override void OnInspectorGUI ()
	    {
            DrawProp("from");
            DrawProp("to");

            DrawCommonProperties();
        }
    }
}