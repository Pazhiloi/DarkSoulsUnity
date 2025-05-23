using System.Collections.Generic;
using UnityEngine;

namespace MR
{
  public class CharacterEffectsManager : MonoBehaviour
  {
    CharacterManager character;

    [Header("Static Effects")]
    [SerializeField] List<StaticCharacterEffect> staticCharacterEffects;

    [Header("Timed Effects")]
    public List<CharacterEffect> timedEffects;
    [SerializeField] float effectTickTimer = 0;


    [Header("Current  FX")]
    public GameObject instantiatedFXModel;
    [Header("Damage FX")]
    public GameObject bloodSplatterFX;
    [Header("Weapon FX")]
    public WeaponManager rightWeaponManager;
    public WeaponManager leftWeaponManager;

    [Header("Right Weapon Buff")]
    public WeaponBuffEffect rightWeaponBuffEffect;

    [Header("Poison FX")]
    public GameObject defaultPoisonParticleFX;
    public GameObject currentPoisonParticleFX;
    public Transform buildUpTransform;

    protected virtual void Awake()
    {
      character = GetComponent<CharacterManager>();
    }
    protected virtual void Start()
    {
      foreach (var effect in staticCharacterEffects)
      {
        effect.AddStaticEffect(character);
      }
    }

    public virtual void ProcessAllTimedEffects()
    {
      effectTickTimer = effectTickTimer + Time.deltaTime;

      if (effectTickTimer >= 1)
      {
        effectTickTimer = 0;

        ProcessWeaponBuffs();

        for (int i = timedEffects.Count - 1; i >= 0; i--)
        {
          timedEffects[i].ProcessEffect(character);
        }
      }
    }

    public void ProcessWeaponBuffs()
    {
      if (rightWeaponBuffEffect != null)
      {
        rightWeaponBuffEffect.ProcessEffect(character);
      }
    }

    public void AddStaticEffect(StaticCharacterEffect effect)
    {
      // CHECK THE LIST TO MAKE SURE WE DON'T ADD A DUPLICATE EFFECT
      StaticCharacterEffect staticEffect;

      for (int i = staticCharacterEffects.Count - 1; i > -1; i--)
      {
        if (staticCharacterEffects[i] != null)
        {
          if (staticCharacterEffects[i].effectID == effect.effectID)
          {
            staticEffect = staticCharacterEffects[i];
            staticEffect.RemoveStaticEffect(character);
            staticCharacterEffects.Remove(staticEffect);
          }
        }
      }

      staticCharacterEffects.Add(effect);
      effect.AddStaticEffect(character);

      // CHECK THE LIST FOR NULL ITEMS AND REMOVE THEM
      for (int i = staticCharacterEffects.Count - 1; i > -1; i--)
      {
        if (staticCharacterEffects[i] == null)
        {
          staticCharacterEffects.RemoveAt(i);
        }
      }
    }

    public void RemoveStaticEffect(int effectID)
    {
      StaticCharacterEffect staticEffect;

      for (int i = staticCharacterEffects.Count - 1; i > -1; i--)
      {
        if (staticCharacterEffects[i] != null)
        {
          if (staticCharacterEffects[i].effectID == effectID)
          {
            staticEffect = staticCharacterEffects[i];
            staticEffect.RemoveStaticEffect(character); // Assuming 'this' refers to the CharacterManager
            staticCharacterEffects.Remove(staticEffect);
            return; // Exit the loop after removing the effect
          }
        }
      }

      // CHECK THE LIST FOR NULL ITEMS AND REMOVE THEM
      for (int i = staticCharacterEffects.Count - 1; i > -1; i--)
      {
        if (staticCharacterEffects[i] == null)
        {
          staticCharacterEffects.RemoveAt(i);
        }
      }
    }



    public virtual void PlayWeaponFX(bool isLeft)
    {
      if (!isLeft)
      {
        if (rightWeaponManager != null)
        {
          rightWeaponManager.PlayWeaponTrailFX();
        }
      }
      else
      {
        if (leftWeaponManager != null)
        {
          leftWeaponManager.PlayWeaponTrailFX();
        }
      }
    }

    public virtual void PlayBloodSplatterFX(Vector3 bloodSplatterLocation)
    {
      GameObject blood = Instantiate(bloodSplatterFX, bloodSplatterLocation, Quaternion.identity);
    }
    

    public virtual void InterruptEffect()
    {
      //Can be used to destroy effects models (Drinking Estus, Having Arrow Drawn Ect)
      if (instantiatedFXModel != null)
      {
        Destroy(instantiatedFXModel);
      }

      //Fires the characters bow and removes the arrow if they are currently holding an arrow
      if (character.isHoldingArrow)
      {
        character.animator.SetBool("isHoldingArrow", false);
        Animator rangedWeaponAnimator = character.characterWeaponSlotManager.rightHandSlot.currentWeaponModel.GetComponentInChildren<Animator>();

        if (rangedWeaponAnimator != null)
        {
          rangedWeaponAnimator.SetBool("isDrawn", false);
          rangedWeaponAnimator.Play("Bow_TH_Fire_01");
        }
      }

      //Removes player from aiming state if they are currently aiming
      if (character.isAiming)
      {
        character.animator.SetBool("isAiming", false);
      }
    }


  }
}