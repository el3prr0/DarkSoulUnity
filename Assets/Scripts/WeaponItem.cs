using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapons Item")]
public class WeaponItem : Item
{
    public GameObject modelPrefab;
    public bool isUnarmed;

    public EnumWeaponTypes type;

    [Header("One Right Attack Animation")]
    public string oh_Light_Attack_01 = "OH_Light_Attack_01";
    public string oh_Light_Attack_02 = "OH_Light_Attack_02";
    public string Heavy_Attack = "OH_Heavy_Attack_01";

    public string oh_Light_Jumping_Attack;

    [Header("Left Weapon Actions")]

    public string Dual_Attack;

    public string Block;

    public string Dual_Block;

    public string Left_Attack;



    [Header("Idle Animations")]
    public string right_Hand_Idle;
    public string left_Hand_Idle;

    [Header("Statmina Cost")]
    public int baseStamina;
    public float lightAttackMultiplier;
    public float heavyAttackMultiplier;
    
}
