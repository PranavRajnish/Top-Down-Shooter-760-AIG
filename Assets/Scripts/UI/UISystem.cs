using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class UISystem : MonoBehaviour
    {
        #region Variables

        [Header("Main Properties")] public UIScreen m_StartScreen;
        [Header("System Events")] public UnityEvent onSwitchedScreen = new();

        [Header("Fader Properties")] public Image fadeImg;
        public float fadeInDuration = 1.0f;
        public float fadeOutDuration = 1.0f;

        private UIScreen[] _screens = Array.Empty<UIScreen>();

        //property to get currentScreen and prevScreen
        public UIScreen PrevScreen { get; private set; }
        public UIScreen CurrentScreen { get; private set; }

        #endregion

        #region Main Methods

        void Start()
        {
            _screens = GetComponentsInChildren<UIScreen>(true);
            foreach (var screen in _screens)
            {
                screen.Init();
            }

            InitializeScreens();
            if (m_StartScreen)
            {
                SwitchScreens(m_StartScreen);
            }

            if (fadeImg)
            {
                fadeImg.gameObject.SetActive(true);
            }

            FadeIn();
        }

        #endregion

        #region Helper Methods

        public void SwitchScreens(UIScreen screen)
        {
            if (screen)
            {
                if (CurrentScreen)
                {
                    CurrentScreen.CloseScreen();
                    PrevScreen = CurrentScreen;
                }

                CurrentScreen = screen;
                CurrentScreen.gameObject.SetActive(true);
                CurrentScreen.StartScreen();
                if (onSwitchedScreen != null)
                {
                    onSwitchedScreen.Invoke();
                }
            }
        }

        public void FadeIn()
        {
            if (fadeImg)
            {
                fadeImg.CrossFadeAlpha(0f, fadeInDuration, false);
            }
        }

        public void FadeOut()
        {
            if (fadeImg)
            {
                fadeImg.CrossFadeAlpha(1f, fadeOutDuration, false);
            }
        }

        public void GoToPrevScreen()
        {
            if (PrevScreen)
            {
                SwitchScreens(PrevScreen);
            }
        }


        void InitializeScreens()
        {
            foreach (var screen in _screens)
            {
                //screen.gameObject.SetActive(true);
            }
        }

        #endregion
    }
}