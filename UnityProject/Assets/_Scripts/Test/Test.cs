using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour
{

    public Color color ;

    public Transform target;

    public static Test instance;

    public delegate void DelTest();

    public DelTest eventTest;

    void Awake() { instance = this; }

    void KMDebug()
    {
        if (eventTest != null) eventTest();
        else Debug.Log("eventTest is null");
        //KMTools.DestroyChildren(target);
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
