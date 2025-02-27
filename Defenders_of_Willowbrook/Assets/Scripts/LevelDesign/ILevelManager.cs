using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILevelManager
{ 
    Transform SpawnPoint {  get; }
    Transform[] Path { get; }
    void IncreaseMoney(int amount);
    void PauseGame(bool isPaused);
    bool SpendMoney(int amount);
    int CurrentMoney { get; }
    void SetUIState(bool state);
    bool isAnyUIOpen { get; }

}
