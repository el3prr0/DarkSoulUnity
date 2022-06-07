using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotsUi : MonoBehaviour
{
    public Image leftWeapon;
    public Image rightWeapon;
    public Image Magic;
    public Image Item;

    public void UpdateWeaponQuickSlot(bool isLeft, WeaponItem weapon)
    {

        if (isLeft)
        {
            if (weapon != null)
            {
                leftWeapon.sprite = weapon.itemIcon;
                leftWeapon.enabled = true;
            }
            else
            {
                leftWeapon.sprite = null;
                leftWeapon.enabled = false;
            }

        }
        else
        {
            if (weapon != null)
            {
                rightWeapon.sprite = weapon.itemIcon;
                rightWeapon.enabled = true;
            }
            else
            {
                rightWeapon.sprite = null;
                rightWeapon.enabled = false;
            }

        }
    }


    public void UpdateMagicSlot(MagicItem magic)
    {
        if (magic != null)
        {
            Magic.sprite = magic.itemIcon;
            Magic.enabled = true;
        }
        else
        {
            Magic.sprite = null;
            Magic.enabled = false;
        }

    }
}
