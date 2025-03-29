using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
  public class EnemyWeaponSlotManager : CharacterWeaponSlotManager
  {
    public WeaponItem rightHandWeapon;
    public WeaponItem leftHandWeapon;

    EnemyStatsManager enemyStatsManager;
    EnemyEffectsManager enemyEffectsManager;

    public void Awake()
    {
      enemyStatsManager = GetComponent<EnemyStatsManager>();
      enemyEffectsManager = GetComponent<EnemyEffectsManager>();
      LoadWeaponHolderSlots();
    }
    public void Start()
    {
      LoadWeaponsOnBothHands();
    }

    private void LoadWeaponHolderSlots(){
      WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
      foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots)
      {
        if (weaponSlot.isLeftHandSlot)
        {
          leftHandSlot = weaponSlot;
          //LoadLeftWeaponDamageCollider();
        }
        else if (weaponSlot.isRightHandSlot)
        {
          rightHandSlot = weaponSlot;
          //LoadRightWeaponDamageCollider();
        }
      }
    }
    public void LoadWeaponOnSlot(WeaponItem weapon, bool isLeft)
    {
      if (isLeft)
      {
        leftHandSlot.currentWeapon = weapon;
        leftHandSlot.LoadWeaponModel(weapon);
        LoadWeaponsDamageCollider(true);
      }
      else
      {
        rightHandSlot.currentWeapon = weapon;
        rightHandSlot.LoadWeaponModel(weapon);
        LoadWeaponsDamageCollider(false);
      }
    }

    public void LoadWeaponsOnBothHands()
    {
      if (rightHandWeapon != null)
      {
        LoadWeaponOnSlot(rightHandWeapon, false);
      }
      if (leftHandWeapon != null)
      {
        LoadWeaponOnSlot(leftHandWeapon, true);
      }
    }

    #region Handle Weapon Damage Colliders
    public void LoadWeaponsDamageCollider(bool isLeft)
    {
      if (isLeft)
      {
        leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        leftHandDamageCollider.characterManager = GetComponentInParent<CharacterManager>();
        enemyEffectsManager.leftWeaponFX = leftHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
      }
      else
      {
        rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        rightHandDamageCollider.characterManager = GetComponentInParent<CharacterManager>();
        enemyEffectsManager.rightWeaponFX = rightHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
      }
    }

    public void OpenDamageCollider()
    {
      if (rightHandDamageCollider != null)
      {
        rightHandDamageCollider.EnableDamageCollider();
      }
    }
    public void CloseDamageCollider()
    {
      if (rightHandDamageCollider != null)
      {
        rightHandDamageCollider.DisableDamageCollider();
      }
    }
    #endregion

    #region Handle Weapon Stamina Drains
    public void DrainStaminaLightAttack()
    {

    }

    public void DrainStaminaHeavyAttack()
    {

    }
    #endregion

    #region Handle Weapon Combos
    public void EnableCombo()
    {

    }

    public void DisableCombo()
    {

    }
    #endregion


    #region Handle Weapon's Poise Bonus
    public void GrantWeaponAttackingPoiseBonus()
    {
      enemyStatsManager.totalPoiseDefence += enemyStatsManager.offensivePoiseBonus;
    }

    public void ResetWeaponAttackingPoiseBonus()
    {
      enemyStatsManager.totalPoiseDefence = enemyStatsManager.armorPoiseBonus;
    }
    #endregion
  }
}