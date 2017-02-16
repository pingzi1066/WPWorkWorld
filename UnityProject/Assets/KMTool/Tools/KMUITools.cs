/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2017-02-16     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// 描述
/// </summary>
public static class KMUITools
{
    static public void SetText(Text text, string str)
    {
        if (text)
            text.text = str;
    }
}
