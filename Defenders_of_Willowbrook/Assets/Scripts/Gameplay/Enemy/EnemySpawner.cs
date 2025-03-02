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
    private ILevelManager levelManager;

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
                break;
            }
        }

        if (levelManager == null)
        {
            Debug.LogError("No Level Manager found!");
        }
        UpdateWaveUI();
        StartCoroutine(StartWave());
        // Ch? quiz hoàn t?t tr??c khi b?t ??u spawn
        //QuizManager.instance.OnQuizComplete.AddListener(() => StartCoroutine(StartWave()));
    }

    private void Update()
    {
        if (!isSpawning) return;
        timeSinceLastSpawn += Time.deltaTime;
        if (timeSinceLastSpawn >= (1f / enemiesPerSecond) && enemiesLeftToSpawn > 0 )
        {
            SpawnEnemy();
            enemiesLeftToSpawn--;
            enemiesAlive++;
            timeSinceLastSpawn = 0f;
        }

        if (levelManager.PlayerHealth == 0)
        {
            RedirectToScene(1);
        }

        if (enemiesLeftToSpawn == 0 && enemiesAlive == 0)
        {
            EndWave();
        }
    }

    private void EnemyDestroyed()
    {
        enemiesAlive--;
    }

    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        isSpawning = true;
        enemiesLeftToSpawn = EnemiesPerWave();
    }

    private void RedirectToScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
    private void EndWave()
    {
        isSpawning = false;
        timeSinceLastSpawn = 0f;
        currentWave++;
        
        if (currentWave > 3)
        {
            RedirectToScene(0);
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
