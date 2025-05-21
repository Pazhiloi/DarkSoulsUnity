using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MR
{
  public class WeaponBuffEffect : CharacterEffect
  {
    [Header("Buff Info")]
    [SerializeField] BuffClass buffClass;
    [SerializeField] float lengthOfBuff;
    public float timeRemainingOnBuff;
    [HideInInspector] public bool isRightHandedBuff;

    [Header("Buff SFX")]
    [SerializeField] AudioClip buffAmbientSound;
    [SerializeField] float ambientSoundVolume = 0.3f;

    [Header("Damage Info")]
    [SerializeField] float buffBaseDamagePercentageMultiplier = 15;

    [Header("Poise Buff")]
    [SerializeField] bool buffPoiseDamage;
    [SerializeField] float buffBasePoiseDamagePercentageMultiplier = 15;

    [Header("General")]
    [SerializeField] bool buffHasStarted = false;
    private WeaponManager weaponManager;
  }
}
