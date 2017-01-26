using UnityEngine;
using System;
using System.Reflection;
using System.ComponentModel;

/// <summary>
/// 
/// 
/// Maintaince Logs: 
/// 2015-07-16		WP			Initial version.
/// 2017-01-26      WP          added HasFlag method for all enum 
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

    /// <summary>
    /// Check to see if a flags enumeration has a specific flag set.
    /// http://stackoverflow.com/questions/4108828/generic-extension-method-to-see-if-an-enum-contains-a-flag
    /// </summary>
    /// <param name="variable">Flags enumeration to check</param>
    /// <param name="value">Flag to check for</param>
    /// <returns></returns>
    public static bool HasFlag(this Enum variable, Enum value)
    {
        if (variable == null)
            return false;

        if (value == null)
            throw new ArgumentNullException("value");

        // Not as good as the .NET 4 version of this function, but should be good enough
        if (!Enum.IsDefined(variable.GetType(), value))
        {
            throw new ArgumentException(string.Format(
                "Enumeration type mismatch.  The flag is of type '{0}', was expecting '{1}'.",
                value.GetType(), variable.GetType()));
        }

        ulong num = Convert.ToUInt64(value);
        return ((Convert.ToUInt64(variable) & num) == num);

    }
}
