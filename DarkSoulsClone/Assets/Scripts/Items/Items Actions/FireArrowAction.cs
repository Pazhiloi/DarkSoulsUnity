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

      Destroy(character.characterEffectsManager.instantiatedFXModel); // Destroys the loaded arrow model

      // RESET THE PLAYERS HOLDING ARROW FLAG
      character.characterAnimatorManager.PlayTargetAnimation("Bow_TH_Fire_01", true);
      character.animator.SetBool("isHoldingArrow", false);


      if (player != null)
      {
        // CREATE AND FIRE THE LIVE ARROW
        GameObject liveArrow = Instantiate(player.playerInventoryManager.currentAmmo.liveAmmoModel, arrowInstantiationLocation.transform.position, player.cameraHandler.cameraPivotTransform.rotation);

        Rigidbody rigidBody = liveArrow.GetComponent<Rigidbody>();
        RangedProjectileDamageCollider damageCollider = liveArrow.GetComponent<RangedProjectileDamageCollider>();

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

        AICharacterManager aiCharacter = character as AICharacterManager;
        // CREATE AND FIRE THE LIVE ARROW
        GameObject liveArrow = Instantiate(player.playerInventoryManager.currentAmmo.liveAmmoModel, arrowInstantiationLocation.transform.position, Quaternion.identity);

        Rigidbody rigidBody = liveArrow.GetComponent<Rigidbody>();
        RangedProjectileDamageCollider damageCollider = liveArrow.GetComponent<RangedProjectileDamageCollider>();


        //GIVE AMMO VELOCITY
        if (aiCharacter.currentTarget != null)
        {
          //Since while locked we are ALWAYS facing our target we can copy our facing direction to our arrows facing direction when fired
          Quaternion arrowRotation = Quaternion.LookRotation(aiCharacter.currentTarget.lockOnTransform.position - liveArrow.transform.position);
          liveArrow.transform.rotation = arrowRotation;
        }

        rigidBody.AddForce(liveArrow.transform.forward * aiCharacter.characterInventoryManager.currentAmmo.forwardVelocity);
        rigidBody.AddForce(liveArrow.transform.up * aiCharacter.characterInventoryManager.currentAmmo.upwardVelocity);

        rigidBody.useGravity = aiCharacter.characterInventoryManager.currentAmmo.useGravity;

        rigidBody.mass = aiCharacter.characterInventoryManager.currentAmmo.ammoMass;
        liveArrow.transform.parent = null;
        // SET LIVE AMMO DAMAGE
        damageCollider.characterManager = character;
        damageCollider.ammoItem = aiCharacter.characterInventoryManager.currentAmmo;
        damageCollider.physicalDamage = aiCharacter.characterInventoryManager.currentAmmo.physicalDamage;
        damageCollider.teamIDNumber = aiCharacter.characterStatsManager.teamIDNumber;
      }
      
    }

  }
}
