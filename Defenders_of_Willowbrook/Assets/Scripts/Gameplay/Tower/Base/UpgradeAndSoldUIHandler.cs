using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeAndSoldUIHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool mouseOver = false;
    private void Update()
    {
        if (mouseOver)
        {
            
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
        UpgradeAndSoldUIManager.main.SetHoveringState(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
        UpgradeAndSoldUIManager.main.SetHoveringState(false);
        gameObject.SetActive(false);
    }

}
