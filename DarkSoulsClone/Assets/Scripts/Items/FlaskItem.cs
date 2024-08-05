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

    public override void AttemptToConsumeItem(PlayerAnimatorManager playerAnimatorManager, WeaponSlotManager weaponSlotManager, PlayerEffectsManager playerEffectsManager)
    {
      base.AttemptToConsumeItem(playerAnimatorManager, weaponSlotManager, playerEffectsManager);
      GameObject flask = Instantiate(itemModel, weaponSlotManager.rightHandSlot.transform);
      playerEffectsManager.currentParticleFX = recoveryFX;
      playerEffectsManager.amountToBeHealed = healthRecoveryAmount;
      playerEffectsManager.instantiatedFXModel = flask;
      weaponSlotManager.rightHandSlot.UnloadWeapon();
    }

  }
}
