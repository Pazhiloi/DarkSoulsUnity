using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MR
{
  public class CharacterStatsManager : MonoBehaviour
  {
    CharacterAnimatorManager characterAnimatorManager;
    [Header("Team I.D")]
    public int teamIDNumber = 0;
    public int maxHealth;
    public int currentHealth;

    public float maxStamina, currentStamina;

    public float maxFocusPoints;
    public float currentFocusPoints;
    public int soulCount = 0;

    public int soulsAwardedOnDeath = 50;

    [Header("CHARACTER LEVEL")]
    public int playerLevel= 1;

    [Header("STAT LEVELS")]
    public int healthLevel = 10;
    public int staminaLevel = 10;
    public int focusLevel = 10;
    public int poiseLevel = 10;
    public int strengthLevel = 10;
    public int dexterityLevel = 10;
    public int intelligenceLevel = 10;
    public int faithLevel = 10;

    [Header("Poise")]
    public float totalPoiseDefence, offensivePoiseBonus, armorPoiseBonus;
    public float totalPoiseResetTime = 15;
    public float poiseResetTimer = 0;

    [Header("Armor Absorptions")]
    public float physicalDamageAbsorptionHead;
    public float physicalDamageAbsorptionBody, physicalDamageAbsorptionLegs, physicalDamageAbsorptionHands;
    public float fireDamageAbsorptionHead;
    public float fireDamageAbsorptionBody;
    public float fireDamageAbsorptionLegs;
    public float fireDamageAbsorptionHands;

    public bool isDead;

    protected virtual void Awake()
    {
      characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
    }

    protected virtual void Update()
    {
      HandlePoiseResetTimer();
    }

    private void Start()
    {
      totalPoiseDefence = armorPoiseBonus;
    }


    public virtual void TakeDamage(int physicalDamage, int fireDamage, string damageAnimation)
    {
      if (isDead) return;

      characterAnimatorManager.EraseHandIKForWeapon();

      float totalPhysicalDamageAbsorption = 1 - (1 - physicalDamageAbsorptionHead / 100) *
                                                (1 - physicalDamageAbsorptionBody / 100) *
                                                (1 - physicalDamageAbsorptionLegs / 100) *
                                                (1 - physicalDamageAbsorptionHands / 100);

      physicalDamage = Mathf.RoundToInt(physicalDamage - (physicalDamage * totalPhysicalDamageAbsorption));


      float totalFireDamageAbsorption = 1 -
          (1 - fireDamageAbsorptionHead / 100) *
          (1 - fireDamageAbsorptionBody / 100) *
          (1 - fireDamageAbsorptionLegs / 100) *
          (1 - fireDamageAbsorptionHands / 100);

      fireDamage = Mathf.RoundToInt(fireDamage - (fireDamage * totalFireDamageAbsorption));

      float finalDamage = physicalDamage + fireDamage; // + magicDamage + lightningDamage + darkDamage

      currentHealth = Mathf.RoundToInt(currentHealth - finalDamage);


      if (currentHealth <= 0)
      {
        currentHealth = 0;
        isDead = true;
      }
    }

    public virtual void TakeDamageNoAnimation(int physicalDamage, int fireDamage)
    {
      if (isDead) return;

      float totalPhysicalDamageAbsorption = 1 - (1 - physicalDamageAbsorptionHead / 100) *
                                                (1 - physicalDamageAbsorptionBody / 100) *
                                                (1 - physicalDamageAbsorptionLegs / 100) *
                                                (1 - physicalDamageAbsorptionHands / 100);

      physicalDamage = Mathf.RoundToInt(physicalDamage - (physicalDamage * totalPhysicalDamageAbsorption));


      float totalFireDamageAbsorption = 1 -
          (1 - fireDamageAbsorptionHead / 100) *
          (1 - fireDamageAbsorptionBody / 100) *
          (1 - fireDamageAbsorptionLegs / 100) *
          (1 - fireDamageAbsorptionHands / 100);

      fireDamage = Mathf.RoundToInt(fireDamage - (fireDamage * totalFireDamageAbsorption));

      float finalDamage = physicalDamage + fireDamage; // + magicDamage + lightningDamage + darkDamage

      currentHealth = Mathf.RoundToInt(currentHealth - finalDamage);


      if (currentHealth <= 0)
      {
        currentHealth = 0;
        isDead = true;
      }
    }
    public virtual void TakePoisonDamage(int damage)
    {
      if (isDead) { return; }
      currentHealth -= damage;

      if (currentHealth <= 0)
      {
        currentHealth = 0;
        isDead = true;
      }
    }


    public virtual void HandlePoiseResetTimer()
    {
      if (poiseResetTimer > 0)
      {
        poiseResetTimer -= Time.deltaTime;
      }
      else
      {
        totalPoiseDefence = armorPoiseBonus;
      }
    }

    public void DrainStaminaBasedOnAttackType()
    {

    }
  }
}