using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
  public class LightAttackAction : ItemAction
  {

    public override void PerformAction(PlayerManager player)
    {
      player.playerAnimatorManager.EraseHandIKForWeapon();
      player.playerAnimatorManager.animator.SetBool("isUsingRightHand", true);\

          if (player.isSprinting)
      {
        HandleRunningAttack(player.playerInventoryManager.rightWeapon);
        return;
      }

      if (player.canDoCombo)
      {
        player.inputHandler.comboFlag = true;
        HandleLightWeaponCombo(player.playerInventoryManager.rightWeapon);
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
        HandleLightAttack(player.playerInventoryManager.rightWeapon);
      }
      player.playerEffectsManager.PlayWeaponFX(false);

    }

    private void HandleLightAttack(WeaponItem weapon)
    {
      if (player.playerStatsManager.currentStamina <= 0) return;
      player.playerWeaponSlotManager.attackingWeapon = weapon;
      if (player.inputHandler.twoHandFlag)
      {
        player.playerAnimatorManager.PlayTargetAnimation(th_light_attack_01, true);
        player.playerCombatManager.lastAttack = th_light_attack_01;
      }
      else
      {
        player.playerAnimatorManager.PlayTargetAnimation(oh_light_attack_01, true);
        player.playerCombatManager.lastAttack = oh_light_attack_01;
      }
    }

  }
}