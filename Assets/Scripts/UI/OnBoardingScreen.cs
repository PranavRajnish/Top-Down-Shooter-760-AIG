using System.Collections;
using TMPro;
using UnityEngine;

namespace UI
{
    public class OnBoardingScreen : UIScreen
    {
        [SerializeField] private TMP_Text timerText;
        [SerializeField] private float waitTime = 5f;

        public override void StartScreen()
        {
            base.StartScreen();
            StartCoroutine(DisableScreen());
        }

        public IEnumerator DisableScreen()
        {
            while (true)
            {
                var startTime = Time.realtimeSinceStartup;
                while (Time.realtimeSinceStartup - startTime < waitTime)
                {
                    timerText.SetText((waitTime - Mathf.FloorToInt(Time.realtimeSinceStartup - startTime)).ToString());
                    yield return 0;
                }

                CloseScreen();
                break;
            }
        }

        // private async void DisableScreen()
        // {
        //     
        // }
    }
}