using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    AnimatorManager animatorManager;
    PlayerManager playerManager;
    PlayerLocomotion playerLocomotion;

    WeaponSlotManager weaponSlotManager;
    public string lastAttack;
    public bool CanCombo;
    public bool comboFlag;

    public bool canJumpAttack;
    public bool DualWeapon;

    public float Parry;

    private void Awake()
    {
        animatorManager = GetComponentInChildren<AnimatorManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        playerManager = GetComponent<PlayerManager>();
        weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
    }
    public void HandleLightAttack(WeaponItem weapon)
    {
        if (playerManager.isInteracting || playerLocomotion.isJumping || CanCombo)
        {
            return;
        }
        weaponSlotManager.attackingWeapon = weapon;
        animatorManager.PlayTargetAnimation(weapon.oh_Light_Attack_01, true, true);
        lastAttack = weapon.oh_Light_Attack_01;
    }

    public void HandleHeavyRightAttack(WeaponItem weapon)
    {
        if (playerManager.isInteracting == true || playerLocomotion.isJumping || CanCombo)
        {
            return;
        }
        weaponSlotManager.attackingWeapon = weapon;
        animatorManager.PlayTargetAnimation(weapon.Heavy_Attack, true, true);
        lastAttack = weapon.Heavy_Attack;
    }


    public void HandleWeaponCombo(WeaponItem weapon)
    {
        if (comboFlag)
        {
            weaponSlotManager.attackingWeapon = weapon;
            animatorManager.anim.SetBool("CanCombo", false);

            if (lastAttack == weapon.oh_Light_Attack_01)
            {
                animatorManager.PlayTargetAnimation(weapon.oh_Light_Attack_02, true, true);
                lastAttack = weapon.oh_Light_Attack_02;
            }

            if (lastAttack == weapon.oh_Light_Attack_02)
            {
                animatorManager.PlayTargetAnimation(weapon.oh_Light_Attack_01, true, true);
                lastAttack = weapon.oh_Light_Attack_01;
            }
        }
    }

    public void HandleBlockAction(WeaponItem weapon)
    {
        if (playerManager.isInteracting == true || playerLocomotion.isJumping)
        {
            return;
        }
        weaponSlotManager.attackingWeapon = weapon;
        if (DualWeapon)
        {
            animatorManager.PlayTargetAnimation(weapon.Dual_Block, true, true);
            lastAttack = weapon.Dual_Block;
        }
        else
        {
            animatorManager.PlayTargetAnimation(weapon.Block, true, true);
            lastAttack = weapon.Block;
        }
    }

    public void HandleJumpingAttack(WeaponItem weapon)
    {
        if (canJumpAttack)
        {
            weaponSlotManager.attackingWeapon = weapon;
            canJumpAttack = false;
            animatorManager.PlayTargetAnimation(weapon.oh_Light_Jumping_Attack, true, false);
            lastAttack = weapon.oh_Light_Jumping_Attack;
        }

    }

    public void HandleRunningJumpingAttack(WeaponItem weapon)
    {
        if (canJumpAttack)
        {
            canJumpAttack = false;
            weaponSlotManager.attackingWeapon = weapon;
            animatorManager.PlayTargetAnimation(weapon.oh_Light_Jumping_Attack, true, true);
            lastAttack = weapon.oh_Light_Jumping_Attack;
        }

    }


    public void CastMagic(MagicItem magic)
    {
        weaponSlotManager.magicSlot = magic;
        animatorManager.PlayTargetAnimation(magic.castMagic, true, true);
        lastAttack = magic.castMagic;
    }


    public void HandleParry()
    {
        if (playerManager.isInteracting || playerLocomotion.isJumping || CanCombo)
        {
            return;
        }

        animatorManager.PlayTargetAnimation("Parry", true, true);
        lastAttack = "Parry";
    }

    public void LeftAttak(WeaponItem weapon)
    {
        if (playerManager.isInteracting == true || playerLocomotion.isJumping || CanCombo)
        {
            return;
        }
        if (DualWeapon)
        {
            animatorManager.PlayTargetAnimation(weapon.Dual_Attack, true, true);
            lastAttack = weapon.Dual_Attack;
        }
        else
        {
            Debug.Log("LEFT ATTACK ");
            //animatorManager.PlayTargetAnimation(weapon.Left_Attack, true, true);
            lastAttack = weapon.Left_Attack;
        }
    }

}
