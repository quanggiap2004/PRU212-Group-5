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
        // Destroy the bullet after 10 seconds
        Destroy(gameObject, 10f);
    }

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    private void Update()
    {
        RotateTowardTarget();
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
        //Destroy(gameObject);
        gameObject.SetActive(false);
    }

    private void RotateTowardTarget()
    {
        if(!target)
        {
            return;
        }

        // Calculate the direction
        Vector3 direction = target.position - transform.position;

        // Get the angle (convert from radians to degrees)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Apply rotation (adjusting Z-axis only)
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }    
}
