/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2016-04-22     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using System.Collections.Generic;

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

    [SerializeField]
    float topDis = 35;

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
        left = 10;
        top = 10;
        width = 120;
        height = 30;

        if (GUI.Button(rect, "Save"))
        {
            text = data.SaveData();
        }

        top += topDis;

        if (GUI.Button(rect, "Load"))
        {
            data.LoadData();
        }


        top += topDis;
        width = 120;
        DemoEnum e = DemoEnum.Coin;
        if (GUI.Button(rect, "Get " + e.ToString()))
        {
            text = e.ToString() + "  value is : " + data.GetData(e);
        }

        top += topDis;

        if (GUI.Button(rect, "Add " + e.ToString()))
        {
            data.AddData(e, curValue);
        }

        top += topDis;
        e = DemoEnum.Gem;
        if (GUI.Button(rect, "Get " + e.ToString()))
        {
            text = e.ToString() + "  value is : " + data.GetData(e);
        }

        top += topDis;

        if (GUI.Button(rect, "Add " + e.ToString()))
        {
            data.AddData(e, curValue);
        }


        top += topDis;
        width = 300;
        GUI.Label(rect, text);

        top += topDis;
        GUI.Label(rect, "cur value is " + curValue + "  change value in inspector!!");

        top += topDis;
        height = 100;
        GUI.Label(rect, logChange + "\n" + Demo_IntData.instance.ToDebug());

        OnGUIList();
    }

    private void OnGUIList()
    {

        height = 30;
        left = 210;
        top = 10;

        if (GUI.Button(rect, "Save "))
        {
            Demo_ListIntData.instance.SaveData();
        }

        width = 220;

        DemoListEnum[] es = EnumTools.EnumConvertArray<DemoListEnum>();
        for (int i = 0; i < es.Length; i++)
        {
            DemoListEnum e = es[i];
            top += topDis; width = 220;

            if (GUI.Button(rect, "Add " + curValue + " To List" + e.ToString()))
            {
                Demo_ListIntData.instance.AddItem(e, curValue);
                return;
            }
            top += topDis;

            if (GUI.Button(rect, "Remove " + curValue + " To List" + e.ToString()))
            {
                Demo_ListIntData.instance.RemoveItem(e, curValue);
                return;
            }

            top += topDis;
            List<int> list = Demo_ListIntData.instance.GetData(e);
            width = 1000;
            string listText = "count is " + list.Count + " value is  :";

            for (int j = 0; j < list.Count; j++)
            {
                listText += list[j] + " ,";
            }

            GUI.Label(rect, listText);
        }

        top += topDis * 3;
        height = 100;
        GUI.Label(rect, Demo_ListIntData.instance.ToDebug());
    }

    void OnValue(DemoEnum d, int value)
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