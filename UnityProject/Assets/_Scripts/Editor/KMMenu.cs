﻿/******************************************************************************
 *
 * Maintaince Logs:
 * 2012-12-15    WP   Initial version. 
 *
 * *****************************************************************************/

using UnityEngine;
using UnityEditor;
using UnityEngineInternal;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[ExecuteInEditMode]
public class KMMenu : MonoBehaviour
{
    const string MENU_HEAD = "Tools/";
    const string MENU_TRANSFORM = MENU_HEAD + "变换/";

    const string MENU_COPY_POS_ROT = MENU_TRANSFORM + "复制世界坐标与旋转 %#c";
    const string MENU_PASTE_POS_ROT = MENU_TRANSFORM + "粘贴世界坐标与旋转 %#d";
    const string MENU_RoundPRS = MENU_TRANSFORM + "使自身PRS变为整数 %#r";
    const string MENU_RoundPRSNChildren = MENU_TRANSFORM + "子对象下所有物品的PRS坐标精确到整数";
    const string MENU_RoundScaleNChildren = MENU_TRANSFORM + "子对象下所有物品的大小精确到整数";
    const string MENU_CopyLocalPosition = MENU_TRANSFORM + "复制局部坐标 %#x";
    const string MENU_PasteLocalPosition = MENU_TRANSFORM + "粘贴局部坐标 %#v";

    const string MENU_TEST = MENU_HEAD + "测试/";

    const string MENU_KMDebug = MENU_TEST + "SendMessage方法KMDebug到对象 %#l";
    const string MENU_KMEditor = MENU_TEST + "SendMessage方法KMEditor到对象 %#k";

    const string MENU_Other = MENU_HEAD + "其它/";
    const string MENU_AUDIO_SET3D = MENU_Other + "设置为默认3D音效";

    const string MENU_Time = MENU_HEAD + "时间/";
    const string MENU_Time_Plus = MENU_Time + "游戏时间递增 %#=";
    const string MENU_Time_Minus = MENU_Time + "游戏时间递减 %#-";
    const string MENU_Time_Default = MENU_Time + "游戏时间默认";

    const string MENU_Other_RemoveMissingScript = MENU_Other + "删除所选对象空脚本（需手动移除）";


    #region 坐标旋转绽放 PRS

    static Vector3 objPos = Vector3.zero;
    static Quaternion objRot = Quaternion.identity;

    /// <summary>
    /// copy Position and rotation of the selected gameobject
    /// </summary>
    [MenuItem(MENU_COPY_POS_ROT)]
    public static void CopyPosAndRot()
    {
        GameObject go = Selection.activeGameObject;
        if (go != null)
        {
            objPos = go.transform.position;
            objRot = go.transform.rotation;
        }
        else
        {
            Debug.Log("you need select one object");
        }
    }

    /// <summary>
    /// paste Position and rotation of the selected gameobject
    /// </summary>
    [MenuItem(MENU_PASTE_POS_ROT)]
    public static void PastePosAndRot()
    {
        GameObject go = Selection.activeGameObject;
        if (go != null)
        {
            go.transform.position = objPos;
            go.transform.rotation = objRot;
        }
        else
        {
            Debug.Log("you need select one object");
        }
    }

    [MenuItem(MENU_RoundPRS)]
    public static void RoundPRS()
    {
        GameObject go = Selection.activeGameObject;
        if (go != null)
        {
            KMTools.RoundToDecByObj(go);
        }
        else
        {
            Debug.Log("you need select one object");
        }
    }

    [MenuItem(MENU_RoundPRSNChildren)]
    public static void RoundLocalNChildren()
    {
        GameObject go = Selection.activeGameObject;
        Transform[] gos = go.GetComponentsInChildren<Transform>();
        //Debug.Log(gos.Length);
        for (int i = 0; i < gos.Length; i++)
        {
            KMTools.RoundToDecByObj(gos[i].gameObject);
        }
    }

    /// <summary>
    /// 使子对象下所有物品的Z绽放到整数
    /// </summary>
    [MenuItem(MENU_RoundScaleNChildren)]
    public static void RoundScaleNChildren()
    {
        GameObject go = Selection.activeGameObject;
        Transform[] gos = go.GetComponentsInChildren<Transform>();
        for (int i = 0; i < gos.Length; i++)
        {
            KMTools.RoundLocalScale(gos[i].gameObject);
        }
    }

    private static Vector3 localPos;
    [MenuItem(MENU_CopyLocalPosition)]
    public static void CopyLocalPosition()
    {
        GameObject go = Selection.activeGameObject;
        if (go != null)
        {
            localPos = go.transform.localPosition;
        }
    }
    [MenuItem(MENU_PasteLocalPosition)]
    public static void PasteLocalPosition()
    {
        GameObject go = Selection.activeGameObject;
        if (go != null)
        {
            go.transform.localPosition = localPos;
        }
    }

