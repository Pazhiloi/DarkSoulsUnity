using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SG
{
  public class DamageCollider : MonoBehaviour
  {
    public CharacterManager characterManager;
    protected Collider damageCollider;
    public bool enabledDamageColliderOnStartUP = false;

    [Header("Team I.D")]
    public int teamIDNumber = 0;

    [Header("Poise")]
    public float poiseBreak, offensivePoiseBonus;
    [Header("Damage")]
    public int physicalDamage;
    public int fireDamage;
    public int magicDamage;
    public int lightningDamage;
    public int darkDamage;
    protected virtual void Awake()
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
      if (other.tag == "Character")
      {
        CharacterStatsManager enemyStats = other.GetComponent<CharacterStatsManager>();
        CharacterManager enemyManager = other.GetComponent<CharacterManager>();
        CharacterEffectsManager enemyEffects = other.GetComponent<CharacterEffectsManager>();
        BlockingCollider shield = other.transform.GetComponentInChildren<BlockingCollider>();

        if (enemyManager != null)
        {
          if (enemyStats.teamIDNumber == teamIDNumber)return;
          if (enemyManager.isParrying)
          {
            characterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Parried", true);
            return;
          }
          else if (shield != null && enemyManager.isBlocking)
          {
            float physicalDamageAfterBlock = physicalDamage - (physicalDamage * shield.blockingPhysicalDamageAbsorption) / 100;
            float fireDamageAfterBlock = fireDamage - (fireDamage * shield.blockingFireDamageAbsorption) / 100;
            if (enemyStats != null)
            { enemyStats.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock), 0, "Block Guard"); }
            // REFACTOR
            return;
          }
        }

        if (enemyStats != null)
        {
          if (enemyStats.teamIDNumber == teamIDNumber) return;
          enemyStats.poiseResetTimer = enemyStats.totalPoiseResetTime;
          enemyStats.totalPoiseDefence = enemyStats.totalPoiseResetTime - poiseBreak;

          Vector3 contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
          enemyEffects.PlayBloodSplatterFX(contactPoint);

          if (enemyStats.totalPoiseDefence > poiseBreak)
          {
            enemyStats.TakeDamageNoAnimation(physicalDamage, 0);
            //REFACTOR
          }
          else
          {
            enemyStats.TakeDamage(physicalDamage, 0);
            //REFACTOR
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