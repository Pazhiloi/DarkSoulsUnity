using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
  [CreateAssetMenu(menuName = "Items Actions/Light Attack Action")]
  public class LightAttackAction : ItemAction
  {

    public override void PerformAction(PlayerManager player)
    {
      player.playerAnimatorManager.EraseHandIKForWeapon();
      player.playerAnimatorManager.animator.SetBool("isUsingRightHand", true);
      player.playerEffectsManager.PlayWeaponFX(false);


      if (player.isSprinting)
      {
        HandleRunningAttack(player.playerInventoryManager.rightWeapon, player);
        return;
      }

      if (player.canDoCombo)
      {
        player.inputHandler.comboFlag = true;
        HandleLightWeaponCombo(player.playerInventoryManager.rightWeapon, player);
        player.inputHandler.comboFlag = false;
      }
      else
      {
        if (player.isInteracting)
        {
          return;
        }
        if (player.canDoCombo)
        {
          return;
        }
        HandleLightAttack(player.playerInventoryManager.rightWeapon, player);
      }

    }

    private void HandleLightAttack(WeaponItem weapon, PlayerManager player)
    {
      if (player.playerStatsManager.currentStamina <= 0) return;
      player.playerWeaponSlotManager.attackingWeapon = weapon;
      if (player.inputHandler.twoHandFlag)
      {
        player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.th_light_attack_01, true);
        player.playerCombatManager.lastAttack = player.playerCombatManager.th_light_attack_01;
      }
      else
      {
        player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_light_attack_01, true);
        player.playerCombatManager.lastAttack = player.playerCombatManager.oh_light_attack_01;
      }
    }

    private void HandleRunningAttack(WeaponItem weapon, PlayerManager player)
    {
      if (player.playerStatsManager.currentStamina <= 0) return;
      player.playerWeaponSlotManager.attackingWeapon = weapon;

      if (player.inputHandler.twoHandFlag)
      {
        player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.th_running_attack_01, true);
        player.playerCombatManager.lastAttack = player.playerCombatManager.th_running_attack_01;
      }
      else
      {
        player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_running_attack_01, true);
        player.playerCombatManager.lastAttack = player.playerCombatManager.oh_running_attack_01;
      }
    }

    private void HandleLightWeaponCombo(WeaponItem weapon, PlayerManager player)
    {
      if (player.playerStatsManager.currentStamina <= 0) return;
      if (player.inputHandler.comboFlag)
      {
        player.playerAnimatorManager.animator.SetBool("canDoCombo", false);

        if (player.isTwoHandingWeapon)
        {
          if (player.playerCombatManager.lastAttack == player.playerCombatManager.th_light_attack_01)
          {
            player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.th_light_attack_02, true);
            player.playerCombatManager.lastAttack = player.playerCombatManager.th_light_attack_02;
          }else{
            player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.th_light_attack_01, true);
            player.playerCombatManager.lastAttack = player.playerCombatManager.th_light_attack_01;
          }
        }
        else
        {
          if (player.playerCombatManager.lastAttack == player.playerCombatManager.oh_light_attack_01)
          {
            player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_light_attack_02, true);
            player.playerCombatManager.lastAttack = player.playerCombatManager.oh_light_attack_02;
          }
          else{
            player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_light_attack_01, true);
            player.playerCombatManager.lastAttack = player.playerCombatManager.oh_light_attack_01;
          }
        }

      }
    }

  }
}