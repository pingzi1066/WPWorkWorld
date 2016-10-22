/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2016-10-22     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public struct ParmsInt
{
    public string name;
    #if UNITY_EDITOR
    public string desc;
    #endif
    public int value;
}

[Serializable]
public partial class Table_GlobalIntParms : DataTable<ParmsInt>
{
    public bool Contains(string name)
    {
        for (int i = 0; i < Rows.Length; i++)
        {
            if (Rows[i].name == name)
                return true;
        }
        return false;
    }

    public int Get(string name)
    {
        for (int i = 0; i < Rows.Length; i++)
        {
            if (Rows[i].name == name)
                return Rows[i].value;
        }
        return -1;
    }
}

[Serializable]
[CreateAssetMenu(fileName = "GlobalIntParms", menuName = "Global Int Parms", order = 111)]
public class ScriptableObjectIntParms : ScriptableObject
{
    [Table(typeof(ParmsInt))]
    public Table_GlobalIntParms listParms;
}