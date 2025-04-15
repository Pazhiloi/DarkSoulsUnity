using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MR
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
    PlayerEffectsManager playerEffectsManager;
    public string lastAttack;
    public LayerMask backStabLayer = 1 << 12;
    public LayerMask riposteLayer = 1 << 13;

    [Header("Attack Animations")]
    public string oh_light_attack_01 = "OH_Light_Attack_01";
    public string oh_light_attack_02 = "OH_Light_Attack_02";
    public string oh_heavy_attack_01 = "OH_Heavy_Attack_01";
    public string oh_heavy_attack_02 = "OH_Heavy_Attack_02";
    public string oh_running_attack_01 = "OH_Running_Attack_01";
    public string oh_jumping_attack_01 = "OH_Jumping_Attack_01";

    public string th_light_attack_01 = "TH_Light_Attack_01";
    public string th_light_attack_02 = "TH_Light_Attack_02";
    public string th_heavy_attack_01 = "TH_Heavy_Attack_01";
    public string th_heavy_attack_02 = "TH_Heavy_Attack_02";
    public string th_running_attack_01 = "TH_Running_Attack_01";
    public string th_jumping_attack_01 = "TH_Jumping_Attack_01";

    public string weapon_art = "Weapon_Art";

    private void Awake()
    {
      cameraHandler = FindObjectOfType<CameraHandler>();
      playerManager = GetComponent<PlayerManager>();
      playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
      playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
      playerStatsManager = GetComponent<PlayerStatsManager>();
      playerInventoryManager = GetComponent<PlayerInventoryManager>();
      playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
      playerEffectsManager = GetComponent<PlayerEffectsManager>();
      inputHandler = GetComponent<InputHandler>();
    }

    public void HandleHoldRBAction()
    {
      if (playerManager.isTwoHandingWeapon)
      {
        PerformRBRangedAction();
      }
      else
      {
        //DO A MELEE ATTACK (Bow Bash)
      }
    }



    public void HandleRBAction()
    {
      PerformMagicAction(playerInventoryManager.rightWeapon, true);
    }


    public void HandleLBAction()
    {
        if (playerInventoryManager.leftWeapon.weaponType == WeaponType.FaithCaster || playerInventoryManager.leftWeapon.weaponType == WeaponType.PyromancyCaster)
        {
          PerformMagicAction(playerInventoryManager.leftWeapon, true);
          playerAnimatorManager.animator.SetBool("isUsingLeftHand", true);
        }
      }
    

    public void HandleLTAction()
    {
      if (playerInventoryManager.leftWeapon.weaponType == WeaponType.Shield || playerInventoryManager.rightWeapon.weaponType == WeaponType.Unarmed)
      {
        PerformLTWeaponArt(inputHandler.twoHandFlag);
      }
      else if (playerInventoryManager.leftWeapon.weaponType == WeaponType.StraightSword)
      {
        // do a light attack
      }
    }


   

    private void PerformRBRangedAction()
    {
      if (playerStatsManager.currentStamina <= 0)
      {
        return;
      }

      playerAnimatorManager.EraseHandIKForWeapon();
      playerAnimatorManager.animator.SetBool("isUsingRightHand", true);
      if (!playerManager.isHoldingArrow)
      {
        if (playerInventoryManager.currentAmmo != null)
        {
        }
        else
        {
          playerAnimatorManager.PlayTargetAnimation("Shrug", true);
        }
      }
    }


    private void PerformMagicAction(WeaponItem weapon, bool isLeftHanded)
    {
      if (playerManager.isInteracting) return;
      if (weapon.weaponType == WeaponType.FaithCaster)
      {
        if (playerInventoryManager.currentSpell != null && playerInventoryManager.currentSpell.isFaithSpell)
        {
          if (playerStatsManager.currentFocusPoints >= playerInventoryManager.currentSpell.focusPointCost)
          {
            playerInventoryManager.currentSpell.AttemptToCastSpell(playerAnimatorManager, playerStatsManager, playerWeaponSlotManager, isLeftHanded);
          }
          else
          {
            playerAnimatorManager.PlayTargetAnimation("Damage_01", true);
          }

        }
      }
      else if (weapon.weaponType == WeaponType.PyromancyCaster)
      {
        if (playerInventoryManager.currentSpell != null && playerInventoryManager.currentSpell.isPyroSpell)
        {
          if (playerStatsManager.currentFocusPoints >= playerInventoryManager.currentSpell.focusPointCost)
          {
            playerInventoryManager.currentSpell.AttemptToCastSpell(playerAnimatorManager, playerStatsManager, playerWeaponSlotManager, isLeftHanded);
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
        playerAnimatorManager.PlayTargetAnimation(weapon_art, true);

      }
    }
    private void SuccessfullyCastSpell()
    {
      playerInventoryManager.currentSpell.SuccessfullyCastSpell(playerAnimatorManager, playerStatsManager, cameraHandler, playerWeaponSlotManager, playerManager.isUsingLeftHand);
      playerAnimatorManager.animator.SetBool("isFiringSpell", true);
    }



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


          int criticalDamage = playerInventoryManager.rightWeapon.criticalDamageMultiplier * rightWeapon.physicalDamage;
          enemyCharacterManager.pendingCriticalDamage = criticalDamage;


          playerAnimatorManager.PlayTargetAnimation("Back Stab", true);
          enemyCharacterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimation("Back Stabbed", true);
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

          int criticalDamage = playerInventoryManager.rightWeapon.criticalDamageMultiplier * rightWeapon.physicalDamage;
          enemyCharacterManager.pendingCriticalDamage = criticalDamage;

          playerAnimatorManager.PlayTargetAnimation("Riposte", true);
          enemyCharacterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimation("Riposted", true);
        }

      }
    }

  }
}