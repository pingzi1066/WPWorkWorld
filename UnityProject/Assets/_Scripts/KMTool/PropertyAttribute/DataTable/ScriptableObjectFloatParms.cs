/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2016-07-21     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using System.Collections.Generic;
using System;


namespace KMTool
{

    [Serializable]
    public struct ParmsFloat
    {
        public string name;
    #if UNITY_EDITOR
        public string desc;
    #endif
        public float value;
    }

    [Serializable]
    public partial class Table_GlobalFloatParms : DataTable<ParmsFloat>
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

        public float Get(string name)
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
    [CreateAssetMenu(fileName = "GlobalFloatParms", menuName = "Global Float Parms", order = 112)]
    public class ScriptableObjectFloatParms : ScriptableObject
    {
        [Table(typeof(ParmsFloat))]
        public Table_GlobalFloatParms listParms;
    }
}