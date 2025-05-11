using UnityEngine;

namespace MR
{
  public class CompanionStateIdle : State
  {

    CompanionStatePursueTarget pursueTargetState;
    CompanionStateFollowHost followHostState;
    public LayerMask detectionLayer;
    public LayerMask layersThatBlockLineOfSight;


    private void Awake() {
      pursueTargetState = GetComponent<CompanionStatePursueTarget>();
      followHostState = GetComponent<CompanionStateFollowHost>();
    }
    public override State Tick(AICharacterManager aiCharacter)
    {
      aiCharacter.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);

      if (aiCharacter.distanceFromCompanion > aiCharacter.maxDistanceFromCompanion)
      {
        return followHostState;
      }

      Collider[] colliders = Physics.OverlapSphere(transform.position, aiCharacter.detectionRadius, detectionLayer);
      for (int i = 0; i < colliders.Length; i++)
      {
        CharacterManager targetCharacter = colliders[i].transform.GetComponent<CharacterManager>();

        if (targetCharacter != null)
        {
          if (targetCharacter.characterStatsManager.teamIDNumber != aiCharacter.enemyStatsManager.teamIDNumber)
          {
            Vector3 targetDirection = targetCharacter.transform.position - transform.position;
            float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

            if (viewableAngle > aiCharacter.minimumDetectionAngle && viewableAngle < aiCharacter.maximumDetectionAngle)
            {
              if (Physics.Linecast(aiCharacter.lockOnTransform.position, targetCharacter.lockOnTransform.position, layersThatBlockLineOfSight))
              {
                return this;
              }
              else
              {
                aiCharacter.currentTarget = targetCharacter;
              }
            }
          }
        }
      }

      if (aiCharacter.currentTarget != null)
      {
        return pursueTargetState;
      }
      else
      {
        return this;
      }
    }
  }
}
