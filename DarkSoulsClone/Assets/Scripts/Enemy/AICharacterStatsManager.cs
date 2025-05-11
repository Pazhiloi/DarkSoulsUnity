using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MR
{

  public class AICharacterStatsManager : CharacterStatsManager
  {
    AICharacterManager aiCharacter;
    public UIEnemyHealthBar enemyHealthBar;


    public bool isBoss = false;

    protected override void Awake()
    {
      base.Awake();
      aiCharacter = GetComponent<AICharacterManager>();
      maxHealth = SetMaxHealthFromHealthLevel();
      currentHealth = maxHealth;
    }

    private void Start()
    {
      if (!isBoss)
      {
        enemyHealthBar.SetMaxHealth(maxHealth);
      }
    }


    private int SetMaxHealthFromHealthLevel()
    {
      maxHealth = healthLevel * 10;
      return maxHealth;
    }

    public override void TakeDamageNoAnimation(int physicalDamage, int fireDamage)
    {
      base.TakeDamageNoAnimation(physicalDamage, fireDamage);

      if (!isBoss)
      {
        enemyHealthBar.SetHealth(currentHealth);
      }
      else if (isBoss && aiCharacter.enemyBossManager != null)
      {
        aiCharacter.enemyBossManager.UpdateBossHealthBar(currentHealth, maxHealth);
      }
    }
    public override void TakePoisonDamage(int damage)
    {
      if (aiCharacter.isDead) return;

      base.TakePoisonDamage(damage);
      if (!isBoss)
      {
        enemyHealthBar.SetHealth(currentHealth);
      }
      else if (isBoss && aiCharacter.enemyBossManager != null)
      {
        aiCharacter.enemyBossManager.UpdateBossHealthBar(currentHealth, maxHealth);
      }
      if (currentHealth <= 0)
      {
        currentHealth = 0;
        aiCharacter.isDead = true;
        aiCharacter.enemyAnimatorManager.PlayTargetAnimation("Dead_01", true);
      }
    }

    public void BreakGuard()
    {
      aiCharacter.enemyAnimatorManager.PlayTargetAnimation("Break Guard", true);
    }
    public override void TakeDamage(int physicalDamage, int fireDamage, string damageAnimation, CharacterManager enemyCharacterDamagingMe)
    {
      base.TakeDamage(physicalDamage, fireDamage, damageAnimation, enemyCharacterDamagingMe);

      if (!isBoss)
      {
        enemyHealthBar.SetHealth(currentHealth);
      }
      else if (isBoss && aiCharacter.enemyBossManager != null)
      {
        aiCharacter.enemyBossManager.UpdateBossHealthBar(currentHealth, maxHealth);
      }

      aiCharacter.enemyAnimatorManager.PlayTargetAnimation(damageAnimation, true);

      if (currentHealth <= 0)
      {
        HandleDeath();
      }
    }

    private void HandleDeath()
    {
      currentHealth = 0;
      aiCharacter.enemyAnimatorManager.PlayTargetAnimation("Dead_01", true);
      aiCharacter.isDead = true;
    }

  }
}