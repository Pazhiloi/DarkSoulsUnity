using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
  public class PlayerStats : CharacterStats
  {

    HealthBar healthBar;
    StaminaBar staminaBar;
    PlayerManager playerManager;
    AnimatorHandler animatorHandler;

    public float staminaRegenerationAmount = 30f;
    public float staminaRegenTimer;

    private void Awake()
    {
      healthBar = FindObjectOfType<HealthBar>();
      staminaBar = FindObjectOfType<StaminaBar>();
      animatorHandler = GetComponentInChildren<AnimatorHandler>();
    }
    private void Start()
    {
      playerManager = GetComponent<PlayerManager>();
      maxHealth = SetMaxHealthFromHealthLevel();
      currentHealth = maxHealth;
      healthBar.SetMaxHealth(maxHealth);


      maxStamina = SetMaxStaminaFromStaminaLevel();
      currentStamina = maxStamina;
      staminaBar.SetMaxStamina(maxStamina);

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

    public void TakeDamage(int damage)
    {

      if (isDead || playerManager.isInvulnerable) return;
      currentHealth -= damage;

      healthBar.SetCurrentHealth(currentHealth);

      animatorHandler.PlayTargetAnimation("Damage_01", true);

      if (currentHealth <= 0)
      {
        currentHealth = 0;
        animatorHandler.PlayTargetAnimation("Dead_01", true);
        isDead = true;
      }
    }
    public void TakeStaminaDamage(int damage)
    {
      currentStamina -= damage;
      staminaBar.SetCurrentStamina(currentStamina);
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
  }
}
