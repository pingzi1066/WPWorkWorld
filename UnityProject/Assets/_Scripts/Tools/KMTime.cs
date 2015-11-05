using UnityEngine;
using System.Collections;

/// <summary>
/// 描述 忽略TimeScale 而提供的一些时间 参数
/// 
/// Maintaince Logs:
/// 2015-03-30	WP			Initial version. 
/// 2015-08-21  WP          update....by unity api ,add scale,rename class to KMTime
/// </summary>
public class KMTime : MonoBehaviour
{
    private static KMTime mInst;
    private float mRealTime = 0f;

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
        mInst = go.AddComponent<KMTime>();
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
			if (mInst == null) Spawn();
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
			if (mInst == null) Spawn();
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
            if (mInst == null) Spawn();
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
    }
#endif
}
