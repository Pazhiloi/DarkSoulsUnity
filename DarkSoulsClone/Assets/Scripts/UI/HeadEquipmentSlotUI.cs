using UnityEngine;
using UnityEngine.UI;

namespace MR
{
    public class HeadEquipmentSlotUI : MonoBehaviour
    {
    UIManager uiManager;
    public Image icon;
    HelmetEquipment helmetItem;
    private void Awake()
    {
      uiManager = FindObjectOfType<UIManager>();
    }

    public void AddItem(HelmetEquipment helmetEquipment)
    {
      helmetItem = helmetEquipment;
      icon.sprite = helmetItem.itemIcon;
      icon.enabled = true;
      gameObject.SetActive(true);
     
    }

    public void ClearItem()
    {
      helmetItem = null;
      icon.sprite = null;
      icon.enabled = false;
    }

    public void SelectThisSlot()
    {
      uiManager.headEquipmentSlotSelected = true;
    }
  }
}
