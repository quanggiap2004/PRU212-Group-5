using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlot : MonoBehaviour
{
    [Header("Referenecs")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor;

    private GameObject tower;
    private Color startColor;
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

        startColor = sr.color;
    }

    private void OnMouseEnter()
    {
        sr.color = hoverColor;
    }

    private void OnMouseExit()
    {
        sr.color = startColor;
    }

    private void OnMouseDown()
    {
        if (tower != null) return;

        Tower towerTobuild = BuildManager.main.GetSelectedTower();
        if (towerTobuild.cost > levelManager.CurrentMoney)
        {
            Debug.Log("Not enough money");
            return;
        }
        levelManager.SpendMoney(towerTobuild.cost);

        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        tower = Instantiate(towerTobuild.prefab, spawnPosition, Quaternion.identity);
    }
}
