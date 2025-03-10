using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public interface ILevelManager
{
    Transform SpawnPoint { get; }
    Transform[] Path { get; }
    void IncreaseMoney(int amount);
    void PauseGame(bool isPaused);
    bool SpendMoney(int amount);
    int CurrentMoney { get; }
    void SetUIState(bool state);
    bool isAnyUIOpen { get; }
    void DecreaseHealth(int amount);
    void UpdateHealthUI();
    TextMeshProUGUI MoneyText { get; }
    int PlayerHealth { get; } 
    TextMeshProUGUI HealthText { get; }
    void CheckGameWinCondition();
    void TriggerGameOver();
    void TriggerGameWin();
}
