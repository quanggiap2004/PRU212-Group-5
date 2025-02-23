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

    private void Start()
    {
        target = Level1Manager.main.path[pathIndex];
    }

    private void Update()
    {
        if(Vector2.Distance(target.position, transform.position) < 0.1f)
        {
            pathIndex++;
            if (pathIndex == Level1Manager.main.path.Length)
            {
                Destroy(gameObject);
                EnemySpawner.onEnemyDestroy.Invoke();
                return;
            } else
            {
                target = Level1Manager.main.path[pathIndex];
            }
        }
    }

    private void FixedUpdate()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;
    }
}
