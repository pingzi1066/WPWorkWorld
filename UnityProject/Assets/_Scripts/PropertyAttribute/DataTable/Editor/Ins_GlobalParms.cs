/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2016-07-21     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

//[CustomEditor(typeof(GlobalParms))]
public class Ins_GlobalParms : Editor
{
    //GlobalParms parms;

    //string search = "";

    void OnEnable()
    {
        //parms = target as GlobalParms;
    }
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        //return;

        //List<GlobalParms.Parms> list = parms.listParms;
        //search = EditorGUILayout.TextField("Search", search);

        //GUILayout.BeginHorizontal();
        //parms.nameLenght = EditorGUILayout.FloatField("Name Width", parms.nameLenght);
        //parms.valueLenght = EditorGUILayout.FloatField("Value Width", parms.valueLenght);
        //GUILayout.EndHorizontal();


        //GUILayout.BeginHorizontal();
        //EditorGUILayout.LabelField("Name", GUILayout.Width(parms.nameLenght));
        //EditorGUILayout.LabelField("Desc");
        //EditorGUILayout.LabelField("Value", GUILayout.Width(parms.valueLenght));
        //GUILayout.EndHorizontal();

        //for (int i = 0; i < list.Count; i++)
        //{
        //    if (!string.IsNullOrEmpty(search))
        //    {
        //        if (!(list[i].desc.Contains(search) || list[i].name.Contains(search)))
        //        {
        //            continue;
        //        }
        //    }

        //    GUILayout.BeginHorizontal();

        //    list[i].name = EditorGUILayout.TextField(list[i].name, GUILayout.Width(parms.nameLenght));
        //    list[i].desc = EditorGUILayout.TextField(list[i].desc);
        //    list[i].value = EditorGUILayout.FloatField(list[i].value, GUILayout.Width(parms.valueLenght));

        //    if (KMGUI.BtnDelete())
        //    {
        //        list.RemoveAt(i);
        //    }

        //    GUILayout.EndHorizontal();
        //}

        //if (KMGUI.Button("Add", Color.green))
        //{
        //    GlobalParms.Parms parm = new GlobalParms.Parms();

        //    parm.name = "NewParm";
        //    parm.desc = "Desc!!!";

        //    list.Add(parm);
        //}

        //if (GUI.changed)
        //{
        //    EditorUtility.SetDirty(parms);
        //}
    }
}