    #endregion

    #region 测试、编辑器方法发送 test

    /// <summary>
    /// 这个专用于调试
    /// </summary>
    [MenuItem(MENU_KMDebug)]
    public static void KMDebug()
    {
        GameObject go = Selection.activeGameObject;
        if (go != null)
        {
            go.SendMessage("KMDebug", SendMessageOptions.DontRequireReceiver);
        }
    }

    /// <summary>
    /// KM的游戏开发者快捷键，用这个可以在游戏没运行的时候运行代码段，覆盖了上面的方法 
    /// </summary>
    [MenuItem(MENU_KMEditor)]
    public static void KMEditor()
    {
        GameObject go = Selection.activeGameObject;
        if (go != null)
        {
            go.SendMessage("KMEditor", SendMessageOptions.DontRequireReceiver);
        }
    }

    #endregion

    #region Prefabs

    /// <summary>
    /// 创建Prefab到指定路径
    /// </summary>
    /// <param name="folderName"></param>
    [MenuItem("Tools/其它/创建Prefab")]
    static void CreatePrefabToFolder()
    {
        GameObject[] gos = Selection.gameObjects;
        string path = "";

        if (gos.Length > 0)
            path = EditorUtility.OpenFolderPanel("Save As", "Assets/", "");
        else return;

        string localPath = path.Substring((path.IndexOf("/Assets") + 1));


        foreach (GameObject go in gos)
        {
            localPath = localPath + "/" + go.name + ".prefab";
            Debug.Log(localPath);
            if (AssetDatabase.LoadAssetAtPath(localPath, typeof(GameObject)))
            {
                if (EditorUtility.DisplayDialog("Are you sure?", "The prefab already exists. Do you want to overwrite it?", "Yes", "No"))
                {
                    Object prefab = PrefabUtility.CreatePrefab(localPath, go);
                    PrefabUtility.ReplacePrefab(go, prefab, ReplacePrefabOptions.ConnectToPrefab);
                    Debug.Log("Replaced Prefab:" + localPath, go);
                }
            }
            else
            {
                Object prefab = PrefabUtility.CreatePrefab(localPath, go);
                PrefabUtility.ReplacePrefab(go, prefab, ReplacePrefabOptions.ConnectToPrefab);
                Debug.Log("Created Prefab:" + localPath, go);
            }

        }

        AssetDatabase.Refresh();
    }

    #endregion

    #region 音效 audio

    [MenuItem(MENU_AUDIO_SET3D)]
    public static void SetAudioTo3D()
    {
        GameObject[] gos = Selection.gameObjects;

        foreach (GameObject go in gos)
        {
            AudioSource audio = go.GetComponent<AudioSource>();
            if (!audio) audio = go.AddComponent<AudioSource>();
            KMTools.SetAudioTo3D(audio);
        }
    }

    #endregion

    #region 时间 TimeScale

    [MenuItem(MENU_Time_Plus)]
    public static void TimeScalePlus()
    {
        Time.timeScale += 0.5f;
        Debug.Log("timeScale:" + Time.timeScale);
    }


    [MenuItem(MENU_Time_Minus)]
    public static void TimeScaleMinus()
    {
        Time.timeScale -= 0.5f;
        Debug.Log("timeScale:" + Time.timeScale);
    }

    [MenuItem(MENU_Time_Default)]
    public static void TimeScaleDefault()
    {
        Time.timeScale = 1;
    }

    #endregion

    #region 粒子 particle

    //[MenuItem("Tools/粒子/TODO改变粒子的数量最大值为60个")]
    //public static void SetParticleCountFormParent()
    //{
    //    GameObject go = Selection.activeGameObject;
    //    ParticleSystem[] ps = go.GetComponentsInChildren<ParticleSystem>();
    //    foreach (ParticleSystem p in ps)
    //    {
    //        Debug.Log(p.name, p.gameObject);
    //    }
    //}

    #endregion

    #region 其它

    [MenuItem(MENU_Other_RemoveMissingScript)]
    public static void RemoveMissingScript()
    {
        //代码中无法Destroy，只能手动在编辑器里进行删除
        //有空脚本的对象
        List<GameObject> selection = new List<GameObject>();

        foreach (Transform select in Selection.transforms)
        {
            foreach (Component c in select.GetComponents(typeof(Component)))
            {
                if (c == null && !selection.Contains(select.gameObject))
                {
                    selection.Add(select.gameObject);
                    Debug.Log(" missing of script for gameobject", select);
                }
            }


            foreach (Transform t in select)
            {
                Component[] components = t.GetComponents<Component>();

                foreach (Component component in components)
                {
                    if (component == null && !selection.Contains(t.gameObject))
                    {
                        Debug.Log(" missing of script for gameobject", t);
                        selection.Add(t.gameObject);
                    }
                }
            }
        }

        //设置对象
        Selection.objects = selection.ToArray();
    }

    #endregion
}


