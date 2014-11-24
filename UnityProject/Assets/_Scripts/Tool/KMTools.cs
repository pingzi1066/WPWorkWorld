/******************************************************************************
 *
 * Maintaince Logs:
 * 2012-11-11   WP      Initial version. 
 * 2012-12-02   WP      Added methods of AddChild
 * 2012-12-15   WP      Added methods.(RoundToDecimal,RoundToDecByObj)
 * 2013-03-07   WP      Added methods.（FindInParents,FindActive)
 * 2013-03-29   Waigo   Added Decrypt.Encrypt
 * 2013-08-27   WP      Fixed Verison To U_4.2.0f
 * 2013-09-05   WP      Added AddItemToList^^^^^^^^^^^ ,two 
 * 2014-11-14   WP      Added string convert to Vector3
 * 
 * *****************************************************************************/

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Security.Cryptography;
using System.IO;

static public class KMTools
{

    /// <summary>
    /// Determines whether the 'parent' contains a 'child' in its hierarchy.
    /// </summary>

    static public bool IsChild(Transform parent, Transform child)
    {
        if (parent == null || child == null) return false;

        while (child != null)
        {
            if (child == parent) return true;
            child = child.parent;
        }
        return false;
    }

    #region Set active or deactivate

    /// <summary>
    /// Activate the specified object and all of its children.
    /// </summary>

    static void Activate(Transform t)
    {
        SetActiveSelf(t.gameObject, true);

        // Prior to Unity 4, active state was not nested. It was possible to have an enabled child of a disabled object.
        // Unity 4 onwards made it so that the state is nested, and a disabled parent results in a disabled child.
#if UNITY_3_5
		for (int i = 0, imax = t.GetChildCount(); i < imax; ++i)
		{
			Transform child = t.GetChild(i);
			Activate(child);
		}
#else
        // If there is even a single enabled child, then we're using a Unity 4.0-based nested active state scheme.
        // 这个是unity4.0的一个状态保存结构。我们不需要
        //for (int i = 0, imax = t.childCount; i < imax; ++i)
        //{
        //    Transform child = t.GetChild(i);
        //    if (child.gameObject.activeSelf) return;
        //}

        // If this point is reached, then all the children are disabled, so we must be using a Unity 3.5-based active state scheme.
        for (int i = 0, imax = t.childCount; i < imax; ++i)
        {
            Transform child = t.GetChild(i);
            Activate(child);
        }
#endif
    }

    /// <summary>
    /// Deactivate the specified object and all of its children.
    /// </summary>

    static void Deactivate(Transform t)
    {
#if UNITY_3_5
		for (int i = 0, imax = t.GetChildCount(); i < imax; ++i)
		{
			Transform child = t.GetChild(i);
			Deactivate(child);
		}
#endif
        SetActiveSelf(t.gameObject, false);
    }

    /// <summary>
    /// SetActiveRecursively enables children before parents. This is a problem when a widget gets re-enabled
    /// and it tries to find a panel on its parent.
    /// </summary>

    static public void SetActive(GameObject go, bool state)
    {
        if (state)
        {
            Activate(go.transform);
        }
        else
        {
            Deactivate(go.transform);
        }
    }

    /// <summary>
    /// Activate or deactivate children of the specified game object without changing the active state of the object itself.
    /// </summary>

    static public void SetActiveChildren(GameObject go, bool state)
    {
        Transform t = go.transform;

        if (state)
        {
            for (int i = 0, imax = t.childCount; i < imax; ++i)
            {
                Transform child = t.GetChild(i);
                Activate(child);
            }
        }
        else
        {
            for (int i = 0, imax = t.childCount; i < imax; ++i)
            {
                Transform child = t.GetChild(i);
                Deactivate(child);
            }
        }
    }

    /// <summary>
    /// Unity4 has changed GameObject.active to GameObject.activeself.
    /// </summary>

    static public bool GetActive(GameObject go)
    {
#if UNITY_3_5
        return go && go.active;
#else
        return go && go.activeInHierarchy;
#endif
    }

    /// <summary>
    /// Unity4 has changed GameObject.active to GameObject.SetActive.
    /// </summary>

    static public void SetActiveSelf(GameObject go, bool state)
    {
#if UNITY_3_5
        go.active = state;
#else
        go.SetActive(state);
#endif
    }

    #endregion

    #region Add GameObject or Component

    static public T AddChild<T>(GameObject parent, T prefab, bool isChangeLayer = true, bool isPreSize = false) where T : Component
    {
        T go = Component.Instantiate(prefab) as T;

        if (go != null && parent != null)
        {
            Transform t = go.gameObject.transform;
            t.parent = parent.transform;
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
            if (!isPreSize) t.localScale = Vector3.one;
            else t.localScale = prefab.transform.localScale;
            //else t.localScale = prefab.transform.localScale;
            if (isChangeLayer) SetLayer(go.gameObject, parent.layer);
        }
        return go;
    }

