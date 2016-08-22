/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2016-08-05     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using System.Collections;
using System;

public class MethodAttribute : PropertyAttribute 
{
    public string name;

    public MethodAttribute(string name = "")
    {
        this.name = name;
    }
}