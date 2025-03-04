using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostTowerBullet : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private int bulletDamage = 1;
    [SerializeField] private float freezeTime = 1f;
    [SerializeField] private Color freezeColor = new Color(0.38f, 0.69f, 0.91f);
    private Transform target;

    private void Start()
    {

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
        //Debug.Log("Collided with: " + other.gameObject.name);
        EnemyHealth eh = other.gameObject.GetComponent<EnemyHealth>();
        if (eh != null)
        {
            eh.TakeDamage(bulletDamage, freezeTime, freezeColor); // Deal damage and apply freeze effect
        }
        Destroy(gameObject);
    }
}
