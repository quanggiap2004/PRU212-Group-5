using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int hitPoints = 2;
    [SerializeField] private int moneyReward = 3;
    private bool isDestroyed = false;
    
    public void TakeDamage(int dmg)
    {
        hitPoints -= dmg;
        if (hitPoints <= 0 && !isDestroyed)
        {
            EnemySpawner.onEnemyDestroy.Invoke();
            Level1Manager.main.IncreaseMoney(moneyReward);
            isDestroyed = true;
            Destroy(gameObject);
        }
    }
}
