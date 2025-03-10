using Assets.Scripts.Gameplay;
using Assets.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private GameObject bossPrefab;

    private ILevelManager levelManager;
    private Level5Manager level5Manager;

    [Header("Attributes")]
    [SerializeField] private int baseEnemies = 8;
    [SerializeField] private float enemiesPerSecond = 0.5f;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float difficultyScalingFactor = 0.75f;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI waveText;

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();

    private int currentWave = 1;
    private float timeSinceLastSpawn;
    private int enemiesAlive;
    private int enemiesLeftToSpawn;
    private bool isSpawning = false;
    private bool bossSpawned = false;

    private void Awake()
    {
        onEnemyDestroy.AddListener(EnemyDestroyed);
    }

    private void Start()
    {
        var managers = FindObjectsOfType<MonoBehaviour>();
        foreach (var manager in managers)
        {
            if (manager is ILevelManager)
            {
                levelManager = (ILevelManager)manager;
            }

            if (manager is Level5Manager)
            {
                level5Manager = (Level5Manager)manager;
            }
        }

        if (levelManager == null)
        {
            Debug.LogError("No Level Manager found!");
        }

        if (level5Manager == null)
        {
            //Debug.LogError("No Level 5 Manager found!");
        }

        UpdateWaveUI();
        StartCoroutine(StartWave());
    }

    private void Update()
    {
        if (!isSpawning) return;
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= (1f / enemiesPerSecond) && enemiesLeftToSpawn > 0)
        {
            SpawnEnemy();
            enemiesLeftToSpawn--;
            enemiesAlive++;
            timeSinceLastSpawn = 0f;
        }

        if (levelManager.PlayerHealth == 0)
        {
            return;
        }

        if (enemiesLeftToSpawn == 0 && enemiesAlive == 0)
        {
            EndWave();
        }
    }

    private void EnemyDestroyed()
    {
        if (bossSpawned)
        {
            BossHealth boss = FindObjectOfType<BossHealth>();

            if (boss != null && boss.GetCurrentHealth() > 0)
            {
                Debug.Log("Boss is still alive! Game Over.");
                level5Manager?.TriggerGameOver(); 
                return; 
            }
        }

        enemiesAlive--; 

        if (bossSpawned && enemiesAlive == 0)
        {
            Debug.Log("Boss Defeated! Triggering Game Win.");
            if (level5Manager != null)
            {
                level5Manager.CheckGameWinCondition();
            }
            else
            {
                Debug.LogError("No Level 5 Manager to trigger game win!");
            }
        }
    }

    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);

        if (currentWave <= 3)
        {
            isSpawning = true;
            enemiesLeftToSpawn = EnemiesPerWave();
        }
        else
        {
            SpawnBoss();
        }
    }

    private void SpawnBoss()
    {
        isSpawning = false; // Stop normal enemy spawning
        enemiesLeftToSpawn = 0; // No more regular enemies
        bossSpawned = true; // Track that the boss has appeared

        if (bossPrefab == null)
        {
            Debug.LogWarning("No boss prefab assigned. Ending game.");
            level5Manager?.CheckGameWinCondition();
            return;
        }

        if (levelManager != null)
        {
            GameObject boss = Instantiate(bossPrefab, levelManager.SpawnPoint.position, Quaternion.identity);
            BossHealth bossHealth = boss.GetComponent<BossHealth>();

            if (bossHealth != null)
            {
                enemiesAlive++;
                Debug.Log("Boss spawned! Waiting for defeat...");
            }
            else
            {
                Debug.LogWarning("Boss prefab missing BossHealth component!");
            }
        }
    }

    private void EndWave()
    {
        isSpawning = false;
        timeSinceLastSpawn = 0f;
        currentWave++;

        if (currentWave > 3) // After 3 waves, boss should be defeated
        {
            if (bossSpawned && enemiesAlive > 0) // If boss is alive, trigger Game Over
            {
                Debug.Log("Boss is still alive after last wave! Game Over.");
                level5Manager?.TriggerGameOver();
            }
            else
            {
                SpawnBoss();
            }
            return;
        }

        UpdateWaveUI();
        StartCoroutine(StartWave());
    }


    private void SpawnEnemy()
    {
        int randomIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject prefabToSpawn = enemyPrefabs[randomIndex];
        if (levelManager != null)
        {
            Instantiate(prefabToSpawn, levelManager.SpawnPoint.position, Quaternion.identity);
        }
    }

    private int EnemiesPerWave()
    {
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultyScalingFactor));
    }

    private void UpdateWaveUI()
    {
        if (waveText != null)
        {
            waveText.text = "Wave: " + currentWave.ToString() + "/3";
        }
    }
}