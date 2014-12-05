using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// /// 路点控制器
/// 
/// Maintaince Logs:
/// 2014-12-05  WP      Initial version. 
/// </summary>
public class WayController : MonoBehaviour
{

    public WayPointBezier _bezier;
  
    public bool normalised = true;


    //the time used in the editor to preview the path animation
    public float editorTime = 0;
    //the time the path animation should last for
    public float pathTime = 10;

    /// <summary>
    /// 播放列表
    /// </summary>
    private List<WayAnimator> anims = new List<WayAnimator>();

    public bool showPreview = true;
    public bool showScenePreview = true;

    public WayController nextAnimation = null;

    //Script based controls - hook up your scripts to these to control your

    /// <summary>
    /// Gets or sets the path speed.
    /// </summary>
    /// <value>
    /// The path speed.
    /// </value>
    public float pathSpeed
    {
        get
        {
            return bezier.storedTotalArcLength / pathTime;
        }
        set
        {
            float newPathSpeed = value;
            pathTime = bezier.storedTotalArcLength / Mathf.Max(newPathSpeed, 0.000001f);
        }
    }

    //set the time of the animtion (0-1)
    public void Seek(float value)
    {
        //TODO:
        //_percentage = Mathf.Clamp01(value);
        //thanks kelnishi!
        //UpdateAnimationTime(false);
        //bool p = playing;
        //playing = true;
        //UpdateAnimation();
        //playing = p;
    }

    public WayPointBezier bezier
    {
        get
        {
            if (!_bezier)
                _bezier = GetComponent<WayPointBezier>();
            return _bezier;
        }
    }

    //normalise the curve and apply easing
    public float RecalculatePercentage(float percentage)
    {
        if (bezier.numberOfControlPoints == 0)
            return percentage;
        float normalisedPercentage = bezier.GetNormalisedT(percentage);
        int numberOfCurves = bezier.numberOfCurves;
        float curveT = 1.0f / (float)numberOfCurves;
        int point = Mathf.FloorToInt(normalisedPercentage / curveT);
        float curvet = Mathf.Clamp01((normalisedPercentage - point * curveT) * numberOfCurves);
        if (bezier.controlPoints[point]._curve != null)
            return bezier.controlPoints[point]._curve.Evaluate(curvet) / numberOfCurves + (point * curveT);
        else
            return percentage;
    }

    //MONOBEHAVIOURS

    public void Login(WayAnimator anim)
    {
        if (anims.Contains(anim)) return;

        anim.Init(bezier.numberOfControlPoints - 1, bezier.GetPathRotation(0).eulerAngles);

        anims.Add(anim);
    }

    public void Logout(WayAnimator anim) { anims.Remove(anim); }


    void Update()
    {
        for (int i = 0; i < anims.Count; )
        {
            if (anims[i] == null) { anims.RemoveAt(i); continue; }

            anims[i].UpdateEvent(bezier, pathTime);

            i++;
        }
    }

}
