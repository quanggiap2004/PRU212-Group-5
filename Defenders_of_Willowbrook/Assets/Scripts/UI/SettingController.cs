using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    [Header("Popup References")]
    [SerializeField] private GameObject settingsPopup;

    [Header("Popup Buttons")]
    [SerializeField] private Button? continueButton;
    [SerializeField] private Button? restartButton;
    [SerializeField] private Button? quitButton;

    [Header("External Settings Button")]
    [SerializeField] private Button openSettingsButton;

    private void Start()
    {
        settingsPopup.SetActive(false);

        if (continueButton != null)
            continueButton.onClick.AddListener(ContinueGame);

        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);

        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);

        openSettingsButton.onClick.AddListener(OpenSettingsPopup);
    }

    public void OpenSettingsPopup()
    {
        Time.timeScale = 0f;
        settingsPopup.SetActive(true);
    }

    public void CloseSettingsPopup()
    {
        Time.timeScale = 1f;
        settingsPopup.SetActive(false);
    }

    private void ContinueGame()
    {
        CloseSettingsPopup();
    }

    private void RestartGame()
    {
        Time.timeScale = 1f;

        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
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
