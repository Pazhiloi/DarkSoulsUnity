using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SG
{

  public class EnemyAnimatorManager : AnimatorManager
  {

    EnemyManager enemyManager;
    EnemyStats enemyStats;
    private void Awake()
    {
      anim = GetComponent<Animator>();
      enemyManager = GetComponentInParent<EnemyManager>();
      enemyStats = GetComponentInParent<EnemyStats>();
    }

    public void EnableIsParrying()
    {
      enemyManager.isParrying = true;
    }

    public void DisableIsParrying()
    {
      enemyManager.isParrying = false;
    }

    public void EnableCanBeRiposted()
    {
      enemyManager.canBeRiposted = true;
    }

    public void DisableCanBeRiposted()
    {
      enemyManager.canBeRiposted = false;
    }


    public override void TakeCriticalDamageAnimationEvent()
    {
      enemyStats.TakeDamageNoAnimation(enemyManager.pendingCriticalDamage);
      enemyManager.pendingCriticalDamage = 0;
    }

    public void AwardSoulsOnDeath()
    {
      PlayerStats playerStats = FindObjectOfType<PlayerStats>();
      SoulCountBar soulCountBar = FindObjectOfType<SoulCountBar>();
      
      if (playerStats != null)
      {
        playerStats.AddSouls(enemyStats.soulsAwardedOnDeath);
        if (soulCountBar != null)
        {
          soulCountBar.SetSoulCountText(playerStats.soulCount);
        }
      }
    }

    public void CanRotate()
    {
      anim.SetBool("canRotate", true);
    }

    public void StopRotation()
    {
      anim.SetBool("canRotate", false);
    }

    private void OnAnimatorMove()
    {
      float delta = Time.deltaTime;
      enemyManager.enemyRigidbody.drag = 0;
      Vector3 deltaPosition = anim.deltaPosition;
      deltaPosition.y = 0;
      Vector3 velocity = deltaPosition / delta;
      enemyManager.enemyRigidbody.velocity = velocity;
    }

  }
}
