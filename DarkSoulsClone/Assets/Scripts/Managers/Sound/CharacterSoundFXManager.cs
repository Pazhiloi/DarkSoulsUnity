using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MR
{
  public class CharacterSoundFXManager : MonoBehaviour
  {
    CharacterManager character;
    private AudioSource audioSource;
    [Header("Taking Damage Sounds")]
    public AudioClip[] takingDamageSounds;
    private List<AudioClip> potentialDamageSounds;
    private AudioClip lastDamageSoundPlayed;

    //FOOT STEP SOUNDS
    [Header("Weapon Whooshes")]
    private List<AudioClip> potentialWeaponWhooshes;
    private AudioClip lastWeaponWhoosh;
    private void Awake()
    {
      audioSource = GetComponent<AudioSource>();
      character = GetComponent<CharacterManager>();
    }

    public virtual void PlayRandomDamageSoundFX()
    {
      potentialDamageSounds = new List<AudioClip>();

      foreach (var damageSound in takingDamageSounds)
      {
        if (damageSound != lastDamageSoundPlayed)
        {
          potentialDamageSounds.Add(damageSound);
        }
      }

      if (potentialDamageSounds.Count > 0)
      {
        int randomValue = Random.Range(0, potentialDamageSounds.Count);
        lastDamageSoundPlayed = potentialDamageSounds[randomValue];
        audioSource.PlayOneShot(potentialDamageSounds[randomValue], 0.4f);
      }
    }


    public virtual void PlayRandomWeaponWhoosh()
    {
      potentialWeaponWhooshes = new List<AudioClip>();

      if (character.isUsingRightHand)
      {
        foreach (var whooshSound in character.characterInventoryManager.rightWeapon.weaponWhooshes)
        {
          if (whooshSound != lastWeaponWhoosh)
          {
            potentialWeaponWhooshes.Add(whooshSound);
          }
        }

        if (potentialWeaponWhooshes.Count > 0)
        {
          int randomValue = Random.Range(0, potentialWeaponWhooshes.Count);
          lastWeaponWhoosh = character.characterInventoryManager.rightWeapon.weaponWhooshes[randomValue];
          audioSource.PlayOneShot(character.characterInventoryManager.rightWeapon.weaponWhooshes[randomValue]);
        }
      }
      else
      {
        foreach (var whooshSound in character.characterInventoryManager.leftWeapon.weaponWhooshes)
        {
          if (whooshSound != lastWeaponWhoosh)
          {
            potentialWeaponWhooshes.Add(whooshSound);
          }
        }

        if (potentialWeaponWhooshes.Count > 0)
        {
          int randomValue = Random.Range(0, potentialWeaponWhooshes.Count);
          lastWeaponWhoosh = character.characterInventoryManager.leftWeapon.weaponWhooshes[randomValue];
          audioSource.PlayOneShot(character.characterInventoryManager.leftWeapon.weaponWhooshes[randomValue]);
        }
      }
    }


  }
}
