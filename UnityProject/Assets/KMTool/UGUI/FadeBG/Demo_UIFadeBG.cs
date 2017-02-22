/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2017-02-22     WP      Initial version
 * 
 * *****************************************************************************/

using KMTool;
using UnityEngine;
using System.Collections;

namespace KMToolDemo
{
    /// <summary>
    /// 背景渐入渐出的Demo
    /// </summary>
    public class Demo_UIFadeBG : MonoBehaviour
    {
        public GameObject[] goMenu;
        public GameObject[] goGame;

        private bool isGame = false;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void BtnToGame()
        {
            isGame = true;
            UIFadeBG.BeginFade(FadeFinish);
        }

        public void BtnToMenu()
        {
            isGame = false;
            UIFadeBG.BeginFade(FadeFinish);
        }

        private void FadeFinish()
        {
            for (int i = 0; i < goGame.Length; i++)
            {
                goGame[i].SetActive(isGame);
            }
            for (int i = 0; i < goMenu.Length; i++)
            {
                goMenu[i].SetActive(!isGame);
            }

        }

        #region 测试

        public void KMDebug()
        {
            Debug.Log(" ---------KMDebug----------", gameObject);
        }

        public void KMEditor()
        {
            Debug.Log(" ---------KMEditor----------", gameObject);
        }

        #endregion
    }
}