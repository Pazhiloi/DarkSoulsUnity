using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace MR
{
  public class UIManager : MonoBehaviour
  {
    PlayerManager playerManager;
    public EquipmentWindowUI equipmentWindowUI;
    public QuickSlotsUI quickSlotsUI;

    [Header("HUD")]
    public GameObject crossHair;
    public Text soulCountText;

    [Header("UI Windows")]
    public GameObject hudWindow;
    public GameObject selectWindow;
    public GameObject weaponInventoryWindow;
    public GameObject equipmentScreenWindow;
    public GameObject levelUpWindow;

    [Header("Equipment Window Slots Selected")]
    public bool rightHandSlot01Selected, rightHandSlot02Selected, leftHandSlot01Selected, leftHandSlot02Selected;

    [Header("Weapon Inventory")]
    public GameObject weaponInventorySlotPrefab;
    public Transform weaponInventorySlotsParent;
    WeaponInventorySlot[] weaponInventorySlots;

    private void Awake()
    {
      quickSlotsUI = GetComponentInChildren<QuickSlotsUI>();
      playerManager = FindObjectOfType<PlayerManager>();
    }

    private void Start()
    {
      weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
      equipmentWindowUI.LoadWeaponsOnEquipmentScreen(playerManager.playerInventoryManager);
      quickSlotsUI.UpdateCurrentSpellIcon(playerManager.playerInventoryManager.currentSpell);
      quickSlotsUI.UpdateCurrentConsumableIcon(playerManager.playerInventoryManager.currentConsumable);
      soulCountText.text = playerManager.playerStatsManager.currentSoulCount.ToString();
    }

    public void UpdateUI()
    {

      #region  Weapon Inventory Slots
      for (int i = 0; i < weaponInventorySlots.Length; i++)
      {
        if (i < playerManager.playerInventoryManager.weaponsInventory.Count)
        {
          if (weaponInventorySlots.Length < playerManager.playerInventoryManager.weaponsInventory.Count)
          {
            Instantiate(weaponInventorySlotPrefab, weaponInventorySlotsParent);
            weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
          }
          weaponInventorySlots[i].AddItem(playerManager.playerInventoryManager.weaponsInventory[i]);
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