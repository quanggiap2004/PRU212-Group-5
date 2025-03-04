using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager main;

    [Header("References")]
    [SerializeField] private Tower[] towers;

    private int selectedTower = 0;

    private void Awake()
    {
        main = this;
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
