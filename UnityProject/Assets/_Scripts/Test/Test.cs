using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour
{

    public Color color ;

    public Transform target;

    void KMDebug()
    {
        KMTools.DestroyChildren(target);
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
