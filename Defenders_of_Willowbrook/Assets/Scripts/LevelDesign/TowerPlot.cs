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

    private void Start()
    {
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
        if (towerTobuild.cost > Level1Manager.main.CurrentMoney)
        {
            Debug.Log("Not enough money");
            return;
        }
        Level2Manager.main.SpendMoney(towerTobuild.cost);

        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z); // Adjust the Y offset as needed
        tower = Instantiate(towerTobuild.prefab, spawnPosition, Quaternion.identity);
    }
}
