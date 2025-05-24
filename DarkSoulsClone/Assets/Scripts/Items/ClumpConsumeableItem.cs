using UnityEngine;

namespace MR
{
  [CreateAssetMenu(menuName = "Items/Consumables/Cure Effect Clump")]
  public class ClumpConsumableItem : ConsumableItem
  {
    [Header("Recovery FX")]
    public GameObject clumpConsumeFX;

    [Header("Cure FX")]
    public bool curePoison;
    //Cure Bleed
    //Cure Curse

    public override void AttemptToConsumeItem(PlayerManager player)
    {
      base.AttemptToConsumeItem(player);
      GameObject clump = Instantiate(itemModel, player.playerWeaponSlotManager.rightHandSlot.transform);
      player.playerEffectsManager.currentParticleFX = clumpConsumeFX;
      player.playerEffectsManager.instantiatedFXModel = clump;

      if (curePoison)
      {
        player.playerStatsManager.poisonBuildUp = 0;
        player.playerStatsManager.isPoisoned = false;
       
      }

      player.playerWeaponSlotManager.rightHandSlot.UnloadWeapon();
    }
  }
}
