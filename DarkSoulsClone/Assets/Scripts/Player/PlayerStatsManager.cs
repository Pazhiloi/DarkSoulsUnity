using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
  public class PlayerStatsManager : CharacterStatsManager
  {

    HealthBar healthBar;
    StaminaBar staminaBar;
    public FocusPointBar focusPointBar;


    PlayerManager playerManager;
    PlayerAnimatorManager playerAnimatorManager;

    public float staminaRegenerationAmount = 30f;
    public float staminaRegenTimer;

    private void Awake()
    {
      healthBar = FindObjectOfType<HealthBar>();
      staminaBar = FindObjectOfType<StaminaBar>();
      focusPointBar = FindObjectOfType<FocusPointBar>();
      playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
    }
    private void Start()
    {
      playerManager = GetComponent<PlayerManager>();
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
      else if(poiseResetTimer <= 0 && !playerManager.isInteracting)
      {
        totalPoiseDefence = armorPoiseBonus;
      }
    }

    private int SetMaxHealthFromHealthLevel()
    {
      maxHealth = healthLevel * 10;
      return maxHealth;
    }
    private float SetMaxStaminaFromStaminaLevel()
    {
      maxStamina = staminaLevel * 10;
      return maxStamina;
    }

    private float SetMaxFocusFromFocusLevel()
    {
      maxFocusPoints = focusLevel * 10;
      return maxFocusPoints;
    }
    public void TakeDamageNoAnimation(int damage)
    {
      if (isDead) { return; }
      currentHealth -= damage;

      if (currentHealth <= 0)
      {
        currentHealth = 0;
        isDead = true;
      }
    }

    public override void TakeDamage(int damage, string damageAnimation = "Damage_01")
    {

      if (playerManager.isInvulnerable) return;
      base.TakeDamage(damage, damageAnimation = "Damage_01");
      healthBar.SetCurrentHealth(currentHealth);

      playerAnimatorManager.PlayTargetAnimation(damageAnimation, true);

      if (currentHealth <= 0)
      {
        currentHealth = 0;
        isDead = true;
        playerAnimatorManager.PlayTargetAnimation("Dead_01", true);
      }
    }
    public void TakeStaminaDamage(int damage)
    {
      currentStamina -= damage;
      staminaBar.SetCurrentStamina(currentStamina);
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
      if (playerManager.isInteracting)
      {
        staminaRegenTimer = 0f;
      }
      else
      {
        if (staminaRegenTimer <= 1f)
        {
          staminaRegenTimer += Time.deltaTime;
        }

        if (currentStamina < maxStamina && staminaRegenTimer > 1f)
        {
          currentStamina += staminaRegenerationAmount * Time.deltaTime;
          staminaBar.SetCurrentStamina(currentStamina);
        }
      }

    }


    public void HealPlayer(int amount)
    {
      currentHealth += amount;

      if (currentHealth > maxHealth)
      {
        currentHealth = maxHealth;
      }

      healthBar.SetCurrentHealth(currentHealth);
    }


    public void AddSouls(int souls){
      soulCount += souls;
    }
  }
}
