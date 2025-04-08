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
    PlayerEffectsManager playerEffectsManager;
    public string lastAttack;
    public LayerMask backStabLayer = 1 << 12;
    public LayerMask riposteLayer = 1 << 13;

    [Header("Attack Animations")]
    string oh_light_attack_01 = "OH_Light_Attack_01";
    string oh_light_attack_02 = "OH_Light_Attack_02";
    string oh_heavy_attack_01 = "OH_Heavy_Attack_01";
    string oh_heavy_attack_02 = "OH_Heavy_Attack_02";
    string oh_running_attack_01 = "OH_Running_Attack_01";
    string oh_jumping_attack_01 = "OH_Jumping_Attack_01";

    string th_light_attack_01 = "TH_Light_Attack_01";
    string th_light_attack_02 = "TH_Light_Attack_02";
    string th_heavy_attack_01 = "TH_Heavy_Attack_01";
    string th_heavy_attack_02 = "TH_Heavy_Attack_02";
    string th_running_attack_01 = "TH_Running_Attack_01";
    string th_jumping_attack_01 = "TH_Jumping_Attack_01";

    string weapon_art = "Weapon_Art";

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
      playerAnimatorManager.EraseHandIKForWeapon();
      if (playerInventoryManager.rightWeapon.weaponType == WeaponType.StraightSword || playerInventoryManager.rightWeapon.weaponType == WeaponType.Unarmed)
      {
        PerformRBMelleAction();
      }
      else if (playerInventoryManager.rightWeapon.weaponType == WeaponType.SpellCaster ||
               playerInventoryManager.rightWeapon.weaponType == WeaponType.FaithCaster ||
               playerInventoryManager.rightWeapon.weaponType == WeaponType.PyromancyCaster)
      {
        PerformMagicAction(playerInventoryManager.rightWeapon, false);
      }
    }
    public void HandleRTAction(){
      playerAnimatorManager.EraseHandIKForWeapon();
      if (playerInventoryManager.rightWeapon.weaponType == WeaponType.StraightSword || playerInventoryManager.rightWeapon.weaponType == WeaponType.Unarmed)
      {
        PerformRTMelleAction();
      }
      else if (playerInventoryManager.rightWeapon.weaponType == WeaponType.SpellCaster ||
               playerInventoryManager.rightWeapon.weaponType == WeaponType.FaithCaster ||
               playerInventoryManager.rightWeapon.weaponType == WeaponType.PyromancyCaster)
      {
        PerformMagicAction(playerInventoryManager.rightWeapon, false);
      }
    }

    public void HandleLBAction()
    {
      if (playerManager.isTwoHandingWeapon)
      {
        if (playerInventoryManager.rightWeapon.weaponType == WeaponType.Bow)
        {
          PerformLBAimingAction();
        }
      }
      else
      {
        if (playerInventoryManager.leftWeapon.weaponType == WeaponType.Shield || playerInventoryManager.leftWeapon.weaponType == WeaponType.StraightSword)
        {
          PerformLBBlockingAction();
        }
        else if (playerInventoryManager.leftWeapon.weaponType == WeaponType.FaithCaster || playerInventoryManager.leftWeapon.weaponType == WeaponType.PyromancyCaster)
        {
          PerformMagicAction(playerInventoryManager.leftWeapon, true);
          playerAnimatorManager.animator.SetBool("isUsingLeftHand", true);
        }
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

    private void HandleLightWeaponCombo(WeaponItem weapon)
    {
      if (playerStatsManager.currentStamina <= 0) return;
      if (inputHandler.comboFlag)
      {
        playerAnimatorManager.animator.SetBool("canDoCombo", false);

        if (lastAttack == oh_light_attack_01)
        {
          playerAnimatorManager.PlayTargetAnimation(oh_light_attack_02, true);
        }
        else if (lastAttack == th_light_attack_01)
        {
          playerAnimatorManager.PlayTargetAnimation(th_light_attack_02, true);
        }
      }
    }

    private void HandleHeavyWeaponCombo(WeaponItem weapon)
    {
      if (playerStatsManager.currentStamina <= 0) return;
      if (inputHandler.comboFlag)
      {
        playerAnimatorManager.animator.SetBool("canDoCombo", false);

        if (lastAttack == oh_heavy_attack_01)
        {
          playerAnimatorManager.PlayTargetAnimation(oh_heavy_attack_02, true);
        }
        else if (lastAttack == th_heavy_attack_01)
        {
          playerAnimatorManager.PlayTargetAnimation(th_heavy_attack_02, true);
        }
      }
    }

    private void HandleLightAttack(WeaponItem weapon)
    {
      if (playerStatsManager.currentStamina <= 0) return;
      playerWeaponSlotManager.attackingWeapon = weapon;
      if (inputHandler.twoHandFlag)
      {
        playerAnimatorManager.PlayTargetAnimation(th_light_attack_01, true);
        lastAttack = th_light_attack_01;
      }
      else
      {
        playerAnimatorManager.PlayTargetAnimation(oh_light_attack_01, true);
        lastAttack = oh_light_attack_01;
      }
    }

    private void HandleJumpingAttack(WeaponItem weapon)
    {
      if (playerStatsManager.currentStamina <= 0) return;
      playerWeaponSlotManager.attackingWeapon = weapon;

      if (inputHandler.twoHandFlag)
      {
        playerAnimatorManager.PlayTargetAnimation(th_jumping_attack_01, true);
        lastAttack = th_jumping_attack_01;
      }
      else
      {
        playerAnimatorManager.PlayTargetAnimation(oh_jumping_attack_01, true);
        lastAttack = oh_jumping_attack_01;
      }
    }


    public void HandleHeavyAttack(WeaponItem weapon)
    {
      if (playerStatsManager.currentStamina <= 0) return;
      playerWeaponSlotManager.attackingWeapon = weapon;

      if (inputHandler.twoHandFlag)
      {
        playerAnimatorManager.PlayTargetAnimation(th_light_attack_01, true);
        lastAttack = th_heavy_attack_01;
      }
      else
      {
        playerAnimatorManager.PlayTargetAnimation(oh_heavy_attack_01, true);
        lastAttack = oh_heavy_attack_01;
      }
    }

    private void HandleRunningAttack(WeaponItem weapon)
    {
      if (playerStatsManager.currentStamina <= 0) return;
      playerWeaponSlotManager.attackingWeapon = weapon;

      if (inputHandler.twoHandFlag)
      {
        playerAnimatorManager.PlayTargetAnimation(th_running_attack_01, true);
        lastAttack = th_running_attack_01;
      }
      else
      {
        playerAnimatorManager.PlayTargetAnimation(oh_running_attack_01, true);
        lastAttack = oh_running_attack_01;
      }
    }



    private void DrawArrowAction()
    {
      playerAnimatorManager.animator.SetBool("isHoldingArrow", true);
      playerAnimatorManager.PlayTargetAnimation("Bow_TH_Draw_01", true);
      GameObject loadedArrow = Instantiate(playerInventoryManager.currentAmmo.loadedItemModel, playerWeaponSlotManager.leftHandSlot.transform);
      Animator bowAnimator = playerWeaponSlotManager.rightHandSlot.GetComponentInChildren<Animator>();
      bowAnimator.SetBool("isDrawn", true);
      bowAnimator.Play("Bow_TH_Draw_01");
      playerEffectsManager.currentRangeFX = loadedArrow;
    }

    public void FireArrowAction()
    {
      ArrowInstantiationLocation arrowInstantiationLocation;
      arrowInstantiationLocation = playerWeaponSlotManager.rightHandSlot.GetComponentInChildren<ArrowInstantiationLocation>();

      // ANIMATE THE BOW FIRING THE ARROW
      Animator bowAnimator = playerWeaponSlotManager.rightHandSlot.GetComponentInChildren<Animator>();

      bowAnimator.SetBool("isDrawn", false);
      bowAnimator.Play("Bow_TH_Fire_01");

      Destroy(playerEffectsManager.currentRangeFX); // Destroys the loaded arrow model

      // RESET THE PLAYERS HOLDING ARROW FLAG
      playerAnimatorManager.PlayTargetAnimation("Bow_TH_Fire_01", true);
      playerAnimatorManager.animator.SetBool("isHoldingArrow", false);


      // CREATE AND FIRE THE LIVE ARROW
      GameObject liveArrow = Instantiate(playerInventoryManager.currentAmmo.liveAmmoModel, arrowInstantiationLocation.transform.position, cameraHandler.cameraPivotTransform.rotation);

      Rigidbody rigidBody = liveArrow.GetComponentInChildren<Rigidbody>();
      RangedProjectileDamageCollider damageCollider = liveArrow.GetComponentInChildren<RangedProjectileDamageCollider>();

      // GIVE AMMO VELOCITY
      if (cameraHandler != null)
      {
        Quaternion arrowRotation = Quaternion.LookRotation(transform.forward);
        liveArrow.transform.rotation = arrowRotation;
      }
      else{
        liveArrow.transform.rotation = Quaternion.Euler(cameraHandler.cameraPivotTransform.eulerAngles.x, playerManager.lockOnTransform.eulerAngles.y, 0);

      }

      rigidBody.AddForce(liveArrow.transform.forward * playerInventoryManager.currentAmmo.forwardVelocity);
      rigidBody.AddForce(liveArrow.transform.up * playerInventoryManager.currentAmmo.upwardVelocity);

      rigidBody.useGravity = playerInventoryManager.currentAmmo.useGravity;

      rigidBody.mass = playerInventoryManager.currentAmmo.ammoMass;
      liveArrow.transform.parent = null;
      // SET LIVE AMMO DAMAGE
      damageCollider.characterManager = playerManager;
      damageCollider.ammoItem = playerInventoryManager.currentAmmo;
      damageCollider.physicalDamage = playerInventoryManager.currentAmmo.physicalDamage;
    }


    private void PerformRBMelleAction()
    {
      playerAnimatorManager.animator.SetBool("isUsingRightHand", true);

      if (playerManager.isSprinting)
      {
        HandleRunningAttack(playerInventoryManager.rightWeapon);
        return;
      }
      
      if (playerManager.canDoCombo)
      {
        inputHandler.comboFlag = true;
        HandleLightWeaponCombo(playerInventoryManager.rightWeapon);
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
        HandleLightAttack(playerInventoryManager.rightWeapon);
      }
      playerEffectsManager.PlayWeaponFX(false);

    }

   private void PerformRTMelleAction(){
      playerAnimatorManager.animator.SetBool("isUsingRightHand", true);

      if (playerManager.isSprinting)
      {
        HandleJumpingAttack(playerInventoryManager.rightWeapon);
        return;
      }

      if (playerManager.canDoCombo)
      {
        inputHandler.comboFlag = true;
        HandleHeavyWeaponCombo(playerInventoryManager.rightWeapon);
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
        HandleHeavyAttack(playerInventoryManager.rightWeapon);
      }
      playerEffectsManager.PlayWeaponFX(false);
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
          DrawArrowAction();
        }
        else
        {
          playerAnimatorManager.PlayTargetAnimation("Shrug", true);
        }
      }
    }

    private void PerformLBAimingAction()
    {
      // playerAnimatorManager.animator.SetBool("isHoldingArrow", true);
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

    private void PerformLBBlockingAction()
    {
      if (playerManager.isInteracting) return;
      if (playerManager.isBlocking) return;

      playerAnimatorManager.PlayTargetAnimation("Block Start", false, true);
      playerEquipmentManager.OpenBlockingCollider();
      playerManager.isBlocking = true;
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