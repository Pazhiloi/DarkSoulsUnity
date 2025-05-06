using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MR
{
  public class IdleStateHumanoid : State
  {
    public PursueTargetStateHumanoid pursueTargetState;
    public LayerMask detectionLayer;
    public LayerMask layersThatBlockLineOfSight;

    private void Awake() {
      pursueTargetState = GetComponent<PursueTargetStateHumanoid>();
    }
    public override State Tick(EnemyManager aiCharacter)
    {

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
