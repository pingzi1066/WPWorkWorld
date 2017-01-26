/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2016-12-09     WP      Initial version
 * 
 * *****************************************************************************/

using KMTool;
using UnityEngine;
using System.Collections;

namespace KMToolDemo
{
    /// <summary>
    /// 描述
    /// </summary>
    public class Demo_UIWorldPos : MonoBehaviour 
    {

        [SerializeField] private GameObject go;
         
        private Vector3 toPos;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void BtnToUI()
        {
            toPos = UIWorldPos.GetPos("demo", transform);
            LerpTools.instance.AddElement(new LerpTools.Element(0, 1, 1, true), Move);

        }

        public void Move(float cur , Color col)
        {
            transform.position = Vector3.Lerp(transform.position, toPos, cur);
        }

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