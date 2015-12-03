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
        int count = 0;
        for (int i = 0; i < 10000; i++)
        {
            if (KMTools.OddsByInt(99))
            {
                count++;
            }
        }
        Debug.Log("-------" + count);
    }
}
