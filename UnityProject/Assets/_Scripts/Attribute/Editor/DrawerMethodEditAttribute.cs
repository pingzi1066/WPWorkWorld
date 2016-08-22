/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2016-08-05     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

[CustomPropertyDrawer(typeof(MethodAttribute))]
public class DrawerMethodEditAttribute : DecoratorDrawer
{
    public override void OnGUI(Rect position)
    {
        base.OnGUI(position);
        //Debug.Log(
    }

    
    //public override void OnInspectorGUI()
    //{
    //    Debug.Log(target.GetType().ToString());
    //}
}