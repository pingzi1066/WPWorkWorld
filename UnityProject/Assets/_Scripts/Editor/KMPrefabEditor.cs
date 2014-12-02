using UnityEngine;
using UnityEditor;


/// <summary>
/// 提供 Prefab 的读取、保存、移动 
/// 
/// Maintaince Logs:
/// 2014-11-01      WP      Initial version
/// </summary>
public class KMPrefabEditor
{
    /// <summary>
    /// 保存到Prefab
    /// </summary>
    /// <param name="srcObject">场景中资源</param>
    /// <param name="tarPrefab">对应的Prefab</param>
    /// <returns></returns>

    public static GameObject SavePrefab(GameObject srcObject, Object tarPrefab)
    {
        if (srcObject == null || tarPrefab == null)
        {
            Debug.LogError("Save is null");
            return null;
        }

        if (PrefabUtility.GetPrefabType(tarPrefab) == PrefabType.ModelPrefab)
        {
            Debug.Log("Equal!~~~~~~");
            return null;
        }
        GameObject ret = PrefabUtility.ReplacePrefab(srcObject, tarPrefab, ReplacePrefabOptions.ConnectToPrefab);

        AssetDatabaseSaveAssets();
        return ret;
    }

    /// <summary>
    /// 
    /// </summary>

    public static void AssetDatabaseSaveAssets()
    {
        Debug.Log("save ----------------");

        AssetDatabase.SaveAssets();
        EditorApplication.SaveAssets();
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// Clone 一个Prefab 到场景中
    /// </summary>
    /// <param name="srcPrefab">Prefab</param>
    /// <param name="parnet">父对象</param>
    /// <returns></returns>

    public static GameObject LoadPrefab(Object srcPrefab, Transform parnet)
    {
        GameObject obj = PrefabUtility.InstantiatePrefab(srcPrefab) as GameObject;
        if (parnet)
            obj.transform.parent = parnet;

        return obj;
    }

    /// <summary>
    /// 复制资源到路径
    /// </summary>
    /// <param name="srcPrefab"></param>
    /// <param name="dstPath"></param>
    /// <returns></returns>
    public static string CreateDefaultUniquePrefab(GameObject srcPrefab, string dstPath)
    {
        string srcPath = AssetDatabase.GetAssetPath(srcPrefab);
        string UnqPath = AssetDatabase.GenerateUniqueAssetPath(dstPath);
        if (AssetDatabase.CopyAsset(srcPath, UnqPath))
        {
            AssetDatabase.Refresh();
            return UnqPath;
        }
        else
        {
            Debug.LogWarning("Copy Error !!!");
            return "";
        }
    }

    public static string ClonePrefab(Object srcPrefab, string newName = "")
    {
        string srcPath = AssetDatabase.GetAssetPath(srcPrefab);
        string UnqPath = AssetDatabase.GenerateUniqueAssetPath(srcPath);
        if (AssetDatabase.CopyAsset(srcPath, UnqPath))
        {
            AssetDatabase.Refresh();
            if (!string.IsNullOrEmpty(newName))
            {
                GameObject newGo = LoadPrefab(UnqPath);
                RenamePrefab(newGo, newName);
                Debug.Log(" clone new prefab success ", newGo);
            }
            AssetDatabase.Refresh();

            return UnqPath;
        }
        else
        {
            Debug.LogWarning("Copy Error !!!");
            return "";
        }
    }

    public static void RenamePrefab(GameObject srcPrefab, string dstName)
    {
        string path = AssetDatabase.GetAssetPath(srcPrefab);
        AssetDatabase.RenameAsset(path, dstName);
    }

    public static GameObject LoadPrefab(string strPrefabPath)
    {
        GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath(strPrefabPath, typeof(GameObject));
        return prefab;
    }
}
