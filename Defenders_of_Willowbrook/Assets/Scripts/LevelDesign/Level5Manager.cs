using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Level5Manager : MonoBehaviour, ILevelManager
{
    public static Level5Manager main;
    public Transform spawnPoint;
    public Transform SpawnPoint => spawnPoint;
    public Transform[] path;
    public Transform[] Path => path;
    public int CurrentMoney { get; private set; }
    private int playerMoney = 0;
    public bool isAnyUIOpen { get; private set; } = false;
    [SerializeField] public TextMeshProUGUI moneyText;
    private void Awake()
    {
        main = this;
    }
        
    public void Start()
    {
        CurrentMoney = 100;
        moneyText.text = CurrentMoney.ToString();
    }

    public void IncreaseMoney(int amount)
    {
        playerMoney += amount;
        Debug.Log("Money: " + playerMoney);
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
    public void SetUIState(bool state)
    {
        isAnyUIOpen = state;
        Time.timeScale = state ? 0f : 1f;
    }
    public void PauseGame(bool isPaused)
    {
        Time.timeScale = isPaused ? 0f : 1f;
    }
}
