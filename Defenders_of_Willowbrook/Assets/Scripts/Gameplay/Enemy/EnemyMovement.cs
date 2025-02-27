using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 2f;

    private Transform target;
    private int pathIndex = 0;
    private ILevelManager levelManager;

    private void Start()
    {
        var managers = FindObjectsOfType<MonoBehaviour>(); // Tìm tất cả MonoBehaviour
        foreach (var manager in managers)
        {
            if (manager is ILevelManager)
            {
                levelManager = (ILevelManager)manager;
                break; // Lấy cái đầu tiên tìm thấy
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
        target = levelManager.Path[pathIndex];
    }

    private void Update()
    {
        if(Vector2.Distance(target.position, transform.position) < 0.1f)
        {
            pathIndex++;
            if (pathIndex == levelManager.Path.Length)
            {
                Destroy(gameObject);
                EnemySpawner.onEnemyDestroy.Invoke();
                return;
            } else
            {
                target = levelManager.Path[pathIndex];
            }
        }
    }

    private void FixedUpdate()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;
    }
}
