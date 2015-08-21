using UnityEngine;
using System.Collections;

/// <summary>
/// 玩家控制器
/// 
/// Maintaince Logs:
/// 2015-	WP			Initial version. 
/// </summary>
public class InputController : MonoBehaviour
{
    private RaycastHit raycastHit;
    private Ray ray;


    private static InputController mInstance;
    public static InputController instance
    {
        get
        {
            if (mInstance == null)
            {
                GameObject go = new GameObject("_InputController");
                mInstance = go.AddComponent<InputController>();
            }
            return mInstance;
        }
    }

    public bool Input(out Vector3 pos, Collider target)
    {
        bool isInput = false;
        pos = Vector3.zero;

        if (target.Raycast(ray,out raycastHit, 100.0f))
        {
            Debug.DrawLine(ray.origin, raycastHit.point);
            pos = raycastHit.point;
        }

        return isInput;
    }


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
