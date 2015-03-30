using UnityEngine;
using System.Collections;

/// <summary>
/// FloatLerp 的测试脚本
/// 
/// Maintaince Logs:
/// 2015-03-31	WP			Initial version. 
/// </summary>
public class DemoFloatLerp : MonoBehaviour
{
    public float timeScale = 0;

    public float onceValue = 0;
    public float pingPongValue = 0;
    public float toggleValue = 0;

    public float minValue = 0;
    public float maxValue = 10;
    public float timeOnce = 5;

    bool begin = false;
    void OnGUI()
    {
        Time.timeScale = timeScale;
        if (!begin)
        {
            if (GUI.Button(new Rect(0, 0, 100, 100), "Begin"))
            {
                FloatLerp.instance.AddElement(new FloatLerp.Element(minValue, maxValue, timeOnce, FloatLerp.LerpType.Once, false, Once));
                FloatLerp.instance.AddElement(new FloatLerp.Element(minValue, maxValue, timeOnce, FloatLerp.LerpType.PingPong, false, PingPong));
                FloatLerp.instance.AddElement(new FloatLerp.Element(minValue, maxValue, timeOnce, FloatLerp.LerpType.ToggleLoop, true, ToggleLoop));
                begin = true;
            }
        }
        else
        {
            if (GUI.Button(new Rect(0, 0, 100, 100), "Stop"))
            {
                FloatLerp.instance.RemoveElement(Once);
                FloatLerp.instance.RemoveElement(PingPong);
                FloatLerp.instance.RemoveElement(ToggleLoop);
                begin = false;
            }
        }
    }



    void Once(float value)
    {
        onceValue = value;
    }

    void PingPong(float value)
    {
        pingPongValue = value;
    }

    void ToggleLoop(float value)
    {
        toggleValue = value;
    }


}
