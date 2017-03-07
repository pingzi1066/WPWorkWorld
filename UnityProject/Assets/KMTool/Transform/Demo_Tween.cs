/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2017-03-07     WP      Initial version
 * 
 * *****************************************************************************/

using KMTool;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

namespace KMToolDemo
{
    /// <summary>
    /// 描述
    /// </summary>
    public class Demo_Tween : MonoBehaviour
    {
        public KMPlayTween twn;
        public Text textCallBack;

        private string text;

        // Use this for initialization
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Play(bool isDir)
        {
            if (!isadd)
            {
                twn.resetOnPlay = true;
                //twn.onFinished.AddListener(KMEditor);
                isadd = true;
            }
            twn.Play(isDir);

            text = isDir ? "forward" : "back";
        }

        #region 测试

        bool isadd = false;

        public void KMDebug()
        {
            Debug.Log(" ---------Play Tween----------", gameObject);
            twn.Play(true);
        }

        public void KMEditor()
        {
            //Debug.Log(" ---------Finished----------", gameObject);
            text += "   finished!!";
            if(textCallBack) textCallBack.text = text;
        }

        #endregion
    }
}