using UnityEngine;
using System.Collections;
using KMTool;


namespace KMToolDemo
{
    /// <summary>
    /// 声音管理的例子
    /// 
    /// Maintaince Logs: 
    /// 2015-05-11		WP			Initial version.
    /// <summary>
    public class Demo_SoundPlay : MonoBehaviour
    {

        float height = 30;
        float width = 120;

        float left = 10;
        float top = 10;


        private Rect rect
        {
            get
            {
                return new Rect(left, top, width, height);
            }
        }

        void OnGUI()
        {


            top = 10;
            width = 120;

            if (GUI.Button(rect, "Play Music 01"))
            {
                SoundManager.PlayMusic("music01");
            }

            top += 35;

            if (GUI.Button(rect, "Play Music 02"))
            {
                SoundManager.PlayMusic("music02");
            }

            top += 35;

            if (GUI.Button(rect, "Pause Music"))
            {
                SoundManager.PauseMusic();
            }

            top += 35;

            if (GUI.Button(rect, "Stop Music"))
            {
                SoundManager.StopMusic();
            }

            top += 35;

            if (GUI.Button(rect, "Play sound_angel"))
            {
                SoundManager.PlaySound("sound_angel", Vector3.zero);
            }

            top += 35;
            width += 50;

            if (GUI.Button(rect, "Play sound_failure"))
            {
                SoundManager.PlaySound("sound_failure", Vector3.zero);
            }

            top += 35;

            if (GUI.Button(rect, "Play Random attack"))
            {
                SoundManager.PlayRangeSound("attack", Vector3.zero);
            }

            top += 35;

            width += 250;

            if (GUI.Button(rect, "Play sound_failure but pause Music and stop other sound "))
            {
                SoundManager.PlayAndWaitOther("sound_failure", Vector3.zero);
            }

            top += 35;

            if (GUI.Button(rect, "Play Audio Source by self "))
            {
                SoundManager.PlaySound(GetComponent<AudioSource>());
            }

            top += 35;

            SoundManager.volume = GUI.HorizontalSlider(rect, SoundManager.volume, 0, 1);

        }
    }
}