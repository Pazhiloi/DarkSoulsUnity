using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SG
{
  [CreateAssetMenu(menuName = "Items/Consumable/Flask")]
public class FlaskItem : ConsumableItem
{
   [Header("Flask Type")]
   public bool estusFlask;
   public bool ashenFlask;
   [Header("Recovery Amount")]
   public int healthRecoveryAmount;
   public int focusPointsRecoverAmount;
    [Header("Recovery FX")]
    public GameObject recoveryFX;

    public override void AttemptToConsumeItem(PlayerAnimatorManager playerAnimatorManager, PlayerWeaponSlotManager playerWeaponSlotManager, PlayerEffectsManager playerEffectsManager)
    {
      base.AttemptToConsumeItem(playerAnimatorManager, playerWeaponSlotManager, playerEffectsManager);
      GameObject flask = Instantiate(itemModel, playerWeaponSlotManager.rightHandSlot.transform);
      playerEffectsManager.currentParticleFX = recoveryFX;
      playerEffectsManager.amountToBeHealed = healthRecoveryAmount;
      playerEffectsManager.instantiatedFXModel = flask;
      playerWeaponSlotManager.rightHandSlot.UnloadWeapon();
    }

  }
}
