/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2016-04-09     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using System.Collections.Generic;
using ThinksquirrelSoftware.Utilities;

namespace KMTool
{

    /// <summary>
    /// 主要用于摄像机震动的脚本
    /// </summary>
    public class CameraShakeCtrl : MonoBehaviour
    {
        [System.Serializable]
        public class ShakeParams
        {
            public string name                = "shackName";
            public int numberOfShakes         = 2;
            public Vector3 shakeAmount        = new Vector3(.3f, .3f, .3f);
            public Vector3 rotationAmount     = Vector3.zero;
            public float distance             = 2;
            public float speed                = 80;
            public float decay                = .2f;
            public float guiShakeModifier     = 1;
            public bool multiplyByTimeScale   = true;
        }

        public List<ShakeParams> shakes = new List<ShakeParams>();

        public static Dictionary<string,ShakeParams> dictShakes = new Dictionary<string, ShakeParams>();

        static public void Shake(string shakeName, System.Action callback = null)
        {
            ShakeParams pars = GetShake(shakeName);
            Shake(pars, callback);
        }

        static public void Shake(ShakeParams pars,System.Action callback = null)
        {
            if (!dictShakes.ContainsKey(pars.name))
            {
                dictShakes.Add(pars.name, pars);
            }

            if (pars != null)
            {
                if (!CameraShake.instance)
                {
                    Camera.main.gameObject.AddComponent<CameraShake>();
                }

                CameraShake.Shake(pars.numberOfShakes, pars.shakeAmount, pars.rotationAmount, 
                    pars.distance, pars.speed, pars.decay, pars.guiShakeModifier, pars.multiplyByTimeScale,callback);
            }
        }


    	// Use this for initialization
    	void Start() 
    	{
            for (int i = 0; i < shakes.Count; i++)
            {
                AddShake(shakes[i]);
            }
    	}

        static public void AddShake(ShakeParams pars)
        {
            if (!dictShakes.ContainsKey(pars.name))
            {
                dictShakes.Add(pars.name, pars);
            }
            else
            {
                Debug.LogWarning("already this share : " + pars.name);
            }
        }

        static public ShakeParams GetShake(string name)
        {
            if (dictShakes.ContainsKey(name))
            {
                return dictShakes[name];
            }
            else
            {
                Debug.LogWarning("not found shake : " + name);
            }
            return null;
        }
    }
}