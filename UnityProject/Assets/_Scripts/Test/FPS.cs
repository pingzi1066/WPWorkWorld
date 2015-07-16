using System;
using UnityEngine;

public class FPS : MonoBehaviour
{
    private float accum;
    private int frames;
    private float timeleft;
    public float updateInterval = 0.5f;

    private void Start()
    {
        if (base.guiText == null)
        {
            Debug.Log("UtilityFramesPerSecond needs a GUIText component!");
            base.enabled = false;
        }
        else
        {
            this.timeleft = this.updateInterval;
        }
    }

    private void Update()
    {
        this.timeleft -= Time.deltaTime;
        this.accum += Time.timeScale / Time.deltaTime;
        this.frames++;
        if (this.timeleft <= 0.0)
        {
            float num = this.accum / ((float) this.frames);
            string str = string.Format("{0:F2} FPS", num);
            base.guiText.text = str;
            if (num < 30f)
            {
                base.guiText.material.color = Color.yellow;
            }
            else if (num < 10f)
            {
                base.guiText.material.color = Color.red;
            }
            else
            {
                base.guiText.material.color = Color.green;
            }
            this.timeleft = this.updateInterval;
            this.accum = 0f;
            this.frames = 0;
        }
    }
}

