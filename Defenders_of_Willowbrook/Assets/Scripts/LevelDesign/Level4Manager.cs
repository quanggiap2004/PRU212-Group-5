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
    [SerializeField] public TextMeshProUGUI moneyText;

    [Header("Shop UI")]
    [SerializeField] private TowerShop towerShop;

    private void Awake()
    {
        main = this;
    }

    public void Start()
    {
        CurrentMoney = 100;
        moneyText.text = CurrentMoney.ToString();

        towerShop.CloseMenu();
        // Bắt đầu câu hỏi trước khi mở Shop
        QuizManager.instance.OnQuizComplete.AddListener(StartGameFlow);
        QuizManager.instance.StartQuiz(this);
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
}
