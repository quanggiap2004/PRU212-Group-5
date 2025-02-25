using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerShop : MonoBehaviour
{
    [SerializeField] Animator anim;

    private bool isMenuOpen = true;

    public void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;
        anim.SetBool("ShopOpen", isMenuOpen);
    }
}
