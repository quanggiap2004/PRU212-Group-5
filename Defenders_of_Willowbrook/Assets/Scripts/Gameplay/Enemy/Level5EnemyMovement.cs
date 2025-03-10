using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Enemy
{
    public class Level5EnemyMovement : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private Animator animator;

        [Header("Attributes")]
        [SerializeField] private float moveSpeed = 2f;

        private Transform target;
        private int pathIndex = 0;
        private ILevelManager levelManager;
        private float baseSpeed;

        private void Start()
        {
            baseSpeed = moveSpeed;
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
            MoveEnemy();
        }

        private void MoveEnemy()
        {
            if (pathIndex < levelManager.Path.Length)
            {
                Vector2 targetPosition = levelManager.Path[pathIndex].position;
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

                if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
                {
                    pathIndex++;

                    if (pathIndex == levelManager.Path.Length)
                    {
                        Level5Manager.main.DecreaseHealth(1);
                        Destroy(gameObject);
                        EnemySpawner.onEnemyDestroy.Invoke();
                        return;
                    }
                    else
                    {
                        target = levelManager.Path[pathIndex];
                        UpdateDirection();
                    }
                }
            }
        }

        private void UpdateDirection()
        {
            if (pathIndex < levelManager.Path.Length)
            {
                Vector2 direction = (levelManager.Path[pathIndex].position - transform.position).normalized;

                if (direction.x > 0)
                {
                    animator.SetBool("IsMovingFront", false);
                    animator.SetBool("IsMovingRight", true);
                }
                else if (direction.y < 0)
                {
                    animator.SetBool("IsMovingRight", false);
                    animator.SetBool("IsMovingFront", true);
                }
            }
        }

        public void UpdateSpeed(float newSpeed)
        {
            moveSpeed = newSpeed;
        }

        public void ResetSpeed()
        {
            moveSpeed = baseSpeed;
        }
    }

}
