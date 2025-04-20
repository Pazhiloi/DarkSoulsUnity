using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MR
{
  [CreateAssetMenu(menuName = "Items Actions/Fire Arrow Action")]
  public class FireArrowAction : ItemAction
  {
    public override void PerformAction(PlayerManager player)
    {
      ArrowInstantiationLocation arrowInstantiationLocation;
      arrowInstantiationLocation = player.playerWeaponSlotManager.rightHandSlot.GetComponentInChildren<ArrowInstantiationLocation>();

      // ANIMATE THE BOW FIRING THE ARROW
      Animator bowAnimator = player.playerWeaponSlotManager.rightHandSlot.GetComponentInChildren<Animator>();

      bowAnimator.SetBool("isDrawn", false);
      bowAnimator.Play("Bow_TH_Fire_01");

      Destroy(player.playerEffectsManager.currentRangeFX); // Destroys the loaded arrow model

      // RESET THE PLAYERS HOLDING ARROW FLAG
      player.playerAnimatorManager.PlayTargetAnimation("Bow_TH_Fire_01", true);
      player.animator.SetBool("isHoldingArrow", false);


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
      damageCollider.characterManager = player;
      damageCollider.ammoItem = player.playerInventoryManager.currentAmmo;
      damageCollider.physicalDamage = player.playerInventoryManager.currentAmmo.physicalDamage;
    }
       
    }
}
