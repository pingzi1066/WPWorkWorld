/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2016-10-27     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;

/// <summary>
/// 描述
/// </summary>
public class ImportGlobalParms : AssetPostprocessor
{
    // runs this script automatically after asset processing is done (reloading), via AssetPostprocessor.OnPostprocessAllAssets 
    static void OnPostprocessAllAssets(String[] importedAssets, String[] deletedAssets, String[] movedAssets, String[] movedFromAssetPaths)
    {
        InitGlobalParms(importedAssets);
        InitGlobalParms(deletedAssets, false);
    }

    static void InitGlobalParms(String[] paths, bool isAdd = true)
    {
//        string log = "";

        if (paths.Length > 0)
        {
            foreach (String path in paths)
            {
//                log += path + "\n";
                //判断是否文件
                int index = path.LastIndexOf(".");

                if (index == -1)
                {
                    continue;
                }

                string file = path.Substring(index);

                if (file == ".asset")
                {
                    ScriptableObject so = AssetDatabase.LoadMainAssetAtPath(path) as ScriptableObject;
                    if (so != null)
                    {
                        if (so is ScriptableObjectFloatParms)
                        {
                            if (!isAdd)
                                RemoveFloat(so as ScriptableObjectFloatParms);
                            else
                                AddFloatFile(so as ScriptableObjectFloatParms);
                        }
                        if (so is ScriptableObjectIntParms)
                        {
                            if (!isAdd)
                                RemoveInt(so as ScriptableObjectIntParms);
                            else
                                AddIntFile(so as ScriptableObjectIntParms);
                        }
                    }
                    else
                    {
                        if(!isAdd)
                            GlobalParms.RefreshRes();
                    }
                }
            }
                
//            Debug.Log(log);

        }
        else
        {
//            Debug.Log("Don't assets change ");
        }
    }

    static void AddIntFile(ScriptableObjectIntParms obj)
    {
        GlobalParms.AddIntObj(obj);
    }

    static void AddFloatFile(ScriptableObjectFloatParms obj)
    {
        GlobalParms.AddFloatObj(obj);
    }

    static void RemoveInt(ScriptableObjectIntParms obj)
    {
        GlobalParms.RemoveIntObj(obj);
    }

    static void RemoveFloat(ScriptableObjectFloatParms obj)
    {
        GlobalParms.RemoveFloatObj(obj);
    }

}