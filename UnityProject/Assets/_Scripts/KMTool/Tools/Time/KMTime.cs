using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 计时器委托
/// </summary>
/// <param name="curTime">当前时间</param>
/// <param name="sumTime">总时间</param>
public delegate void DelTimeCount(float curTime, float sumTime);

/// <summary>
/// 描述 忽略TimeScale 而提供的一些时间 参数
/// 
/// Maintaince Logs:
/// 2015-03-30	WP			Initial version. 
/// 2015-08-21  WP          update....by unity api ,add scale,rename class to KMTime
/// 2015-11-09  WP          添加计时功能
/// </summary>
public class KMTime : MonoBehaviour
{
    private static KMTime mInstance;
    private static KMTime mInst
    {
        get
        {
            if (mInstance == null) Spawn();
            return mInstance;
        }
    }
    private float mRealTime = 0f;
    /// <summary>
    /// 计时器方法列表
    /// </summary>
    private List<KMTimeCount> listTimeCount = new List<KMTimeCount>();

    static private float m_timeScale = 1;
    static public float timeScale
    {
        get { return m_timeScale; }
        set
        {
            if (value != m_timeScale)
            {
                m_timeScale = value;
                if (m_timeScale < 0)
                {
                    m_timeScale = 0;
                    Debug.LogError("time scale < 0");
                }
            }
        }
    }

    static void Spawn()
    {
        GameObject go = new GameObject("_KMTime");
        DontDestroyOnLoad(go);
        mInstance = go.AddComponent<KMTime>();
    }

    static public void AddTimeCount(float time, DelTimeCount method)
    {
        KMTimeCount tc = new KMTimeCount(time, method);
        mInst.listTimeCount.Add(tc);
    }

    private class KMTimeCount
    {
        private float timeSum = 1;
        private float timeParam = 0;
        private DelTimeCount eventTimeCount;

        public KMTimeCount(float sum, DelTimeCount method)
        {
            timeSum = sum;
            eventTimeCount = method;
        }

        public bool Update()
        {
            if (timeParam >= timeSum) return true;

            timeParam += KMTime.deltaTime;
            timeParam = Mathf.Min(timeParam, timeSum);

            if (eventTimeCount != null) eventTimeCount(timeParam, timeSum);

            return false;
        }
    }

#if UNITY_4_3

	float mRealDelta = 0f;

	/// <summary>
	/// Real time since startup.
	/// </summary>

	static public float time
	{
		get
		{
#if UNITY_EDITOR
			if (!Application.isPlaying) return Time.realtimeSinceStartup;
#endif
			return mInst.mRealTime;
		}
	}

	/// <summary>
	/// Real delta time.
	/// </summary>

	static public float deltaTime
	{
		get
		{
#if UNITY_EDITOR
			if (!Application.isPlaying) return 0f;
#endif
			return mInst.mRealDelta;
		}
	}

	void Update ()
	{
		float rt = Time.realtimeSinceStartup;
		mRealDelta = Mathf.Clamp01(rt - mRealTime) * timeScale;
		mRealTime = rt;
	}
#else



    static public float time
    {
        get
        {
            return mInst.mRealTime;
        }
    }

    /// <summary>
    /// Real delta time.
    /// </summary>

    static public float deltaTime { get { return Time.unscaledDeltaTime * timeScale; } }

    void Update()
    {
        mRealTime += deltaTime;

        for (int i = 0; i < listTimeCount.Count; )
        {
            if (listTimeCount[i].Update())
            {
                listTimeCount.RemoveAt(i);
                continue;
            }
            i++;
        }
    }
#endif
}