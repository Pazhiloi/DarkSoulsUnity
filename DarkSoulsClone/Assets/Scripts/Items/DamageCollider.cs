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
        PlayerStatsManager PlayerStatsManager = other.GetComponent<PlayerStatsManager>();
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
            if (PlayerStatsManager != null)
            { PlayerStatsManager.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock), "Block Guard"); }
            return;
          }
        }

        if (PlayerStatsManager != null)
        {
          PlayerStatsManager.poiseResetTimer = PlayerStatsManager.totalPoiseResetTime;
          PlayerStatsManager.totalPoiseDefence = PlayerStatsManager.totalPoiseResetTime - poiseBreak;
          if (PlayerStatsManager.totalPoiseDefence > poiseBreak)
          {
            PlayerStatsManager.TakeDamageNoAnimation(currentWeaponDamage);
          }
          else
          {
            PlayerStatsManager.TakeDamage(currentWeaponDamage);
          }
        }
      }

      if (other.tag == "Enemy")
      {
        EnemyStatsManager enemyStatsManager = other.GetComponent<EnemyStatsManager>();
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
            if (enemyStatsManager != null)
            { enemyStatsManager.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock), "Block Guard"); }
            return;
          }
        }

        if (enemyStatsManager != null)
        {
          enemyStatsManager.poiseResetTimer = enemyStatsManager.totalPoiseResetTime;
          enemyStatsManager.totalPoiseDefence = enemyStatsManager.totalPoiseResetTime - poiseBreak;
          if (enemyStatsManager.isBoss)
          {
            if (enemyStatsManager.totalPoiseDefence > poiseBreak)
            {
              enemyStatsManager.TakeDamageNoAnimation(currentWeaponDamage);
            }
            else
            {
              enemyStatsManager.TakeDamageNoAnimation(currentWeaponDamage);
              enemyStatsManager.BreakGuard();
            }
          }
          else
          {
            if (enemyStatsManager.totalPoiseDefence > poiseBreak)
            {
              enemyStatsManager.TakeDamageNoAnimation(currentWeaponDamage);
            }
            else
            {
              enemyStatsManager.TakeDamage(currentWeaponDamage);
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
