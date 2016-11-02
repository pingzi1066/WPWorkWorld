using UnityEngine;
using System.Collections.Generic;

namespace KMTool
{
    /// <summary>
    /// 路点播放者
    /// 
    /// Maintaince Logs:
    /// 2014-12-05  WP      Initial version. 
    ///         07  WP      加入递归注册事件：PingPong 事件 为单独一个PingPong然后随后，或许可以把所有Anims走完后再倒回来走一次
    /// </summary>
    public class WayAnimator : MonoBehaviour
    {
        public enum modes
        {
            once,
            loop,
            reverse,
            reverseLoop,
            pingPong
        }

        public bool playOnStart = true;

        public modes mode = modes.once;

        public bool normalised = true;

        /// <summary>
        /// 路线
        /// </summary>
        public List<WayController> wayList = new List<WayController>();

        /// <summary>
        /// 当前注册位置
        /// </summary>
        private int curIndex = 0;

        //用于判断Pingpong时 来与回
        private float pingPongDirection = 1;
        private bool playing = false;

        private int atPointNumber = 0;

        private float _percentage = 0;
        private float usePercentage;

        /// <summary>
        /// 当前路线
        /// </summary>
        private WayController curWay;

        private WayBezier bezier { get { return curWay.bezier; } }

        //Events
        public delegate void AnimationEvent();

        public delegate void AnimationPointReachedWithNumberEvent(int pointNumber);

        public event AnimationEvent AnimStarted;
        public event AnimationEvent AnimPaused;
        public event AnimationEvent AnimStopped;
        public event AnimationEvent AnimFinished;
        public event AnimationEvent AnimLooped;
        public event AnimationEvent AnimPingPong;
        public event AnimationEvent AnimPointReached;
        public event AnimationPointReachedWithNumberEvent AnimPointReachedWithNumber;

        void Start()
        {
            LoginoutRecursion();
        }

        public void SetParms(List<WayController> ways, modes mode)
        {
            wayList = ways;
            this.mode = mode;
        }

        //play the animation as runtime
        public void Play()
        {
            playing = true;
            if (!isReversed)
            {
                if (_percentage == 0)
                    if (AnimStarted != null) AnimStarted();
            }
            else
            {
                if (_percentage == 1)
                    if (AnimStarted != null) AnimStarted();
            }
        }

        //stop and set the animation at the beginning
        public void Stop()
        {
            playing = false;
            CancelInvoke("Play");
            _percentage = 0;
            if (AnimStopped != null) AnimStopped();
        }

        //pasue the animation where it is
        public void Pause()
        {
            playing = false;
            CancelInvoke("Play");
            if (AnimPaused != null) AnimPaused();
        }

        public void Reverse()
        {
            switch (mode)
            {
                case modes.once:
                    mode = modes.reverse;
                    break;
                case modes.reverse:
                    mode = modes.once;
                    break;
                case modes.pingPong:
                    pingPongDirection = pingPongDirection == -1 ? 1 : -1;
                    break;
                case modes.loop:
                    mode = modes.reverseLoop;
                    break;
                case modes.reverseLoop:
                    mode = modes.loop;
                    break;
            }
        }

        public void Init(int reversedIndex, Vector3 initalRotation)
        {
            if (!isReversed)
            {
                _percentage = 0;
                atPointNumber = -1;
            }
            else
            {
                _percentage = 1;
                atPointNumber = reversedIndex;
                //Debug.Log("Init for reversed and index is : " + reversedIndex);
            }

            if (playOnStart)
                Play();
        }

        public void UpdateEvent(float pathTime)
        {
            if (playing)
            {
                UpdateAnimationTime(pathTime);
                UpdateAnimation();
                UpdatePointReached();
            }
        }

        private bool isReversed
        {
            get { return (mode == modes.reverse || mode == modes.reverseLoop || pingPongDirection < 0); }
        }

