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
     [Header("Damage")]
     public int baseDamage = 25;
     public int criticalDamageMultiplier = 4;
     [Header("Poise")]
     public int poiseBreak, offensivePoiseBonus;

    [Header("Absorpsion")]
    public float physicalDamageAbsorption;

    [Header("Idle Animations")]

    public string right_hand_idle;
    public string left_hand_idle;
    public string th_idle;
    
    [Header("Attack Animations")]
    public string oh_light_attack_01;
    public string oh_light_attack_02;
    public string th_light_attack_01;
    public string th_light_attack_02;
    public string oh_heavy_attack_01;

    [Header("Weapon Art")]
    public string weapon_art;

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