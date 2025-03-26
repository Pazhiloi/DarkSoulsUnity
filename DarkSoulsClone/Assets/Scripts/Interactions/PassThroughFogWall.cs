using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class PassThroughFogWall : Interactable
    {
        WorldEventManager worldEventManager;

        private void Awake() {
      worldEventManager = FindObjectOfType<WorldEventManager>();
        }

    public override void Interact(PlayerManager playerManager)
    {
      base.Interact(playerManager);
      playerManager.PassThroughFogWallInteraction(transform);
    }
  }
}
