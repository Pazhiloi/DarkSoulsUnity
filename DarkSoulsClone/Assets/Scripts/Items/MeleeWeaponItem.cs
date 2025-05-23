using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MR
{
  [CreateAssetMenu(menuName = "Items/Melee Weapon Item")]

  public class MeleeWeaponItem : WeaponItem
  {
    public bool canBeBuffed = true;
  }
}
