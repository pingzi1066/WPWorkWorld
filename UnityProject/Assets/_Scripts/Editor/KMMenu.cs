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

    [MenuItem(StrsEditor.MENU_RoundPRS)]
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

    [MenuItem(StrsEditor.MENU_RoundPRSNChildren)]
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
    [MenuItem(StrsEditor.MENU_RoundScaleNChildren)]
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
    [MenuItem(StrsEditor.MENU_CopyLocalPosition)]
    public static void CopyLocalPosition()
    {
        GameObject go = Selection.activeGameObject;
        if (go != null)
        {
            localPos = go.transform.localPosition;
        }
    }
    [MenuItem(StrsEditor.MENU_PasteLocalPosition)]
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
    [MenuItem(StrsEditor.MENU_KMDebug)]
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
    [MenuItem(StrsEditor.MENU_KMEditor)]
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

    [MenuItem(StrsEditor.MENU_AUDIO_SET3D)]
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

    [MenuItem(StrsEditor.MENU_Time_Plus)]
    public static void TimeScalePlus()
    {
        Time.timeScale += 0.5f;
        Debug.Log("timeScale:" + Time.timeScale);
    }


    [MenuItem(StrsEditor.MENU_Time_Minus)]
    public static void TimeScaleMinus()
    {
        Time.timeScale -= 0.5f;
        Debug.Log("timeScale:" + Time.timeScale);
    }

    [MenuItem(StrsEditor.MENU_Time_Default)]
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
}


