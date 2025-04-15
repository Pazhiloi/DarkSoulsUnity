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