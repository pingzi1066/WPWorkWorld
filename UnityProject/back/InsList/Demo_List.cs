using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// List Demo
/// 
/// Maintaince Logs:
/// 2014-12-07  WP      Initial version. 
/// </summary>
public class Demo_List : MonoBehaviour
{

    /// <summary>
    /// 需要在Ins显示的List
    /// </summary>
    public List<Demo_Test_Ins_List> demoList = new List<Demo_Test_Ins_List>();

    /// <summary>
    /// 记录状态的数组，必须加上。
    /// </summary>
    public Dictionary<object, bool> _editorListItemStates = new Dictionary<object, bool>();

    void Start() { }
}

/// <summary>
/// [System.Serializable] 必须加
/// </summary>
[System.Serializable]
public class Demo_Test_Ins_List
{

    public GameObject demoGo;
    public Transform dmoTrans;
    public Rigidbody demoRb;
    //public Collider demoCollider; //暂时不支持

    public int demoInt;
    public string demoStr;
    public float demoFloat;

    /// <summary>
    /// 这个参数会决定之下的参数是否显示
    /// </summary>
    public bool isShowDown;

    public int param1;
    public float param2;
    //public int fuck;
}
