using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeAndSoldUIManager : MonoBehaviour
{
    public static UpgradeAndSoldUIManager main;
    private bool isHovering;

    private void Awake()
    {
        main = this;
    }

    public void SetHoveringState(bool state)
    {
        isHovering = state;
    }

    public bool IsHovering()
    {
        return isHovering;
    }
}
