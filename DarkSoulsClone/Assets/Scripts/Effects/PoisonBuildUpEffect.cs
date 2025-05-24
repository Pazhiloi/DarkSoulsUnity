using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MR
{
  [CreateAssetMenu(menuName = "Character Effects/Poison Build Up")]
  public class PoisonBuildUpEffect : CharacterEffect
  {
    // THE AMOUNT OF POISON BUILD UP GIVEN BEFORE RESISTANCES ARE CALCULATED, PER GAME TICK
    [SerializeField] float basePoisonBuildUpAmount = 7;

    // THE AMOUNT OF POISON TIME THE CHARACTER RECEIVES OF THEY ARE POISONED
    [SerializeField] float poisonAmount = 100;

    // THE AMOUNT OF DAMAGE TAKEN FROM THE POISON PER TICK IF IT IS BUILT UP TO 100%
    [SerializeField] int poisonDamagePerTick = 5;

    public override void ProcessEffect(CharacterManager character)
    {

      PlayerManager player = character as PlayerManager;
      // POISON BUILD UP AFTER WE FACTOR IN OUR PLAYERS RESISTANCES
      float finalPoisonBuildUp = 0;

      if (character.characterStatsManager.poisonResistance > 0)
      {
        // IF OUR CHARACTER HAS 100 OR MORE POISON RESIS THEY ARE IMMUNUE
        if (character.characterStatsManager.poisonResistance >= 100)
        {
          finalPoisonBuildUp = 0;
        }
        else
        {
          float resistancePercentage = character.characterStatsManager.poisonResistance / 100;
          finalPoisonBuildUp = basePoisonBuildUpAmount - (basePoisonBuildUpAmount * resistancePercentage);
        }
      }

      // EACH TICK WE ADD THE BUILD UP AMOUNT TO THE CHARACTERS OVERALL BUILD UP
      character.characterStatsManager.poisonBuildUp += finalPoisonBuildUp;

      // IF THE CHARACTER IS ALREADY POISONED, REMOVE ALL POISON BUILD UP EFFECTS
      if (character.characterStatsManager.isPoisoned)
      {
        character.characterEffectsManager.timedEffects.Remove(this);
      }



      // IF OUR BUILD UP IS 100 OR MORE, POISON THE CHARACTER
      if (character.characterStatsManager.poisonBuildUp >= 100)
      {
        character.characterStatsManager.isPoisoned = true;
        character.characterStatsManager.poisonAmount = poisonAmount;
        character.characterStatsManager.poisonBuildUp = 0;

        if (player != null)
        {
          player.playerEffectsManager.poisonAmountBar.SetPoisonAmount(Mathf.RoundToInt(poisonAmount));
        }

        PoisonedEffect poisonedEffect = Instantiate(WorldCharacterEffectsManager.instance.poisonedEffect);
        poisonedEffect.poisonDamage = poisonDamagePerTick;
        character.characterEffectsManager.timedEffects.Add(poisonedEffect);
        character.characterEffectsManager.timedEffects.Remove(this);
        character.characterSoundFXManager.PlayerSoundFX(WorldCharacterEffectsManager.instance.poisonSFX);

        character.characterEffectsManager.AddTimedEffectParticle(WorldCharacterEffectsManager.instance.poisonFX);
      }
      character.characterEffectsManager.timedEffects.Remove(this);

    }
    
  }
}