using UnityEngine;
using System.Collections;

namespace KMToolDemo
{
    /// <summary>
    /// 
    /// 
    /// Maintaince Logs: 
    /// 2015		WP			Initial version.
    /// <summary>
    public class Test02 : MonoBehaviour 
    {
        public int i1 = 2;

        public int i2 = 1;

        public HideFlags hf1;
        public HideFlags hf2;
        //[System.Flags]
        public HideFlags hf;

        public Light l;

        void KMDebug()
        {
            Debug.Log("i1 & i2 is " + (i1 & i2 ));
            Debug.Log("i1 | i2 is " + (i1 | i2 ));

            hf = hf1 & hf2;

            if (l)
            {
    //            Debug.Log("  int " + l.cullingMask + " enum ");
            }

            hf = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
            int intHf = (int)hf;

            int has = (int)(hf & hf1);
            if (has > 0)
            {
                Debug.Log("has flag" + has + " and hf is " + intHf);

                // 做减法
                intHf &= ~(int)hf1;

                hf = (HideFlags)intHf;

                Debug.Log("减法：" + intHf + "  " + hf.ToString());
    //            has &= ~has;
            }
        }

        void KMEditor()
        {
            hf = hf1 | hf2;
        }
        // Use this for initialization
        void Start()
        {
            Test.instance.eventTest += TestMethod;
        }

        void TestMethod()
        {
            Debug.Log("Test method", gameObject);
        }

        public string email = "@163.com";

        [ContextMenu("测试邮箱地址")]
        public void TestEmail()
        {
            Debug.Log(email + " - email " + KMTools.IsEmail(email));
        }

    }
}