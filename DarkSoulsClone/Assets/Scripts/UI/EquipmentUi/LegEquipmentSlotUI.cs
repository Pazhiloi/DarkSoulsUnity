using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MR
{
    public class LegEquipmentSlotUI : MonoBehaviour
    {
    UIManager uiManager;
    public Image icon;
    LegEquipment item;
    private void Awake()
    {
      uiManager = FindObjectOfType<UIManager>();
    }

    public void AddItem(LegEquipment legEquipment)
    {
      item = legEquipment;
      icon.sprite = item.itemIcon;
      icon.enabled = true;
      gameObject.SetActive(true);
     
    }

    public void ClearItem()
    {
      item = null;
      icon.sprite = null;
      icon.enabled = false;
    }

    public void SelectThisSlot()
    {
      uiManager.legEquipmentSlotSelected = true;
      uiManager.itemStatsWindowUI.UpdateArmorItemStats(item);
    }
    }
}
