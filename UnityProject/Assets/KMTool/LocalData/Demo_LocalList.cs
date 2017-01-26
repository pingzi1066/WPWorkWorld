/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2016-11-17     WP      Initial version
 * 
 * *****************************************************************************/
using KMTool;
using UnityEngine;
using System.Collections;

public enum E_Demo_Lsl
{
    AAA,
    BB,
    CC,
}
public class Demo_Lsl : LocalListString<Demo_Lsl,E_Demo_Lsl>
{
    
}
    
public class Demo_LS : LocalString < Demo_LS,E_Demo_Lsl>
{
    
}

/// <summary>
/// 描述
/// </summary>
public class Demo_LocalList : MonoBehaviour 
{
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    #region 测试

    public void KMDebug()
    {
        Debug.Log(" ---------KMDebug----------", gameObject);
        Demo_Lsl d = Demo_Lsl.instance;
        d.AddItem(E_Demo_Lsl.AAA, "ffss");
        d.AddItem(E_Demo_Lsl.BB, "我日" + Random.Range(0,10));
        d.SaveData();
        Debug.Log(d.ToDebug());
//        Demo_LS d = Demo_LS.instance;
//        d.SetData(E_Demo_Lsl.AAA, "sdddccc");
//        d.SetData(E_Demo_Lsl.BB, "中文");
//        d.SetData(E_Demo_Lsl.CC," adskfjdskjf.\nddfaskldjkldjflkadjsflkdsjafldsakfja;ldskfjl;kasdfjla;dskfj;dalsfj;adlskfjdla;sfj;adlskfjfd;alj");
//        d.SaveData();
//
//        Debug.Log(d.ToDebug());
          
    }

    public void KMEditor()
    {
        Debug.Log(" ---------KMEditor----------", gameObject);
//        Demo_Lsl.instance.ClearData();

        Demo_Lsl d = Demo_Lsl.instance;
        d.LoadData();
        Debug.Log(d.ToDebug());

    }

    #endregion
}