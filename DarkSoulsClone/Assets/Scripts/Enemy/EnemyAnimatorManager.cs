using UnityEngine;
namespace SG
{

  public class EnemyAnimatorManager : AnimatorManager
  {
    EnemyManager enemyManager;
    EnemyEffectsManager enemyEffectsManager;
    EnemyBossManager enemyBossManager;
    protected override void Awake()
    {
      base.Awake();
      animator = GetComponent<Animator>();
      enemyManager = GetComponent<EnemyManager>();
      enemyEffectsManager = GetComponent<EnemyEffectsManager>();
      enemyBossManager = GetComponent<EnemyBossManager>();
    }
  

    public void AwardSoulsOnDeath()
    {
      PlayerStatsManager PlayerStatsManager = FindObjectOfType<PlayerStatsManager>();
      SoulCountBar soulCountBar = FindObjectOfType<SoulCountBar>();
      
      if (PlayerStatsManager != null)
      {
        PlayerStatsManager.AddSouls(characterStatsManager.soulsAwardedOnDeath);
        if (soulCountBar != null)
        {
          soulCountBar.SetSoulCountText(PlayerStatsManager.soulCount);
        }
      }
    }

    public void InstantiateBossParticeFX()
    {
      BossFXTransform bossFXTransform = GetComponentInChildren<BossFXTransform>();
      GameObject phaseFX = Instantiate(enemyBossManager.particleFX, bossFXTransform.transform);
    }

    public void PlayWeaponTrailFX()
    {
        enemyEffectsManager.PlayWeaponFX(false);
    }

   

    private void OnAnimatorMove()
    {
      float delta = Time.deltaTime;
      enemyManager.enemyRigidbody.drag = 0;
      Vector3 deltaPosition = animator.deltaPosition;
      deltaPosition.y = 0;
      Vector3 velocity = deltaPosition / delta;
      enemyManager.enemyRigidbody.velocity = velocity;

      if (enemyManager.isRotatingWithRootMotion)
      {
        enemyManager.transform.rotation *= animator.deltaRotation;
      }
    }

  }
}
