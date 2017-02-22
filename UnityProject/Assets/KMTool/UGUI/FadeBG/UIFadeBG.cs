/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2017-02-22     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace KMTool
{
    /// <summary>
    /// 背景的淡入淡出
    /// </summary>
    public class UIFadeBG : MonoBehaviour
    {
        [SerializeField] protected Image imgFadeBg;
        [SerializeField][Range(0.1f, 3f)] protected float inTime = 1f;
        [SerializeField][Range(0, 2f)] protected float stayTime = .1f;
        [SerializeField][Range(0.1f, 3f)] protected float outTime = 1f;
        [SerializeField][DisableEdit] protected Color startCol;
        [SerializeField][DisableEdit] protected Color outCol;

        [SerializeField] protected Image imgFill;
        [SerializeField] protected bool setClockwise = true;
        protected bool isClockwise { get { return setClockwise; } }

        [SerializeField][DisableEdit] protected bool isFadding = false;

        public delegate void DelFinished();
        protected DelFinished finishedFadeIn;
        protected DelFinished finishedFadeOut;
        protected static UIFadeBG curFadBG;

        void Awake()
        {
            SetColr(imgFadeBg.color);
            imgFadeBg.enabled = false;
        }

        // Use this for initialization
        void Start()
        {

        }

        void OnEnable()
        {
            curFadBG = this;
        }

        static public void BeginCurrent(DelFinished methodIn = null, DelFinished methodOut = null)
        {
            if(curFadBG)
                curFadBG.Fade(methodIn,methodOut);
            else
                Debug.Log("Don't find ui fade bg mono!!!");
        }

        public virtual void SetColr(Color col)
        {
            startCol = col;
            outCol = new Color(startCol.r, startCol.g, startCol.b, 0);
        }

        public virtual void Fade(DelFinished methodIn = null, DelFinished methodOut = null)
        {
            if(isFadding) return;
            isFadding = true;

            finishedFadeIn = methodIn;
            finishedFadeOut = methodOut;
            
            imgFadeBg.color = outCol;
            imgFadeBg.enabled = true;

            if (imgFill)
            {
                imgFill.fillAmount = 0;
                if (isClockwise)
                    imgFill.fillClockwise = true;
                imgFill.enabled = true;
            }

            KMTime.AddTimeCount(inTime, FadeIn);
        }

        // show the bg
        protected virtual void FadeIn(float cur, float sum)
        {
            if(imgFill) imgFill.fillAmount = Mathf.Lerp(0, 1, cur / sum);
            imgFadeBg.color = Color.Lerp(outCol, startCol, cur / sum);

            if (cur == sum)
                FadeInFinished();
        }

        protected virtual void FadeInFinished()
        {
            if(finishedFadeIn != null) finishedFadeIn();

            if (stayTime > 0)
                KMTime.AddTimeCount(stayTime, FadeStay);
            else
                FadeStayFinished();
        }

        protected virtual void FadeStay(float cur ,float sum)
        {
            if(cur == sum)
            {
                FadeStayFinished();
            }
        }

        protected virtual void FadeStayFinished()
        {
            if(imgFill && isClockwise)
                imgFill.fillClockwise = false;
            KMTime.AddTimeCount(outTime, FadeOut);
        }

        // hide the bg
        protected virtual void FadeOut(float cur, float sum)
        {
            if(imgFill) imgFill.fillAmount = Mathf.Lerp(1, 0, cur / sum);
            imgFadeBg.color = Color.Lerp(startCol, outCol, cur / sum);

            if(cur == sum)
                FadeOutFinished();
        }

        protected virtual void FadeOutFinished()
        {
            if(finishedFadeOut != null) finishedFadeOut();

            if(imgFill)
                imgFill.enabled = false;

            imgFadeBg.enabled = false;

            isFadding = false;
        }

        // Update is called once per frame
        void Update()
        {

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