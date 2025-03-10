using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class GameResultController : MonoBehaviour
    {
        [Header("Popup Prefabs")]
        [SerializeField] private GameObject gameOverPopup;
        [SerializeField] private GameObject gameWinPopup;

        [Header("Game Over Buttons")]
        [SerializeField] private Button gameOverRestartButton;
        [SerializeField] private Button gameOverQuitButton;

        [Header("Game Win Buttons")]
        [SerializeField] private Button gameWinNextLevelButton;
        [SerializeField] private Button gameWinQuitButton;

        [Header("Next Scene")]
        [SerializeField] private string nextSceneName;

        private void Start()
        {
            if (gameOverPopup != null)
                gameOverPopup.SetActive(false);

            if (gameWinPopup != null)
                gameWinPopup.SetActive(false);

            if (gameOverRestartButton != null)
                gameOverRestartButton.onClick.AddListener(RestartGame);

            if (gameOverQuitButton != null)
                gameOverQuitButton.onClick.AddListener(QuitGame);

            if (gameWinNextLevelButton != null)
                gameWinNextLevelButton.onClick.AddListener(LoadNextLevel);

            if (gameWinQuitButton != null)
                gameWinQuitButton.onClick.AddListener(QuitGame);
        }

        public void ShowGameOverPopup()
        {
            Time.timeScale = 0f;

            if (gameWinPopup != null)
                gameWinPopup.SetActive(false);

            if (gameOverPopup != null)
                gameOverPopup.SetActive(true);
        }

        public void ShowGameWinPopup()
        {
            Time.timeScale = 0f;

            if (gameOverPopup != null)
                gameOverPopup.SetActive(false);

            if (gameWinPopup != null)
                gameWinPopup.SetActive(true);
        }

        public void ClosePopup()
        {
            Time.timeScale = 1f;

            if (gameOverPopup != null)
                gameOverPopup.SetActive(false);

            if (gameWinPopup != null)
                gameWinPopup.SetActive(false);
        }

        private void RestartGame()
        {
            Time.timeScale = 1f;

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void LoadNextLevel()
        {
            Time.timeScale = 1f;

            if (!string.IsNullOrEmpty(nextSceneName))
            {
                SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                Debug.LogWarning("Next scene name is not set!");
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        private void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

    }

}

