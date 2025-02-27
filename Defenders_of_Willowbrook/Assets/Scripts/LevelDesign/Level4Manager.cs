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
    private int playerMoney = 0;
    public bool isAnyUIOpen { get; private set; } = false;

    public TextMeshProUGUI MoneyText => throw new System.NotImplementedException();

    public int PlayerHealth => throw new System.NotImplementedException();

    public TextMeshProUGUI HealthText => throw new System.NotImplementedException();

    [SerializeField] public TextMeshProUGUI moneyText;

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
        playerMoney += amount;
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
        throw new System.NotImplementedException();
    }

    public void UpdateHealthUI()
    {
        throw new System.NotImplementedException();
    }
}
