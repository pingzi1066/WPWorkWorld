/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2016-07-21     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public struct Parms
{
    public string name;
#if UNITY_EDITOR
    public string desc;
#endif
    public float value;
}

[Serializable]
public partial class ListParms
{
    public List<Parms> listParms = new List<Parms>();
}

[System.Serializable]
[CreateAssetMenu(fileName = "GlobalParms", menuName = "GlobalParms", order = 111)]
public class GlobalParms : ScriptableObject
{
    [Table(typeof(Parms))]
    public ListParms listParms;

}
