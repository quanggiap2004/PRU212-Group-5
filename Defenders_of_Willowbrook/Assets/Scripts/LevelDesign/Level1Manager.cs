using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Level1Manager : MonoBehaviour
{
    public static Level1Manager main;
    public Transform spawnPoint;
    public Transform[] path;
    public int CurrentMoney;
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
