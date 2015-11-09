/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2015-11-09     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using System.Collections;

/// <summary>
/// 计时器 Demo
/// </summary>
public class Demo_KMTime : MonoBehaviour
{
    private float timeScale = 0;
    private float curTime = 0;
    private float sumTime = 0;
    private bool isFinished = true;

    void OnGUI()
    {
        Time.timeScale = timeScale;
        float left = 0;
        float top = 0;
        Rect rect = new Rect(left, top, 400, 30);
        Rect rect1 = new Rect(left, top + 30, 400, 30);
        GUI.TextField(rect, "Time Scale is " + timeScale);
        timeScale = GUI.HorizontalScrollbar(rect1, timeScale, .2f, 0, 5);

        Rect rect2 = new Rect(left, top + 60, 400, 30);
        sumTime = GUI.HorizontalScrollbar(rect2, sumTime, .2f, 1, 20);
        GUI.TextField(new Rect(left, top + 90, 400, 30), "sumTime is  " + sumTime);

        if (isFinished)
        {
            if (GUI.Button(new Rect(left, top + 120, 100, 100), "Add"))
            {
                KMTime.AddTimeCount(sumTime, TestMethod);
                isFinished = false;
            }
        }
        else
        {
            GUI.TextField(new Rect(left, top + 120, 400, 100), "cur Time is  " + curTime);
        }

    }

    void TestMethod(float cur , float sum)
    {
        curTime = cur;
        sumTime = sum;
        //Debug.Log(cur + " ----------- " + sum);

        if (cur >= sum)
        {
            isFinished = true;
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}