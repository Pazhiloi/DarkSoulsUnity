using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MR
{
  [CreateAssetMenu(menuName = "Items Actions/Draw Arrow Action")]
  public class DrawArrowAction : ItemAction
  {
    public override void PerformAction(CharacterManager character)
    {
      if (character.isInteracting) return;
      if (character.isHoldingArrow) return;
      
      character.animator.SetBool("isHoldingArrow", true);
      character.characterAnimatorManager.PlayTargetAnimation("Bow_TH_Draw_01", true);

      GameObject loadedArrow = Instantiate(character.characterInventoryManager.currentAmmo.loadedItemModel, character.characterWeaponSlotManager.leftHandSlot.transform);
      character.characterEffectsManager.instantiatedFXModel = loadedArrow;
      

      Animator bowAnimator = character.characterWeaponSlotManager.rightHandSlot.GetComponentInChildren<Animator>();
      bowAnimator.SetBool("isDrawn", true);
      bowAnimator.Play("Bow_TH_Draw_01");
    }
  }
}
