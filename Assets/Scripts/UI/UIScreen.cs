using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIScreen : MonoBehaviour
    {
        #region Variables

        [Header("Main Properties")] public Selectable m_StartSelectable;

        [Header("Screen Events")] public UnityEvent onScreenStart = new();
        public UnityEvent onScreenClose = new();

        private static readonly int Show = Animator.StringToHash("show");

        #endregion

        #region Main Methods

        public virtual void Init()
        {
            if (m_StartSelectable)
            {
                EventSystem.current.SetSelectedGameObject(m_StartSelectable.gameObject);
            }
        }

        #endregion

        #region Helper Methods

        public virtual void StartScreen()
        {
            gameObject.SetActive(true);
            onScreenStart?.Invoke();
        }

        public virtual void CloseScreen()
        {
            gameObject.SetActive(false);
            onScreenClose?.Invoke();
        }


        #endregion
    }
}