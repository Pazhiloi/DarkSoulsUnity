using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SG{
[CreateAssetMenu(menuName = "Spells/Healing Spell")]
public class HealingSpell : SpellItem
{
    public int healAmount;

    public override void AttemptToCastSpell(PlayerAnimatorManager playerAnimatorManager, PlayerStats playerStats, WeaponSlotManager weaponSlotManager)
    {
      base.AttemptToCastSpell(playerAnimatorManager, playerStats, weaponSlotManager);
      GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, playerAnimatorManager.transform);
      playerAnimatorManager.PlayTargetAnimation(spellAnimation, true);
      // Debug.Log("Attempting to cast spell");
    }
    public override void SuccessfullyCastSpell(PlayerAnimatorManager playerAnimatorManager, PlayerStats playerStats, CameraHandler cameraHandler, WeaponSlotManager weaponSlotManager)
    {
      base.SuccessfullyCastSpell(playerAnimatorManager, playerStats, cameraHandler, weaponSlotManager);
      GameObject instantiatedSpellFX = Instantiate(spellCastFX, playerAnimatorManager.transform);
      playerStats.HealPlayer(healAmount);
      // Debug.Log("SpellCast successful");
    }

    
  }
}
