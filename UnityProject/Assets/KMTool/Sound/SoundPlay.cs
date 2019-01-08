using UnityEngine;
using System.Collections;

namespace KMTool
{
    /// <summary>
    /// 声音播放器
    /// 
    /// Maintaince Logs: 
    /// 2015-05-08		WP			Initial version.
    /// <summary>
    [RequireComponent(typeof(AudioSource))]
    public class SoundPlay : MonoBehaviour
    {
        public delegate void DelFinish();

        public DelFinish eventFinish;

        private AudioSource mAudio;
        public AudioSource audioSource
        {
            get
            {
                if (mAudio == null)
                {
                    mAudio = GetComponent<AudioSource>();
                }
                return mAudio;
            }
        }

        public string SoundName
        {
            get
            {
                if(audioSource && audioSource.clip)
                    return audioSource.clip.name;
                return "null";
            }
        }

        public void Play(AudioClip clip, Vector3 pos, float time = 0)
        {
            if (clip != null)
            {
                audioSource.clip = clip;
                transform.position = pos;
                audioSource.volume = SoundManager.volume;
                audioSource.Stop();


                gameObject.name = "SoundPlay(" + clip.name + ")";

                if (time <= 0)
                {
                    audioSource.loop = false;
                    Invoke("SetDisable", clip.length);
                }
                else
                {
                    audioSource.loop = true;
                    Invoke("SetDisable", time);

                }
                audioSource.Play();
            }
            else SetDisable();
        }

        void SetDisable()
        {
            audioSource.Stop();
            SoundManager.instance.AddToGC(this);

            if (eventFinish != null)
            {
                eventFinish();
                eventFinish = null;
            }
        }

        public void SetVolume(float volume)
        {
            audioSource.volume = volume;
        }

        public void Pause()
        {

        }

        public void Stop()
        {
            if (audioSource && audioSource.isPlaying)
            {
                SetDisable();
            }
        }

        void KMDebug()
        {
            audioSource.Play();
        }


    }
}