using UnityEngine;

namespace SG
{
  public class CharacterWeaponSlotManager : MonoBehaviour
  {
    [Header("Unarmed Weapon")]
    public WeaponItem unarmedWeapon;

    [Header("Weapon Slots")]
    public WeaponHolderSlot leftHandSlot;
    public WeaponHolderSlot rightHandSlot;
    public WeaponHolderSlot backSlot;

    [Header("Damage Colliders")]
    public DamageCollider leftHandDamageCollider;
    public DamageCollider rightHandDamageCollider;

    protected virtual void LoadWeaponHolderSlots()
    {
      WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
      foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots)
      {
        if (weaponSlot.isLeftHandSlot)
        {
          leftHandSlot = weaponSlot;
        }
        else if (weaponSlot.isRightHandSlot)
        {
          rightHandSlot = weaponSlot;
        }
        else if (weaponSlot.isBackSlot)
        {
          backSlot = weaponSlot;
        }
      }
    }

    public virtual void LoadBothWeaponsOnSlots()
    {
      // LoadWeaponOnSlot(playerInventoryManager.rightWeapon, false);
      // LoadWeaponOnSlot(playerInventoryManager.leftWeapon, true);
    }

  }
}