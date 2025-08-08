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

  public enum AIAttackActionType
  {
    meleeAttackAction,
    magicAttackAction,
    rangedAttackAction
  }

  public enum DamageType
  {
    Physical,
    Fire
  }
  public enum BuffClass
  {
    Physical,
    Fire
  }
  public enum EffectParticleType
  {
    poison
  }

  public enum EncumbranceLevel
  {
    Light, // LIGHT ROLL
    Medium, // MED ROLL
    Heavy, // HEAVY ROLL
    Overloaded // WALK SPEED ONLY
  }
  
  public class Enums : MonoBehaviour
  {

  }
}