/******************************************************************************
 *
 * Maintaince Logs:
 * 2012-12-15    WP   Initial version. 
 *
 * *****************************************************************************/

using UnityEngine;
using UnityEditor;
using System.Collections;

[ExecuteInEditMode]
public class KMMenu : MonoBehaviour
{
    static Vector3 objPos = Vector3.zero;
    static Quaternion objRot = Quaternion.identity;

    static Vector3 backPos = Vector3.zero;
    static Quaternion backRot = Quaternion.identity;
    static GameObject backObj = null;
    /// <summary>
    /// copy Position and rotation of the selected gameobject
    /// </summary>
    [MenuItem("108KM/Copy Position and Rotation %#c")]
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
    [MenuItem("108KM/Paste Position and Rotation %#d")]
    public static void PastePosAndRot()
    {
        GameObject go = Selection.activeGameObject;
        if (go != null)
        {
            if (go != backObj)
            {
                backPos = go.transform.position;
                backRot = go.transform.rotation;
                backObj = go;
            }
            go.transform.position = objPos;
            go.transform.rotation = objRot;
        }
        else
        {
            Debug.Log("you need select one object");
        }
    }

    /// <summary>
    /// Only a step back
    /// </summary>
    [MenuItem("108KM/Only a step back %#z")]
    public static void BackPaste()
    {
        if (backObj != null)
        {
            backObj.transform.position = backPos;
            backObj.transform.rotation = backRot;
        }
    }

    [MenuItem("108KM/Round local Z精确到小数点后2位 %#r")]
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

    [MenuItem("108KM/子对象下所有物品的PRS坐标精确到整数，pos.z为小数点后两位 ")]
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
    [MenuItem("108KM/子对象下所有物品的大小精确到整数 ")]
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
    [MenuItem("108KM/复制局部坐标 ")]
    public static void CopyLocalPosition()
    {
        GameObject go = Selection.activeGameObject;
        if (go != null)
        {
            localPos = go.transform.localPosition;
        }
    }
    [MenuItem("108KM/粘贴局部坐标 ")]
    public static void Paste()
    {
        GameObject go = Selection.activeGameObject;
        if (go != null)
        {
            go.transform.localPosition = localPos;
        }
    }

    /// <summary>
    /// 这个专用于调试
    /// </summary>
    [MenuItem("108KM/send to DebugLog to obj %#l")]
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
    [MenuItem("108KM/KM Editor %#k")]
    public static void KMEditor()
    {
        GameObject go = Selection.activeGameObject;
        if (go != null)
        {
            go.SendMessage("KMEditor", SendMessageOptions.DontRequireReceiver);
        }
    }

}
