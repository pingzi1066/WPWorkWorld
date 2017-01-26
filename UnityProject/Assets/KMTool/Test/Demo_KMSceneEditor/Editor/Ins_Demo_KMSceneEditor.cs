/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2016-05-19     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using UnityEditor;
using System.Collections;
using KMTool;

namespace KMToolDemo
{
    [CustomEditor(typeof(Demo_KMSceneEditor),true)]
    public class Ins_Demo_KMSceneEditor : Editor 
    {
        
        Demo_KMSceneEditor script;

        void OnEnable()
        {
            script = target as Demo_KMSceneEditor;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }

        void OnSceneGUI()
        {
            if (Selection.activeGameObject == script.gameObject)
            {
                KMSceneEditor.DrawAxis(script.objs);

                KMSceneEditor.DrawName(script.center);
            }
        }
    }
}