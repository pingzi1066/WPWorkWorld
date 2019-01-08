/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2017-03-07     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using System.Collections;

namespace KMTool
{

    /// <summary>
    /// 平移
    /// </summary>
    public class KMTranslate : MonoBehaviour
    {
        public bool moveOnEnable = false;

        public bool isDisplacement = true;

        /// <summary>
        /// 移动速度
        /// </summary>
        public Vector3 displacement = Vector3.zero;

        /// <summary>
        /// 叠加位置
        /// </summary>
        public Vector3 addPos = Vector3.zero;

        /// <summary>
        /// 如果这个时间
        /// </summary>
        public float moveTime = 1f;

        public bool ignoreTimeScale = false;
        
        [SerializeField][DisableEdit] private float timeParam = 0f;
        [SerializeField][DisableEdit] private Vector3 formPos;
        [SerializeField][DisableEdit] private Vector3 toPos;
        

        // Use this for initialization
        void Start()
        {
            if(!moveOnEnable)
                timeParam = moveTime;
        }

        void OnEnable()
        {
            if(moveOnEnable)
                Begin();
        }

        // Update is called once per frame
        void Update()
        {
            if (timeParam < moveTime)
            {
                float deltaTime = (ignoreTimeScale ? KMTime.deltaTime : Time.deltaTime);
                timeParam += deltaTime;

                if (isDisplacement)
                {
                    transform.Translate(displacement * deltaTime);
                }
                else
                {
                    transform.position = Vector3.Lerp(formPos, toPos, timeParam / moveTime);
                }
            }
        }

        public void Begin()
        {
            timeParam = 0;
            if(!isDisplacement)
            {
                formPos = transform.position;
                toPos = addPos + formPos;
            }

        }

        public void BeginByDis(Vector3 dis, float time)
        {
            displacement = dis;
            moveTime = time;
            isDisplacement = true;

            Begin();
        }

        public void BeginByAdd(Vector3 pos ,float time)
        {
            addPos = pos;
            moveTime = time;
            isDisplacement = false;
            
            Begin();
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