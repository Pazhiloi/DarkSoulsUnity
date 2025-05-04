using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MR
{
  [CreateAssetMenu(menuName = "Items Actions/Blocking Action")]
  public class BlockingAction : ItemAction
  {

    public override void PerformAction(CharacterManager character)
    {
      if (character.isInteracting) return;
      if (character.isBlocking) return;
      character.characterCombatManager.SetBlockingAbsorptionsFromBlockingWeapon();

      character.isBlocking = true;
    }

  

  }
}