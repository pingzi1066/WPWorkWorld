/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2016-04-22     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using System.Collections;

/// <summary>
/// 描述
/// </summary>
public class Demo_IntDataMono : MonoBehaviour
{
    float height = 30;
    float width = 120;

    float left = 10;
    float top = 10;

    [SerializeField]
    int curValue = 10;

    string text;

    private Demo_IntData data;

    private Rect rect
    {
        get
        {
            return new Rect(left, top, width, height);
        }
    }

    private string logChange = "";

    // Use this for initialization
    void Start()
    {
        data = Demo_IntData.instance;
//        System.ValueType a = 0;
//        float b = a + 1;
        Demo_IntData.eventOnValue += OnValue;
    }

    void OnGUI()
    {
        top = 10;
        width = 120;

        if (GUI.Button(rect, "Save"))
        {
            text = data.SaveData();
        }

        top += 35;

        if (GUI.Button(rect, "Load"))
        {
            data.LoadData();
        }


        top += 35;
        width = 120;
        DemoEnum e = DemoEnum.Coin;
        if (GUI.Button(rect, "Get " + e.ToString()))
        {
            text = e.ToString() + "  value is : " + data.GetInt(e);
        }

        top += 35;

        if (GUI.Button(rect, "Add " + e.ToString()))
        {
            data.AddInt(e, curValue);
        }

        top += 35;
        e = DemoEnum.Gem;
        if (GUI.Button(rect, "Get " + e.ToString()))
        {
            text = e.ToString() + "  value is : " + data.GetInt(e);
        }

        top += 35;

        if (GUI.Button(rect, "Add " + e.ToString()))
        {
            data.AddInt(e, curValue);
        }


        top += 35; 
        width = 300;
        GUI.Label(rect, text);
        
        top += 35;
        GUI.Label(rect, "cur value is " + curValue + "  change value in inspector!!");

        top += 35;
        GUI.Label(rect, logChange);
    }

    void OnValue(DemoEnum d,int value)
    {
        logChange = "Set  " + d.ToString() + "  To: " + value;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void KMDebug()
    {

    }
}