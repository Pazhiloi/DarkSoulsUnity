using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MR
{
  public class CharacterCombatManager : MonoBehaviour
  {

    public LayerMask backStabLayer = 1 << 12;
    public LayerMask riposteLayer = 1 << 13;

    CharacterManager character;
    [Header("Attack Type")]
    public AttackType currentAttackType;
    [Header("Attack Animations")]
    public string oh_light_attack_01 = "OH_Light_Attack_01";
    public string oh_light_attack_02 = "OH_Light_Attack_02";
    public string oh_heavy_attack_01 = "OH_Heavy_Attack_01";
    public string oh_heavy_attack_02 = "OH_Heavy_Attack_02";
    public string oh_running_attack_01 = "OH_Running_Attack_01";
    public string oh_jumping_attack_01 = "OH_Jumping_Attack_01";

    public string oh_charge_attack_01 = "OH_Ch_At_charge_01";
    public string oh_charge_attack_02 = "OH_Ch_At_charge_02";

    public string th_light_attack_01 = "TH_Light_Attack_01";
    public string th_light_attack_02 = "TH_Light_Attack_02";
    public string th_heavy_attack_01 = "TH_Heavy_Attack_01";
    public string th_heavy_attack_02 = "TH_Heavy_Attack_02";
    public string th_running_attack_01 = "TH_Running_Attack_01";
    public string th_jumping_attack_01 = "TH_Jumping_Attack_01";

    public string th_charge_attack_01 = "TH_Ch_At_charge_01";
    public string th_charge_attack_02 = "TH_Ch_At_charge_02";

    public string weapon_art = "Weapon_Art";
    public string lastAttack;


    protected virtual void Awake()
    {
      character = GetComponent<CharacterManager>();
    }

    public virtual void SetBlockingAbsorptionsFromBlockingWeapon()
    {
      if (character.isUsingRightHand)
      {
        character.characterStatsManager.blockingPhysicalDamageAbsorption = character.characterInventoryManager.rightWeapon.physicalBlockingDamageAbsorption;
        character.characterStatsManager.blockingFireDamageAbsorption = character.characterInventoryManager.rightWeapon.fireBlockingDamageAbsorption;
        character.characterStatsManager.blockingStabilityRating = character.characterInventoryManager.rightWeapon.stability;
      }
      else if (character.isUsingLeftHand)
      {
        character.characterStatsManager.blockingPhysicalDamageAbsorption = character.characterInventoryManager.leftWeapon.physicalBlockingDamageAbsorption;
        character.characterStatsManager.blockingFireDamageAbsorption = character.characterInventoryManager.leftWeapon.fireBlockingDamageAbsorption;
        character.characterStatsManager.blockingStabilityRating = character.characterInventoryManager.leftWeapon.stability;
      }
    }

    public virtual void DrainStaminaBasedOnAttack()
    {

    }


    public virtual void AttemptBlock(DamageCollider attackingWeapon, float physicalDamage, float fireDamage, string blockAnimation)
    {

      float staminaDamageAbsorption = ((physicalDamage + fireDamage) * attackingWeapon.guardBreakModifier) * (character.characterStatsManager.blockingStabilityRating / 100);

      float staminaDamage = ((physicalDamage + fireDamage) * attackingWeapon.guardBreakModifier) - staminaDamageAbsorption;
      character.characterStatsManager.currentStamina -= staminaDamage;
      if (character.characterStatsManager.currentStamina <= 0)
      {
        character.isBlocking = false;
        character.characterAnimatorManager.PlayTargetAnimation("Guard_Break_01", true);
      }
      else{
        character.characterAnimatorManager.PlayTargetAnimation(blockAnimation, true);
        
      }
    }




    private void SuccessfullyCastSpell()
    {
      character.characterInventoryManager.currentSpell.SuccessfullyCastSpell(character);
    }



    public void AttemptBackStabOrRiposte()
    {
      if (character.characterStatsManager.currentStamina <= 0) return;
      RaycastHit hit;
      if (Physics.Raycast(character.criticalAttackRayCastStartPoint.position, transform.TransformDirection(Vector3.forward), out hit, 0.5f, backStabLayer))
      {
        CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
        DamageCollider rightWeapon = character.characterWeaponSlotManager.rightHandDamageCollider;
        if (enemyCharacterManager != null)
        {

          character.transform.position = enemyCharacterManager.backStabCollider.criticalDamageStandPosition.position;

          // rotate towards enemy transform
          Vector3 rotationDirection = character.transform.root.eulerAngles;
          rotationDirection = hit.transform.position - character.transform.position;
          rotationDirection.y = 0;
          rotationDirection.Normalize();
          Quaternion tr = Quaternion.LookRotation(rotationDirection);
          Quaternion targetRotation = Quaternion.Slerp(character.transform.rotation, tr, 500 * Time.deltaTime);
          character.transform.rotation = targetRotation;


          int criticalDamage = character.characterInventoryManager.rightWeapon.criticalDamageMultiplier * rightWeapon.physicalDamage;
          enemyCharacterManager.pendingCriticalDamage = criticalDamage;


          character.characterAnimatorManager.PlayTargetAnimation("Back Stab", true);
          enemyCharacterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimation("Back Stabbed", true);
        }
      }
      else if (Physics.Raycast(character.criticalAttackRayCastStartPoint.position, transform.TransformDirection(Vector3.forward), out hit, 0.7f, riposteLayer))
      {
        CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
        DamageCollider rightWeapon = character.characterWeaponSlotManager.rightHandDamageCollider;

        if (enemyCharacterManager != null && enemyCharacterManager.canBeRiposted)
        {
          character.transform.position = enemyCharacterManager.riposteCollider.criticalDamageStandPosition.position;

          Vector3 rotationDirection = character.transform.root.eulerAngles;
          rotationDirection = hit.transform.position - character.transform.position;
          rotationDirection.y = 0;
          rotationDirection.Normalize();
          Quaternion tr = Quaternion.LookRotation(rotationDirection);
          Quaternion targetRotation = Quaternion.Slerp(character.transform.rotation, tr, 500 * Time.deltaTime);
          character.transform.rotation = targetRotation;

          int criticalDamage = character.characterInventoryManager.rightWeapon.criticalDamageMultiplier * rightWeapon.physicalDamage;
          enemyCharacterManager.pendingCriticalDamage = criticalDamage;

          character.characterAnimatorManager.PlayTargetAnimation("Riposte", true);
          enemyCharacterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimation("Riposted", true);
        }

      }
    }

    

    
  }
}
