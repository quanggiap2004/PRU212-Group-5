using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Gameplay
{

    public class BossHealth : MonoBehaviour
    {
        [Header("Attributes")]
        [SerializeField] private int maxHitPoints = 100;
        private int currentHitPoints;
        [SerializeField] private int moneyReward = 10;
        private bool isDestroyed = false;
        private ILevelManager levelManager;

        [Header("Boss Health Bar")]
        //[SerializeField] private Image healthBarFill; // Assign in Inspector

        [SerializeField] private Slider healthBarSlider;

        private void Start()
        {
            currentHitPoints = maxHitPoints; 
            UpdateHealthBar();

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
        }

        public void TakeDamage(int dmg)
        {
            Debug.Log("Boss took damage: " + dmg);
            currentHitPoints -= dmg;
            currentHitPoints = Mathf.Max(0, currentHitPoints);

            UpdateHealthBar();

            if (currentHitPoints <= 0 && !isDestroyed)
            {
                Debug.Log("Boss Died!");
                EnemySpawner.onEnemyDestroy.Invoke();
                levelManager?.IncreaseMoney(moneyReward);
                isDestroyed = true;
                Destroy(gameObject);

                if (levelManager is Level5Manager level5)
                {
                    level5.CheckGameWinCondition();
                }
            }

        }

        public int GetCurrentHealth()
        {
            return currentHitPoints;
        }



        private void UpdateHealthBar()
        {
            if(healthBarSlider == null)
            {
                return;
            }
            float healthPercentage = (float)currentHitPoints / maxHitPoints *100;
            Debug.Log("health percentage: " + healthPercentage);
            healthBarSlider.value = healthPercentage;
        }
    }
}
