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
        private Transform target;
        private ILevelManager levelManager;

        public Transform[] waypoints; 
        public float speed = 5f; 
        private int currentWaypointIndex = 0; 
        private Animator animator; 

        void Start()
        {
            animator = GetComponent<Animator>(); 
        }

        void Update()
        {
            MoveEnemy(); 
        }

        void MoveEnemy()
        {
            if (currentWaypointIndex < waypoints.Length)
            {
                Vector2 targetPosition = waypoints[currentWaypointIndex].position;
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

                if ((Vector2)transform.position == targetPosition)
                {
                    currentWaypointIndex++; 
                    UpdateDirection(); 
                }
            }
        }

       

        void UpdateDirection()
        {
            if (currentWaypointIndex < waypoints.Length)
            {
                Vector2 direction = (waypoints[currentWaypointIndex].position - transform.position).normalized;

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

        public void Die()
        {
            if (animator.GetBool("IsMovingFront"))
            {
                animator.SetTrigger("FrontDeath");
            }
            else if (animator.GetBool("IsMovingRight"))
            {
                animator.SetTrigger("RightDeath");
            }
        }
    }
}
