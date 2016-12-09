/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2016-11-27     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;

namespace KMTool
{

    /// <summary>
    /// ResourcesPrefab 生成时设置
    /// </summary>
    public class Editor_ResourcesPrefab : AssetPostprocessor
    {
        
        static void OnPostprocessAllAssets(String[] importedAssets, String[] deletedAssets, String[] movedAssets, String[] movedFromAssetPaths)
        {
            Init(importedAssets);
        }

        static void Init(String[] paths, bool isAdd = true)
        {
            if (paths.Length > 0)
            {
                foreach (String path in paths)
                {
                    //判断是否文件
                    int index = path.LastIndexOf(".");

                    if (index == -1)
                    {
                        continue;
                    }

                    string file = path.Substring(index);
                    //   Assets/Resources/Prefab/ResourcesManager 1.prefab 
//                    Debug.Log(path);

                    if (file == ".prefab" && path.StartsWith("Assets/Resources/"))
                    {
                        GameObject go = AssetDatabase.LoadMainAssetAtPath(path) as GameObject;

                        if (go != null)
                        {
                            Type tp = typeof(ResourcesInstance<>);
                            MonoBehaviour[] all = go.GetComponents<MonoBehaviour>();

                            for (int i = 0; i < all.Length; i++)
                            {
                                Type baseType = all[i].GetType().BaseType;
                                if (IsAssignableToGenericType(baseType, tp))
                                {
//                                    Debug.Log(baseType + "  ------  " + tp + "  ");
//                                    Debug.Log(all[i].GetType().FullName);
                                    // Do this
                                    string pt = path.Replace("Assets/Resources/","").Replace(file,"");
                                    Add(all[i].GetType().FullName, pt);

                                    break;
                                }
                            }

                        }
                    }
                }

            }
        }

        static void Add(string key,string path)
        {
//            Debug.Log(path);
            ResourcesManager.AddItem(key, path);
            AssetDatabase.Refresh();
        }

        public static bool IsAssignableToGenericType(Type givenType, Type genericType)
        {
            var interfaceTypes = givenType.GetInterfaces();

            foreach (var it in interfaceTypes)
            {
                if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
                    return true;
            }

            if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
                return true;

            Type baseType = givenType.BaseType;
            if (baseType == null) return false;

            return IsAssignableToGenericType(baseType, genericType);
        }
    }
}