        private void UpdateAnimation()
        {

            if (!playing)
                return;

            transform.position = bezier.GetPathPosition(usePercentage);

            Vector3 minusPoint, plusPoint;
            switch (bezier.mode)
            {
                case WayBezier.viewmodes.usercontrolled:
                    transform.rotation = bezier.GetPathRotation(usePercentage);
                    break;

                case WayBezier.viewmodes.target:

                    transform.LookAt(bezier.target.transform.position);
                    break;

                case WayBezier.viewmodes.followpath:
                    if (!bezier.loop)
                    {
                        minusPoint = bezier.GetPathPosition(Mathf.Clamp01(usePercentage - 0.05f));
                        plusPoint = bezier.GetPathPosition(Mathf.Clamp01(usePercentage + 0.05f));
                    }
                    else
                    {
                        float minus = usePercentage - 0.05f;
                        if (minus < 0)
                            minus += 1;
                        float plus = usePercentage + 0.05f;
                        if (plus > 1)
                            plus += -1;
                        minusPoint = bezier.GetPathPosition(minus);
                        plusPoint = bezier.GetPathPosition(plus);
                    }

                    transform.LookAt(transform.transform.position + (plusPoint - minusPoint));
                    transform.eulerAngles += transform.forward * -bezier.GetPathTilt(usePercentage);
                    break;

                case WayBezier.viewmodes.reverseFollowpath:
                    if (!bezier.loop)
                    {
                        minusPoint = bezier.GetPathPosition(Mathf.Clamp01(usePercentage - 0.05f));
                        plusPoint = bezier.GetPathPosition(Mathf.Clamp01(usePercentage + 0.05f));
                    }
                    else
                    {
                        float minus = usePercentage - 0.05f;
                        if (minus < 0)
                            minus += 1;
                        float plus = usePercentage + 0.05f;
                        if (plus > 1)
                            plus += -1;
                        minusPoint = bezier.GetPathPosition(minus);
                        plusPoint = bezier.GetPathPosition(plus);
                    }

                    transform.LookAt(transform.position + (minusPoint - plusPoint));
                    break;
            }
        }

        private void UpdateAnimationTime(float pathTime)
        {
            switch (mode)
            {

                case modes.once:
                    if (_percentage >= 1)
                    {
                        playing = false;
                        if (AnimPointReached != null) AnimPointReached();
                        if (AnimPointReachedWithNumber != null) AnimPointReachedWithNumber(bezier.numberOfControlPoints - 1);
                        if (AnimFinished != null) AnimFinished();
                    }
                    else
                    {
                        _percentage += Time.deltaTime * (1.0f / pathTime);
                    }
                    break;

                case modes.loop:
                    if (_percentage >= 1)
                    {
                        _percentage = 0;
                        if (AnimPointReached != null) AnimPointReached();
                        if (AnimPointReachedWithNumber != null) AnimPointReachedWithNumber(bezier.numberOfControlPoints - 1);
                        if (AnimLooped != null) AnimLooped();
                    }
                    _percentage += Time.deltaTime * (1.0f / pathTime);
                    break;

                case modes.reverseLoop:
                    if (_percentage <= 0)
                    {
                        _percentage = 1;
                        if (AnimPointReached != null) AnimPointReached();
                        if (AnimPointReachedWithNumber != null) AnimPointReachedWithNumber(0);
                        if (AnimLooped != null) AnimLooped();
                    }
                    _percentage += -Time.deltaTime * (1.0f / pathTime);
                    break;

                case modes.reverse:
                    if (_percentage <= 0)
                    {
                        playing = false;
                        if (AnimPointReached != null) AnimPointReached();
                        if (AnimPointReachedWithNumber != null) AnimPointReachedWithNumber(0);
                        if (AnimFinished != null) AnimFinished();
                    }
                    else
                    {
                        _percentage += -Time.deltaTime * (1.0f / pathTime);
                    }
                    break;

                case modes.pingPong:
                    _percentage += Time.deltaTime * (1.0f / pathTime) * pingPongDirection;
                    if (_percentage >= 1)
                    {
                        _percentage = 0.99f;
                        pingPongDirection = -1;
                        if (AnimPointReached != null) AnimPointReached();
                        if (AnimPointReachedWithNumber != null) AnimPointReachedWithNumber(bezier.numberOfControlPoints - 1);
                        if (AnimPingPong != null) AnimPingPong();
                    }
                    if (_percentage <= 0)
                    {
                        _percentage = 0.01f;
                        pingPongDirection = 1;
                        if (AnimPointReached != null) AnimPointReached();
                        if (AnimPointReachedWithNumber != null) AnimPointReachedWithNumber(0);
                        if (AnimPingPong != null) AnimPingPong();
                    }
                    break;
            }

            _percentage = Mathf.Clamp01(_percentage);
            usePercentage = normalised ? curWay.RecalculatePercentage(_percentage) : _percentage;//this is the percentage used by everything but the rotation
        }

