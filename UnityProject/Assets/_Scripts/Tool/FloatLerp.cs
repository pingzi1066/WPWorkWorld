using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 用于浮点插值 Update来回用
/// 
/// Maintaince Logs:
/// 2015-03-30	WP			Initial version. 
/// </summary>
public class FloatLerp : MonoBehaviour
{
    public enum LerpType
    {
        Once,
        PingPong,
        ToggleLoop, //无限来回
    }

    public struct Element
    {
        private float minValue;
        private float maxValue;
        private float timeOnce;

        private float timeParam;

        private float curValue;

        private DelLerp eventLerp;
        private bool ignoreTimeScale;

        private bool forward;

        private delegate void DelUpdate();

        private event DelUpdate eventUpdate;

        private event DelElementFinish eventFinish;

        public Element(float minValue, float maxValue, float timeOnce, LerpType tp, bool ignoreTimeScale, DelLerp method)
        {
            eventFinish = null;
            forward = true;
            curValue = 0;
            timeParam = 0;
            this.minValue = minValue;
            this.maxValue = maxValue;
            this.timeOnce = timeOnce;
            eventLerp = method;
            this.ignoreTimeScale = ignoreTimeScale;

            eventUpdate = null;
            switch (tp)
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
            if (eventLerp == null) eventUpdate = null;
        }

        private void Once()
        {
            timeParam += ignoreTimeScale ? RealTime.deltaTime : Time.deltaTime;
            curValue = Mathf.Lerp(minValue, maxValue, timeParam / timeOnce);
            eventLerp(curValue);
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

        private void PingPong()
        {
            if (forward)
            {
                timeParam += ignoreTimeScale ? RealTime.deltaTime : Time.deltaTime;
                curValue = Mathf.Lerp(minValue, maxValue, timeParam / timeOnce);

                eventLerp(curValue);
                if (timeParam >= timeOnce)
                {
                    forward = false;
                }
            }
            else
            {
                timeParam -= ignoreTimeScale ? RealTime.deltaTime : Time.deltaTime;
                curValue = Mathf.Lerp(minValue, maxValue, timeParam / timeOnce);

                eventLerp(curValue);
                if (timeParam <= 0)
                {
                    eventUpdate = null;
                    if (eventFinish != null) eventFinish();
                }
            }
        }

        private void ToggleLoop()
        {
            if (forward)
            {
                timeParam += ignoreTimeScale ? RealTime.deltaTime : Time.deltaTime;
                curValue = Mathf.Lerp(minValue, maxValue, timeParam / timeOnce);

                eventLerp(curValue);
                if (timeParam >= timeOnce)
                {
                    forward = false;
                }
            }
            else
            {
                timeParam -= ignoreTimeScale ? RealTime.deltaTime : Time.deltaTime;
                curValue = Mathf.Lerp(minValue, maxValue, timeParam / timeOnce);

                eventLerp(curValue);
                if (timeParam <= 0)
                {
                    forward = true;
                }
            }
        }

        public bool Update()
        {
            if (eventUpdate != null)
            {
                eventUpdate();
                return true;
            }
            return false;
        }
    }

    private List<Element> elements = new List<Element>();

    public delegate void DelLerp(float curValue);

    private delegate void DelElementFinish();

    //private event DelLerp eventRefreshValue;

    private static FloatLerp mInstance;
    public static FloatLerp instance
    {
        get
        {
            if (mInstance == null)
            {
                GameObject go = new GameObject("_FloatLerp");
                mInstance = go.AddComponent<FloatLerp>();
            }
            return mInstance;
        }
    }

    public void AddElement(Element element)
    {
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
        for (int i = 0; i < elements.Count; )
        {
            if (!elements[i].Update())
            {
                elements.RemoveAt(i);
                continue;
            }
            i++;
        }
    }
}
