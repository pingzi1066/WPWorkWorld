/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2017-03-21     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using KMTool;
using System;

namespace KMToolDemo
{

    public enum DE_LocalData
    {
        Id,
        Level,
        Score,
    }

    public class Demo_LLD : ListLocalInt<Demo_LLD,DE_LocalData>
    {
        protected override DE_LocalData HeadKey()
        {
            return DE_LocalData.Id;
        }

    }


    /// <summary>
    /// 描述
    /// </summary>
    public class Demo_ListLocalData : MonoBehaviour
    {
        public Text text;
        public Text textLog;

        // Use this for initialization
        void Start()
        {
            Demo_LLD.eventOnValue += CallBack;
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void CallBack(int id, DE_LocalData key, int value)
        {
            //Debug.Log(id +  "  data is change " + key + "  " + value);

            if(textLog)
            {
                textLog.text = id +  "  data is change " + key + "  " + value;
            }

        }

        #region 测试

        public void KMDebug()
        {
            Debug.Log(" ---------KMDebug----------", gameObject);
            int id = UnityEngine.Random.Range(1,10);
            int level = UnityEngine.Random.Range(1,2);
            int score = UnityEngine.Random.Range(20,50);
            Demo_LLD.instance.SetData(id, DE_LocalData.Level,level);
            Demo_LLD.instance.AddValueToData(id, DE_LocalData.Score, score);
            Demo_LLD.instance.SaveData();
        }

        public void KMEditor()
        {
            Debug.Log(" ---------KMEditor----------", gameObject);
            //Debug.Log(Demo_LLD.instance.ToDebug());
            if(text) text.text = Demo_LLD.instance.ToDebug();
        }

        #endregion
    }


}