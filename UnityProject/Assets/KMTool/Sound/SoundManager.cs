using UnityEngine;
using System.Collections.Generic;

namespace KMTool
{
    /// <summary>
    /// 音乐音效管理
    /// 
    /// Maintaince Logs: 
    /// 2015-05-08		WP			Initial version.
    /// 2016-04-08      WP          加入外部自带音效的播放接口
    /// <summary>
    public class SoundManager : MonoBehaviour
    {
        private static float mVolume = 1;
        public static float volume
        {
            get
            {
                return mVolume;
            }
            set
            {
                if (mVolume != value)
                {
                    mVolume = Mathf.Clamp01(value);

                    if (instance)
                    {
                        foreach (SoundPlay sp in instance.existingAudios)
                        {
                            sp.SetVolume(mVolume);
                        }
                        instance.music.volume = mVolume;
                    }
                }
            }
        }

        [System.Serializable]
        public class RandomSound
        {
            public string name;

            public AudioClip[] sounds;
        }

        [SerializeField] private AudioClip[] sounds;

        [SerializeField] private AudioClip[] musics;

        [SerializeField] private List<RandomSound> rangeSounds = new List<RandomSound>();

        public int limitCount = 20;

        [SerializeField][DisableEdit] private AudioSource music;

        [SerializeField][DisableEdit] private List<SoundPlay> gcAudios = new List<SoundPlay>();

        [SerializeField][DisableEdit] private List<SoundPlay> existingAudios = new List<SoundPlay>();

        public static SoundManager instance;

        private SoundPlay waitForPlay;

        void Awake()
        {
            instance = this;

            //create audio sound
            GameObject go = KMTools.AddGameObj(gameObject);
            go.name = "SoundPlay";
            SoundPlay prbSoundPlay = go.AddComponent<SoundPlay>();

            for (int i = 0; i < limitCount; i++)
            {
                SoundPlay aSource = KMTools.AddChild<SoundPlay>(gameObject, prbSoundPlay, false, false);
                gcAudios.Add(aSource);
            }

            Destroy(go);

            GameObject goMusic = KMTools.AddGameObj(gameObject);
            goMusic.name = "Music";
            music = goMusic.AddComponent<AudioSource>();
        }

        void Start()
        {

        }

        public void AddToGC(SoundPlay soundPlay)
        {
            if (existingAudios.Contains(soundPlay))
            {
                existingAudios.Remove(soundPlay);
                gcAudios.Add(soundPlay);
                soundPlay.name = "gc_" + soundPlay.name;
            }
        }

        public SoundPlay GetSoundPlay()
        {
            if (gcAudios.Count > 0)
            {
                SoundPlay sp = instance.gcAudios[0];
                instance.gcAudios.RemoveAt(0);
                instance.existingAudios.Add(sp);
                return sp;
            }
            else
            {
                Debug.Log(" sound exceed limit count   ");
            }
            return null;
        }

        public static void PlaySound(string name, Vector3 pos)
        {
            if (instance)
            {
                SoundPlay sp = instance.GetSoundPlay();
                if (sp)
                {
                    sp.Play(instance.GetSound(name), pos);
                }
                else
                {
                    Debug.LogError(" sound exceed limit count   " + name);
                }
            }
        }

        public static void PlaySound(AudioSource audio)
        {
            if (audio)
            {
                //这里的音量设置：
                //audio.volume = volume;
                audio.Stop();
                audio.Play();
            }
        }

        public static void PlaySound(AudioClip clip, Vector3 pos)
        {
            if (instance)
            {
                SoundPlay sp = instance.GetSoundPlay();
                if (sp)
                {
                    sp.Play(clip, pos);
                }
                else
                {
                    Debug.LogError(" sound exceed limit count   ");
                }
            }
        }

        public static void PlayMusic(string name, bool isLoop = true)
        {
            if (instance)
            {
                AudioClip clip = instance.GetMusic(name);
                if (clip)
                {
                    instance.music.clip = clip;
                    instance.music.loop = isLoop;
                    instance.music.Play();

                    instance.music.name = "Music(" + clip.name + ")";
                }
                else
                {
                    Debug.LogError("music is null  " + name);
                }
            }
        }

        public static void PauseMusic() { if (instance) instance.PauseCurMusic(); }

        public static void StopMusic() { if (instance) instance.StopCurMusic(); }

        public static void PlayRangeSound(string name, Vector3 pos)
        {
            if (instance)
            {
                SoundPlay sp = instance.GetSoundPlay();
                if (sp)
                {
                    sp.Play(instance.GetRange(name), pos);
                }
                else
                {
                    Debug.LogError(" sound exceed limit count   " + name);
                }
            }
        }

        public static void PlayAndWaitOther(string name, Vector3 pos)
        {
            if (instance)
            {
                SoundPlay sp = instance.GetSoundPlay();
                if (sp)
                {
                    instance.StopAllAndPauseMusic();
                    
                    sp.Play(instance.GetSound(name), pos);

                    if (instance.waitForPlay != null)
                    {
                        instance.waitForPlay.eventFinish = null;
                    }
                    sp.eventFinish += instance.ContinueCurMusic;
                    instance.waitForPlay = sp;
                }
                else
                {
                    Debug.LogError(" sound exceed limit count   " + name);
                }
            }
        }

        private AudioClip GetSound(string name)
        {
            foreach (AudioClip a in sounds)
            {
                if (a.name == name) return a;
            }
            return null;
        }

        private AudioClip GetMusic(string name)
        {
            foreach (AudioClip a in musics)
            {
                if (a.name == name) return a;
            }
            return null;
        }

        private AudioClip GetRange(string name)
        {
            foreach (RandomSound rs in rangeSounds)
            {
                if (rs.name == name)
                {
                    AudioClip[] acs = rs.sounds;
                    AudioClip ac = acs[Random.Range(0, acs.Length)];
                    return ac;
                }
            }

            return null;
        }

        private void StopAllAndPauseMusic()
        {
            foreach (SoundPlay ps in existingAudios)
            {
                ps.Stop();
            }

            PauseCurMusic();
        }

        private void PauseCurMusic() { if (music) music.Pause(); }

        private void ContinueCurMusic() { if (music) music.Play(); }

        private void StopCurMusic() { if (music) music.Stop(); }

        void KMDebug()
        {
            SoundManager.PlayRangeSound("hero_walk", transform.position);
            Debug.Log("  sound name is " + musics[0].name + "  lenght  " + musics[0].length);
        }


    }
}