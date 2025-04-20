using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MR
{
  [CreateAssetMenu(menuName = "Items Actions/Draw Arrow Action")]
  public class DrawArrowAction : ItemAction
  {
    public override void PerformAction(PlayerManager player)
    {
      if (player.isInteracting) return;
      if (player.isHoldingArrow) return;
      
      player.animator.SetBool("isHoldingArrow", true);
      player.playerAnimatorManager.PlayTargetAnimation("Bow_TH_Draw_01", true);

      GameObject loadedArrow = Instantiate(player.playerInventoryManager.currentAmmo.loadedItemModel, player.playerWeaponSlotManager.leftHandSlot.transform);
      player.playerEffectsManager.currentRangeFX = loadedArrow;
      

      Animator bowAnimator = player.playerWeaponSlotManager.rightHandSlot.GetComponentInChildren<Animator>();
      bowAnimator.SetBool("isDrawn", true);
      bowAnimator.Play("Bow_TH_Draw_01");
    }
  }
}
