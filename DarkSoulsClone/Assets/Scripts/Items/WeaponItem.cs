using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SG
{
  [CreateAssetMenu(menuName = "Items/Weapon Item")]
  public class WeaponItem : Item
  {
    public GameObject modelPrefab;
    public bool isUnarmed;

    [Header("Animator Replacer")]
    public AnimatorOverrideController weaponController;
    [Header("Weapon Type")]
    WeaponType weaponType;

    [Header("Damage")]
    public int physicalDamage;
    public int fireDamage;
    public int criticalDamageMultiplier = 4;
    [Header("Poise")]
    public int poiseBreak, offensivePoiseBonus;

    [Header("Absorpsion")]
    public float physicalDamageAbsorption;

    [Header("Stamina Costs")]
    public int baseStamina;
    public float lightAttackMultiplier;
    public float heavyAttackMultiplier;

    [Header("Weapon Type")]

    public bool isSpellCaster;
    public bool isFaithCaster;
    public bool isPyroCaster;
    public bool isMelleWeapon;
    public bool isShieldWeapon;
  }
}