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
        [SerializeField] protected Image imgBG;
        [SerializeField][Range(0.1f, 3f)] protected float inTime = 1f;
        [SerializeField][Range(0, 2f)] protected float stayTime = .1f;
        [SerializeField][Range(0.1f, 3f)] protected float outTime = 1f;
        [SerializeField] protected bool setClockwise = true;

        [SerializeField][DisableEdit] protected bool isFadding = false;

        public delegate void DelFinished();
        protected DelFinished finishedFadeIn;
        protected DelFinished finishedFadeOut;
        protected static UIFadeBG instance;

        void Awake()
        {
            instance = this;
        }

        // Use this for initialization
        void Start()
        {

        }

        static public void BeginFade(DelFinished methodIn = null, DelFinished methodOut = null)
        {
            if(instance)
                instance.Fade(methodIn,methodOut);
            else
                Debug.Log("Don't find ui fade bg mono!!!");
        }

        public virtual void Fade(DelFinished methodIn = null, DelFinished methodOut = null)
        {
            if(isFadding) return;
            isFadding = true;

            finishedFadeIn = methodIn;
            finishedFadeOut = methodOut;

            imgBG.fillAmount = 0;
            if(setClockwise)
                imgBG.fillClockwise = true;
            imgBG.enabled = true;

            KMTime.AddTimeCount(inTime, FadeIn);
        }

        // show the bg
        protected virtual void FadeIn(float cur, float sum)
        {
            imgBG.fillAmount = Mathf.Lerp(0, 1, cur / sum);

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
            if(setClockwise)
                imgBG.fillClockwise = false;
            KMTime.AddTimeCount(outTime, FadeOut);
        }

        // hide the bg
        protected virtual void FadeOut(float cur, float sum)
        {
            imgBG.fillAmount = Mathf.Lerp(1, 0, cur / sum);

            if(cur == sum)
                FadeOutFinished();
        }

        protected virtual void FadeOutFinished()
        {
            if(finishedFadeOut != null) finishedFadeOut();

            imgBG.enabled = false;
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