        private void UpdatePointReached()
        {
            int currentPointNumber = bezier.GetPointNumber(usePercentage);

            if (currentPointNumber != atPointNumber)
            {
                //we've hit a point
                WayPoint atPoint;
                if (!isReversed)
                    atPoint = bezier.controlPoints[currentPointNumber];
                else
                {
                    //Debug.Log(bezier.controlPoints.Length + "  ------ number " + atPointNumber, curWay);
                    atPoint = bezier.controlPoints[atPointNumber];
                }

                if (AnimPointReached != null) AnimPointReached();
                if (!isReversed)
                    if (AnimPointReachedWithNumber != null) AnimPointReachedWithNumber(currentPointNumber);
                    else
                        if (AnimPointReachedWithNumber != null) AnimPointReachedWithNumber(atPointNumber);

                switch (atPoint.delayMode)
                {
                    case WayPoint.DELAY_MODES.none:
                        //do nothing extra
                        break;
                    case WayPoint.DELAY_MODES.indefinite:
                        Pause();
                        break;
                    case WayPoint.DELAY_MODES.timed:
                        Pause();
                        Invoke("Play", atPoint.delayTime);
                        break;
                }
            }

            atPointNumber = currentPointNumber;
        }

        /// <summary>
        /// 注册事件注销递归
        /// </summary>
        private void LoginoutRecursion()
        {
            //无路可选
            if (wayList.Count < 1 || curIndex > (wayList.Count - 1))
            {
                Debug.Log("way list Finished----------------", gameObject);
                return;
            }

            //登出
            if (curWay != null)
            {
                //注销事件
                switch (mode)
                {
                    case modes.once:
                    case modes.reverse:
                        AnimFinished -= LoginoutRecursion;
                        break;
                    case modes.loop:
                    case modes.reverseLoop:
                        AnimLooped -= LoginoutRecursion;
                        break;
                    case modes.pingPong:
                        if (pingPongDirection == 1)
                        {
                            AnimPingPong -= LoginoutRecursion;
                        }
                        else
                        {
                            return; //完成ping
                        }
                        break;
                }
                curWay.Logout(this);
                curWay = null;
            }

            //设置
            curWay = wayList[curIndex];
            //Debug.Log("login index is : " + curIndex, curWay);

            if (curWay != null) //注册
            {
                curWay.Login(this);
                //下一事件
                curIndex++;
                //下一动画
                switch (mode)
                {
                    case modes.once:
                    case modes.reverse:
                        AnimFinished += LoginoutRecursion;
                        break;
                    case modes.loop:
                    case modes.reverseLoop:
                        AnimLooped += LoginoutRecursion;
                        //重复
                        if (curIndex == (wayList.Count))
                        {
                            curIndex = 0;
                        }
                        break;


                    case modes.pingPong:
                        AnimPingPong += LoginoutRecursion;
                        //重复
                        if (curIndex == (wayList.Count))
                        {
                            curIndex = 0;
                        }
                        break;
                }

                //Debug.Log("Login event ", curWay);
                //Debug.Log("next index is : " + curIndex);
            }
            else
            {
                curIndex++;
                LoginoutRecursion();
                return;
            }
        }
    }
}