using UnityEngine;

namespace SG
{
  [CreateAssetMenu(menuName = "Items/Consumables/Bomb Item")]
  public class BombConsumeableItem : ConsumableItem
    {
    [Header("Velocity")]
    public int upwardVelocity = 50;
    public int forwardVelocity = 50;
    public int bombMass = 1;

    [Header("Live Bomb Model")]
    public GameObject liveBombModel;

    [Header("Base Damage")]
    public int baseDamage = 200;

    public override void AttemptToConsumeItem(PlayerAnimatorManager playerAnimatorManager, PlayerWeaponSlotManager weaponSlotManager, PlayerEffectsManager playerEffectsManager)
    {
      if (currentItemAmount > 0)
      {
        weaponSlotManager.rightHandSlot.UnloadWeapon();
        playerAnimatorManager.PlayTargetAnimation(consumeAnimation, true);
        GameObject bombModel = Instantiate(itemModel, weaponSlotManager.rightHandSlot.transform.position, Quaternion.identity, weaponSlotManager.rightHandSlot.transform);
        playerEffectsManager.instantiatedFXModel = bombModel;
      }
      else
      {
        playerAnimatorManager.PlayTargetAnimation("Shrug", true);
      }
    }
  }
}
