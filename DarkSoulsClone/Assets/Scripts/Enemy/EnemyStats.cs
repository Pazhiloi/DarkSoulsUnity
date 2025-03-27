using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SG
{

  public class EnemyStats : CharacterStatsManager
  {
    EnemyManager enemyManager;
    EnemyAnimatorManager enemyAnimatorManager;
    EnemyBossManager enemyBossManager;
    public UIEnemyHealthBar enemyHealthBar;

    public int soulsAwardedOnDeath = 50;

    public bool isBoss = false;

    private void Awake()
    {
      enemyManager = GetComponent<EnemyManager>();
      enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
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

    public void TakeDamageNoAnimation(int damage)
    {
      if (isDead) { return; }
      currentHealth -= damage;
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
      }
    }

    public void BreakGuard(){
      enemyAnimatorManager.PlayTargetAnimation("Break Guard", true);
    }
    public override void TakeDamage(int damage, string damageAnimation = "Damage_01")
    {

      base.TakeDamage(damage, damageAnimation = "Damage_01");
      if (!isBoss)
      {
        enemyHealthBar.SetHealth(currentHealth);

      }
      else if(isBoss && enemyBossManager != null){
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
