using UnityEngine;
using System;
using System.Reflection;
using System.ComponentModel;

/// <summary>
/// 
/// 
/// Maintaince Logs: 
/// 2015-07-16		WP			Initial version.
public static class EnumTools
{
    public static string GetDescription(Enum obj)
    {
        string objName = obj.ToString();
        Type t = obj.GetType();
        FieldInfo fi = t.GetField(objName);
        DescriptionAttribute[] arrDesc = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

        if (arrDesc.Length < 1) return "";

        return arrDesc[0].Description;
    }

    public static T[] EnumConvertArray<T>()
    {
        T[] array = Enum.GetValues(typeof(T)) as T[];
        return array;
    }

}
