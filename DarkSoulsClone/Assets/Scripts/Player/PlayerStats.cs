using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
  public class PlayerStats : CharacterStats
  {
    
    HealthBar healthBar;
    StaminaBar staminaBar;

    AnimatorHandler animatorHandler;

    private void Awake()
    {
      healthBar = FindObjectOfType<HealthBar>();
      staminaBar = FindObjectOfType<StaminaBar>();
      animatorHandler  = GetComponentInChildren<AnimatorHandler>();
    }
    private void Start() {
      maxHealth = SetMaxHealthFromHealthLevel();
      currentHealth = maxHealth;
      healthBar.SetMaxHealth(maxHealth);


      maxStamina = SetMaxStaminaFromStaminaLevel();
      currentStamina = maxStamina;
      staminaBar.SetMaxStamina(maxStamina);

    }

    private int SetMaxHealthFromHealthLevel() {
      maxHealth = healthLevel * 10;
      return maxHealth;
    }
    private int SetMaxStaminaFromStaminaLevel() {
      maxStamina = staminaLevel * 10;
      return maxStamina;
    }

    public void TakeDamage(int damage) {

      if (isDead) return;
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
    public void TakeStaminaDamage(int damage) {
      currentStamina -= damage;
      staminaBar.SetCurrentStamina(currentStamina);
    }

  }
}
