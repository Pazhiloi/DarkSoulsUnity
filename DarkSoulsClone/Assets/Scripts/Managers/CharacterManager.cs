using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SG
{
  public class CharacterManager : MonoBehaviour
  {
    CharacterAnimatorManager characterAnimatorManager;
    CharacterWeaponSlotManager characterWeaponSlotManager;
    [Header("Look On Transform")]
    [Header("Lock On Transform")]
    public Transform lockOnTransform;

    [Header("Combat Colliders")]
    public CriticalDamageCollider backStabCollider;
    public CriticalDamageCollider riposteCollider;
    [Header("Interaction")]
    public bool isInteracting;

    [Header("Combat Flags")]
    public bool canBeRiposted;
    public bool canBeParried;
    public bool canDoCombo;
    public bool isParrying;
    public bool isBlocking;
    public bool isInvulnerable;
    public bool isUsingRightHand;
    public bool isUsingLeftHand;
    public bool isHoldingArrow;
    public bool isAiming;
    public bool isTwoHandingWeapon;



    [Header("Movement Flags")]
    public bool isRotatingWithRootMotion;
    public bool canRotate;
    // public bool isInteracting;
    public bool isSprinting;
    public bool isInAir;
    public bool isGrounded;


    [Header("Spells")]
    public bool isFiringSpell;

    // damage to be inflicted during an animation event (backstab/riposte)
    public int pendingCriticalDamage;

    protected virtual void Awake()
    {
      characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
      characterWeaponSlotManager = GetComponent<CharacterWeaponSlotManager>();
    }

    protected virtual void FixedUpdate()
    {
      characterAnimatorManager.CheckHandIKWeight(characterWeaponSlotManager.rightHandIKTarget, characterWeaponSlotManager.leftHandIKTarget, isTwoHandingWeapon);
    }
  }
}