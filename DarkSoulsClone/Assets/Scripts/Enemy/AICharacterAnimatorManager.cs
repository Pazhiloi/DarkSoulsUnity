using UnityEngine;
namespace MR
{

  public class AICharacterAnimatorManager : CharacterAnimatorManager
  {
    AICharacterManager aiCharacter;
    protected override void Awake()
    {
      base.Awake();
      aiCharacter = GetComponent<AICharacterManager>();
    }
  

    public void AwardSoulsOnDeath()
    {
      PlayerStatsManager PlayerStatsManager = FindObjectOfType<PlayerStatsManager>();
      SoulCountBar soulCountBar = FindObjectOfType<SoulCountBar>();
      
      if (PlayerStatsManager != null)
      {
        PlayerStatsManager.AddSouls(aiCharacter.enemyStatsManager.soulsAwardedOnDeath);
        if (soulCountBar != null)
        {
          soulCountBar.SetSoulCountText(PlayerStatsManager.currentSoulCount);
        }
      }
    }

    public void InstantiateBossParticeFX()
    {
      BossFXTransform bossFXTransform = GetComponentInChildren<BossFXTransform>();
      GameObject phaseFX = Instantiate(aiCharacter.enemyBossManager.particleFX, bossFXTransform.transform);
    }

    public void PlayWeaponTrailFX()
    {
        aiCharacter.enemyEffectsManager.PlayWeaponFX(false);
    }

   

    private void OnAnimatorMove()
    {
      float delta = Time.deltaTime;
      aiCharacter.enemyRigidbody.drag = 0;
      Vector3 deltaPosition = aiCharacter.animator.deltaPosition;
      deltaPosition.y = 0;
      Vector3 velocity = deltaPosition / delta;
      aiCharacter.enemyRigidbody.velocity = velocity;

      if (aiCharacter.isRotatingWithRootMotion)
      {
        aiCharacter.transform.rotation *= aiCharacter.animator.deltaRotation;
      }
    }

  }
}
