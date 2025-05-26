using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MR
{
  public class TakeBlockedDamageEffect : CharacterEffect
  {
    [Header("Character Causing Damage")]
    public CharacterManager characterCausingDamage; // IF THE DAMAGE IS CAUSED BY A CHARACTER, THEY ARE LISTED HERE

    [Header("Base Damage")]
    public float physicalDamage = 0;
    public float fireDamage = 0;
    public float staminaDamage = 0;
    public float poiseDamage = 0;

    [Header("Final Damage")]

    [Header("Animation")]
    public string blockAnimation;

    public override void ProcessEffect(CharacterManager character)
    {
      if (character.isDead) return;
      if (character.isInvulnerable) return;

      CalculateDamage(character);
      CalculateStaminaDamage(character);
      DecideBlockAnimationBasedOnPoiseDamage(character);
      PlayBlockSoundFX(character);
      AssignNewAITarget(character);

      if (character.isDead)
      {
        character.characterAnimatorManager.PlayTargetAnimation("Dead_01", true);
      }
      else
      {
        if (character.characterStatsManager.currentStamina <= 0)
        {
          character.characterAnimatorManager.PlayTargetAnimation("Guard_Break_01", true);
          character.canBeRiposted = true;
          //character.characterSoundFXManager. PLAY GUARD BREAK SOUND
          character.isBlocking = false;
        }
        else
        {
          character.characterAnimatorManager.PlayTargetAnimation(blockAnimation, true);
          character.isAttacking = false;
        }
      }
    }


    private void CalculateDamage(CharacterManager character)
    {
      if (characterCausingDamage != null)
      {
        physicalDamage = Mathf.RoundToInt(physicalDamage * (characterCausingDamage.characterStatsManager.physicalDamagePercentageModifier / 100));
        fireDamage = Mathf.RoundToInt(fireDamage * (characterCausingDamage.characterStatsManager.fireDamagePercentageModifier / 100));
      }


      float totalPhysicalDamageAbsorption = 1 - (1 - character.characterStatsManager.physicalDamageAbsorptionHead / 100) *
                                                (1 - character.characterStatsManager.physicalDamageAbsorptionBody / 100) *
                                                (1 - character.characterStatsManager.physicalDamageAbsorptionLegs / 100) *
                                                (1 - character.characterStatsManager.physicalDamageAbsorptionHands / 100);

      physicalDamage = Mathf.RoundToInt(physicalDamage - (physicalDamage * totalPhysicalDamageAbsorption));


      float totalFireDamageAbsorption = 1 -
          (1 - character.characterStatsManager.fireDamageAbsorptionHead / 100) *
          (1 - character.characterStatsManager.fireDamageAbsorptionBody / 100) *
          (1 - character.characterStatsManager.fireDamageAbsorptionLegs / 100) *
          (1 - character.characterStatsManager.fireDamageAbsorptionHands / 100);

      fireDamage = Mathf.RoundToInt(fireDamage - (fireDamage * totalFireDamageAbsorption));

      float finalDamage = physicalDamage + fireDamage; // + magicDamage + lightningDamage + darkDamage

      character.characterStatsManager.currentHealth = Mathf.RoundToInt(character.characterStatsManager.currentHealth - finalDamage);


      if (character.characterStatsManager.currentHealth <= 0)
      {
        character.characterStatsManager.currentHealth = 0;
        character.isDead = true;
      }
    }

    private void CalculateStaminaDamage(CharacterManager character)
    {
      float staminaDamageAbsorption = staminaDamage * (character.characterStatsManager.blockingStabilityRating / 100);
      float staminaDamageAfterAbsorption = staminaDamage - staminaDamageAbsorption;
      character.characterStatsManager.currentStamina -= staminaDamageAfterAbsorption;
    }

    private void DecideBlockAnimationBasedOnPoiseDamage(CharacterManager character)
    {
      //ONE HANDED BLOCK ANIMATION
      if (!character.isTwoHandingWeapon)
      {
        // POISE BRACKET < 25      SMALL
        // POISE BRACKET > 25 < 50 MEDIUM
        // POISE BRACKET > 50 < 75 LARGE
        // POISE BRACKET > 75      COLOSAAL

        if (poiseDamage <= 24 && poiseDamage >= 0)
        {
          blockAnimation = "OH_Block_Guard_Ping_01";
          return;
        }
        else if (poiseDamage <= 49 && poiseDamage >= 25)
        {
          blockAnimation = "OH_Block_Guard_Light_01";
          return;
        }
        else if (poiseDamage <= 74 && poiseDamage >= 50)
        {
          blockAnimation = "OH_Block_Guard_Medium_01";
          return;
        }
        else if (poiseDamage >= 75)
        {
          blockAnimation = "OH_Block_Guard_Heavy_01";
          return;
        }
      }
      //TWO HANDED BLOCK ANIMATION
      else
      {
        //TWO HAND ANIMATION BLOCK HIT HERE (IF HAVE)
        if (poiseDamage <= 24 && poiseDamage >= 0)
        {
          blockAnimation = "TH_Block_Guard_Ping_01";
          return;
        }
        else if (poiseDamage <= 49 && poiseDamage >= 25)
        {
          blockAnimation = "TH_Block_Guard_Light_01";
          return;
        }
        else if (poiseDamage <= 74 && poiseDamage >= 50)
        {
          blockAnimation = "TH_Block_Guard_Medium_01";
          return;
        }
        else if (poiseDamage >= 75)
        {
          blockAnimation = "TH_Block_Guard_Heavy_01";
          return;
        }

      }
    }


    private void PlayBlockSoundFX(CharacterManager character)
    {
      //WE ARE BLOCKING WITH OUR RIGHT HANDED WEAPON
      if (character.isTwoHandingWeapon)
      {
        character.characterSoundFXManager.PlayRandomSoundFXFromArray(character.characterInventoryManager.rightWeapon.blockingNoises);
      }
      //WE ARE BLOCKING WITH OUR OFF (LEFT) HANDED WEAPON
      else
      {
        character.characterSoundFXManager.PlayRandomSoundFXFromArray(character.characterInventoryManager.leftWeapon.blockingNoises);
      }
    }

    private void AssignNewAITarget(CharacterManager character)
    {
      AICharacterManager aICharacter = character as AICharacterManager;

      if (aICharacter != null && characterCausingDamage != null)
      {
        aICharacter.currentTarget = characterCausingDamage;
      }
    }


  }
}
