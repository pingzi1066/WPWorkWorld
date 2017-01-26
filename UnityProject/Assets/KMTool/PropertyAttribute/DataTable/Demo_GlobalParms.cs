/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2016-10-22     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using System.Collections;
using KMTool;

namespace KMToolDemo
{
    public class Demo_GlobalParms : MonoBehaviour 
    {

        private string intParmName = "test";
        private int curInt = 0;
        private string floatParmName = "test";
        private float curFloat = 0;


        // -------gui 
        [SerializeField]
        float topDis = 100;

        float height = 30;
        float width = 120;

        float left = 10;
        float top = 10;
        private Rect rect
        {
            get
            {
                return new Rect(left, top, width, height);
            }
        }

        // -------gui 


    	// Use this for initialization
    	void Start () {
            Debug.Log(GlobalParms.GetDouble("icbc"));
    	}
    	
    	// Update is called once per frame
    	void Update () {
    	
    	}

        void OnGUI()
        {
            left = 10;
            top = 10;
            width = 500;
            height = 95;

            GUI.Label(rect, "int name");
            top += topDis;
            intParmName = GUI.TextField(rect, intParmName);

            top += topDis;

            GUI.Label(rect, "cur int value is " + curInt);
            top += topDis;
            if (GUI.Button(rect, "Get Int"))
                curInt = GlobalParms.GetInt(intParmName);


            top += topDis;
            GUI.Label(rect, "float name");
            top += topDis;
            floatParmName = GUI.TextField(rect, floatParmName);
            top += topDis;

            GUI.Label(rect, "cur float value is " + curFloat);
            top += topDis;
            if (GUI.Button(rect, "Get Float"))
                curFloat = GlobalParms.GetFloat(floatParmName);
        }
    }
}