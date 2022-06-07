using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    WeaponSlotManager weaponSlotManager;
    public WeaponItem rightWeapon;
    public WeaponItem leftWeapon;

    public WeaponItem unarmedWeapon;

    public MagicItem magic;

    public WeaponItem[] weaponsInRightHandSlots = new WeaponItem[1];
    public WeaponItem[] weaponsInLeftHandSlots = new WeaponItem[1];

    public MagicItem[] magicSlots = new MagicItem[1];

    public List<WeaponItem> weaponInventory;

    PlayerManager playerManager;
    PlayerAttacker playerAttacker;

    public int currentRightWeaponIndex = -1;
    public int currentLeftWeaponIndex = -1;

    public int currentMagicIndex = -1;

    private void Awake()
    {
        weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        playerManager = GetComponent<PlayerManager>();
        playerAttacker = GetComponent<PlayerAttacker>();
    }

    private void Start()
    {
        rightWeapon = unarmedWeapon;
        leftWeapon = unarmedWeapon;

    }

    public void ChangeRightWeapon(int Add)
    {
        if(playerManager.isInteracting){
            return;
        }
        currentRightWeaponIndex = currentRightWeaponIndex + Add;
        if (currentRightWeaponIndex > weaponsInRightHandSlots.Length - 1)
        {
            currentRightWeaponIndex = -1;
            rightWeapon = unarmedWeapon;
            weaponSlotManager.LoadWeaponOnSlot(unarmedWeapon, false);
        }
        else if (weaponsInRightHandSlots[currentRightWeaponIndex] != null)
        {
            rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
            weaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlots[currentRightWeaponIndex], false);
        }
        else
        {
            currentRightWeaponIndex = currentRightWeaponIndex + 1;
        }
        if(leftWeapon.type == rightWeapon.type){
            playerAttacker.DualWeapon = true;
        }else{playerAttacker.DualWeapon = false;}
    }

    public void ChangeLeftWeapon(int Add)
    {
        if(playerManager.isInteracting){
            return;
        }
        currentLeftWeaponIndex = currentLeftWeaponIndex + Add;
        if (currentLeftWeaponIndex > weaponsInLeftHandSlots.Length - 1)
        {
            currentLeftWeaponIndex = -1;
            leftWeapon = unarmedWeapon;
            weaponSlotManager.LoadWeaponOnSlot(unarmedWeapon, true);
        }
        else if (weaponsInLeftHandSlots[currentLeftWeaponIndex] != null)
        {
            leftWeapon = weaponsInLeftHandSlots[currentLeftWeaponIndex];
            weaponSlotManager.LoadWeaponOnSlot(weaponsInLeftHandSlots[currentLeftWeaponIndex], true);
        }
        else
        {
            currentLeftWeaponIndex = currentLeftWeaponIndex + 1;
        }

        if(leftWeapon.type == rightWeapon.type){
            playerAttacker.DualWeapon = true;
        }else{playerAttacker.DualWeapon = false;}
    }


    public void ChangeMagic(){
        if(playerManager.isInteracting){
            return;
        }
        currentMagicIndex = currentMagicIndex + 1;
        if (currentMagicIndex > magicSlots.Length - 1)
        {
            currentMagicIndex = -1;
            weaponSlotManager.LoadMagic(magic);
        }
        else if (magicSlots[currentMagicIndex] != null)
        {
            magic = magicSlots[currentMagicIndex];
            weaponSlotManager.LoadMagic(magicSlots[currentMagicIndex]);
        }
        else
        {
            currentMagicIndex = currentMagicIndex + 1;
        }
    }


    public void ChangeToAimMode(){
        weaponSlotManager.LoadWeaponOnSlot(unarmedWeapon, true);
         weaponSlotManager.LoadWeaponOnSlot(unarmedWeapon, false);
    }

    public void LoadWeaponAfterMagic(){
        weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
        weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
    }
}
