using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MR
{
  public class TakeDamageEffect : CharacterEffect
  {

    [Header("Character Causing Damage")]
    public CharacterManager characterCausingDamage; // IF THE DAMAGE IS CAUSED BY A CHARACTER, THEY ARE LISTED HERE

    [Header("Damage")]
    public float physicalDamage = 0;
    public float fireDamage = 0;

    [Header("Poise")]
    public float poiseDamage = 0;
    public bool poiseIsBroken = false;

    [Header("Animation")]
    public bool playDamageAnimation = true;
    public bool manuallySelectDamageAnimation = false;
    public string damageAnimation;

    [Header("SFX")]
    public bool willPlayDamageSFX = true;
    public AudioClip elementalDamageSoundSFX; // EXTRA SFX THAT IS PLAYED WHEN THERE IS ELEMENTAL DAMAGE (FIRE, MAGIC, DARKNES, LIGHTNING)

    [Header("Direction Damage Taken From")]
    public float angleHitFrom;
    public Vector3 contactPoint; // WHERE THE DAMAGE STRIKES THE PLAYER ON THEIR BODY


    public override void ProcessEffect(CharacterManager character)
    {
      // IF THE CHARACTER IS DEAD, RETURN WITHOUT RUNNING ANY LOGIC
      if (character.isDead)
        return;

      // IF THE CHARACTER IS INVULNERABLE, NO DAMAGE IS TAKEN
      if (character.isInvulnerable)
        return;

      CalculateDamage(character);
      // CHECK WHICH DIRECTION THE DAMAGE CAME FROM SO WE CAN PLAY THE RIGHT ANIMATION
      // PLAY A DAMAGE ANIMATION
      PlayDamageSoundFX(character);
      // PLAY BLOOD SPLATTER FX
      // IF THE CHARACTER IS A.I, ASSIGN THEM THE DAMAGING CHARACTER AS A TARGET
    }

    private void CalculateDamage(CharacterManager character)
    {

      physicalDamage = Mathf.RoundToInt(physicalDamage * (characterCausingDamage.characterStatsManager.physicalDamagePercentageModifier / 100));
      fireDamage = Mathf.RoundToInt(fireDamage * (characterCausingDamage.characterStatsManager.fireDamagePercentageModifier / 100));


      character.characterAnimatorManager.EraseHandIKForWeapon();

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
      physicalDamage -= Mathf.RoundToInt(physicalDamage - (character.characterStatsManager.physicalAbsorptionPercentageModifier / 100));
      fireDamage -= Mathf.RoundToInt(fireDamage - (character.characterStatsManager.fireAbsorptionPercentageModifier / 100));

      float finalDamage = physicalDamage + fireDamage; // + magicDamage + lightningDamage + darkDamage



      character.characterStatsManager.currentHealth = Mathf.RoundToInt(character.characterStatsManager.currentHealth - finalDamage);


      if (character.characterStatsManager.currentHealth <= 0)
      {
        character.characterStatsManager.currentHealth = 0;
        character.isDead = true;
      }
    }

    private void CheckWhichDirectionDamageCameFrom(CharacterManager character)
    {
      if (direction >= 145 && direction <= 180)
      {
        currentDamageAnimation = "Damage_Forward_01";
      }
      else if (direction <= -145 && direction >= -180)
      {
        currentDamageAnimation = "Damage_Forward_01";
      }
      else if (direction >= -45 && direction <= 45)
      {
        currentDamageAnimation = "Damage_Back_01";
      }
      else if (direction >= -144 && direction <= -45)
      {
        currentDamageAnimation = "Damage_Left_01";
      }
      else if (direction >= 45 && direction <= 144)
      {
        currentDamageAnimation = "Damage_Right_01";
      }
    }
   
    private void PlayDamageSoundFX(CharacterManager character)
    {
      character.characterSoundFXManager.PlayRandomDamageSoundFX();

      if (fireDamage > 0)
      {
        character.characterSoundFXManager.PlayerSoundFX(elementalDamageSoundSFX);
      }

    }


  }
}
