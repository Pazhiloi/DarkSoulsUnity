using UnityEngine;
using UnityEngine.UI;

namespace MR
{
    public class HeadEquipmentSlotUI : MonoBehaviour
    {
    UIManager uiManager;
    public Image icon;
    HelmetEquipment item;
    private void Awake()
    {
      uiManager = FindObjectOfType<UIManager>();
    }

    public void AddItem(HelmetEquipment helmetEquipment)
    {
      item = helmetEquipment;
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
      uiManager.headEquipmentSlotSelected = true;
    }
  }
}
