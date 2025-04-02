using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SG
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

    public override void AttemptToCastSpell(PlayerAnimatorManager playerAnimatorManager, PlayerStatsManager playerStatsManager, PlayerWeaponSlotManager playerWeaponSlotManager)
    {
      base.AttemptToCastSpell(playerAnimatorManager, playerStatsManager, playerWeaponSlotManager);
      GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, playerWeaponSlotManager.rightHandSlot.transform);
      playerAnimatorManager.PlayTargetAnimation(spellAnimation, true);
    }
    public override void SuccessfullyCastSpell(PlayerAnimatorManager playerAnimatorManager, PlayerStatsManager playerStatsManager, CameraHandler cameraHandler, PlayerWeaponSlotManager playerWeaponSlotManager)
    {
      base.SuccessfullyCastSpell(playerAnimatorManager, playerStatsManager, cameraHandler, playerWeaponSlotManager);
      GameObject instantiatedSpellFX = Instantiate(spellCastFX, playerWeaponSlotManager.rightHandSlot.transform.position, cameraHandler.transform.rotation);
      SpellDamageCollider spellDamageCollider = instantiatedSpellFX.GetComponent<SpellDamageCollider>();
      spellDamageCollider.teamIDNumber = playerStatsManager.teamIDNumber;
      rigidbody = instantiatedSpellFX.GetComponent<Rigidbody>();

      if (cameraHandler.currentLockOnTarget!= null)
      {
        instantiatedSpellFX.transform.LookAt(cameraHandler.currentLockOnTarget.transform);
      }else{
        instantiatedSpellFX.transform.rotation = Quaternion.Euler(cameraHandler.cameraPivotTransform.eulerAngles.x, playerStatsManager.transform.eulerAngles.y, 0);
      }

      rigidbody.AddForce(instantiatedSpellFX.transform.forward * projectileForwardVelocity);
      rigidbody.AddForce(instantiatedSpellFX.transform.up * projectileUpwardVelocity);
      rigidbody.useGravity = isEffectedByGravity;
      rigidbody.mass = projectileMass;
      instantiatedSpellFX.transform.parent = null;
    }
  }
}
