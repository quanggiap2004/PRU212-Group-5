using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerShop : MonoBehaviour
{
    [SerializeField] Animator anim;

    private bool isMenuOpen = false;
    private void Start()
    {
        CloseMenu(); // ??m b?o menu shop ?óng khi b?t ??u
    }
    public void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;
        anim.SetBool("ShopOpen", isMenuOpen);
    }
    public void CloseMenu()
    {
        isMenuOpen = false;
        anim.SetBool("ShopOpen", false);
    }
}
