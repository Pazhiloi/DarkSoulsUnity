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

    public override void AttemptToCastSpell(PlayerAnimatorManager playerAnimatorManager, PlayerStats playerStats, WeaponSlotManager weaponSlotManager)
    {
      base.AttemptToCastSpell(playerAnimatorManager, playerStats, weaponSlotManager);
      GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, weaponSlotManager.rightHandSlot.transform);
      playerAnimatorManager.PlayTargetAnimation(spellAnimation, true);
    }
    public override void SuccessfullyCastSpell(PlayerAnimatorManager playerAnimatorManager, PlayerStats playerStats, CameraHandler cameraHandler, WeaponSlotManager weaponSlotManager)
    {
      base.SuccessfullyCastSpell(playerAnimatorManager, playerStats, cameraHandler, weaponSlotManager);
      GameObject instantiatedSpellFX = Instantiate(spellCastFX, weaponSlotManager.rightHandSlot.transform.position, cameraHandler.transform.rotation);
      rigidbody = instantiatedSpellFX.GetComponent<Rigidbody>();

      if (cameraHandler.currentLockOnTarget!= null)
      {
        instantiatedSpellFX.transform.LookAt(cameraHandler.currentLockOnTarget.transform);
      }else{
        instantiatedSpellFX.transform.rotation = Quaternion.Euler(cameraHandler.cameraPivotTransform.eulerAngles.x, playerStats.transform.eulerAngles.y, 0);
      }

      rigidbody.AddForce(instantiatedSpellFX.transform.forward * projectileForwardVelocity);
      rigidbody.AddForce(instantiatedSpellFX.transform.up * projectileUpwardVelocity);
      rigidbody.useGravity = isEffectedByGravity;
      rigidbody.mass = projectileMass;
      instantiatedSpellFX.transform.parent = null;
    }
  }
}
