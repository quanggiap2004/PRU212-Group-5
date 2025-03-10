using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Level4Manager : MonoBehaviour, ILevelManager
{
    public static Level4Manager main;
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

    [Header("Shop UI")]
    [SerializeField] private TowerShop towerShop;

    private void Awake()
    {
        main = this;
    }

    public void Start()
    {
        Debug.Log("Level4Manager Started");
        CurrentMoney = 1000;
        moneyText.text = CurrentMoney.ToString();

        towerShop.CloseMenu();
        // Bắt đầu câu hỏi trước khi mở Shop
        if (QuizManager.instance != null)
        {
            Debug.Log("QuizManager instance found!");
            QuizManager.instance.OnQuizComplete.AddListener(StartGameFlow);
            QuizManager.instance.StartQuiz(this);
            Debug.Log("Quiz Started");
        }
        else
        {
            Debug.LogError("QuizManager instance is NULL!");
        }
    }

    private void StartGameFlow()
    {
        towerShop.ToggleMenu(); // Mở shop sau khi hoàn thành câu hỏi
    }

    public void SetUIState(bool state)
    {
        isAnyUIOpen = state;
    }

    public void PauseGame(bool isPaused)
    {
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void IncreaseMoney(int amount)
    {
        CurrentMoney += amount;
        UpdateMoneyUI();
    }

    public bool SpendMoney(int amount)
    {
        if (amount <= CurrentMoney)
        {
            CurrentMoney -= amount;
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
            moneyText.text = CurrentMoney.ToString();
    }

    public void DecreaseHealth(int amount)
    {
        playerHealth -= amount;
        if (playerHealth <= 0)
        {
            playerHealth = 0;
            // Handle game over logic here
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
        throw new System.NotImplementedException();
    }

    public void LevelComplete()
    {
        throw new System.NotImplementedException();
    }
}
