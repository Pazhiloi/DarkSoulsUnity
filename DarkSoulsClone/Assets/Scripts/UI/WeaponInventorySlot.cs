using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace SG
{
  public class WeaponInventorySlot : MonoBehaviour
  {

    PlayerInventoryManager playerInventoryManager;
    PlayerWeaponSlotManager playerWeaponSlotManager;
    UIManager uiManager;
    public Image icon;
    WeaponItem item;

    private void Awake()
    {
      playerInventoryManager = FindObjectOfType<PlayerInventoryManager>();
      playerWeaponSlotManager = FindObjectOfType<PlayerWeaponSlotManager>();
      uiManager = FindObjectOfType<UIManager>();
    }

    public void AddItem(WeaponItem newItem)
    {
      item = newItem;
      icon.sprite = item.itemIcon;
      icon.enabled = true;
      gameObject.SetActive(true);
    }

    public void ClearInventorySlot()
    {
      item = null;
      icon.sprite = null;
      icon.enabled = false;
      gameObject.SetActive(false);
    }

    public void EquipThisItem()
    {
      if (uiManager.rightHandSlot01Selected)
      {
        playerInventoryManager.weaponsInventory.Add(playerInventoryManager.weaponsInRightHandSlots[0]);
        playerInventoryManager.weaponsInRightHandSlots[0] = item;
        playerInventoryManager.weaponsInventory.Remove(item);
      }
      else if (uiManager.rightHandSlot02Selected)
      {
        playerInventoryManager.weaponsInventory.Add(playerInventoryManager.weaponsInRightHandSlots[1]);
        playerInventoryManager.weaponsInRightHandSlots[1] = item;
        playerInventoryManager.weaponsInventory.Remove(item);
      }
      else if (uiManager.leftHandSlot01Selected)
      {
        playerInventoryManager.weaponsInventory.Add(playerInventoryManager.weaponsInLeftHandSlots[0]);
        playerInventoryManager.weaponsInLeftHandSlots[0] = item;
        playerInventoryManager.weaponsInventory.Remove(item);
      }
      else if(uiManager.leftHandSlot02Selected)
      {
        playerInventoryManager.weaponsInventory.Add(playerInventoryManager.weaponsInLeftHandSlots[1]);
        playerInventoryManager.weaponsInLeftHandSlots[1] = item;
        playerInventoryManager.weaponsInventory.Remove(item);
      }
      else{
        return;
      }
      playerInventoryManager.rightWeapon = playerInventoryManager.weaponsInRightHandSlots[playerInventoryManager.currentRightWeaponIndex];
      playerInventoryManager.leftWeapon = playerInventoryManager.weaponsInLeftHandSlots[playerInventoryManager.currentLeftWeaponIndex];

      playerWeaponSlotManager.LoadWeaponOnSlot(playerInventoryManager.rightWeapon, false);
      playerWeaponSlotManager.LoadWeaponOnSlot(playerInventoryManager.leftWeapon, true);

      uiManager.equipmentWindowUI.LoadWeaponsOnEquipmentScreen(playerInventoryManager);
      uiManager.ResetAllSelectedSlots();
    }
  }
}
