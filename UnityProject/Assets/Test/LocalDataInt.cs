/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2016-12-10     WP      Initial version
 * 
 * *****************************************************************************/

using KMTool;
using UnityEngine;
using System.Collections;

public enum E_Demo_LocalData
{
    No1,
    No2,
    No3,
}

public class LocalDataInt :  LocalData<LocalDataInt,E_Demo_LocalData,int>
{
    protected override int ConvertFormString(string str)
    {
        return int.Parse(str);
    }

    protected override int GetDefaultValue(E_Demo_LocalData e)
    {
        return 0;
    }
}

public class LocalDataString : LocalData<LocalDataString,E_Demo_LocalData,string>
{
    protected override string ConvertFormString(string str)
    {
        return str;
    }

    protected override string GetDefaultValue(E_Demo_LocalData e)
    {
        return "0";
    }
}

