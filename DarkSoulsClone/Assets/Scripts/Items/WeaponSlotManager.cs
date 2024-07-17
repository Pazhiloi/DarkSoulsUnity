using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SG
{

  public class WeaponSlotManager : MonoBehaviour
  {
    public WeaponItem attackingWeapon;

    WeaponHolderSlot leftHandSlot;
    WeaponHolderSlot rightHandSlot;

    DamageCollider leftHandDamageCollider;
    DamageCollider rightHandDamageCollider;


    Animator animator;

    QuickSlotsUI quickSlotsUI;
    PlayerStats playerStats;
    InputHandler inputHandler;

    private void Awake()
    {
      animator = GetComponent<Animator>();
      quickSlotsUI = FindObjectOfType<QuickSlotsUI>();
      playerStats = GetComponentInParent<PlayerStats>();
      inputHandler = GetComponentInParent<InputHandler>();
      WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
      foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots)
      {
        if (weaponSlot.isLeftHandSlot)
        {
          leftHandSlot = weaponSlot;
        }
        else if (weaponSlot.isRightHandSlot)
        {
          rightHandSlot = weaponSlot;
        }
      }
    }

    #region Handle Weapons Damage Collider

    public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
    {
      if (isLeft)
      {
        leftHandSlot.LoadWeaponModel(weaponItem);
        LoadLeftWeaponDamageCollider();
        quickSlotsUI.UpdateWeaponQuickSlotsUI(true, weaponItem);
        #region  Handle Left Weapon Item Idle Animations

        if (weaponItem != null)
        {
          animator.CrossFade(weaponItem.left_hand_idle, 0.2f);
        }
        else
        {
          animator.CrossFade("Left Arm Empty", 0.2f);
        }

        #endregion
      }
      else
      {

        if (inputHandler.twoHandFlag)
        {
          animator.CrossFade(weaponItem.th_idle, 0.2f);
        }
        else{
          #region  Handle Right  Weapon Item Idle Animations
          animator.CrossFade("Both Arms Empty", 0.2f);
          if (weaponItem != null)
          {
            animator.CrossFade(weaponItem.right_hand_idle, 0.2f);
          }
          else
          {
            animator.CrossFade("Right Arm Empty", 0.2f);
          }
          #endregion
        }

        rightHandSlot.LoadWeaponModel(weaponItem);
        LoadRightWeaponDamageCollider();
        quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);
      }
    }

    private void LoadLeftWeaponDamageCollider()
    {
      leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
    }
    private void LoadRightWeaponDamageCollider()
    {
      rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
    }

    public void OpenRightDamageCollider()
    {
      rightHandDamageCollider.EnableDamageCollider();
    }
    public void OpenLeftDamageCollider()
    {
      leftHandDamageCollider.EnableDamageCollider();
    }
    public void CloseRightDamageCollider()
    {
      rightHandDamageCollider.DisableDamageCollider();
    }
    public void CloseLeftDamageCollider()
    {
      leftHandDamageCollider.DisableDamageCollider();
    }

    #endregion
    
    #region  Handle Weapons Stamina Drainage
    public void DrainStaminaLightAttack()
    {
      playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.lightAttackMultiplier));
    }
    public void DrainStaminaHeavyAttack()
    {
      playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.heavyAttackMultiplier));
    }
    #endregion


  }

}