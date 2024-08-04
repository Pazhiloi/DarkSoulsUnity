using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SG
{
  public class PlayerAttacker : MonoBehaviour
  {
    CameraHandler cameraHandler;
    PlayerAnimatorManager playerAnimatorManager;
    PlayerEquipmentManager playerEquipmentManager;
    PlayerManager playerManager;
    PlayerStats playerStats;
    PlayerInventory playerInventory;
    InputHandler inputHandler;
    WeaponSlotManager weaponSlotManager;
    public string lastAttack;
    public LayerMask backStabLayer = 1 << 12;
    public LayerMask riposteLayer = 1 << 13;

    private void Awake()
    {
      cameraHandler = FindObjectOfType<CameraHandler>();
      playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
      playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
      playerManager = GetComponentInParent<PlayerManager>();
      playerStats = GetComponentInParent<PlayerStats>();
      playerInventory = GetComponentInParent<PlayerInventory>();
      weaponSlotManager = GetComponent<WeaponSlotManager>();
      inputHandler = GetComponentInParent<InputHandler>();
    }

    public void HandleWeaponCombo(WeaponItem weapon)
    {
      if (playerStats.currentStamina <= 0) return;
      if (inputHandler.comboFlag)
      {
        playerAnimatorManager.anim.SetBool("canDoCombo", false);

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
      if (playerStats.currentStamina <= 0) return;
      weaponSlotManager.attackingWeapon = weapon;
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
      if (playerStats.currentStamina <= 0) return;
      weaponSlotManager.attackingWeapon = weapon;

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
      if (playerInventory.rightWeapon.isMelleWeapon)
      {
        PerformRBMelleAction();
      }
      else if (playerInventory.rightWeapon.isSpellCaster || playerInventory.rightWeapon.isFaithCaster || playerInventory.rightWeapon.isPyroCaster)
      {
        PerformRBMagicAction(playerInventory.rightWeapon);
      }
    }

    public void HandleLBAction()
    {
      PerformLBBlockingAction();
    }

    public void HandleLTAction()
    {
      if (playerInventory.leftWeapon.isShieldWeapon)
      {
        PerformLTWeaponArt(inputHandler.twoHandFlag);
      }
      else if (playerInventory.leftWeapon.isMelleWeapon)
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
        HandleWeaponCombo(playerInventory.rightWeapon);
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
        playerAnimatorManager.anim.SetBool("isUsingRightHand", true);
        HandleLightAttack(playerInventory.rightWeapon);
      }
    }


    private void PerformRBMagicAction(WeaponItem weapon)
    {
      if (playerManager.isInteracting) return;
      if (weapon.isFaithCaster)
      {
        if (playerInventory.currentSpell != null && playerInventory.currentSpell.isFaithSpell)
        {
          if (playerStats.currentFocusPoints >= playerInventory.currentSpell.focusPointCost)
          {
            playerInventory.currentSpell.AttemptToCastSpell(playerAnimatorManager, playerStats, weaponSlotManager);
          }
          else
          {
            playerAnimatorManager.PlayTargetAnimation("Damage_01", true);
          }

        }
      }else if(weapon.isPyroCaster)
      {
        if (playerInventory.currentSpell != null && playerInventory.currentSpell.isPyroSpell)
        {
          if (playerStats.currentFocusPoints >= playerInventory.currentSpell.focusPointCost)
          {
            playerInventory.currentSpell.AttemptToCastSpell(playerAnimatorManager, playerStats, weaponSlotManager);
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
        playerAnimatorManager.PlayTargetAnimation(playerInventory.leftWeapon.weapon_art, true);

      }
    }
    private void SuccessfullyCastSpell()
    {
      playerInventory.currentSpell.SuccessfullyCastSpell(playerAnimatorManager, playerStats, cameraHandler, weaponSlotManager);
      playerAnimatorManager.anim.SetBool("isFiringSpell", true);
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
      if (playerStats.currentStamina <= 0) return;
      RaycastHit hit;
      if (Physics.Raycast(inputHandler.criticalAttackRayCastStartPoint.position, transform.TransformDirection(Vector3.forward), out hit, 0.5f, backStabLayer))
      {
        CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
        DamageCollider rightWeapon = weaponSlotManager.rightHandDamageCollider;
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


          int criticalDamage = playerInventory.rightWeapon.criticalDamageMultiplier * rightWeapon.currentWeaponDamage;
          enemyCharacterManager.pendingCriticalDamage = criticalDamage;


          playerAnimatorManager.PlayTargetAnimation("Back Stab", true);
          enemyCharacterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Back Stabbed", true);
        }
      }
      else if (Physics.Raycast(inputHandler.criticalAttackRayCastStartPoint.position, transform.TransformDirection(Vector3.forward), out hit, 0.7f, riposteLayer))
      {
        CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
        DamageCollider rightWeapon = weaponSlotManager.rightHandDamageCollider;

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

          int criticalDamage = playerInventory.rightWeapon.criticalDamageMultiplier * rightWeapon.currentWeaponDamage;
          enemyCharacterManager.pendingCriticalDamage = criticalDamage;

          playerAnimatorManager.PlayTargetAnimation("Riposte", true);
          enemyCharacterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Riposted", true);
        }

      }
    }

  }
}
