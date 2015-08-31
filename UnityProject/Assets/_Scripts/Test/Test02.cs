using UnityEngine;
using System.Collections;

/// <summary>
/// 
/// 
/// Maintaince Logs: 
/// 2015		WP			Initial version.
/// <summary>
public class Test02 : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Test.instance.eventTest += TestMethod;
    }

    void TestMethod()
    {
        Debug.Log("Test method", gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void KMDebug()
    {
        //Debug.Log(transform.childCount);

        MonoBehaviour[] monos = GetComponents<MonoBehaviour>();

        //Debug.Log(monos.Length);

        foreach (Transform t in transform)
        {
            Component[] components = t.GetComponents<Component>();
            //if(tMonos != null)
            //    Debug.Log(tMonos.Length, t);

            foreach (Component component in components)
            {
                if (component == null)
                {
                    Debug.Log(" destroy missing of script for gameobject", t);
                    GameObject.DestroyImmediate(component);
                }
                //else
                //    Debug.Log(component.GetType());
            }
        }
    }
}
