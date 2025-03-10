using Assets.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level5Manager : MonoBehaviour, ILevelManager
{
    public static Level5Manager main;

    [Header("Game Result")]
    [SerializeField] public GameResultController gameResultPopupManager;
    //[SerializeField] private string nextLevelSceneName = "Level1"; this for changing scene in case game winning

    [Header("Spawn Point")]
    [SerializeField] private Transform spawnPoint;
    public Transform SpawnPoint => spawnPoint;

    [Header("Path")]
    [SerializeField] private Transform[] path;
    public Transform[] Path => path;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI moneyText;
    public TextMeshProUGUI MoneyText => moneyText;

    [Header("Player Health")]
    [SerializeField] private int playerHealth = 10;
    public int PlayerHealth => playerHealth;
    [SerializeField] private TextMeshProUGUI healthText;
    public TextMeshProUGUI HealthText => healthText;

    [Header("Game State")]
    private bool _isAnyUIOpen = false;
    public bool isAnyUIOpen => _isAnyUIOpen;

    private int _currentMoney;
    public int CurrentMoney => _currentMoney;

    private bool isGameOver = false;
    private bool isGameWin = false;

    private void Awake()
    {
        if (main == null)
        {
            main = this;
        }
        else if (main != this)
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        _currentMoney = 100;
        UpdateMoneyUI();
        UpdateHealthUI();
    }

    public void IncreaseMoney(int amount)
    {
        _currentMoney += amount;
        UpdateMoneyUI();
        Debug.Log("Money: " + _currentMoney);
    }

    public bool SpendMoney(int amount)
    {
        if (amount <= _currentMoney)
        {
            // Buy
            _currentMoney -= amount;
            UpdateMoneyUI();
            return true;
        }
        else
        {
            Debug.Log("Not enough money");
            return false;
        }
    }

    private void UpdateMoneyUI()
    {
        if (moneyText != null)
            moneyText.text = _currentMoney.ToString();
    }

    public void SetUIState(bool state)
    {
        _isAnyUIOpen = state;
        Time.timeScale = state ? 0f : 1f;
    }

    public void PauseGame(bool isPaused)
    {
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void DecreaseHealth(int amount)
    {
        if (isGameOver) return;

        playerHealth -= amount;
        if (playerHealth <= 0)
        {
            playerHealth = 0;
            TriggerGameOver();
        }
        UpdateHealthUI();
    }

    public void UpdateHealthUI()
    {
        if (healthText != null)
            healthText.text = playerHealth.ToString();
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

    public void GameOver()
    {
        throw new System.NotImplementedException();
    }

    public void LevelComplete()
    {
        throw new System.NotImplementedException();
    }
}
