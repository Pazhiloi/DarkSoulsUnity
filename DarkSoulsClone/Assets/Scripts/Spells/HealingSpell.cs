using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MR{
[CreateAssetMenu(menuName = "Spells/Healing Spell")]
public class HealingSpell : SpellItem
{
    public int healAmount;

    public override void AttemptToCastSpell(PlayerAnimatorManager playerAnimatorManager, PlayerStatsManager PlayerStatsManager, PlayerWeaponSlotManager playerWeaponSlotManager, bool isLeftHanded)
    {
      base.AttemptToCastSpell(playerAnimatorManager, PlayerStatsManager, playerWeaponSlotManager, isLeftHanded);
      GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, playerAnimatorManager.transform);
      playerAnimatorManager.PlayTargetAnimation(spellAnimation, true, false, isLeftHanded);
      // Debug.Log("Attempting to cast spell");
    }
    public override void SuccessfullyCastSpell(PlayerAnimatorManager playerAnimatorManager, PlayerStatsManager PlayerStatsManager, CameraHandler cameraHandler, PlayerWeaponSlotManager playerWeaponSlotManager, bool isLeftHanded)
    {
      base.SuccessfullyCastSpell(playerAnimatorManager, PlayerStatsManager, cameraHandler, playerWeaponSlotManager, isLeftHanded);
      GameObject instantiatedSpellFX = Instantiate(spellCastFX, playerAnimatorManager.transform);
      PlayerStatsManager.HealPlayer(healAmount);
      // Debug.Log("SpellCast successful");
    }

    
  }
}
