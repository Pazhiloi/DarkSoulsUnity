using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MR
{
  public enum WeaponType
  {
    PyromancyCaster,
    FaithCaster,
    SpellCaster,
    Unarmed,
    StraightSword,
    SmallShield,
    Shield,
    Bow
  }
  public enum AmmoType
  {
    Arrow,
    Bolt
  }
  public enum AttackType
  {
    light,
    heavy
  }

  public enum AICombatStyle
  {
    swordAndShield,
    archer
  }
  public class Enums : MonoBehaviour
  {

  }
}