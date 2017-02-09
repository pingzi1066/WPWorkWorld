/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2016-12-02     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace KMTool
{

    [ExecuteInEditMode]
    /// <summary>
    /// UGUI 转世界坐标
    /// </summary>
    public class UIWorldPos : MonoBehaviour
    {
        /// <summary>
        /// 保存UI名字与坐标的字典
        /// </summary>
        private static Dictionary<string, UIWorldPos> dictWorldPos = new Dictionary<string, UIWorldPos>();

        [SerializeField]
        private string posName = "new pos name";

        [SerializeField]
        [DisableEdit]
        private Vector3 curWorldPos;

        [SerializeField]
        [DisableEdit]
        private Vector3 prePos;

        void Awake()
        {
            AddToDict(posName, this);

            UpdatePos();
        }

        // Use this for initialization
        void Start()
        {
        }

        [ContextMenu("Init")]
        void UpdatePos()
        {
            if (prePos != transform.position)
            {
                prePos = transform.position;
                Canvas c = transform.root.GetComponent<Canvas>();
                if (c != null && c.renderMode == RenderMode.ScreenSpaceCamera)
                {
                    curWorldPos = transform.position;
                    return;
                }
                curWorldPos = ConvertToWorldPos(Camera.main, prePos);

                //            Debug.Log(transform.InverseTransformPoint(transform.position) + "    " + RectTransformUtility.PixelAdjustPoint);
            }
        }

        // Update is called once per frame
        void Update()
        {
            UpdatePos();
        }

        void OnDestroy()
        {
            RemoveToDict(posName);
        }

        private void AddToDict(string name, UIWorldPos pos)
        {
            if (!dictWorldPos.ContainsKey(name))
            {
                dictWorldPos.Add(name, pos);
            }
            else
            {
                dictWorldPos[name] = pos;
            }
        }

        private void RemoveToDict(string name)
        {
            if (dictWorldPos.ContainsKey(name))
            {
                dictWorldPos.Remove(name);
            }
        }

        static public Vector3 ConvertToWorldPos(Camera cam, Vector3 pos, float z = 5)
        {
            pos.z = z;//Mathf.Clamp(pos.z, cam.nearClipPlane, cam.farClipPlane);
            Vector3 worldPos = cam.ScreenToWorldPoint(pos);

            return worldPos;
        }

        static public UIWorldPos Get(string name)
        {
            if (dictWorldPos.ContainsKey(name))
                return dictWorldPos[name];

            Debug.LogError("Don't find to ui pos " + name);
            return null;
        }

        static public Vector3 GetPos(string name, Transform target)
        {
            //Vector3 uiPos = Get(name).transform.position;
            //return ConvertToWorldPos(Camera.main, uiPos, Vector3.Distance(target.position, Camera.main.transform.position));
            Vector3 pos = Get(name).curWorldPos;
            return new Vector3(pos.x, pos.y, target.position.z);
        }

        #region 测试

        protected void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(curWorldPos, 1);
        }

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