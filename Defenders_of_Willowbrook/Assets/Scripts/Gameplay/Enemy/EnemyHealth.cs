using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int hitPoints = 2;
    [SerializeField] private int moneyReward = 3;
    private bool isDestroyed = false;
    private ILevelManager levelManager;

    private void Start()
    {
        var managers = FindObjectsOfType<MonoBehaviour>(); // T�m t?t c? MonoBehaviour
        foreach (var manager in managers)
        {
            if (manager is ILevelManager)
            {
                levelManager = (ILevelManager)manager;
                break; // L?y c�i ??u ti�n t�m th?y
            }
        }

        if (levelManager == null)
        {
            Debug.LogError("No Level Manager found!");
        }
        else
        {
            //Debug.Log("Level Manager found: " + levelManager.GetType().Name);
        }
    }
    public void TakeDamage(int dmg)
    {
        hitPoints -= dmg;
        if (hitPoints <= 0 && !isDestroyed)
        {
            EnemySpawner.onEnemyDestroy.Invoke();
            levelManager?.IncreaseMoney(moneyReward);
            isDestroyed = true;
            Destroy(gameObject);
        }
    }
}
