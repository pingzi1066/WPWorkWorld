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
}
