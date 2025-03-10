using Assets.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level1Manager : MonoBehaviour, ILevelManager
{
    public static Level1Manager main;
    public Transform spawnPoint;
    public Transform SpawnPoint => spawnPoint;
    public Transform[] path;
    public Transform[] Path => path;
    public int CurrentMoney { get; private set; }
    public bool isAnyUIOpen { get; private set; } = false;

    [SerializeField] public TextMeshProUGUI moneyText;
    public TextMeshProUGUI MoneyText => moneyText;

    [Header("Player Health")]
    [SerializeField] private int playerHealth = 10;
    public int PlayerHealth => playerHealth;
    [SerializeField] private TextMeshProUGUI healthText;
    public TextMeshProUGUI HealthText => healthText;
    [Header("Game Result")]
    [SerializeField] private GameResultController gameResultPopupManager;
    [SerializeField] private string nextLevelSceneName = "Level 2";
    private bool isGameOver = false;
    private bool isGameWin = false;

    private void Awake()
    {
        main = this;
    }

    public void Start()
    {
        CurrentMoney = 1000;
        moneyText.text = CurrentMoney.ToString();
        healthText.text = playerHealth.ToString();
    }

    public void IncreaseMoney(int amount)
    {
        CurrentMoney += amount;
        UpdateMoneyUI();
    }

    public bool SpendMoney(int amount)
    {
        if(amount <= CurrentMoney)
        {
            //Buy
            CurrentMoney -= amount;
            UpdateMoneyUI();
            return true;
        }
        else
        {
            return false;
        }
    }

    private void UpdateMoneyUI()
    {
        if (moneyText != null)
            moneyText.text = CurrentMoney.ToString();
    }
    public void SetUIState(bool state)
    {
        isAnyUIOpen = state;
        Time.timeScale = state ? 0f : 1f; // D?ng game khi UI m?
    }
    public void PauseGame(bool isPaused)
    {
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void DecreaseHealth(int amount)
    {
        playerHealth -= amount;
        if (playerHealth <= 0)
        {
            playerHealth = 0;
            // Handle game over logic here
            TriggerGameOver();
            Debug.Log("Game Over!");
        }
        UpdateHealthUI();
    }

    public void UpdateHealthUI()
    {
        if (healthText != null)
            healthText.text = playerHealth.ToString();
    }

    public void GameOver()
    {
        // Redirect to the level list scene
        SceneManager.LoadScene("LevelList");
    }

    public void LevelComplete()
    {
        // Redirect to the next level scene
        SceneManager.LoadScene("Level2");
    }


    public void CheckGameWinCondition()
    {
        if (isGameWin) return;

        isGameWin = true;
        TriggerGameWin();
    }

    public void TriggerGameOver()
    {
        isGameOver = true;

        if (gameResultPopupManager != null)
        {
            gameResultPopupManager.ShowGameOverPopup();
        }
        else
        {
            Debug.LogError("Game Result Popup Manager is not assigned!");
            // Fallback to scene reload
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void TriggerGameWin()
    {
        isGameWin = true;

        if (gameResultPopupManager != null)
        {
            gameResultPopupManager.ShowGameWinPopup();
        }
        else
        {
            Debug.LogError("Game Result Popup Manager is not assigned!");
        }
    }
}
