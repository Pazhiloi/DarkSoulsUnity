using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MR
{
  [CreateAssetMenu(menuName = "Spells/Projectile Spell")]
  public class ProjectileSpell : SpellItem
  {
    [Header("Projectile Damage")]
    public float baseDamage;
    [Header("Projectile Physics")]
    public float projectileForwardVelocity, projectileUpwardVelocity, projectileMass;
    public bool isEffectedByGravity;
    Rigidbody rigidbody;

    public override void AttemptToCastSpell(CharacterManager character)
    {
      base.AttemptToCastSpell(character);

      if (character.isUsingLeftHand)
      {
        GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, character.characterWeaponSlotManager.leftHandSlot.transform);
        character.characterAnimatorManager.PlayTargetAnimation(spellAnimation, true, false, character.isUsingLeftHand);
      }
      else
      {
        GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, character.characterWeaponSlotManager.rightHandSlot.transform);
        character.characterAnimatorManager.PlayTargetAnimation(spellAnimation, true, false, character.isUsingLeftHand);
      }

    }
    public override void SuccessfullyCastSpell(CharacterManager character)
    {
      base.SuccessfullyCastSpell(character);
      PlayerManager player = character as PlayerManager;

      if (player != null) 
      {
        if (player.isUsingLeftHand)
        {
          GameObject instantiatedSpellFX = Instantiate(spellCastFX, player.playerWeaponSlotManager.leftHandSlot.transform.position, player.cameraHandler.transform.rotation);
          SpellDamageCollider spellDamageCollider = instantiatedSpellFX.GetComponent<SpellDamageCollider>();
          spellDamageCollider.teamIDNumber = player.playerStatsManager.teamIDNumber;
          rigidbody = instantiatedSpellFX.GetComponent<Rigidbody>();
          if (player.cameraHandler.currentLockOnTarget != null)
          {
            instantiatedSpellFX.transform.LookAt(player.cameraHandler.currentLockOnTarget.transform);
          }
          else
          {
            instantiatedSpellFX.transform.rotation = Quaternion.Euler(player.cameraHandler.cameraPivotTransform.eulerAngles.x, player.playerStatsManager.transform.eulerAngles.y, 0);
          }

          rigidbody.AddForce(instantiatedSpellFX.transform.forward * projectileForwardVelocity);
          rigidbody.AddForce(instantiatedSpellFX.transform.up * projectileUpwardVelocity);
          rigidbody.useGravity = isEffectedByGravity;
          rigidbody.mass = projectileMass;
          instantiatedSpellFX.transform.parent = null;
        }
        else
        {
          GameObject instantiatedSpellFX = Instantiate(spellCastFX, player.playerWeaponSlotManager.rightHandSlot.transform.position, player.cameraHandler.transform.rotation);
          SpellDamageCollider spellDamageCollider = instantiatedSpellFX.GetComponent<SpellDamageCollider>();
          spellDamageCollider.teamIDNumber = player.playerStatsManager.teamIDNumber;
          rigidbody = instantiatedSpellFX.GetComponent<Rigidbody>();
        }
      }
      else
      {
        
      }

      
     
    }
  }
}
