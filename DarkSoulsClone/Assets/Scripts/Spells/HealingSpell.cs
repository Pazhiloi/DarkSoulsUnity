using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SG{
[CreateAssetMenu(menuName = "Spells/Healing Spell")]
public class HealingSpell : SpellItem
{
    public int healAmount;

    public override void AttemptToCastSpell(PlayerAnimatorManager playerAnimatorManager, PlayerStatsManager PlayerStatsManager, PlayerWeaponSlotManager playerWeaponSlotManager)
    {
      base.AttemptToCastSpell(playerAnimatorManager, PlayerStatsManager, playerWeaponSlotManager);
      GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, playerAnimatorManager.transform);
      playerAnimatorManager.PlayTargetAnimation(spellAnimation, true);
      // Debug.Log("Attempting to cast spell");
    }
    public override void SuccessfullyCastSpell(PlayerAnimatorManager playerAnimatorManager, PlayerStatsManager PlayerStatsManager, CameraHandler cameraHandler, PlayerWeaponSlotManager playerWeaponSlotManager)
    {
      base.SuccessfullyCastSpell(playerAnimatorManager, PlayerStatsManager, cameraHandler, playerWeaponSlotManager);
      GameObject instantiatedSpellFX = Instantiate(spellCastFX, playerAnimatorManager.transform);
      PlayerStatsManager.HealPlayer(healAmount);
      // Debug.Log("SpellCast successful");
    }

    
  }
}
