using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SG
{
public class SpellItem : MonoBehaviour
{
    public GameObject spellWarmUpFX, spellCastFX;
    public string spellAnimation;

    [Header("Spell Type")]
    public bool isFaithSpell;
    public bool isMagicSpell;
    public bool isPyroSpell;
    [Header("Spell Description")]
    [TextArea]
    public string spellDescription;

    public virtual void AttemptToCastSpell(){
      Debug.Log("You attempted to cast the spell!");
    }
    public virtual void SuccessfullyCastSpell(){
      Debug.Log("You Successfully cast a spell!");
    }
  }
}
