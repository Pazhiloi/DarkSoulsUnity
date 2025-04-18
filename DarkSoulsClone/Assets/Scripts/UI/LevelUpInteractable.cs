using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MR
{
  public class LevelUpInteractable : Interactable
  {
    public override void Interact(PlayerManager playerManager)
    {
      playerManager.uiManager.levelUpWindow.SetActive(true);
    }
  }
}