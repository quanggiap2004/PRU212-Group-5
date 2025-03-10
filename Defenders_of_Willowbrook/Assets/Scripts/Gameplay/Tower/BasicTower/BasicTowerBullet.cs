using Assets.Scripts.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTowerBullet : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private int bulletDamage = 1;

    private Transform target;

    private void Start()
    {
        // Destroy the bullet after 10 seconds
        Destroy(gameObject, 10f);
    }
    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    private void FixedUpdate()
    {
        if (!target) return;
        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * bulletSpeed;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        BossHealth bossHealth = other.gameObject.GetComponent<BossHealth>();
        EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();

        if (bossHealth != null)
        {
            bossHealth.TakeDamage(bulletDamage);
        }
        else if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(bulletDamage);
        }

        Destroy(gameObject);
    }

}
