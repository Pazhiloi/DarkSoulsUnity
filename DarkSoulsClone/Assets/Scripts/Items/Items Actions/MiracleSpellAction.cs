using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MR
{
  [CreateAssetMenu(menuName = "Items Actions/Miracle Spell Action")]
  public class MiracleSpellAction : ItemAction
  {
    public override void PerformAction(CharacterManager character)
    {
      if (character.isInteracting)
        return;

      if (character.characterInventoryManager.currentSpell != null && character.characterInventoryManager.currentSpell.isFaithSpell)
      {
        if (character.characterStatsManager.currentFocusPoints >= character.characterInventoryManager.currentSpell.focusPointCost)
        {
          character.characterInventoryManager.currentSpell.AttemptToCastSpell(
              character);
        }
        else
        {
          character.characterAnimatorManager.PlayTargetAnimation("Shrug", true);
        }
      }
    }
  }
}