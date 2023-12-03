using UnityEngine;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        public void StartGame()
        {
            Time.timeScale = 1f;
        }
        public void StopGame()
        {
            Time.timeScale = 0f;
        }
    }
}