using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "MAgic Item")]
public class MagicItem : Item
{
    public GameObject modelPrefab;
    public bool isUnarmed;

    public EnumWeaponTypes type;

    [Header("Magic Cast Animation")]
    public string castMagic = "OH_Light_Attack_01";
    public string left_Hand_Idle;
}
