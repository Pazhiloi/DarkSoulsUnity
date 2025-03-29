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
        PlayerStatsManager playerStatsManager = other.GetComponent<PlayerStatsManager>();
        CharacterManager playerCharacterManager = other.GetComponent<CharacterManager>();
        CharacterEffectsManager playerEffectsManager = other.GetComponent<CharacterEffectsManager>();
        BlockingCollider shield = other.transform.GetComponentInChildren<BlockingCollider>();

        if (playerCharacterManager != null)
        {
          if (playerCharacterManager.isParrying)
          {
            characterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Parried", true);
            return;
          }
          else if (shield != null && playerCharacterManager.isBlocking)
          {
            float physicalDamageAfterBlock = currentWeaponDamage - (currentWeaponDamage * shield.blockingPhysicalDamageAbsorption) / 100;
            if (playerStatsManager != null)
            { playerStatsManager.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock), "Block Guard"); }
            return;
          }
        }

        if (playerStatsManager != null)
        {
          playerStatsManager.poiseResetTimer = playerStatsManager.totalPoiseResetTime;
          playerStatsManager.totalPoiseDefence = playerStatsManager.totalPoiseResetTime - poiseBreak;

          Vector3 contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
          playerEffectsManager.PlayBloodSplatterFX(contactPoint);

          if (playerStatsManager.totalPoiseDefence > poiseBreak)
          {
            playerStatsManager.TakeDamageNoAnimation(currentWeaponDamage);
          }
          else
          {
            playerStatsManager.TakeDamage(currentWeaponDamage);
          }
        }
      }

      if (other.tag == "Enemy")
      {
        EnemyStatsManager enemyStatsManager = other.GetComponent<EnemyStatsManager>();
        CharacterManager enemyCharacterManager = other.GetComponent<CharacterManager>();
        CharacterEffectsManager enemyEffectsManager = other.GetComponent<CharacterEffectsManager>();
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

          Vector3 contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
          enemyEffectsManager.PlayBloodSplatterFX(contactPoint);
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
