using UnityEngine;
using System.Collections;

/// <summary>
/// 声音播放器
/// 
/// Maintaince Logs: 
/// 2015-05-08		WP			Initial version.
/// <summary>
public class SoundPlay : MonoBehaviour
{
    public delegate void DelFinish();

    public DelFinish eventFinish;

    public void Play(AudioClip clip, Vector3 pos)
    {
        if (clip != null)
        {
            audio.clip = clip;
            transform.position = pos;
            audio.volume = SoundManager.volume;
            audio.Stop();
            audio.Play();

            gameObject.name = "SoundPlay(" + clip.name + ")";

            Invoke("SetDisable", clip.length);
        }
        else SetDisable();
    }

    void SetDisable()
    {
        SoundManager.instance.AddToGC(this);

        if (eventFinish != null)
        {
            eventFinish();
            eventFinish = null;
        }
    }

    public void SetVolume(float volume)
    {
        audio.volume = volume;
    }

    public void Pause()
    {

    }

    public void Stop()
    {
        if (audio && audio.isPlaying)
        {
            audio.Stop();
        }
    }

    void KMDebug()
    {
        audio.Play();
    }


}
