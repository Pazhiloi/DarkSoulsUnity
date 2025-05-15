using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace MR
{
  public class WorldItemDataBase : MonoBehaviour
  {
    public static WorldItemDataBase Instance;

    public List<WeaponItem> weaponItems = new List<WeaponItem>();
    public List<EquipmentItem> equipmentItems = new List<EquipmentItem>();

    private void Awake()
    {
      SingletonInit();
    }

    public WeaponItem GetWeaponItemByID(int weaponID)
    {
      return weaponItems.FirstOrDefault(weapon => weapon.itemID == weaponID);
    }

    public EquipmentItem GetEquipmentItemByID(int equipmentID)
    {
      return equipmentItems.FirstOrDefault(equipment => equipment.itemID == equipmentID);
    }























    private void SingletonInit()
    {
      if (Instance == null)
      {
        Instance = this;
      }
      else
      {
        Destroy(gameObject);
      }
    }
  }
}