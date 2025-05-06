using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MR
{
  [CreateAssetMenu(menuName = "Items Actions/Fire Arrow Action")]
  public class FireArrowAction : ItemAction
  {
    public override void PerformAction(CharacterManager character)
    {

      PlayerManager player = character as PlayerManager;

      ArrowInstantiationLocation arrowInstantiationLocation;
      arrowInstantiationLocation = character.characterWeaponSlotManager.rightHandSlot.GetComponentInChildren<ArrowInstantiationLocation>();

      // ANIMATE THE BOW FIRING THE ARROW
      Animator bowAnimator = character.characterWeaponSlotManager.rightHandSlot.GetComponentInChildren<Animator>();

      bowAnimator.SetBool("isDrawn", false);
      bowAnimator.Play("Bow_TH_Fire_01");

      Destroy(character.characterEffectsManager.currentRangeFX); // Destroys the loaded arrow model

      // RESET THE PLAYERS HOLDING ARROW FLAG
      character.characterAnimatorManager.PlayTargetAnimation("Bow_TH_Fire_01", true);
      character.animator.SetBool("isHoldingArrow", false);


      if (player != null)
      {
        // CREATE AND FIRE THE LIVE ARROW
        GameObject liveArrow = Instantiate(player.playerInventoryManager.currentAmmo.liveAmmoModel, arrowInstantiationLocation.transform.position, player.cameraHandler.cameraPivotTransform.rotation);

        Rigidbody rigidBody = liveArrow.GetComponentInChildren<Rigidbody>();
        RangedProjectileDamageCollider damageCollider = liveArrow.GetComponentInChildren<RangedProjectileDamageCollider>();

        if (player.isAiming)
        {
          Ray ray = player.cameraHandler.cameraObject.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
          RaycastHit hitPoint;

          if (Physics.Raycast(ray, out hitPoint, 100.0f))
          {
            liveArrow.transform.LookAt(hitPoint.point);
          }
          else
          {
            liveArrow.transform.rotation = Quaternion.Euler(player.cameraHandler.cameraPivotTransform.eulerAngles.x, player.lockOnTransform.eulerAngles.y, 0);
          }
        }
        else
        {
          // GIVE AMMO VELOCITY
          if (player.cameraHandler != null)
          {
            Quaternion arrowRotation = Quaternion.LookRotation(player.cameraHandler.currentLockOnTarget.lockOnTransform.position - liveArrow.gameObject.transform.position);
            liveArrow.transform.rotation = arrowRotation;
          }
          else
          {
            liveArrow.transform.rotation = Quaternion.Euler(player.cameraHandler.cameraPivotTransform.eulerAngles.x, player.lockOnTransform.eulerAngles.y, 0);
          }
        }



        rigidBody.AddForce(liveArrow.transform.forward * player.playerInventoryManager.currentAmmo.forwardVelocity);
        rigidBody.AddForce(liveArrow.transform.up * player.playerInventoryManager.currentAmmo.upwardVelocity);

        rigidBody.useGravity = player.playerInventoryManager.currentAmmo.useGravity;

        rigidBody.mass = player.playerInventoryManager.currentAmmo.ammoMass;
        liveArrow.transform.parent = null;
        // SET LIVE AMMO DAMAGE
        damageCollider.characterManager = character;
        damageCollider.ammoItem = player.playerInventoryManager.currentAmmo;
        damageCollider.physicalDamage = player.playerInventoryManager.currentAmmo.physicalDamage;
      }
      else{

        EnemyManager enemy = character as EnemyManager;
        // CREATE AND FIRE THE LIVE ARROW
        GameObject liveArrow = Instantiate(player.playerInventoryManager.currentAmmo.liveAmmoModel, arrowInstantiationLocation.transform.position, Quaternion.identity);

        Rigidbody rigidBody = liveArrow.GetComponentInChildren<Rigidbody>();
        RangedProjectileDamageCollider damageCollider = liveArrow.GetComponentInChildren<RangedProjectileDamageCollider>();


        //GIVE AMMO VELOCITY
        if (enemy.currentTarget != null)
        {
          //Since while locked we are ALWAYS facing our target we can copy our facing direction to our arrows facing direction when fired
          Quaternion arrowRotation = Quaternion.LookRotation(enemy.currentTarget.lockOnTransform.position - liveArrow.transform.position);
          liveArrow.transform.rotation = arrowRotation;
        }

        rigidBody.AddForce(liveArrow.transform.forward * enemy.characterInventoryManager.currentAmmo.forwardVelocity);
        rigidBody.AddForce(liveArrow.transform.up * enemy.characterInventoryManager.currentAmmo.upwardVelocity);

        rigidBody.useGravity = enemy.characterInventoryManager.currentAmmo.useGravity;

        rigidBody.mass = enemy.characterInventoryManager.currentAmmo.ammoMass;
        liveArrow.transform.parent = null;
        // SET LIVE AMMO DAMAGE
        damageCollider.characterManager = character;
        damageCollider.ammoItem = enemy.characterInventoryManager.currentAmmo;
        damageCollider.physicalDamage = enemy.characterInventoryManager.currentAmmo.physicalDamage;
        damageCollider.teamIDNumber = enemy.characterStatsManager.teamIDNumber;
      }
      
    }

  }
}
