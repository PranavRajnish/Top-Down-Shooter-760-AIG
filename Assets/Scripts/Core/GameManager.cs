using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject endScreen;

        public void StartGame()
        {
            Time.timeScale = 1f;
        }
        public void StopGame()
        {
            Time.timeScale = 0f;
        }

        public void OnGameEnd()
        {
            Time.timeScale = 0f;
            endScreen.SetActive(true);
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

    }

   
}