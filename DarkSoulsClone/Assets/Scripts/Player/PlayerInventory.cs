using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
  public class PlayerInventory : MonoBehaviour
  {
    WeaponSlotManager weaponSlotManager;
    
    [Header("Quick Slots Items")]
    public SpellItem currentSpell;
    public WeaponItem rightWeapon;
    public WeaponItem leftWeapon;
    public WeaponItem unarmedWeapon;
    public ConsumableItem currentConsumable;

    [Header("Current Equipment")]
    public HelmetEquipment currentHelmetEquipment;
    public TorsoEquipment currentTorsoEquipment;

    public WeaponItem[] weaponsInRightHandSlots = new WeaponItem[2];
    public WeaponItem[] weaponsInLeftHandSlots = new WeaponItem[2];

    public int currentRightWeaponIndex = -1;
    public int currentLeftWeaponIndex = -1;

    public List<WeaponItem> weaponsInventory;
    private void Awake()
    {
      weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
    }
    private void Start()
    {
      rightWeapon = weaponsInRightHandSlots[0];
      leftWeapon = weaponsInLeftHandSlots[0];
      weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
      weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
    }

    public void ChangeRightWeapon()
    {
      currentRightWeaponIndex += 1;

      if (currentRightWeaponIndex == 0 && weaponsInRightHandSlots[0] != null)
      {
        rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
        weaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlots[currentRightWeaponIndex], false);
      }
      else if (currentRightWeaponIndex == 0 && weaponsInRightHandSlots[0] == null)
      {
        currentRightWeaponIndex += 1;
      }
      else if (currentRightWeaponIndex == 1 && weaponsInRightHandSlots[1] != null)
      {
        rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
        weaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlots[currentRightWeaponIndex], false);
      }
      else
      {
        currentRightWeaponIndex += 1;
      }

      if (currentRightWeaponIndex > weaponsInRightHandSlots.Length - 1)
      {
        currentRightWeaponIndex = -1;
        rightWeapon = unarmedWeapon;
        weaponSlotManager.LoadWeaponOnSlot(unarmedWeapon, false);
      }
    }

    public void ChangeLeftWeapon()
    {
      #region Sebastions Old Code
      currentLeftWeaponIndex += 1;

      if (currentLeftWeaponIndex == 0 && weaponsInLeftHandSlots[0] != null)
      {
        leftWeapon = weaponsInLeftHandSlots[currentLeftWeaponIndex];
        weaponSlotManager.LoadWeaponOnSlot(weaponsInLeftHandSlots[currentLeftWeaponIndex], true);
      }
      else if (currentLeftWeaponIndex == 0 && weaponsInLeftHandSlots[0] == null)
      {
        currentLeftWeaponIndex += 1;
      }

      else if (currentLeftWeaponIndex == 1 && weaponsInLeftHandSlots[1] != null)
      {
        leftWeapon = weaponsInLeftHandSlots[currentLeftWeaponIndex];
        weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
      }
      else
      {
        currentLeftWeaponIndex += 1;
      }

      if (currentLeftWeaponIndex > weaponsInLeftHandSlots.Length - 1)
      {
        currentLeftWeaponIndex = -1;
        leftWeapon = unarmedWeapon;
        weaponSlotManager.LoadWeaponOnSlot(unarmedWeapon, true);
      }
      #endregion
    }
  }

}