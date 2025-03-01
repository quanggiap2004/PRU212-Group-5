using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FrostTower : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform weaponRotation;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;

    [Header("Attributes")]
    [SerializeField] private float targetingRange = 5f;
    [SerializeField] private float rotationSpeed = 200f;
    [SerializeField] private float fireRate = 1f;

    private Transform target;
    private float timeUntilFire;
    private void Update()
    {
        if (target == null)
        {
            FindTarget();
            return;
        }
        RotateTowardsTarget();

        if (!CheckTargetIsInRange()) { 
            target = null;
        } else
        {
            timeUntilFire += Time.deltaTime;
            if (timeUntilFire >= 1f / fireRate)
            {
                Shoot();
                timeUntilFire = 0f;
            }
        }
    }

    private void Shoot()
    {
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        FrostTowerBullet bulletScript = bulletObj.GetComponent<FrostTowerBullet>();
        bulletScript.SetTarget(target);
        //Debug.Log("Shoot");
    }
    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2)transform.position, 0f, enemyMask);
        if (hits.Length > 0)
        {
            target = hits[0].transform;
        }
    }
    private bool CheckTargetIsInRange()
    {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }
    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        weaponRotation.rotation = Quaternion.RotateTowards(weaponRotation.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
    private void OnDrawGizmosSelected()
    {
       Handles.color = Color.blue;
       Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }
}
