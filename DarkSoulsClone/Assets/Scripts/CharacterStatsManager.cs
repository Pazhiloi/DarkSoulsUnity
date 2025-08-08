using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MR
{
  public class CharacterStatsManager : MonoBehaviour
  {
    CharacterManager characterManager;
    [Header("CHARACTER NAME")]
    public Image _characterAvatar;
    public string characterName;
    [Header("Team I.D")]
    public int teamIDNumber = 0;
    public int maxHealth;
    public int currentHealth;

    public float maxStamina, currentStamina;

    public float maxFocusPoints;
    public float currentFocusPoints;
    public int currentSoulCount = 0;

    public int soulsAwardedOnDeath = 50;

    [Header("CHARACTER LEVEL")]
    public int playerLevel = 1;

    [Header("STAT LEVELS")]
    public int healthLevel = 10;
    public int staminaLevel = 10;
    public int focusLevel = 10;
    public int poiseLevel = 10;
    public int strengthLevel = 10;
    public int dexterityLevel = 10;
    public int intelligenceLevel = 10;
    public int faithLevel = 10;

    [Header("Equip Load")]
    public float currentEquipLoad = 0;
    public float maxEquipLoad = 0;
    public EncumbranceLevel encumbranceLevel;

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

    [Header("Resistances")]
    public float poisonResistance;


    [Header("Blocking Absorptions")]
    public float blockingPhysicalDamageAbsorption;
    public float blockingFireDamageAbsorption;
    public float blockingStabilityRating;

    [Header("Damage Type Modifiers")]
    public float physicalDamagePercentageModifier = 100;
    public float fireDamagePercentageModifier = 100;
    [Header("Damage Absorption Modifiers")]
    public float physicalAbsorptionPercentageModifier = 0;
    public float fireAbsorptionPercentageModifier = 0;

    [Header("Poison")]
    public bool isPoisoned;
    public float poisonBuildUp = 0; //The build up over time that poisons the player after reaching 100
    public float poisonAmount = 100; //The amount of poison the player has to process before becoming unpoisoned


    protected virtual void Awake()
    {
      characterManager = GetComponent<CharacterManager>();
    }

    protected virtual void Update()
    {
      HandlePoiseResetTimer();
    }

    protected virtual void Start()
    {
      totalPoiseDefence = armorPoiseBonus;
      CalculateAndSetMaxEquipLoad();
    }



    public virtual void TakeDamageAfterBlock(int physicalDamage, int fireDamage, CharacterManager enemyCharacterDamagingMe)
    {
      if (characterManager.isDead) return;

      physicalDamage = Mathf.RoundToInt(physicalDamage * (enemyCharacterDamagingMe.characterStatsManager.physicalDamagePercentageModifier / 100));
      fireDamage = Mathf.RoundToInt(fireDamage * (enemyCharacterDamagingMe.characterStatsManager.fireDamagePercentageModifier / 100));


      characterManager.characterAnimatorManager.EraseHandIKForWeapon();

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
      physicalDamage -= Mathf.RoundToInt(physicalDamage - (physicalAbsorptionPercentageModifier / 100));
      fireDamage -= Mathf.RoundToInt(fireDamage - (fireAbsorptionPercentageModifier / 100));

      float finalDamage = physicalDamage + fireDamage; // + magicDamage + lightningDamage + darkDamage


      if (enemyCharacterDamagingMe.isPerformingFullyChargedAttack)
      {
        finalDamage = finalDamage * 2;
      }


      currentHealth = Mathf.RoundToInt(currentHealth - finalDamage);


      if (currentHealth <= 0)
      {
        currentHealth = 0;
        characterManager.isDead = true;
      }
    }

    public virtual void TakeDamageNoAnimation(int physicalDamage, int fireDamage)
    {
      
    }
    public virtual void TakePoisonDamage(int damage)
    {
      if (characterManager.isDead) { return; }
      currentHealth -= damage;

      if (currentHealth <= 0)
      {
        currentHealth = 0;
        characterManager.isDead = true;
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

    public virtual void DeductStamina(float staminaToDeduct)
    {
      currentStamina = currentStamina - staminaToDeduct;

    }


    public int SetMaxHealthFromHealthLevel()
    {
      maxHealth = healthLevel * 10;
      return maxHealth;
    }
    public float SetMaxStaminaFromStaminaLevel()
    {
      maxStamina = staminaLevel * 10;
      return maxStamina;
    }

    public float SetMaxFocusFromFocusLevel()
    {
      maxFocusPoints = focusLevel * 10;
      return maxFocusPoints;
    }


    public void CalculateAndSetMaxEquipLoad()
    {
      float totalEquipLoad = 40;

      for (int i = 0; i < staminaLevel; i++)
      {
        // CHANGE RETURNS BASED ON STAMINA LEVEL
        if (i < 25)
        {
          totalEquipLoad = totalEquipLoad + 1.2f;
        }
        if (i >= 25 && i <= 50)
        {
          totalEquipLoad = totalEquipLoad + 1.4f;
        }
        if (i > 50)
        {
          totalEquipLoad = totalEquipLoad + 1;
        }
      }

      maxEquipLoad = totalEquipLoad;
    }

    public void CalculateAndSetCurrentEquipLoad(float equipLoad)
    {
      currentEquipLoad = equipLoad;

      encumbranceLevel = EncumbranceLevel.Light;

      if (currentEquipLoad > (maxEquipLoad * 0.3f))
      {
        encumbranceLevel = EncumbranceLevel.Medium;
      }

      if (currentEquipLoad > (maxEquipLoad * 0.7f))
      {
        encumbranceLevel = EncumbranceLevel.Heavy;
      }

      if (currentEquipLoad > maxEquipLoad)
      {
        encumbranceLevel = EncumbranceLevel.Overloaded;
      }
    }

    public virtual void HealCharacter(int amount)
    {
      currentHealth += amount;

      if (currentHealth > maxHealth)
      {
        currentHealth = maxHealth;
      }
    }
  }
}