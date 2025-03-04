using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int hitPoints = 2;
    [SerializeField] private int moneyReward = 3;
    private bool isDestroyed = false;
    private ILevelManager levelManager;
    private Color originalColor;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        var managers = FindObjectsOfType<MonoBehaviour>(); // Tìm t?t c? MonoBehaviour
        foreach (var manager in managers)
        {
            if (manager is ILevelManager)
            {
                levelManager = (ILevelManager)manager;
                break; // L?y cái ??u tiên tìm th?y
            }
        }

        if (levelManager == null)
        {
            Debug.LogError("No Level Manager found!");
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }
    public void TakeDamage(int dmg, float freezeTime = 0f, Color? freezeColor = null)
    {
        hitPoints -= dmg;
        if (hitPoints <= 0 && !isDestroyed)
        {
            EnemySpawner.onEnemyDestroy.Invoke();
            levelManager?.IncreaseMoney(moneyReward);
            isDestroyed = true;
            Destroy(gameObject);
        }
        else if (freezeTime > 0f && freezeColor.HasValue)
        {
            StartCoroutine(ApplyFreezeEffect(freezeTime, freezeColor.Value));
        }
    }

    private IEnumerator ApplyFreezeEffect(float freezeTime, Color freezeColor)
    {
        EnemyMovement em = GetComponent<EnemyMovement>();
        if (em != null)
        {
            em.UpdateSpeed(0.5f); // Slow down the enemy
            if (spriteRenderer != null)
            {
                spriteRenderer.color = freezeColor; // Change color to freeze color
            }
            yield return new WaitForSeconds(freezeTime);
            em.ResetSpeed(); // Reset speed after freezeTime
            if (spriteRenderer != null)
            {
                spriteRenderer.color = originalColor; // Reset color to original
            }
        }
    }
}
