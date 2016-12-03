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

        public void Play(AudioClip clip, Vector3 pos)
        {
            if (clip != null)
            {
                audioSource.clip = clip;
                transform.position = pos;
                audioSource.volume = SoundManager.volume;
                audioSource.Stop();
                audioSource.Play();

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
            audioSource.volume = volume;
        }

        public void Pause()
        {

        }

        public void Stop()
        {
            if (audioSource && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }

        void KMDebug()
        {
            audioSource.Play();
        }


    }
}