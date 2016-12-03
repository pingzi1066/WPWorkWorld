/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2014-10-10   WP      Initial version: Added member
 * 2015-11-05   WP      add kmtime
 * 
 * *****************************************************************************/

using UnityEngine;
using System.Collections;

namespace KMTool
{
    /// <summary>
    /// 用于粒子发射
    /// </summary>
    public class Ef_Particle : MonoBehaviour
    {
        [SerializeField]
        ParticleSystem mEmitter;
        float timeParam;

        public bool playOnAwake = false;

        /// <summary>
        /// 粒子播放时间
        /// </summary>
        public float activeTime = 0.5f;

        public bool ignoreTimeScale = false;
        float timeAtLastFrame = 0;
        float curFrame = 0;

        bool isInit = false;

        void Awake()
        {
            Init();

            if (!playOnAwake)
                gameObject.SetActive(false);
        }

        void Init()
        {
            if (!isInit)
            {
                if (mEmitter == null)
                {
                    mEmitter = GetComponent<ParticleSystem>();
                    if (!mEmitter)
                        mEmitter = gameObject.GetComponentInChildren<ParticleSystem>();
                    //
                    if (mEmitter == null)
                    {
                        Debug.LogWarning("===========-=-=-");
                        Destroy(this);
                    }
                }
            }
        }

        void OnEnable()
        {
            timeParam = 0;
            if (!ignoreTimeScale)
                mEmitter.Play();

            timeAtLastFrame = KMTime.time;

        }

        void OnDisable()
        {
            mEmitter.Stop();
        }

        void Update()
        {
            curFrame = KMTime.time - timeAtLastFrame;
            timeParam += ignoreTimeScale ? KMTime.deltaTime : Time.deltaTime;
            if (timeParam >= activeTime)
            {
                gameObject.SetActive(false);
            }
            else
            {
                if (ignoreTimeScale)
                {
                    mEmitter.Play();
                    mEmitter.Simulate(curFrame, true, false);
                }
            }
            timeAtLastFrame = KMTime.time;
        }

        public void Play()
        {
            if (!isInit)
                Init();

            if (!playOnAwake) playOnAwake = true;
            gameObject.SetActive(true);
        }
    }
}