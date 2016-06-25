/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2016-06-25     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using System.Collections;

/// <summary>
/// 视窗射线
/// </summary>
public class TestConvertPos : MonoBehaviour 
{
    [SerializeField]
    private Camera cam2;

    [SerializeField]
    private Camera mainCam;

    [SerializeField]
    private GameObject mView;
    private GameObject goView
    {
        get
        { 
            if (mView == null)
            {
                mView = KMTools.AddGameObj(gameObject);
                mView.name = "view";
            }

            return mView;
        }
    }

    public bool isThreeToTwo = true;

	// Use this for initialization
	void Start() 
	{
	
	}
	
	// Update is called once per frame
	void Update() 
	{
	
    }

    protected void OnDrawGizmos()
    {
        if (mainCam && cam2)
        {
            if (!isThreeToTwo)
            {
                Vector3 pos = KMTools.ConvertPosByCamKeepDis(cam2, mainCam, transform.position, goView.transform.position);
                goView.transform.position = pos;
            }
            else
            {
                Vector3 pos = KMTools.ConvertPosByCam(mainCam, cam2, transform.position);
                goView.transform.position = pos;
            }
        }
    }

    void KMDebug()
    {
        Vector3 pos = goView.transform.position;
        Vector3 pos2 = KMTools.ConvertPosByCamKeepDis(cam2, mainCam, transform.position,goView.transform.position);
        Debug.Log(pos + "----" + pos2);
//        Debug.Log(Mathf.Cos((Mathf.PI / 180) * angle));
    }
}