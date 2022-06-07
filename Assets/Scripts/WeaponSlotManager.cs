using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlotManager : MonoBehaviour
{
    WeaponHolderSlot leftHandSlot;
    WeaponHolderSlot rightHandSlot;
    DamageCollider leftHandDamageCollider;
    DamageCollider rightHandeDamageCollider;

    public MagicItem magicSlot;

    public WeaponItem DefaultWeapon;

    public WeaponItem attackingWeapon;

    InputManager inputManager;

    [SerializeField] Transform spawnMagic;

    Animator animator;

    QuickSlotsUi quickSlotsUi;

    PlayerStats playerStats;

    

    private void Awake()
    {
        animator = GetComponent<Animator>();
        quickSlotsUi = FindObjectOfType<QuickSlotsUi>();
        inputManager = GetComponentInParent<InputManager>();
        playerStats = GetComponentInParent<PlayerStats>();
        WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
        foreach (WeaponHolderSlot weaponslot in weaponHolderSlots)
        {
            if (weaponslot.isLeftHandSlot)
            {
                leftHandSlot = weaponslot;

            }
            else if (weaponslot.isRightHandSlot)
            {
                rightHandSlot = weaponslot;

            }
        }

    }

    public void LoadWeaponOnSlot(WeaponItem weaponItem, bool left)
    {
        if (left)
        {
            leftHandSlot.LoadWeaponModel(weaponItem);
            LoadLeftWeaponDamageCollider();
            quickSlotsUi.UpdateWeaponQuickSlot(true,weaponItem);
            if (weaponItem != null)
            {

                animator.CrossFade(weaponItem.left_Hand_Idle, 0.2f);
            }
            else
            {
                leftHandSlot.LoadWeaponModel(DefaultWeapon);
                animator.CrossFade(DefaultWeapon.left_Hand_Idle, 0.2f);
            }
        }
        else
        {
            rightHandSlot.LoadWeaponModel(weaponItem);
            LoadRightWeaponDamageCollider();
            quickSlotsUi.UpdateWeaponQuickSlot(false,weaponItem);
            if (weaponItem != null)
            {
                animator.CrossFade(weaponItem.right_Hand_Idle, 0.2f);
            }
            else
            {
                rightHandSlot.LoadWeaponModel(DefaultWeapon);
                animator.CrossFade(DefaultWeapon.right_Hand_Idle, 0.2f);
            }
        }
    }

    public void LoadMagic(MagicItem magic){
        this.magicSlot = magic;
        quickSlotsUi.UpdateMagicSlot(magic);
    }



    #region WeaponColliders


    private void LoadLeftWeaponDamageCollider()
    {
        leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
    }

    private void LoadRightWeaponDamageCollider()
    {
        rightHandeDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
    }

    public void OpenRightDamageCollider()
    {
        rightHandeDamageCollider.EnableDamageCollider();
    }

    public void OpenLeftDamageCollider()
    {
        leftHandDamageCollider.EnableDamageCollider();
    }

    public void CloseRightDamageCollider()
    {
        rightHandeDamageCollider.DisableDamageCollider();
    }

    public void CloseLeftDamageCollider()
    {
        leftHandDamageCollider.DisableDamageCollider();
    }
    #endregion

    public void CastMagic(){
        Vector3 aimDir = (inputManager.mouseWorldPosition - spawnMagic.position).normalized;
        Instantiate(magicSlot.modelPrefab,spawnMagic.position,Quaternion.LookRotation(aimDir,Vector3.up));
    }

    public void DrainStaminaLightAttack(){
        playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.lightAttackMultiplier));
    }

    public void DrainStaminaHeavyAttack(){
        playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.heavyAttackMultiplier));
    }
}
