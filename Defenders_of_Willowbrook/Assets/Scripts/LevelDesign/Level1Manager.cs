using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Level1Manager : MonoBehaviour, ILevelManager
{
    public static Level1Manager main;
    public Transform spawnPoint;
    public Transform SpawnPoint => spawnPoint;
    public Transform[] path;
    public Transform[] Path => path;
    public int CurrentMoney { get; private set; }
    private int playerMoney = 0;
    public bool isAnyUIOpen { get; private set; } = false;

    [SerializeField] public TextMeshProUGUI moneyText;
    public TextMeshProUGUI MoneyText => moneyText;

    [Header("Player Health")]
    [SerializeField] private int playerHealth = 10;
    public int PlayerHealth => playerHealth;
    [SerializeField] private TextMeshProUGUI healthText;
    public TextMeshProUGUI HealthText => healthText;

    private void Awake()
    {
        main = this;
    }

    public void Start()
    {
        CurrentMoney = 100;
        moneyText.text = CurrentMoney.ToString();
        healthText.text = playerHealth.ToString();
    }

    public void IncreaseMoney(int amount)
    {
        playerMoney += amount;
        Debug.Log("Money: " + playerMoney);
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
            Debug.Log("Not enough money");
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
            Debug.Log("Game Over!");
        }
        UpdateHealthUI();
    }

    public void UpdateHealthUI()
    {
        if (healthText != null)
            healthText.text = playerHealth.ToString();
    }
}
