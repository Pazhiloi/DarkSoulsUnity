using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SG
{
[CreateAssetMenu(menuName = "Spells/Projectile Spell")]
public class ProjectileSpell : SpellItem
{
    public float baseDamage;
    public float projectileVelocity;
    Rigidbody rigidbody;

    public override void AttemptToCastSpell(PlayerAnimatorManager playerAnimatorManager, PlayerStats playerStats, WeaponSlotManager weaponSlotManager)
    {
      base.AttemptToCastSpell(playerAnimatorManager, playerStats, weaponSlotManager);
      GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, weaponSlotManager.rightHandSlot.transform);
      playerAnimatorManager.PlayTargetAnimation(spellAnimation, true);
    }
    public override void SuccessfullyCastSpell(PlayerAnimatorManager playerAnimatorManager, PlayerStats playerStats)
    {
      base.SuccessfullyCastSpell(playerAnimatorManager, playerStats);
    }
  }
}
