using UnityEngine;
namespace MR
{

  public class EnemyAnimatorManager : CharacterAnimatorManager
  {
    EnemyManager enemy;
    protected override void Awake()
    {
      base.Awake();
      enemy = GetComponent<EnemyManager>();
    }
  

    public void AwardSoulsOnDeath()
    {
      PlayerStatsManager PlayerStatsManager = FindObjectOfType<PlayerStatsManager>();
      SoulCountBar soulCountBar = FindObjectOfType<SoulCountBar>();
      
      if (PlayerStatsManager != null)
      {
        PlayerStatsManager.AddSouls(enemy.enemyStatsManager.soulsAwardedOnDeath);
        if (soulCountBar != null)
        {
          soulCountBar.SetSoulCountText(PlayerStatsManager.currentSoulCount);
        }
      }
    }

    public void InstantiateBossParticeFX()
    {
      BossFXTransform bossFXTransform = GetComponentInChildren<BossFXTransform>();
      GameObject phaseFX = Instantiate(enemy.enemyBossManager.particleFX, bossFXTransform.transform);
    }

    public void PlayWeaponTrailFX()
    {
        enemy.enemyEffectsManager.PlayWeaponFX(false);
    }

   

    private void OnAnimatorMove()
    {
      float delta = Time.deltaTime;
      enemy.enemyRigidbody.drag = 0;
      Vector3 deltaPosition = enemy.animator.deltaPosition;
      deltaPosition.y = 0;
      Vector3 velocity = deltaPosition / delta;
      enemy.enemyRigidbody.velocity = velocity;

      if (enemy.isRotatingWithRootMotion)
      {
        enemy.transform.rotation *= enemy.animator.deltaRotation;
      }
    }

  }
}
