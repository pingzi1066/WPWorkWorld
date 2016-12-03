/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2016-04-22     WP      Initial version
 * 
 * *****************************************************************************/

using KMTool;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using System;

public enum DemoEnum
{ 
    Coin,
    Gem,
}
public class Demo_IntData : LocalInt<Demo_IntData,DemoEnum>{}
public enum DemoListEnum
{
    CharIds,
    CIds,
}
public class Demo_ListIntData : LocalListInt<Demo_ListIntData, DemoListEnum> {}
public enum E_Demo_Lsl
{
    AAA,
    BB,
    CC,
}
public class Demo_Lsl : LocalListString<Demo_Lsl,E_Demo_Lsl>{}

public class Demo_LS : LocalString < Demo_LS,E_Demo_Lsl>{}

public class Demo_LocalData : MonoBehaviour
{
    [System.Serializable]
    public class UI_View//UI_View<U>
    {
        [SerializeField] private Dropdown intDropD;
        [SerializeField] private Text intText;
        [SerializeField] private InputField intInput;
        public int curIntKey = 0;
        public string curValue;

        public void Init<T>(T[] es,bool isInt)
        {
            SetDrop(EnumTools.EnumConvertArray<T>(), intDropD);
            intDropD.onValueChanged.AddListener(OnSelectInt);
            intInput.onEndEdit.AddListener(OnChangeInt);

            if(isInt)
                intInput.contentType = InputField.ContentType.IntegerNumber;
        }

        void SetDrop<U>(U[] es , Dropdown dd)
        {
            dd.ClearOptions();
            List<string> list = new List<string>();
            for (int i = 0; i < es.Length; i++)
                list.Add(es[i].ToString());
            dd.AddOptions(list);
        }

        void OnSelectInt(int i)
        {
            curIntKey = i;
        }

        void OnChangeInt(string s)
        {
            curValue = s;
        }

        public T GetKey<T>()
        {
            object t = Enum.Parse(typeof(T), intDropD.options[curIntKey].text);
            return (T)t;
        }

        public void SetText(string s)
        {
            intText.text = s;
        }
    }

    public UI_View intUI;
    public UI_View intListUI;
    public UI_View stringUI;
    public UI_View stringListUI;

    // Use this for initialization
    void Start()
    {
        intUI.Init(EnumTools.EnumConvertArray<DemoEnum>(), true);
        intListUI.Init(EnumTools.EnumConvertArray<DemoListEnum>(), true);
        stringUI.Init(EnumTools.EnumConvertArray<E_Demo_Lsl>(), false);
        stringListUI.Init(EnumTools.EnumConvertArray<E_Demo_Lsl>(), false);

        Demo_IntData.eventOnValue += OnIntChange;
        Demo_ListIntData.eventOnValue += OnIntListChange;
        Demo_LS.eventOnValue += OnStrChange;
        Demo_Lsl.eventOnValue += OnStrListChange;
        Demo_IntData.instance.RefreshEvent();
        Demo_ListIntData.instance.RefreshEvent();
        Demo_LS.instance.RefreshEvent();
        Demo_Lsl.instance.RefreshEvent();
    }

    void OnIntChange( DemoEnum key ,int value)
    {
        string text = " Set Key " + key + " To " + value + "\n";
        text += Demo_IntData.instance.ToDebug();
        intUI.SetText(text);
    }
    void OnIntListChange( DemoListEnum key ,List<int> value)
    {
        string text = " Set List Key " + key + " ----- ";
        text += Demo_ListIntData.instance.ToDebug();
        intListUI.SetText(text);
    }
    void OnStrChange( E_Demo_Lsl key ,string value)
    {
        string text = " Set Key " + key + " To " + value + "\n";
        text += Demo_LS.instance.ToDebug();
        stringUI.SetText(text);
    }
    void OnStrListChange( E_Demo_Lsl key ,List<string> value)
    {
        string text = " Set List Key " + key + " ----- ";
        text += Demo_Lsl.instance.ToDebug();
        stringListUI.SetText(text);
    }

    public void BtnSetInt()
    {
        Demo_IntData.instance.SetData(intUI.GetKey<DemoEnum>(), int.Parse(intUI.curValue));
    }

    public void BtnAddIntToList()
    {
        Demo_ListIntData.instance.AddItem(intListUI.GetKey<DemoListEnum>(), int.Parse(intListUI.curValue));
    }

    public void BtnRemIntToList()
    {
        Demo_ListIntData.instance.RemoveItem(intListUI.GetKey<DemoListEnum>(), int.Parse(intListUI.curValue));
    }

    public void BtnSetString()
    {
        Demo_LS.instance.SetData(stringUI.GetKey<E_Demo_Lsl>(), stringUI.curValue);
    }

    public void BtnAddStringToList()
    {
        Demo_Lsl.instance.AddItem(stringListUI.GetKey<E_Demo_Lsl>(), stringListUI.curValue);
    }

    public void BtnRemStringToList()
    {
        Demo_Lsl.instance.RemoveItem(stringListUI.GetKey<E_Demo_Lsl>(), stringListUI.curValue);
    }

    public void BtnSaveAll()
    {
        Demo_IntData.instance.SaveData();
        Demo_ListIntData.instance.SaveData();
        Demo_LS.instance.SaveData();
        Demo_Lsl.instance.SaveData();
    }

    public void BtnDelAll()
    {
        Demo_IntData.instance.ClearData();
        Demo_ListIntData.instance.ClearData();
        Demo_LS.instance.ClearData();
        Demo_Lsl.instance.ClearData();
        BtnSaveAll();
    }
}