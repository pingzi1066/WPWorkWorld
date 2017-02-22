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

        static private Camera mCam;
        static private Camera mainCam
        {
            get
            {
                if (mCam == null)
                    mCam = Camera.main;
                return mCam;
            }
        }

        [SerializeField][DisableEdit] private Canvas mCanvas;
        private Canvas canvas
        {
            get
            {
                if (mCanvas == null)
                {
                    mCanvas = gameObject.GetComponentInParent<Canvas>();
                    //Debug.Log(" ----------- null", mCanvas);
                }
                return mCanvas;
            }
        }

        [SerializeField] private string posName = "new pos name";

        [SerializeField][DisableEdit] private Vector3 curWorldPos;

        [SerializeField][DisableEdit] private Vector3 prePos;

        [SerializeField][DisableEdit] private Vector3 preCamPos;

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
            if (prePos != transform.position || preCamPos != mainCam.transform.position)
            {
                prePos = transform.position;
                preCamPos = mainCam.transform.position;

                //当为Camera的时候，使用自身坐标
                if (canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay)
                {
                    curWorldPos = transform.position;
                    //Debug.Log(transform.position);
                }
                else
                {
                    curWorldPos = ConvertToWorldPos(mainCam, prePos);
                }
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

        /// <summary>
        /// 屏幕转世界坐标
        /// </summary>
        /// <param name="cam"></param>
        /// <param name="pos"></param>
        /// <param name="z"></param>
        /// <returns></returns>
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
            Vector3 pos = Get(name).curWorldPos;
            float dis = Vector3.Distance(target.position, mainCam.transform.position);

            // camera 为平视的时候，距离就是z坐标
            if (mainCam.orthographic)
            {
                return new Vector3(pos.x, pos.y, dis);
            }
            // camera 为透视的时候，是发射距离确定坐标
            else
            {
                //5的距离为 ConvertToWorldPos 方法中默认加的5
                //dis -= 5;
                Vector3 dir = -mainCam.transform.position + pos;
                pos += dis * dir.normalized ;
            }
            return pos;
        }

        #region 测试

#if UNITY_EDITOR

        public Transform target;
        public float size = 1;
        public Color color = Color.red;
        public Vector3 drawPos;
        /// <summary>
        /// 对象位置投影到摄影机向前射线上，与摄像机的距离
        /// </summary>
        public float drawDis = 5;

        protected void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            drawPos = curWorldPos;
            //float dis = 5;
            // camera 为平视的时候，距离就是z坐标

            Vector3 camPos = mainCam.transform.position;
            if (target != null)
            {
                Gizmos.DrawLine(camPos, target.position);

                Vector3 camForward = mainCam.transform.forward;

                //向量的投影点
                Vector3 czPos = Vector3.Project(target.position,camForward);
                Gizmos.DrawLine(target.position, (czPos));
                drawDis = Vector3.Distance(czPos, camPos);
            }
            //平视
            if (mainCam.orthographic)
            {
                drawPos = new Vector3(drawPos.x, drawPos.y, drawDis);

            }
            else
            {
                // camera 为透视的时候，是发射距离确定坐标
                Vector3 dir = -mainCam.transform.position + drawPos;
                drawPos += drawDis * dir.normalized;
            }
            Gizmos.DrawWireSphere(drawPos, size);

            Gizmos.DrawLine(camPos ,mainCam.transform.forward * drawDis);
        }

#endif

        public void KMDebug()
        {

        }

        public void KMEditor()
        {
            Debug.Log(" ---------KMEditor----------", gameObject);
        }

        #endregion
    }
}