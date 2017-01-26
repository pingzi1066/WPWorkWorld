/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2016-07-19     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using System.Collections;

namespace KMToolDemo
{
    /// <summary>
    /// 当游戏中想动态调试的时候，可以用到此脚本。
    /// </summary>
    public class TestMono : MonoBehaviour
    {
    #if UNITY_EDITOR

        void Awake()
        {
            Debug.Log("-------- Awake ---------", gameObject);
        }

        void OnEnable()
        {
            Debug.Log("-------- OnEnable ---------", gameObject);
        }

        void OnDisable()
        {
            Debug.Log("-------- OnDisable ---------", gameObject);
        }

        // Use this for initialization
        void Start()
        {
            Debug.Log("-------- Start ---------", gameObject);
        }

        // Update is called once per frame
        void Update()
        {

        }

    #endif

        #region 测试

        public void KMDebug()
        {
            Debug.Log(" ---------KMDebug----------", gameObject);
        }

        public void KMEditor()
        {
            Debug.Log(" ---------KMEditor----------", gameObject);
        }

        #endregion
    }
}