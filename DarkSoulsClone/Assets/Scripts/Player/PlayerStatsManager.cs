using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MR
{
  public class PlayerStatsManager : CharacterStatsManager
  {

    public HealthBar healthBar;
    public StaminaBar staminaBar;
    public FocusPointBar focusPointBar;


    PlayerManager player;

    public float staminaRegenerationAmount = 30f;
    public float staminaRegenTimer;

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

    public override void TakeDamage(int physicalDamage, int fireDamage, string damageAnimation, CharacterManager enemyCharacterDamagingMe)
    {
      if (player.isInvulnerable)
        return;

      base.TakeDamage(physicalDamage, fireDamage, damageAnimation, enemyCharacterDamagingMe);

      player.playerAnimatorManager.PlayTargetAnimation(damageAnimation, true);

      if (currentHealth <= 0)
      {
        currentHealth = 0;
        player.isDead = true;
        player.playerAnimatorManager.PlayTargetAnimation("Dead_01", true);
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
      if (player.isInteracting)
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


    public void AddSouls(int souls)
    {
      currentSoulCount += souls;
    }
  }
}