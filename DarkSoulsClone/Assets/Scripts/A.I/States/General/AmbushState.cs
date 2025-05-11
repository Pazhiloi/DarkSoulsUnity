using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MR
{

  public class AmbushState : State
  {
    public bool isSleeping;
    public float detectionRadius = 2;

    public string sleepAnimation, wakeAnimation;

   public LayerMask detectionLayer;

    public PursueTargetState pursueTargetState;
    public override State Tick(AICharacterManager aiCharacter)
    {

      if (isSleeping && !aiCharacter.isInteracting)
      {
        aiCharacter.enemyAnimatorManager.PlayTargetAnimation(sleepAnimation, true);
      }

      #region Handle Target Detection

      Collider[] colliders = Physics.OverlapSphere(aiCharacter.transform.position, detectionRadius, detectionLayer);

      for (int i = 0; i < colliders.Length; i++)
      {
        CharacterManager potentialTarget = colliders[i].transform.GetComponent<CharacterManager>();

        if (potentialTarget != null)
        {
          Vector3 targetsDirection = potentialTarget.transform.position - aiCharacter.transform.position;

          float viewableAngle = Vector3.Angle(targetsDirection, aiCharacter.transform.forward);

          if (viewableAngle > aiCharacter.minimumDetectionAngle && viewableAngle < aiCharacter.maximumDetectionAngle)
          {
            aiCharacter.currentTarget = potentialTarget;
            isSleeping = false;
            aiCharacter.enemyAnimatorManager.PlayTargetAnimation(wakeAnimation, true);
          }
        }
      }

      #endregion

      #region Handle State Change
      if (aiCharacter.currentTarget != null)
      {
        return pursueTargetState;
      }
      else
      {
        return this;
      }

      #endregion


    }
  }
}
