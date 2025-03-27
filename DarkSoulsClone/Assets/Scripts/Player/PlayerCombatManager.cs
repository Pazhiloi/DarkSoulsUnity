using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SG
{
  public class PlayerCombatManager : MonoBehaviour
  {
    CameraHandler cameraHandler;
    PlayerAnimatorManager playerAnimatorManager;
    PlayerEquipmentManager playerEquipmentManager;
    PlayerManager playerManager;
    PlayerStatsManager playerStatsManager;
    PlayerInventoryManager playerInventoryManager;
    InputHandler inputHandler;
    PlayerWeaponSlotManager playerWeaponSlotManager;
    public string lastAttack;
    public LayerMask backStabLayer = 1 << 12;
    public LayerMask riposteLayer = 1 << 13;

    private void Awake()
    {
      cameraHandler = FindObjectOfType<CameraHandler>();
      playerManager = GetComponent<PlayerManager>();
      playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
      playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
      playerStatsManager = GetComponent<PlayerStatsManager>();
      playerInventoryManager = GetComponent<PlayerInventoryManager>();
      playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
      inputHandler = GetComponent<InputHandler>();
    }

    public void HandleWeaponCombo(WeaponItem weapon)
    {
      if (playerStatsManager.currentStamina <= 0) return;
      if (inputHandler.comboFlag)
      {
        playerAnimatorManager.animator.SetBool("canDoCombo", false);

        if (lastAttack == weapon.oh_light_attack_01)
        {
          playerAnimatorManager.PlayTargetAnimation(weapon.oh_light_attack_02, true);
        }
        else if (lastAttack == weapon.th_light_attack_01)
        {
          playerAnimatorManager.PlayTargetAnimation(weapon.th_light_attack_02, true);
        }
      }

    }

    public void HandleLightAttack(WeaponItem weapon)
    {
      if (playerStatsManager.currentStamina <= 0) return;
      playerWeaponSlotManager.attackingWeapon = weapon;
      if (inputHandler.twoHandFlag)
      {
        playerAnimatorManager.PlayTargetAnimation(weapon.th_light_attack_01, true);
        lastAttack = weapon.th_light_attack_01;
      }
      else
      {
        playerAnimatorManager.PlayTargetAnimation(weapon.oh_light_attack_01, true);
        lastAttack = weapon.oh_light_attack_01;
      }
    }
    public void HandleHeavyAttack(WeaponItem weapon)
    {
      if (playerStatsManager.currentStamina <= 0) return;
      playerWeaponSlotManager.attackingWeapon = weapon;

      if (inputHandler.twoHandFlag)
      {
        playerAnimatorManager.PlayTargetAnimation(weapon.oh_light_attack_01, true);
        lastAttack = weapon.oh_light_attack_01;
      }
      else
      {
        playerAnimatorManager.PlayTargetAnimation(weapon.oh_light_attack_01, true);
        lastAttack = weapon.oh_light_attack_01;
      }
    }


    #region  Input Actions
    public void HandleRBAction()
    {
      if (playerInventoryManager.rightWeapon.isMelleWeapon)
      {
        PerformRBMelleAction();
      }
      else if (playerInventoryManager.rightWeapon.isSpellCaster || playerInventoryManager.rightWeapon.isFaithCaster || playerInventoryManager.rightWeapon.isPyroCaster)
      {
        PerformRBMagicAction(playerInventoryManager.rightWeapon);
      }
    }

    public void HandleLBAction()
    {
      PerformLBBlockingAction();
    }

    public void HandleLTAction()
    {
      if (playerInventoryManager.leftWeapon.isShieldWeapon)
      {
        PerformLTWeaponArt(inputHandler.twoHandFlag);
      }
      else if (playerInventoryManager.leftWeapon.isMelleWeapon)
      {

      }
    }
    #endregion

    #region Attack Actions

    private void PerformRBMelleAction()
    {
      if (playerManager.canDoCombo)
      {
        inputHandler.comboFlag = true;
        HandleWeaponCombo(playerInventoryManager.rightWeapon);
        inputHandler.comboFlag = false;
      }
      else
      {
        if (playerManager.isInteracting)
        {
          return;
        }
        if (playerManager.canDoCombo)
        {
          return;
        }
        playerAnimatorManager.animator.SetBool("isUsingRightHand", true);
        HandleLightAttack(playerInventoryManager.rightWeapon);
      }
    }


    private void PerformRBMagicAction(WeaponItem weapon)
    {
      if (playerManager.isInteracting) return;
      if (weapon.isFaithCaster)
      {
        if (playerInventoryManager.currentSpell != null && playerInventoryManager.currentSpell.isFaithSpell)
        {
          if (playerStatsManager.currentFocusPoints >= playerInventoryManager.currentSpell.focusPointCost)
          {
            playerInventoryManager.currentSpell.AttemptToCastSpell(playerAnimatorManager, playerStatsManager, playerWeaponSlotManager);
          }
          else
          {
            playerAnimatorManager.PlayTargetAnimation("Damage_01", true);
          }

        }
      }else if(weapon.isPyroCaster)
      {
        if (playerInventoryManager.currentSpell != null && playerInventoryManager.currentSpell.isPyroSpell)
        {
          if (playerStatsManager.currentFocusPoints >= playerInventoryManager.currentSpell.focusPointCost)
          {
            playerInventoryManager.currentSpell.AttemptToCastSpell(playerAnimatorManager, playerStatsManager, playerWeaponSlotManager);
          }
          else
          {
            playerAnimatorManager.PlayTargetAnimation("Damage_01", true);
          }

        }
      }
    }

    private void PerformLTWeaponArt(bool isTwoHanding)
    {
      if (playerManager.isInteracting) return;

      if (isTwoHanding)
      {
      }
      else
      {
        playerAnimatorManager.PlayTargetAnimation(playerInventoryManager.leftWeapon.weapon_art, true);

      }
    }
    private void SuccessfullyCastSpell()
    {
      playerInventoryManager.currentSpell.SuccessfullyCastSpell(playerAnimatorManager, playerStatsManager, cameraHandler, playerWeaponSlotManager);
      playerAnimatorManager.animator.SetBool("isFiringSpell", true);
    }

    #endregion

    #region  Defense Actions

    private void PerformLBBlockingAction()
    {
      if (playerManager.isInteracting) return;
      if (playerManager.isBlocking) return;

      playerAnimatorManager.PlayTargetAnimation("Block Start", false, true);
      playerEquipmentManager.OpenBlockingCollider();
      playerManager.isBlocking = true;
    }
    #endregion


    public void AttemptBackStabOrRiposte()
    {
      if (playerStatsManager.currentStamina <= 0) return;
      RaycastHit hit;
      if (Physics.Raycast(inputHandler.criticalAttackRayCastStartPoint.position, transform.TransformDirection(Vector3.forward), out hit, 0.5f, backStabLayer))
      {
        CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
        DamageCollider rightWeapon = playerWeaponSlotManager.rightHandDamageCollider;
        if (enemyCharacterManager != null)
        {

          playerManager.transform.position = enemyCharacterManager.backStabCollider.criticalDamageStandPosition.position;

          // rotate towards enemy transform
          Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
          rotationDirection = hit.transform.position - playerManager.transform.position;
          rotationDirection.y = 0;
          rotationDirection.Normalize();
          Quaternion tr = Quaternion.LookRotation(rotationDirection);
          Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
          playerManager.transform.rotation = targetRotation;


          int criticalDamage = playerInventoryManager.rightWeapon.criticalDamageMultiplier * rightWeapon.currentWeaponDamage;
          enemyCharacterManager.pendingCriticalDamage = criticalDamage;


          playerAnimatorManager.PlayTargetAnimation("Back Stab", true);
          enemyCharacterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Back Stabbed", true);
        }
      }
      else if (Physics.Raycast(inputHandler.criticalAttackRayCastStartPoint.position, transform.TransformDirection(Vector3.forward), out hit, 0.7f, riposteLayer))
      {
        CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
        DamageCollider rightWeapon = playerWeaponSlotManager.rightHandDamageCollider;

        if (enemyCharacterManager != null && enemyCharacterManager.canBeRiposted)
        {
          playerManager.transform.position = enemyCharacterManager.riposteCollider.criticalDamageStandPosition.position;

          Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
          rotationDirection = hit.transform.position - playerManager.transform.position;
          rotationDirection.y = 0;
          rotationDirection.Normalize();
          Quaternion tr = Quaternion.LookRotation(rotationDirection);
          Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
          playerManager.transform.rotation = targetRotation;

          int criticalDamage = playerInventoryManager.rightWeapon.criticalDamageMultiplier * rightWeapon.currentWeaponDamage;
          enemyCharacterManager.pendingCriticalDamage = criticalDamage;

          playerAnimatorManager.PlayTargetAnimation("Riposte", true);
          enemyCharacterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Riposted", true);
        }

      }
    }

  }
}