    /// <summary>
    /// 添加一个带有组件的物品在世界中
    /// </summary>
    /// <typeparam name="T">组件类型</typeparam>
    /// <param name="parent">父对象</param>
    /// <param name="prefab">Prefab</param>
    /// <param name="isChangeLayer">是否改变layer</param>
    /// <param name="isPosAndRot">是否重置世界坐标与旋转，false则为已经调好的相对父对象坐标和旋转</param>
    /// <param name="isPreSize">是否保留为Prefab的大小</param>
    /// <returns>返回这个物品</returns>
    static public T AddChild<T>(GameObject parent, T prefab, bool isChangeLayer, bool isPosAndRot, bool isPreSize) where T : Component
    {
        T go = Component.Instantiate(prefab) as T;

        if (go != null && parent != null)
        {
            Transform t = go.gameObject.transform;
            t.parent = parent.transform;
            if (isPosAndRot)
            {
                t.localPosition = Vector3.zero;
                t.localRotation = Quaternion.identity;
            }
            else
            {
                t.localPosition = prefab.transform.localPosition;
                t.localRotation = prefab.transform.localRotation;
            }
            if (!isPreSize) t.localScale = Vector3.one;
            if (isChangeLayer) SetLayer(go.gameObject, parent.layer);
        }
        return go;
    }

    /// <summary>
    /// 添加一个GameObject 的Prefab
    /// </summary>
    static public GameObject AddGameObj(GameObject parent, GameObject prefab, bool isChangeLayer, bool isPreSize = false)
    {
        GameObject go = GameObject.Instantiate(prefab) as GameObject;

        if (go != null && parent != null)
        {
            Transform t = go.transform;
            t.parent = parent.transform;
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
            if (!isPreSize) t.localScale = Vector3.one;
            if (isChangeLayer) SetLayer(go.gameObject, parent.layer);
        }
        return go;
    }

    /// <summary>
    /// 添加一个新的GameObject
    /// </summary>
    static public GameObject AddGameObj(GameObject parent)
    {
        GameObject go = new GameObject();

        if (go != null && parent != null)
        {
            Transform t = go.transform;
            t.parent = parent.transform;
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
            t.localScale = Vector3.one;
            SetLayer(go.gameObject, parent.layer);
        }
        return go;
    }

    //static public T AddChild<T>(GameObject parent, bool isChangeLayer) where T : Component
    //{
    //    GameObject go = new GameObject();
    //    if (parent != null)
    //    {
    //        Transform t = go.transform;
    //        t.parent = parent.transform;
    //        t.localPosition = Vector3.zero;
    //        t.localRotation = Quaternion.identity;
    //        t.localScale = Vector3.one;
    //        if (isChangeLayer) SetLayer(go.gameObject, parent.layer);
    //    }
    //    T self = go.AddComponent<T>();
    //    return self;
    //}

    #endregion

    /// <summary>
    /// Recursively set the game object's layer.
    /// </summary>

    static public void SetLayer(GameObject go, int layer)
    {
        go.layer = layer;

        Transform t = go.transform;

        for (int i = 0, imax = t.childCount; i < imax; ++i)
        {
            Transform child = t.GetChild(i);
            SetLayer(child.gameObject, layer);
        }
    }

    /// <summary>
    /// Will a floating-point point accurately to small points
    /// 将一个数精确到小数点后几位
    /// </summary>
    /// <param name="param">参数</param>
    /// <param name="toInt">小数点后几位</param>
    /// <returns></returns>
    public static float RoundToDecimal(float param, int toInt)
    {
        if (toInt < 0) return param;
        param = Mathf.RoundToInt(param * Mathf.Pow(10, toInt)) / Mathf.Pow(10, toInt);
        return param;
    }

    /// <summary>
    /// Round position and rotation and localScale of one GameObject!
    /// 使对象局部坐标（除Z轴），自身旋转，自身缩放都四舍五入到整数
    /// </summary>
    public static void RoundToDecByObj(GameObject go)
    {
        RoundLocalPosition(go);
        RoundLocalEulerAngles(go);
        RoundLocalScale(go);
    }

    /// <summary>
    /// 使对象的自身坐标精确到整数。（Z精确到小数点后2位）
    /// </summary>
    /// <param name="go"></param>
    public static void RoundLocalPosition(GameObject go)
    {
        Vector3 pos = go.transform.localPosition;
        pos.x = Mathf.RoundToInt(pos.x);
        pos.y = Mathf.RoundToInt(pos.y);
        pos.z = RoundToDecimal(pos.z, 2);
        go.transform.localPosition = pos;
    }

    /// <summary>
    /// 使自身旋转精确到整数
    /// </summary>
    /// <param name="go"></param>
    public static void RoundLocalEulerAngles(GameObject go)
    {
        Vector3 rot = go.transform.localEulerAngles;
        rot.x = Mathf.RoundToInt(rot.x);
        rot.y = Mathf.RoundToInt(rot.y);
        rot.z = Mathf.RoundToInt(rot.z);
        go.transform.localEulerAngles = rot;
    }

