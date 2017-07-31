/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2017-07-31     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
/// <summary>
/// Orthographic Camera - Same Width - Different Height
/// 以宽度为主来固定
/// </summary>
public class CameraFit : MonoBehaviour 
{
    public float orthographicSize = 5;

    [SerializeField] private Camera mc;
    private Camera mCam
    {
        get
        {
            return mc;
        }
        set
        {
            mc = value;
            if(mc)
            {
                orthographicSize = mc.orthographicSize / Screen.height * Screen.width;
                //Debug.Log("mc.orthographicSize " + mc.orthographicSize + " Screen.width " + Screen.width 
                //    + "Screen.height " + Screen.height);
            }
            Calc();
        }
    }

    // Use this for initialization
    void Start()
    {
        if(mCam == null) mCam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Application.isEditor && !Application.isPlaying) Calc();
    }

    void Calc()
    {
        if (mCam)
        {
            float size = orthographicSize / Screen.width * Screen.height;
            if(mCam.orthographicSize != size)
            {
                mCam.orthographicSize = size;
            }
        }
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