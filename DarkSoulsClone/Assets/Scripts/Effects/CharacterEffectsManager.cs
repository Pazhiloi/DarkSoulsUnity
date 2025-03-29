using UnityEngine;

namespace SG
{
  public class CharacterEffectsManager : MonoBehaviour
  {
    CharacterStatsManager characterStatsManager;
    [Header("Damage FX")]
    public GameObject bloodSplatterFX;
    [Header("Weapon FX")]
    public WeaponFX rightWeaponFX;
    public WeaponFX  leftWeaponFX;

    [Header("Poison FX")]
    public bool isPoisoned;
    public float poisonBuildup = 0; 
    public float poisonAmount = 100; 
    public float defaultPoisonAmount; 
    public float poisonTimer =2;
    public int poisonDamage = 1;
    float timer;

    protected virtual void Awake()
    {
      characterStatsManager = GetComponent<CharacterStatsManager>();
    }
    public virtual void PlayWeaponFX(bool isLeft)
    {
      if (!isLeft)
      {
        if (rightWeaponFX != null)
        {
          rightWeaponFX.PlayWeaponFX();
        }
      }
      else
      {
        if (leftWeaponFX != null)
        {
          leftWeaponFX.PlayWeaponFX();
        }
      }
    }

    public virtual void PlayBloodSplatterFX(Vector3 bloodSplatterLocation)
    {
      GameObject blood  = Instantiate(bloodSplatterFX, bloodSplatterLocation, Quaternion.identity);
    }
    public virtual void HandleAllBuildUpEffects()
    {
      if (characterStatsManager.isDead)
      {
        return;
      }

      HandlePoisonBuildUp();
      HandleIsPoisonedEffect();
    }

    protected virtual void HandlePoisonBuildUp()
    {
      if (isPoisoned) return;

      if (poisonBuildup > 0 && poisonBuildup < 100)
      {
        poisonBuildup -= 1 * Time.deltaTime;
      }
      else if (poisonBuildup >= 100)
      {
        isPoisoned = true;
        poisonBuildup = 0;
      }
    }

    protected virtual void HandleIsPoisonedEffect()
    {
      if (isPoisoned)
      {
        if (poisonAmount > 0)
        {
          timer += Time.deltaTime;
          if (timer >= poisonTimer)
          {
            characterStatsManager.TakePoisonDamage(poisonDamage);
            timer = 0;
          }
          poisonAmount -= 1 * Time.deltaTime;
        }
        else
        {
          isPoisoned = false;
          poisonAmount = defaultPoisonAmount;
        }
      }
    }
  }
}