    /// <summary>
    /// 使自身绽放精确到整数
    /// </summary>
    /// <param name="go"></param>
    public static void RoundLocalScale(GameObject go)
    {
        Vector3 loc = go.transform.localScale;
        if (Mathf.RoundToInt(loc.x) != 0) loc.x = Mathf.RoundToInt(loc.x);
        if (Mathf.RoundToInt(loc.y) != 0) loc.y = Mathf.RoundToInt(loc.y);
        if (Mathf.RoundToInt(loc.z) != 0) loc.z = Mathf.RoundToInt(loc.z);
        go.transform.localScale = loc;
    }

    /// <summary>
    /// Will z of localPosition of the GameObject Change to zPos
    /// </summary>
    public static void ChangeZOfLocalPosition(Transform t, float zPos = 0)
    {
        Vector3 pos = t.localPosition;
        pos.z = zPos;
        t.localPosition = pos;
    }

    /// <summary>
    /// ,odds = 1 ~ 100,such as %60 -> odds = 60 .机率百分比
    /// </summary>
    public static bool OddsByInt(int odds)
    {
        if (odds > 100 || odds < 0) return false;
        if (UnityEngine.Random.Range(0, 10000) < odds * 100)
        {
            return true;
        }
        else return false;
    }

    /// <summary>
    /// 找到场景之中所有的组件
    /// </summary>
    static public T[] FindActive<T>() where T : Component
    {
        //return GameObject.FindSceneObjectsOfType(typeof(T)) as T[];
        return GameObject.FindObjectsOfType(typeof(T)) as T[];
    }

    /// <summary>
    /// 往上面找组件,包括自身
    /// </summary>
    static public T FindInParents<T>(GameObject go) where T : Component
    {
        if (go == null) return null;
        object comp = go.GetComponent<T>();

        if (comp == null)
        {
            Transform t = go.transform.parent;

            while (t != null && comp == null)
            {
                comp = t.gameObject.GetComponent<T>();
                t = t.parent;
            }
        }
        return (T)comp;
    }

    /// <summary>
    /// 改变这个物品的大小到世界大小
    /// </summary>
    /// <param name="go">物品</param>
    /// <param name="size">在世界中的大小</param>
    /// <returns></returns>
    static public GameObject ChangeScaleInWorld(GameObject go, Vector3 size)
    {
        Transform parent = go.transform.parent;
        go.transform.parent = null;
        go.transform.localScale = size;
        go.transform.parent = parent;
        return go;
    }

    /// <summary> 
    /// 加密字符串 
    /// 注意:密钥必须为８位 
    /// </summary> 
    /// <param name="strText">字符串</param> 
    /// <param name="encryptKey">密钥</param> 
    /// <param name="encryptKey">返回加密后的字符串</param> 
    public static string Encrypt(string inputString, string encryptKey)
    {
        byte[] byKey = null;
        byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        try
        {
            byKey = System.Text.Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.UTF8.GetBytes(inputString);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Convert.ToBase64String(ms.ToArray());
        }
        catch (System.Exception error)
        {
            Debug.Log(error);
            //return error.Message; 
            return null;
        }
    }

    /// <summary> 
    /// 解密字符串 
    /// </summary> 
    /// <param name="this.inputString">加了密的字符串</param> 
    /// <param name="decryptKey">密钥</param> 
    /// <param name="decryptKey">返回解密后的字符串</param> 
    public static string Decrypt(string inputString, string decryptKey)
    {
        byte[] byKey = null;
        byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        byte[] inputByteArray = new Byte[inputString.Length];
        try
        {
            byKey = System.Text.Encoding.UTF8.GetBytes(decryptKey.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            inputByteArray = Convert.FromBase64String(inputString);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            System.Text.Encoding encoding = new System.Text.UTF8Encoding();
            return encoding.GetString(ms.ToArray());
        }
        catch (System.Exception error)
        {
            Debug.Log(error);
            //return error.Message; 
            return null;
        }
    }

    /// <summary>
    /// 设置3D的音效
    /// </summary>
    /// <param name="audio"></param>
    public static void SetAudioTo3D(AudioSource audio)
    {
        audio.playOnAwake = false;
        audio.minDistance = 0.5f;
        audio.maxDistance = 10f;
        audio.bypassEffects = true;
        audio.rolloffMode = AudioRolloffMode.Linear;
    }

    /// <summary>
    /// vector3.ToString() convert to vector3;
    /// </summary>
    /// <param name="rString"></param>
    /// <returns></returns>
    public static Vector3 StringToVector3(string rString)
    {
        string[] temp = rString.Substring(1, rString.Length - 2).Split(',');
        float x = float.Parse(temp[0]);
        float y = float.Parse(temp[1]);
        float z = float.Parse(temp[2]);
        Vector3 rValue = new Vector3(x, y, z);
        return rValue;
    }
}
