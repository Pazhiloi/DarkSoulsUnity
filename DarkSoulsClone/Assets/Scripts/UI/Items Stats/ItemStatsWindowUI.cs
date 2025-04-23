using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MR
{
  public class ItemStatsWindowUI : MonoBehaviour
  {
    public Text itemNameText;
    public Image itemIconImage;
    [Header("Equipment Stats Windows")]
    public GameObject weaponStats;
    public GameObject armorStats;

    [Header("Weapon Stats")]
    public Text physicalDamageText;
    public Text magicDamageText;
    public Text physicalAbsorptionText;
    public Text magicAbsorptionText;

    [Header("Armor Stats")]
    public Text armorPhysicalAbsorptionText;
    public Text armorMagicAbsorptionText;
    public Text armorPoisonResistanceText;
    public void UpdateWeaponItemStats(WeaponItem weapon)
    {
      CloseAllStatWindows();
      if (weapon != null)
      {
        if (weapon.itemName != null)
        {
          itemNameText.text = weapon.itemName;
        }
        else
        {
          itemNameText.text = "";
        }

        if (weapon.itemIcon != null)
        {
          itemIconImage.gameObject.SetActive(true);
          itemIconImage.enabled = true;
          itemIconImage.sprite = weapon.itemIcon;
        }
        else
        {
          itemIconImage.gameObject.SetActive(false);
          itemIconImage.enabled = false;
          itemIconImage.sprite = null;
        }
        physicalDamageText.text = weapon.physicalDamage.ToString();
        physicalAbsorptionText.text = weapon.physicalDamageAbsorption.ToString();

        weaponStats.SetActive(true);
      }
      else
      {
        itemNameText.text = "";
        itemIconImage.gameObject.SetActive(false);
        itemIconImage.sprite = null;
        weaponStats.SetActive(false);
      }
    }

    public void UpdateArmorItemStats(EquipmentItem armor)
    {
      CloseAllStatWindows();
      if (armor != null)
      {
        if (armor.itemName != null)
        {
          itemNameText.text = armor.itemName;
        }
        else
        {
          itemNameText.text = "";
        }

        if (armor.itemIcon != null)
        {
          itemIconImage.gameObject.SetActive(true);
          itemIconImage.enabled = true;
          itemIconImage.sprite = armor.itemIcon;
        }
        else
        {
          itemIconImage.gameObject.SetActive(false);
          itemIconImage.enabled = false;
          itemIconImage.sprite = null;
        }
        armorPhysicalAbsorptionText.text = armor.physicalDefense.ToString();
        armorMagicAbsorptionText.text = armor.magicDefense.ToString();
        armorPoisonResistanceText.text = armor.poisonResistance.ToString();
        armorStats.SetActive(true);
      }
      else
      {
        itemNameText.text = "";
        itemIconImage.gameObject.SetActive(false);
        itemIconImage.sprite = null;
        armorStats.SetActive(false);
      }
    }

    private void CloseAllStatWindows()
    {
      weaponStats.SetActive(false);
      armorStats.SetActive(false);
    }
  }
}