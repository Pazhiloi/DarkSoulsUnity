using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SG
{
  public class DamageCollider : MonoBehaviour
  {
    public CharacterManager characterManager;
    Collider damageCollider;
    public bool enabledDamageColliderOnStartUP = false;

    [Header("Poise")]
    public float poiseBreak, offensivePoiseBonus;
    [Header("Damage")]
    public int currentWeaponDamage = 25;
    private void Awake()
    {
      damageCollider = GetComponent<Collider>();
      damageCollider.gameObject.SetActive(true);
      damageCollider.isTrigger = true;
      damageCollider.enabled = enabledDamageColliderOnStartUP;
    }

    public void EnableDamageCollider()
    {
      damageCollider.enabled = true;
    }
    public void DisableDamageCollider()
    {
      damageCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
      if (other.tag == "Player")
      {
        PlayerStats playerStats = other.GetComponent<PlayerStats>();
        CharacterManager enemyCharacterManager = other.GetComponent<CharacterManager>();
        BlockingCollider shield = other.transform.GetComponentInChildren<BlockingCollider>();

        if (enemyCharacterManager != null)
        {
          if (enemyCharacterManager.isParrying)
          {
            characterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Parried", true);
            return;
          }
          else if (shield != null && enemyCharacterManager.isBlocking)
          {
            float physicalDamageAfterBlock = currentWeaponDamage - (currentWeaponDamage * shield.blockingPhysicalDamageAbsorption) / 100;
            if (playerStats != null)
            { playerStats.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock), "Block Guard"); }
            return;
          }
        }

        if (playerStats != null)
        {
          playerStats.poiseResetTimer = playerStats.totalPoiseResetTime;
          playerStats.totalPoiseDefence = playerStats.totalPoiseResetTime - poiseBreak;
          if (playerStats.totalPoiseDefence > poiseBreak)
          {
            playerStats.TakeDamageNoAnimation(currentWeaponDamage);
          }
          else
          {
            playerStats.TakeDamage(currentWeaponDamage);
          }
        }
      }

      if (other.tag == "Enemy")
      {
        EnemyStats enemyStats = other.GetComponent<EnemyStats>();
        CharacterManager enemyCharacterManager = other.GetComponent<CharacterManager>();
        BlockingCollider shield = other.transform.GetComponentInChildren<BlockingCollider>();
        if (enemyCharacterManager != null)
        {
          if (enemyCharacterManager.isParrying)
          {
            characterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Parried", true);
            return;
          }
          else if (shield != null && enemyCharacterManager.isBlocking)
          {
            float physicalDamageAfterBlock = currentWeaponDamage - (currentWeaponDamage * shield.blockingPhysicalDamageAbsorption) / 100;
            if (enemyStats != null)
            { enemyStats.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock), "Block Guard"); }
            return;
          }
        }

        if (enemyStats != null)
        {
          enemyStats.poiseResetTimer = enemyStats.totalPoiseResetTime;
          enemyStats.totalPoiseDefence = enemyStats.totalPoiseResetTime - poiseBreak;
          if (enemyStats.isBoss)
          {
            if (enemyStats.totalPoiseDefence > poiseBreak)
            {
              enemyStats.TakeDamageNoAnimation(currentWeaponDamage);
            }
            else
            {
              enemyStats.TakeDamageNoAnimation(currentWeaponDamage);
              enemyStats.BreakGuard();
            }
          }
          else
          {
            if (enemyStats.totalPoiseDefence > poiseBreak)
            {
              enemyStats.TakeDamageNoAnimation(currentWeaponDamage);
            }
            else
            {
              enemyStats.TakeDamage(currentWeaponDamage);
            }
          }

        }
      }

      if (other.tag == "Illusionary Wall")
      {
        IllusionaryWall illusionaryWall = other.GetComponent<IllusionaryWall>();

        illusionaryWall.wallHasBeenHit = true;
      }
    }
  }
}
