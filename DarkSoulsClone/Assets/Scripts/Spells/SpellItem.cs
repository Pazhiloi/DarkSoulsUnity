using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SG
{
public class SpellItem : Item
{
    public GameObject spellWarmUpFX, spellCastFX;
    public string spellAnimation;
    [Header("Spell Cast")]
    public int focusPointCost;

    [Header("Spell Type")]
    public bool isFaithSpell;
    public bool isMagicSpell;
    public bool isPyroSpell;
    [Header("Spell Description")]
    [TextArea]
    public string spellDescription;

    public virtual void AttemptToCastSpell(PlayerAnimatorManager playerAnimatorManager, PlayerStats playerStats, WeaponSlotManager weaponSlotManager)
    {
      Debug.Log("You attempted to cast the spell!");
    }
    public virtual void SuccessfullyCastSpell(PlayerAnimatorManager playerAnimatorManager, PlayerStats playerStats){
      Debug.Log("You Successfully cast a spell!");
      playerStats.DrainFocusPoints(focusPointCost);
    }
  }
}
