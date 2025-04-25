using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MR
{
  public class RangedProjectileDamageCollider : DamageCollider
  {

    public RangedAmmoItem ammoItem;
    protected bool hasAlreadyPenetratedASurface;
    protected GameObject penetratedProjectile;
    protected override void OnTriggerEnter(Collider other)
    {
      if (other.tag == "Character")
      {
        shieldHasBeenHit = false;
        hasBeenParried = false;

        CharacterStatsManager enemyStats = other.GetComponent<CharacterStatsManager>();
        CharacterManager enemyManager = other.GetComponent<CharacterManager>();
        CharacterEffectsManager enemyEffects = other.GetComponent<CharacterEffectsManager>();
        BlockingCollider shield = other.transform.GetComponentInChildren<BlockingCollider>();

        if (enemyManager != null)
        {
          if (enemyStats.teamIDNumber == teamIDNumber) return;

          CheckForParry(enemyManager);
          CheckForBlock(enemyManager, enemyStats, shield);


        }

        if (enemyStats != null)
        {
          if (enemyStats.teamIDNumber == teamIDNumber) return;
          if (hasBeenParried) return;
          if (shieldHasBeenHit) return;

          enemyStats.poiseResetTimer = enemyStats.totalPoiseResetTime;
          enemyStats.totalPoiseDefence = enemyStats.totalPoiseResetTime - poiseBreak;

          Vector3 contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
          float directionHitFrom = (Vector3.SignedAngle(characterManager.transform.forward, enemyManager.transform.forward, Vector3.up));
          ChooseWhichDirectionDamageCameFrom(directionHitFrom);
          enemyEffects.PlayBloodSplatterFX(contactPoint);

          if (enemyStats.totalPoiseDefence > poiseBreak)
          {
            enemyStats.TakeDamageNoAnimation(physicalDamage, 0);
          }
          else
          {
            enemyStats.TakeDamage(physicalDamage, 0, currentDamageAnimation, characterManager);
          }
        }
      }

      if (other.tag == "Illusionary Wall")
      {
        IllusionaryWall illusionaryWall = other.GetComponent<IllusionaryWall>();

        illusionaryWall.wallHasBeenHit = true;
      }

      if (!hasAlreadyPenetratedASurface && penetratedProjectile == null)
      {
        hasAlreadyPenetratedASurface = true;
        Vector3 contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
        GameObject penetratedArrow = Instantiate(ammoItem.penetratedModel, contactPoint, Quaternion.Euler(0, 0, 0));
        penetratedProjectile = penetratedArrow;
        penetratedArrow.transform.parent = other.transform;
        penetratedArrow.transform.rotation = Quaternion.LookRotation(other.gameObject.transform.forward);
      }

      Destroy(transform.root.gameObject);

    }
  }
}