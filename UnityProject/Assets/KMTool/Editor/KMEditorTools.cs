/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2017-03-02     WP      Initial version
 * 
 * *****************************************************************************/

using KMTool;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// 用于编辑器脚本
/// </summary>
static public class KMEditorTools 
{
    static public void RegisterUndo (string name, params Object[] objects)
	{
		if (objects != null && objects.Length > 0)
		{
			UnityEditor.Undo.RecordObjects(objects, name);

			foreach (Object obj in objects)
			{
				if (obj == null) continue;
				EditorUtility.SetDirty(obj);
			}
		}
	}
}