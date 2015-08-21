using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 用于浮点插值 Update来回用
/// 
/// Maintaince Logs:
/// 2015-03-30	WP			Initial version. 
/// 2015-07-16  WP          修改函数传参，优化性能，加入颜色变化，改名
/// </summary>
public class LerpTools : MonoBehaviour
{
    public enum LerpType
    {
        Once,
        PingPong,
        ToggleLoop, //无限来回
    }

    [System.Serializable]
    public class Element
    {
        public float fromValue = 8;
        public float toValue = 0.5f;
        public Color fromColor = Color.black;
        public Color toColor = Color.white;
        public float timeOnce = 1;
        public bool ignoreTimeScale = false;
        public LerpType lerpType;
        public DelLerp eventLerp;

        private float timeParam;

        private float curValue;
        private Color curColor;

        private bool forward;

        private delegate void DelUpdate(float realDeltaTime, float gameDelteTime);

        private event DelUpdate eventUpdate;

        private event DelElementFinish eventFinish;

        public Element(float fromValue, float toValue,
           Color fromColor, Color toColor, float timeOnce, LerpType tp, bool ignoreTimeScale, DelLerp method)
        {
            Init(fromValue, toValue, fromColor, toColor, timeOnce, tp, ignoreTimeScale, method);
        }

        public Element(float fromValue, float toValue, float timeOnce, bool ignoreTimeScale, LerpType tp = LerpType.Once)
        {
            Init(fromValue, toValue, Color.white, Color.white, timeOnce, tp, ignoreTimeScale, null);
        }

        void Init(float minValue, float maxValue,
           Color formColor, Color toColor, float timeOnce, LerpType tp, bool ignoreTimeScale, DelLerp method)
        {
            eventFinish = null;
            forward = true;
            curValue = 0;
            timeParam = 0;
            this.fromValue = minValue;
            this.toValue = maxValue;
            this.timeOnce = timeOnce;
            eventLerp = method;
            this.ignoreTimeScale = ignoreTimeScale;
            this.fromColor = formColor;
            this.toColor = toColor;
            curColor = formColor;

            lerpType = tp;
            eventUpdate = null;
            Init();
            if (eventLerp == null) eventUpdate = null;
        }

        public void Init(DelElementFinish finished = null)
        {
            switch (lerpType)
            {
                case LerpType.Once:
                    eventUpdate = Once;
                    break;
                case LerpType.PingPong:
                    eventUpdate = PingPong;
                    break;
                case LerpType.ToggleLoop:
                    eventUpdate = ToggleLoop;
                    break;
            }

            if (finished != null) eventFinish = finished;
        }

        private void Once(float realDeltaTime, float gameDelteTime)
        {
            timeParam += ignoreTimeScale ? realDeltaTime : gameDelteTime;
            float percent = timeParam / timeOnce;
            curValue = Mathf.Lerp(fromValue, toValue, percent);
            curColor = Color.Lerp(fromColor, toColor, percent);
            eventLerp(curValue, curColor);
            if (timeParam > timeOnce)
            {
                eventUpdate = null;
                if (eventFinish != null) eventFinish();
            }
        }

        public bool isEqual(DelLerp method)
        {
            if (method.Target == eventLerp.Target && method.Method == eventLerp.Method)
            {
                return true;
            }
            return false;
        }

        private void PingPong(float realDeltaTime, float gameDelteTime)
        {
            if (forward)
            {
                timeParam += ignoreTimeScale ? realDeltaTime : gameDelteTime;
                float percent = timeParam / timeOnce;
                curValue = Mathf.Lerp(fromValue, toValue, percent);
                curColor = Color.Lerp(fromColor, toColor, percent);

                eventLerp(curValue, curColor);
                if (timeParam >= timeOnce)
                {
                    forward = false;
                }
            }
            else
            {
                timeParam -= ignoreTimeScale ? realDeltaTime : gameDelteTime;
                float percent = timeParam / timeOnce;
                curValue = Mathf.Lerp(fromValue, toValue, percent);
                curColor = Color.Lerp(fromColor, toColor, percent);

                eventLerp(curValue, curColor);
                if (timeParam <= 0)
                {
                    eventUpdate = null;
                    if (eventFinish != null) eventFinish();
                }
            }
        }

        private void ToggleLoop(float realDeltaTime, float gameDelteTime)
        {
            if (forward)
            {
                timeParam += ignoreTimeScale ? realDeltaTime : gameDelteTime;
                float percent = timeParam / timeOnce;
                curValue = Mathf.Lerp(fromValue, toValue, percent);
                curColor = Color.Lerp(fromColor, toColor, percent);

                eventLerp(curValue, curColor);
                if (timeParam >= timeOnce)
                {
                    forward = false;
                }
            }
            else
            {
                timeParam -= ignoreTimeScale ? realDeltaTime : gameDelteTime;
                float percent = timeParam / timeOnce;
                curValue = Mathf.Lerp(fromValue, toValue, percent);
                curColor = Color.Lerp(fromColor, toColor, percent);

                eventLerp(curValue, curColor);

                if (timeParam <= 0)
                {
                    forward = true;
                }
            }
        }

        public bool Update(float realTime, float gameTime)
        {
            if (eventUpdate != null)
            {
                eventUpdate(realTime, gameTime);
                return true;
            }
            return false;
        }
    }

    private List<Element> elements = new List<Element>();

    public delegate void DelLerp(float curValue, Color curColor);

    public delegate void DelElementFinish();

    private static LerpTools mInstance;
    public static LerpTools instance
    {
        get
        {
            if (mInstance == null)
            {
                GameObject go = new GameObject("_FloatLerp");
                mInstance = go.AddComponent<LerpTools>();
            }
            return mInstance;
        }
    }

    public void AddElement(Element element, DelLerp method, DelElementFinish finished = null)
    {
        if (element.eventLerp == null)
        {
            element.eventLerp = method;
            element.Init(finished);
        }
        elements.Add(element);
    }

    public void RemoveElement(DelLerp method)
    {
        foreach (Element e in elements)
        {
            if (e.isEqual(method))
            {
                elements.Remove(e);
                return;
            }
        }
    }

    void Update()
    {
        if (elements.Count > 0)
        {
            float realDeltaTime = KMTime.deltaTime;
            float gameDeltaTime = Time.deltaTime;
            for (int i = 0; i < elements.Count; )
            {
                if (!elements[i].Update(realDeltaTime, gameDeltaTime))
                {
                    elements.RemoveAt(i);
                    continue;
                }
                i++;
            }
        }
    }
}
