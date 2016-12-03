/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2016-10-22     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using System.Collections.Generic;
using System;

namespace KMTool
{
    [Serializable]
    public struct ParmsString
    {
        public string name;
        #if UNITY_EDITOR
        public string desc;
        #endif
        public string value;
    }

    [Serializable]
    public partial class Table_GlobalStringParms : DataTable<ParmsString>
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

        public string Get(string name)
        {
            for (int i = 0; i < Rows.Length; i++)
            {
                if (Rows[i].name == name)
                    return Rows[i].value;
            }
            return "";
        }
    }

    [Serializable]
    [CreateAssetMenu(fileName = "GlobalStringParms", menuName = "Global String Parms", order = 111)]
    public class ScriptableObjectStringParms : ScriptableObject
    {
        [Table(typeof(ParmsString))]
        public Table_GlobalStringParms listParms;
    }
}