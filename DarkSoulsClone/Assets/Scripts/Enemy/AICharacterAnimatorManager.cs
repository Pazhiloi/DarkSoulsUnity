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



    public override void OnAnimatorMove()
    {
      if (character.isInteracting == false)
        return;

      Vector3 velocity = character.animator.deltaPosition;
      character.characterController.Move(velocity);

      if (aiCharacter.isRotatingWithRootMotion)
      {
        character.transform.rotation *= character.animator.deltaRotation;
      }
    }

  }
}
