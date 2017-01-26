using UnityEngine;
using System.Collections;

namespace KMToolDemo
{
    public class Test : MonoBehaviour
    {

        public Color color;

        public Transform target;

        public static Test instance;

        public delegate void DelTest();

        public DelTest eventTest;

        void Awake() { instance = this; }

        public TA a;
        public TA b;

     

        // Use this for initialization
        void Start()
        {
            if (b is TB)
            {
                print("b is tb and -- b.b is " + (b as TB).b);
            }
            else
            {
                print("b isn't TB");
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

    [System.Serializable]
    public class TA
    {
        public int a;
    }

    [System.Serializable]
    public class TB : TA
    {
        public float b;
    }
}
