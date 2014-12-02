/******************************************************************************
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
    #region 坐标旋转绽放 PRS

    static Vector3 objPos = Vector3.zero;
    static Quaternion objRot = Quaternion.identity;

    /// <summary>
    /// copy Position and rotation of the selected gameobject
    /// </summary>
    [MenuItem(StrsEditor.MENU_COPY_POS_ROT)]
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
    [MenuItem(StrsEditor.MENU_PASTE_POS_ROT)]
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

    [MenuItem("Tools/变换/使自身PRS变为整数，pos.z为小数点后两位 %#r")]
    public static void RoundPosAndRot()
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

    [MenuItem("Tools/变换/子对象下所有物品的PRS坐标精确到整数，pos.z为小数点后两位 ")]
    public static void RoundLocal()
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
    [MenuItem("Tools/变换/子对象下所有物品的大小精确到整数 ")]
    public static void RoundLocalscaleOfObjs()
    {
        GameObject go = Selection.activeGameObject;
        Transform[] gos = go.GetComponentsInChildren<Transform>();
        for (int i = 0; i < gos.Length; i++)
        {
            KMTools.RoundLocalScale(gos[i].gameObject);
        }
    }

    private static Vector3 localPos;
    [MenuItem("Tools/变换/复制局部坐标 ")]
    public static void CopyLocalPosition()
    {
        GameObject go = Selection.activeGameObject;
        if (go != null)
        {
            localPos = go.transform.localPosition;
        }
    }
    [MenuItem("Tools/变换/粘贴局部坐标 ")]
    public static void Paste()
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
    [MenuItem("Tools/测试/SendMessage方法KMDebug到对象 %#l")]
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
    [MenuItem("Tools/测试/SendMessage方法KMEditor到对象 %#k")]
    public static void KMEditor()
    {
        GameObject go = Selection.activeGameObject;
        if (go != null)
        {
            go.SendMessage("KMEditor", SendMessageOptions.DontRequireReceiver);
        }
    }

    #endregion

    #region 寻路 Navigation

    [MenuItem("Tools/寻路/所选->静态寻路对象(包括OffMeshLinkGeneration)")]
    public static void SetToNavigationStatic()
    {
        GameObject[] gos = Selection.gameObjects;

        foreach (GameObject go in gos)
        {
            go.isStatic = true;
            var newFlags = StaticEditorFlags.BatchingStatic |
                StaticEditorFlags.LightmapStatic |
                    StaticEditorFlags.NavigationStatic |
                     StaticEditorFlags.OccluderStatic |
                      StaticEditorFlags.OccludeeStatic |
                       StaticEditorFlags.OffMeshLinkGeneration;
            GameObjectUtility.SetStaticEditorFlags(go, newFlags);
            Debug.Log("静态寻路对象  :  " + go.name, go);
        }
    }

    [MenuItem("Tools/寻路/所选->静态寻路对象(不包括OffMeshLinkGeneration)")]
    public static void SetToNavigationStaticAndWithOutOffMeshLinkGeneration()
    {
        GameObject[] gos = Selection.gameObjects;

        foreach (GameObject go in gos)
        {
            go.isStatic = true;
            var newFlags = StaticEditorFlags.BatchingStatic |
                StaticEditorFlags.LightmapStatic |
                    StaticEditorFlags.NavigationStatic |
                     StaticEditorFlags.OccluderStatic |
                      StaticEditorFlags.OccludeeStatic;
            GameObjectUtility.SetStaticEditorFlags(go, newFlags);
            Debug.Log("静态寻路对象  :  " + go.name, go);
        }
    }

    [MenuItem("Tools/寻路/所选->静态非寻路对象(不包括OffMeshLinkGeneration)")]
    public static void SetToWithOutNavigationStatic()
    {
        GameObject[] gos = Selection.gameObjects;

        foreach (GameObject go in gos)
        {
            go.isStatic = true;
            var newFlags = StaticEditorFlags.BatchingStatic |
                StaticEditorFlags.LightmapStatic |
                     StaticEditorFlags.OccluderStatic |
                      StaticEditorFlags.OccludeeStatic;
            GameObjectUtility.SetStaticEditorFlags(go, newFlags);
            Debug.Log("静态非寻路对象  :  " + go.name, go);
            //StaticEditorFlags.BatchingStatic
        }
    }

    #endregion

    #region 标签 Label

    [MenuItem("Tools/标签/所选->_Ground")]
    public static void AddLabelForGround()
    {
        string[] labels = new string[] { "_Ground" };
        AddLabelToPrefab(labels);
    }

    [MenuItem("Tools/标签/所选->_Obstacle")]
    public static void AddLabelForObstacle()
    {
        string[] labels = new string[] { "_Obstacle" };
        AddLabelToPrefab(labels);
    }

    [MenuItem("Tools/标签/所选->_Deco")]
    public static void AddLabelForDeco()
    {
        string[] labels = new string[] { "_Deco" };
        AddLabelToPrefab(labels);
    }

    [MenuItem("Tools/标签/所选->_Wall")]
    public static void AddLabelForWall()
    {
        string[] labels = new string[] { "_Wall" };
        AddLabelToPrefab(labels);
    }

    [MenuItem("Tools/标签/所选->_Particles")]
    public static void AddLabelForParticles()
    {
        string[] labels = new string[] { "_Particles" };
        AddLabelToPrefab(labels);
        //_Particles
    }

    [MenuItem("Tools/标签/所选->_Group")]
    public static void AddLabelForGroup()
    {
        string[] labels = new string[] { "_Group" };
        AddLabelToPrefab(labels);
    }

    [MenuItem("Tools/标签/所选->清空所有Labels")]
    public static void ClearLabelsForPrefab()
    {
        GameObject[] gos = Selection.gameObjects;
        foreach (GameObject go in gos)
        {
            AssetDatabase.ClearLabels(go);
        }
        AssetDatabase.Refresh();
    }

    static void AddLabelToPrefab(string[] labels)
    {
        GameObject[] gos = Selection.gameObjects;
        foreach (GameObject go in gos)
        {
            AssetDatabase.SetLabels(go, labels);
        }
        AssetDatabase.Refresh();
    }

    #endregion

    #region Prefabs

    const string prefabPath = "Assets/_Prefabs/_SceneObject/";

    /// <summary>
    /// 创建Prefab到指定路径
    /// </summary>
    /// <param name="folderName"></param>
    static void CreatePrefabToFolder(string folderName)
    {
        GameObject[] gos = Selection.gameObjects;

        foreach (GameObject go in gos)
        {
            string localPath = prefabPath + folderName + go.name + ".prefab";
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

    [MenuItem("Tools/Prefabs/Tip:打印出所会创建的目录")]
    public static void PathTipForLabel()
    {
        GameObject[] gos = Selection.gameObjects;

        foreach (GameObject go in gos)
        {
            Debug.Log("所选 -> " + go.name, go);
        }
        Debug.Log("将被创建到：" + prefabPath);
    }

    [MenuItem("Tools/Prefabs/所选->创建到Ground")]
    public static void CreatePrefabsToGround()
    {
        CreatePrefabToFolder("Ground/");
    }

    [MenuItem("Tools/Prefabs/所选->创建到Obstacle")]
    public static void CreatePrefabsToObstacle()
    {
        CreatePrefabToFolder("Obstacle/");
    }

    [MenuItem("Tools/Prefabs/所选->创建到Deco")]
    public static void CreatePrefabsToDeco()
    {
        CreatePrefabToFolder("Deco/");
    }

    [MenuItem("Tools/Prefabs/所选->创建到Wall")]
    public static void CreatePrefabsToWall()
    {
        CreatePrefabToFolder("Wall/");
    }

    [MenuItem("Tools/Prefabs/所选->创建到ElementGround")]
    public static void CreatePrefabsToElementGround()
    {
        CreatePrefabToFolder("ElementGround/");
    }

    [MenuItem("Tools/Prefabs/所选->创建到ElementObstacle")]
    public static void CreatePrefabsToElementObstacle()
    {
        CreatePrefabToFolder("ElementObstacle/");
    }

    [MenuItem("Tools/Prefabs/所选->创建到ElementDeco")]
    public static void CreatePrefabsToElementDeco()
    {
        CreatePrefabToFolder("ElementDeco/");
    }

    [MenuItem("Tools/Prefabs/所选->创建到Group")]
    public static void CreatePrefabsToGroup()
    {
        CreatePrefabToFolder("Group/");
    }

    [MenuItem("Tools/Prefabs/所选->创建到Wait")]
    public static void CreatePrefabsToWait()
    {
        CreatePrefabToFolder("Wait/");
    }

    #endregion

    #region 音效 audio

    [MenuItem("Tools/音效/将所选设置为Tools默认3D音效")]
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

    [MenuItem("Tools/时间/游戏时间递增 %#=")]
    public static void TimeScalePlus()
    {
        Time.timeScale += 0.5f;
        Debug.Log("timeScale:" + Time.timeScale);
    }


    [MenuItem("Tools/时间/游戏时间递减 %#-")]
    public static void TimeScaleMinus()
    {
        Time.timeScale -= 0.5f;
        Debug.Log("timeScale:" + Time.timeScale);
    }

    [MenuItem("Tools/时间/游戏时间默认")]
    public static void TimeScaleDefault()
    {
        Time.timeScale = 1;
    }

    #endregion

    #region 粒子 particle

    [MenuItem("Tools/粒子/TODO改变粒子的数量最大值为60个")]
    public static void SetParticleCountFormParent()
    {
        GameObject go = Selection.activeGameObject;
        ParticleSystem[] ps = go.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem p in ps)
        {
            Debug.Log(p.name, p.gameObject);
        }
    }

    #endregion
}


