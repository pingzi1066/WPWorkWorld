/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2015-11-26     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using UnityEditor;
using System.Collections;

public class ScriptImportEditor : UnityEditor.AssetModificationProcessor
{
    public static void OnWillCreateAsset(string path)
    {
        path = path.Replace(".meta", "");
        int index = path.LastIndexOf(".");
        if (index == -1)
        {
            return;
        }
        //Debug.Log("---" + path + "  index: " + index);
        string file = path.Substring(index);
        if (file != ".cs" && file != ".js" && file != ".boo") return;

        index = Application.dataPath.LastIndexOf("Assets");
        path = Application.dataPath.Substring(0, index) + path;
        file = System.IO.File.ReadAllText(path);

        file = file.Replace("#CREATIONDATE#", System.DateTime.Now.ToString("yyyy-MM-dd"));

        System.IO.File.WriteAllText(path, file);
        AssetDatabase.Refresh();
    }
}