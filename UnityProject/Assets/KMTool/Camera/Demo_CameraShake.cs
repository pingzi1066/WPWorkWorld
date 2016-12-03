/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2016-11-27     WP      Initial version
 * 
 * *****************************************************************************/

using KMTool;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// 描述
/// </summary>
public class Demo_CameraShake : MonoBehaviour 
{
    public CameraCtrl.ShakeParams parms;

    [SerializeField] private Text textInfo;

    // Use this for initialization
    void Start()
    {
        parms = new CameraCtrl.ShakeParams();
        textInfo.text = "";
    }

    public void BtnShake()
    {
        CameraCtrl.Shake(parms,ShakeFinished);
        textInfo.text = "Shaking " + parms.name;
    }

    private void ShakeFinished()
    {
        textInfo.text = "Shake finished!!!";
    }

    #region 测试

    public void KMDebug()
    {
        Debug.Log(" ---------KMDebug----------", gameObject);
    }

    public void KMEditor()
    {
        Debug.Log(" ---------KMEditor----------", gameObject);
    }

    #endregion
}