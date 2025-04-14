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

    public override void AttemptToCastSpell(PlayerAnimatorManager playerAnimatorManager, PlayerStatsManager playerStatsManager, PlayerWeaponSlotManager playerWeaponSlotManager, bool isLeftHanded)
    {
      base.AttemptToCastSpell(playerAnimatorManager, playerStatsManager, playerWeaponSlotManager, isLeftHanded);

      if (isLeftHanded)
      {
        GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, playerWeaponSlotManager.leftHandSlot.transform);
        playerAnimatorManager.PlayTargetAnimation(spellAnimation, true, false, isLeftHanded);
      }
      else
      {
        GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, playerWeaponSlotManager.rightHandSlot.transform);
        playerAnimatorManager.PlayTargetAnimation(spellAnimation, true, false, isLeftHanded);
      }

    }
    public override void SuccessfullyCastSpell(PlayerAnimatorManager playerAnimatorManager, PlayerStatsManager playerStatsManager, CameraHandler cameraHandler, PlayerWeaponSlotManager playerWeaponSlotManager, bool isLeftHanded)
    {
      base.SuccessfullyCastSpell(playerAnimatorManager, playerStatsManager, cameraHandler, playerWeaponSlotManager, isLeftHanded);

      if (isLeftHanded)
      {
        GameObject instantiatedSpellFX = Instantiate(spellCastFX, playerWeaponSlotManager.leftHandSlot.transform.position, cameraHandler.transform.rotation);
        SpellDamageCollider spellDamageCollider = instantiatedSpellFX.GetComponent<SpellDamageCollider>();
        spellDamageCollider.teamIDNumber = playerStatsManager.teamIDNumber;
        rigidbody = instantiatedSpellFX.GetComponent<Rigidbody>();
        if (cameraHandler.currentLockOnTarget != null)
        {
          instantiatedSpellFX.transform.LookAt(cameraHandler.currentLockOnTarget.transform);
        }
        else
        {
          instantiatedSpellFX.transform.rotation = Quaternion.Euler(cameraHandler.cameraPivotTransform.eulerAngles.x, playerStatsManager.transform.eulerAngles.y, 0);
        }

        rigidbody.AddForce(instantiatedSpellFX.transform.forward * projectileForwardVelocity);
        rigidbody.AddForce(instantiatedSpellFX.transform.up * projectileUpwardVelocity);
        rigidbody.useGravity = isEffectedByGravity;
        rigidbody.mass = projectileMass;
        instantiatedSpellFX.transform.parent = null;
      }
      else
      {
        GameObject instantiatedSpellFX = Instantiate(spellCastFX, playerWeaponSlotManager.rightHandSlot.transform.position, cameraHandler.transform.rotation);
        SpellDamageCollider spellDamageCollider = instantiatedSpellFX.GetComponent<SpellDamageCollider>();
        spellDamageCollider.teamIDNumber = playerStatsManager.teamIDNumber;
        rigidbody = instantiatedSpellFX.GetComponent<Rigidbody>();
      }

     
    }
  }
}
