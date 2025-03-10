using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager main;

    [Header("References")]
    [SerializeField] private Tower[] towers;
    [SerializeField] private TextMeshProUGUI frostTowerPrice;
    [SerializeField] private TextMeshProUGUI basicTowerPrice;

    private int selectedTower = 0;

    private void Awake()
    {
        main = this;
        basicTowerPrice.text = towers[0].cost.ToString();
        frostTowerPrice.text = towers[1].cost.ToString();
    }

    public Tower GetSelectedTower()
    {
        Debug.Log(towers[0].prefab);
        return towers[selectedTower];
    }

    public void SelectedTower(int index)
    {
        selectedTower = index;
    }
}
