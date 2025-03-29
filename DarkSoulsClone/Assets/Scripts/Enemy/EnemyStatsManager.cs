using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SG
{

  public class EnemyStatsManager : CharacterStatsManager
  {
    EnemyAnimatorManager enemyAnimatorManager;
    EnemyBossManager enemyBossManager;
    public UIEnemyHealthBar enemyHealthBar;


    public bool isBoss = false;

    private void Awake()
    {
      enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();
      enemyBossManager = GetComponent<EnemyBossManager>();
      enemyHealthBar = GetComponentInChildren<UIEnemyHealthBar>();
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

    public override void TakeDamageNoAnimation(int damage)
    {
      base.TakeDamageNoAnimation(damage);
      if (!isBoss)
      {
        enemyHealthBar.SetHealth(currentHealth);
      }
      else if (isBoss && enemyBossManager != null)
      {
        enemyBossManager.UpdateBossHealthBar(currentHealth, maxHealth);
      }
    }
    public override void TakePoisonDamage(int damage)
    {
      if (isDead) return;

      base.TakePoisonDamage(damage);
      if (!isBoss)
      {
        enemyHealthBar.SetHealth(currentHealth);
      }
      else if (isBoss && enemyBossManager != null)
      {
        enemyBossManager.UpdateBossHealthBar(currentHealth, maxHealth);
      }
      if (currentHealth <= 0)
      {
        currentHealth = 0;
        isDead = true;
        enemyAnimatorManager.PlayTargetAnimation("Dead_01", true);
      }
    }

    public void BreakGuard()
    {
      enemyAnimatorManager.PlayTargetAnimation("Break Guard", true);
    }
    public override void TakeDamage(int damage, string damageAnimation = "Damage_01")
    {

      base.TakeDamage(damage, damageAnimation = "Damage_01");
      if (!isBoss)
      {
        enemyHealthBar.SetHealth(currentHealth);

      }
      else if (isBoss && enemyBossManager != null)
      {
        enemyBossManager.UpdateBossHealthBar(currentHealth, maxHealth);
      }

      enemyAnimatorManager.PlayTargetAnimation(damageAnimation, true);

      if (currentHealth <= 0)
      {
        HandleDeath();
      }
    }

    private void HandleDeath()
    {
      currentHealth = 0;
      enemyAnimatorManager.PlayTargetAnimation("Dead_01", true);
      isDead = true;
    }

  }
}
