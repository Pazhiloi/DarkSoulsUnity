using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MR
{
  [CreateAssetMenu(menuName = "Items Actions/Blocking Action")]
  public class BlockingAction : ItemAction
  {

    public override void PerformAction(PlayerManager player)
    {
      if (player.isInteracting) return;
      if (player.isBlocking) return;
      player.playerCombatManager.SetBlockingAbsorptionsFromBlockingWeapon();

      player.isBlocking = true;
    }

  

  }
}