using UnityEditor;
using UnityEngine;
using System;
using System.IO;

/// <summary>
/// Auto delete empty folders
/// 
/// Maintaince Logs:
/// 2014-12-01  WP      Initial version. 
/// </summary>
public class DeleteEmptyFolders : AssetPostprocessor
{
    public const string TITLE_AREYOUSURE = "你确定吗";
    public const string DESC_DELETE_FOLDERS = "是否删除所有的空文件夹";

    static int numFoldersDeleted = 0;
    static int numFoldersChecked = 0;

    //delete floder by isDelete
    static bool isDelete = true;

    // runs this script automatically after asset processing is done (reloading), via AssetPostprocessor.OnPostprocessAllAssets	
    static void OnPostprocessAllAssets(String[] importedAssets, String[] deletedAssets, String[] movedAssets, String[] movedFromAssetPaths)
    {
        if (isDelete)
        {

            DeleteFolders();
            isDelete = false;
            return;

        }

        isDelete = true;
    }

    [MenuItem("Tools/其它/Delete Empty Folders")]// add item to menu
    static void DeleteFolders()
    {
        numFoldersChecked = 0;
        string[] dirs = Directory.GetDirectories("Assets");

        foreach (string dirPath in dirs)
        {
            if (Directory.GetFiles(dirPath).Length == 0 && Directory.GetDirectories(dirPath).Length == 0)
            {
                numFoldersChecked++;
            }
        }

        if (numFoldersChecked > 0)
            if (EditorUtility.DisplayDialog
                    (TITLE_AREYOUSURE,
                    DESC_DELETE_FOLDERS, "Yes", "No"))
            {
                //Debug.Log("Running DeleteEmptyFolders editor script...");	
                RemoveFolders("Assets");					// start recursive call from root of Assets folder
                ShowDeletedFolderCount();					// display output log of empty folders found
                AssetDatabase.Refresh();					// refresh project hierarchy window in Unity editor
            }
    }

    static void RemoveFolders(string path)			// recursive function	
    {
        string[] dirs = Directory.GetDirectories(path);

        foreach (string dirPath in dirs)
        {
            RemoveFolders(dirPath);					// recursive call, performing depth-first search

            // check if no files or folders inside the current path, to see if it's empty
            if (Directory.GetFiles(dirPath).Length == 0 && Directory.GetDirectories(dirPath).Length == 0)
            {
                //Debug.Log("Empty folder found!!!! called: " + dirPath);				
                Directory.Delete(dirPath);			// delete empty folder
                File.Delete(dirPath + ".meta");		// delete metafile also, if exists
                numFoldersDeleted++;
            }
        }
    }

    static void ShowDeletedFolderCount()
    {
        if (numFoldersDeleted > 0)
            Debug.Log("Empty folders deleted: " + numFoldersDeleted + ".  Folders checked: " + numFoldersChecked + ".");
        numFoldersDeleted = 0;						// reset counters for next run
        numFoldersChecked = 0;
    }
}
