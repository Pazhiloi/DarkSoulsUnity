using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MR
{
  public class PlayerStatsManager : CharacterStatsManager
  {
    PlayerManager player;
    public HealthBar healthBar;
    public StaminaBar staminaBar;
    public FocusPointBar focusPointBar;




    public float staminaRegenerationAmount = 30f;
    public float staminaRegenerationAmountWhilstBlocking = 3f;
    public float staminaRegenTimer;

    private float sprintingTimer = 0;

    protected override void Awake()
    {
      base.Awake();
      staminaBar = FindObjectOfType<StaminaBar>();
      focusPointBar = FindObjectOfType<FocusPointBar>();
    }
    private void Start()
    {
      player = GetComponent<PlayerManager>();
      maxHealth = SetMaxHealthFromHealthLevel();
      currentHealth = maxHealth;
      healthBar.SetMaxHealth(maxHealth);
      healthBar.SetCurrentHealth(currentHealth);


      maxStamina = SetMaxStaminaFromStaminaLevel();
      currentStamina = maxStamina;
      staminaBar.SetMaxStamina(maxStamina);
      staminaBar.SetCurrentStamina(currentStamina);

      maxFocusPoints = SetMaxFocusFromFocusLevel();
      currentFocusPoints = maxFocusPoints;
      focusPointBar.SetMaxFocusPoints(maxFocusPoints);
      focusPointBar.SetCurrentFocusPoints(currentFocusPoints);

    }

    public override void HandlePoiseResetTimer()
    {
      if (poiseResetTimer > 0)
      {
        poiseResetTimer -= Time.deltaTime;
      }
      else if (poiseResetTimer <= 0 && !player.isInteracting)
      {
        totalPoiseDefence = armorPoiseBonus;
      }
    }


    public override void TakeDamageNoAnimation(int physicalDamage, int fireDamage)
    {
      base.TakeDamageNoAnimation(physicalDamage, fireDamage);
      healthBar.SetCurrentHealth(currentHealth);
    }

    public override void DeductStamina(float staminaToDeduct)
    {
      base.DeductStamina(staminaToDeduct);
      staminaBar.SetCurrentStamina(currentStamina);
    }
    public void DeductSprintingStamina(float staminaToDeduct)
    {
      if (player.isSprinting)
      {
        sprintingTimer += Time.deltaTime;

        if (sprintingTimer > 0.1f)
        {
          sprintingTimer = 0;
          currentStamina = currentStamina - staminaToDeduct;
          staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
        }
      }
      else
      {
        sprintingTimer = 0;
      }
    }

    public override void TakePoisonDamage(int damage)
    {
      if (player.isDead) return;

      base.TakePoisonDamage(damage);
      healthBar.SetCurrentHealth(currentHealth);
      if (currentHealth <= 0)
      {
        currentHealth = 0;
        player.isDead = true;
        player.playerAnimatorManager.PlayTargetAnimation("Dead_01", true);
      }
    }



    public void DrainFocusPoints(int focusPoints)
    {
      currentFocusPoints -= focusPoints;
      if (currentFocusPoints < 0)
      {
        currentFocusPoints = 0;
      }

      focusPointBar.SetCurrentFocusPoints(currentFocusPoints);
    }

    public void RegenerateStamina()
    {
      if (player.isInteracting || player.isSprinting)
      {
        staminaRegenTimer = 0f;
      }
      else
      {
        staminaRegenTimer += Time.deltaTime;

        if (currentStamina < maxStamina && staminaRegenTimer > 1f)
        {
          if (player.isBlocking)
          {
            currentStamina += staminaRegenerationAmountWhilstBlocking * Time.deltaTime;
            staminaBar.SetCurrentStamina(currentStamina);
          }
          else
          {
            currentStamina += staminaRegenerationAmount * Time.deltaTime;
            staminaBar.SetCurrentStamina(currentStamina);
          }

        }

      }

    }


    public override void HealCharacter(int amount)
    {
      base.HealCharacter(amount);

      healthBar.SetCurrentHealth(currentHealth);
    }


    public void AddSouls(int souls)
    {
      currentSoulCount += souls;
    }
  }
}