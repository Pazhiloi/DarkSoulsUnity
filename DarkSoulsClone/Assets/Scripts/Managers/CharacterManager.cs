using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SG
{
  public class CharacterManager : MonoBehaviour
{
    [Header("Lock On Transform")]
    public Transform lockOnTransform;

    [Header("Combat Colliders")]
    public CriticalDamageCollider backStabCollider;
    public CriticalDamageCollider riposteCollider;

    [Header("Combat Flags")]
    public bool canBeRiposted;
    public bool canBeParried;
    public bool isParrying, isBlocking;

    [Header("Spells")]
    public bool isFiringSpell;

    // damage to be inflicted during an animation event (backstab/riposte)
    public int pendingCriticalDamage;
  }
}
