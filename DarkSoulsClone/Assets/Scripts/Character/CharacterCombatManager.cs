using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MR
{
  public class CharacterCombatManager : MonoBehaviour
  {

    CharacterManager character;
    [Header("Attack Type")]
    public AttackType currentAttackType;

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
  }
}
