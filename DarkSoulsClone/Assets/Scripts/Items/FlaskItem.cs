using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MR
{
  [CreateAssetMenu(menuName = "Items/Consumable/Flask")]
public class FlaskItem : ConsumableItem
{
   [Header("Flask Type")]
   public bool estusFlask;
   public bool ashenFlask;
   [Header("Recovery Amount")]
   public int healthRecoverAmount;
   public int focusPointsRecoverAmount;
    [Header("Recovery FX")]
    public GameObject recoveryFX;

    public override void AttemptToConsumeItem(PlayerManager player)
    {
      base.AttemptToConsumeItem(player);
      GameObject flask = Instantiate(itemModel, player.playerWeaponSlotManager.rightHandSlot.transform);
      player.playerEffectsManager.currentParticleFX = recoveryFX;
      player.playerEffectsManager.amountToBeHealed = healthRecoverAmount;
      player.playerEffectsManager.instantiatedFXModel = flask;
      player.playerWeaponSlotManager.rightHandSlot.UnloadWeapon();
    }

  }
}
