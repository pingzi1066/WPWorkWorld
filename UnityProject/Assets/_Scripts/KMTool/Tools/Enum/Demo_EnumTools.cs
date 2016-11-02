using UnityEngine;
using System.Collections;
using System.ComponentModel;

public class Demo_EnumTools : MonoBehaviour
{

    void OnGUI()
    {
        int top = 10;
        EquipmentTpye[] es = EnumTools.EnumConvertArray<EquipmentTpye>();

        foreach (EquipmentTpye e in es)
        {
            GUI.TextField(new Rect(10, top, 250, 20), EnumTools.GetDescription(e));
            top += 20;
        }
    }

    public enum EquipmentTpye
    {
        [Description("武器")]
        E_WEAPON = 1,//武器
        [Description("胸甲")]
        E_BODY = 2,  //胸甲
        [Description("护腿")]
        E_LEG = 3,  //护腿
        [Description("手套")]
        E_HAND = 4,  //手套
        [Description("长靴")]
        E_BOOT = 5,  //长靴
        [Description("时装")]
        E_CLOTHES = 6,//时装
    }

}
