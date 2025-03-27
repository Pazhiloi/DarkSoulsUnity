using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SG
{
  public class UIManager : MonoBehaviour
  {
    public PlayerInventoryManager playerInventoryManager;
    public EquipmentWindowUI equipmentWindowUI;

    [Header("UI Windows")]
    public GameObject hudWindow;
    public GameObject selectWindow;
    public GameObject weaponInventoryWindow;
    public GameObject equipmentScreenWindow;

    [Header("Equipment Window Slots Selected")]
    public bool rightHandSlot01Selected, rightHandSlot02Selected, leftHandSlot01Selected, leftHandSlot02Selected;

    [Header("Weapon Inventory")]
    public GameObject weaponInventorySlotPrefab;
    public Transform weaponInventorySlotsParent;
    WeaponInventorySlot[] weaponInventorySlots;


    private void Start()
    {
      weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
      equipmentWindowUI.LoadWeaponsOnEquipmentScreen(playerInventoryManager);
    }

    public void UpdateUI()
    {

      #region  Weapon Inventory Slots
      for (int i = 0; i < weaponInventorySlots.Length; i++)
      {
        if (i < playerInventoryManager.weaponsInventory.Count)
        {
          if (weaponInventorySlots.Length < playerInventoryManager.weaponsInventory.Count)
          {
            Instantiate(weaponInventorySlotPrefab, weaponInventorySlotsParent);
            weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
          }
          weaponInventorySlots[i].AddItem(playerInventoryManager.weaponsInventory[i]);
        }
        else
        {
          weaponInventorySlots[i].ClearInventorySlot();
        }
      }
      #endregion

    }
    public void OpenSelectWindow()
    {
      selectWindow.SetActive(true);
    }

    public void CloseSelectWindow()
    {
      selectWindow.SetActive(false);
    }

    public void CloseAllInventoryWindows()
    {
      ResetAllSelectedSlots();
      weaponInventoryWindow.SetActive(false);
      equipmentScreenWindow.SetActive(false);
    }

    public void ResetAllSelectedSlots()
    {
      rightHandSlot01Selected = false;
      rightHandSlot02Selected = false;
      leftHandSlot01Selected = false;
      leftHandSlot02Selected = false;
    }
  }
}
