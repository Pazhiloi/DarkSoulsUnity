using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SG
{
  public class PlayerAttacker : MonoBehaviour
  {
    AnimatorHandler animatorHandler;
    PlayerManager playerManager;
    PlayerInventory playerInventory;
    InputHandler inputHandler;
    WeaponSlotManager weaponSlotManager;
    public string lastAttack;

    private void Awake()
    {
      animatorHandler = GetComponent<AnimatorHandler>();
      playerManager = GetComponentInParent<PlayerManager>();
      playerInventory = GetComponentInParent<PlayerInventory>();
      weaponSlotManager = GetComponent<WeaponSlotManager>();
      inputHandler = GetComponentInParent<InputHandler>();
    }

    public void HandleWeaponCombo(WeaponItem weapon)
    {

      if (inputHandler.comboFlag)
      {
        animatorHandler.anim.SetBool("canDoCombo", false);

        if (lastAttack == weapon.oh_light_attack_01)
        {
          animatorHandler.PlayTargetAnimation(weapon.oh_light_attack_02, true);
        }
        else if (lastAttack == weapon.th_light_attack_01)
        {
          animatorHandler.PlayTargetAnimation(weapon.th_light_attack_02, true);
        }
      }

    }

    public void HandleLightAttack(WeaponItem weapon)
    {
      weaponSlotManager.attackingWeapon = weapon;
      if (inputHandler.twoHandFlag)
      {
        animatorHandler.PlayTargetAnimation(weapon.th_light_attack_01, true);
        lastAttack = weapon.th_light_attack_01;
      }
      else
      {
        animatorHandler.PlayTargetAnimation(weapon.oh_light_attack_01, true);
        lastAttack = weapon.oh_light_attack_01;
      }
    }
    public void HandleHeavyAttack(WeaponItem weapon)
    {
      weaponSlotManager.attackingWeapon = weapon;

      if (inputHandler.twoHandFlag)
      {
        animatorHandler.PlayTargetAnimation(weapon.oh_light_attack_01, true);
        lastAttack = weapon.oh_light_attack_01;
      }
      else
      {
        animatorHandler.PlayTargetAnimation(weapon.oh_light_attack_01, true);
        lastAttack = weapon.oh_light_attack_01;
      }
    }


    #region  Input Actions
    public void HandleRBAction()
    {
      if (playerInventory.rightWeapon.isMelleWeapon)
      {
        PerformRBMelleAction();
      }
      else if (playerInventory.rightWeapon.isSpellCaster || playerInventory.rightWeapon.isFaithCaster || playerInventory.rightWeapon.isPyroCaster)
      {
        PerformRBMagicAction(playerInventory.rightWeapon);
      }
    }

    #endregion

    #region Attack Actions

    private void PerformRBMelleAction()
    {
      if (playerManager.canDoCombo)
      {
        inputHandler.comboFlag = true;
        HandleWeaponCombo(playerInventory.rightWeapon);
        inputHandler.comboFlag = false;
      }
      else
      {
        if (playerManager.isInteracting)
        {
          return;
        }
        if (playerManager.canDoCombo)
        {
          return;
        }
        animatorHandler.anim.SetBool("isUsingRightHand", true);
        HandleLightAttack(playerInventory.rightWeapon);
      }
    }

    private void PerformRBMagicAction(WeaponItem weapon)
    {
      if (weapon.isFaithCaster)
      {
        if (playerInventory.currentSpell != null && playerInventory.currentSpell.isFaithSpell)
        {
          
        }
      }
    }
    #endregion



  }
}
