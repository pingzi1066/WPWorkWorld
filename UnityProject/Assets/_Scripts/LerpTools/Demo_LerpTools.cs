using UnityEngine;
using System.Collections;

/// <summary>
/// FloatLerp 的测试脚本
/// 
/// Maintaince Logs:
/// 2015-03-31	WP			Initial version. 
/// </summary>
public class Demo_LerpTools : MonoBehaviour
{
    private float timeScale = 1;
    private float onceValue = 0;
    private bool oncePlaying = false;
    private float pingPongValue = 0;
    private bool pingPongPlaying = false;
    private float toggleValue = 0;
    private bool togglePlaying = false;

    private float toggleIgnoreTime = 0;

    public float fromValue = 0;
    public float toValue = 10;
    public float timeOnce = 5;

    bool begin = false;
    void OnGUI()
    {
        Time.timeScale = timeScale;
        if (!begin)
        {
            if (GUI.Button(new Rect(0, 0, 100, 100), "Begin"))
            {
                LerpTools.instance.AddElement(new LerpTools.Element(fromValue, toValue, timeOnce, false, LerpTools.LerpType.Once), Once, OnceDone);
                LerpTools.instance.AddElement(new LerpTools.Element(fromValue, toValue, timeOnce, false, LerpTools.LerpType.PingPong), PingPong, PingPongDone);
                LerpTools.instance.AddElement(new LerpTools.Element(fromValue, toValue, timeOnce, false, LerpTools.LerpType.ToggleLoop), ToggleLoop, ToggleLoopDone);
                LerpTools.instance.AddElement(new LerpTools.Element(fromValue, toValue, timeOnce, true, LerpTools.LerpType.ToggleLoop), ToggleLoopIgnoreTimeScale);
                begin = true;
            }
        }
        else
        {
            if (GUI.Button(new Rect(0, 0, 100, 100), "Stop"))
            {
                LerpTools.instance.RemoveElement(Once);
                LerpTools.instance.RemoveElement(PingPong);
                LerpTools.instance.RemoveElement(ToggleLoop);
                LerpTools.instance.RemoveElement(ToggleLoopIgnoreTimeScale);
                begin = false;
            }

            int left = 100;
            int top = 0;
            Rect rect1 = new Rect(left, top, 400, 30);

            GUI.TextField(rect1, "Once value : " + onceValue + "  state " + (oncePlaying ? "playing" : "Done"));
            top += 30;

            Rect rect2 = new Rect(left, top, 400, 30);
            GUI.TextField(rect2, "PingPong value : " + pingPongValue + "  state " + (pingPongPlaying ? "playing" : "Done"));
            top += 30;

            Rect rect3 = new Rect(left, top, 400, 30);
            GUI.TextField(rect3, "ToggleLoop value : " + toggleValue + "  state " + (togglePlaying ? "playing" : "Done"));
            top += 30;

            Rect rect4 = new Rect(left, top, 400, 30);
            GUI.TextField(rect4, "ToggleLoop IgnoreTimeScale value : " + toggleIgnoreTime);
            top += 30;

            Rect rect5 = new Rect(left, top, 400, 30);
            Rect rect5_1 = new Rect(left, top + 30, 400, 30);
            GUI.TextField(rect5, "Time Scale is " + timeScale);
            timeScale = GUI.HorizontalScrollbar(rect5_1, timeScale, .2f, 0, 5);
            top += 60;

            Rect rect6 = new Rect(left, top, 400, 30);
            Rect rect6_1 = new Rect(left, top + 30, 400, 30);
            GUI.TextField(rect6, "KMTime Scale is " + KMTime.timeScale);
            KMTime.timeScale = GUI.HorizontalScrollbar(rect6_1, KMTime.timeScale, .2f, 0, 5);
        }
    }

    void Once(float value, Color col)
    {
        onceValue = value;
        oncePlaying = true;
    }

    void OnceDone()
    {
        oncePlaying = false;
    }

    void PingPong(float value, Color col)
    {
        pingPongValue = value;
        pingPongPlaying = true;
    }

    void PingPongDone()
    {
        pingPongPlaying = false;
    }

    void ToggleLoop(float value, Color col)
    {
        toggleValue = value;
        togglePlaying = true;
    }

    void ToggleLoopDone()
    {
        togglePlaying = false;
    }

    void ToggleLoopIgnoreTimeScale(float value, Color col)
    {
        toggleIgnoreTime = value;
    }
}
