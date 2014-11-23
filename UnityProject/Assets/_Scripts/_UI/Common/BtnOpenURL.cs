/******************************************************************************
 *
 * Maintaince Logs:
 * 2013-05-22   WP      Initial version. 
 *
 * *****************************************************************************/

using UnityEngine;
using System.Collections;

/// <summary>
/// 打开链接的按钮
/// </summary>
public class BtnOpenURL : MonoBehaviour
{

    public string url = "";

    void OnClick()
    {
        if (url.Length > 6)
        {
            if (url.Contains("http://"))
            {
                Application.OpenURL(url);
            }
            else
            {
                Application.OpenURL("http://" + url);
            }
        }
    }
}
