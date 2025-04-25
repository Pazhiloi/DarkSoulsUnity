using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MR
{

  public class EnemyStatsManager : CharacterStatsManager
  {
    EnemyManager enemy;
    public UIEnemyHealthBar enemyHealthBar;


    public bool isBoss = false;

    protected override void Awake()
    {
      base.Awake();
      enemy = GetComponent<EnemyManager>();
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
      else if (isBoss && enemy.enemyBossManager != null)
      {
        enemy.enemyBossManager.UpdateBossHealthBar(currentHealth, maxHealth);
      }
    }
    public override void TakePoisonDamage(int damage)
    {
      if (enemy.isDead) return;

      base.TakePoisonDamage(damage);
      if (!isBoss)
      {
        enemyHealthBar.SetHealth(currentHealth);
      }
      else if (isBoss && enemy.enemyBossManager != null)
      {
        enemy.enemyBossManager.UpdateBossHealthBar(currentHealth, maxHealth);
      }
      if (currentHealth <= 0)
      {
        currentHealth = 0;
        enemy.isDead = true;
        enemy.enemyAnimatorManager.PlayTargetAnimation("Dead_01", true);
      }
    }

    public void BreakGuard()
    {
      enemy.enemyAnimatorManager.PlayTargetAnimation("Break Guard", true);
    }
    public override void TakeDamage(int physicalDamage, int fireDamage, string damageAnimation, CharacterManager enemyCharacterDamagingMe)
    {
      base.TakeDamage(physicalDamage, fireDamage, damageAnimation, enemyCharacterDamagingMe);

      if (!isBoss)
      {
        enemyHealthBar.SetHealth(currentHealth);
      }
      else if (isBoss && enemy.enemyBossManager != null)
      {
        enemy.enemyBossManager.UpdateBossHealthBar(currentHealth, maxHealth);
      }

      enemy.enemyAnimatorManager.PlayTargetAnimation(damageAnimation, true);

      if (currentHealth <= 0)
      {
        HandleDeath();
      }
    }

    private void HandleDeath()
    {
      currentHealth = 0;
      enemy.enemyAnimatorManager.PlayTargetAnimation("Dead_01", true);
      enemy.isDead = true;
    }

  }
}