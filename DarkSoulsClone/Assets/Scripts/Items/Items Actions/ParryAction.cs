using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MR
{
  [CreateAssetMenu(menuName = "Items Actions/Parry Action")]
  public class ParryAction : ItemAction
  {
    public override void PerformAction(CharacterManager character)
    {
      character.characterAnimatorManager.EraseHandIKForWeapon();
      WeaponItem parryingWeapon = character.characterInventoryManager.currentItemBeingUsed as WeaponItem;

      //CHECK IF PARRYING WEAPON IS A FAST PARRY WEAPON OR A MEDIUM PARRY WEAPON
      if (parryingWeapon.weaponType == WeaponType.SmallShield)
      {
        //FAST PARRY ANIM
        character.characterAnimatorManager.PlayTargetAnimation("Parry_01", true);
      }
      else if (parryingWeapon.weaponType != WeaponType.Shield)
      {
        //NORMAL PARRY ANIM
        character.characterAnimatorManager.PlayTargetAnimation("Parry_01", true);
      }
    }
  }
}