using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SG
{

  public class AmbushState : State
  {
    public bool isSleeping;
    public float detectionRadius = 2;

    public string sleepAnimation, wakeAnimation;

   public LayerMask detectionLayer;

    public PursueTargetState pursueTargetState;
    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
    {

      if (isSleeping && !enemyManager.isInteracting)
      {
        enemyAnimatorManager.PlayTargetAnimation(sleepAnimation, true);
      }

      #region Handle Target Detection

      Collider[] colliders = Physics.OverlapSphere(enemyManager.transform.position, detectionRadius, detectionLayer);

      for (int i = 0; i < colliders.Length; i++)
      {
        CharacterStatsManager CharacterStatsManager = colliders[i].transform.GetComponent<CharacterStatsManager>();

        if (CharacterStatsManager != null)
        {
          Vector3 targetsDirection = CharacterStatsManager.transform.position - enemyManager.transform.position;

          float viewableAngle = Vector3.Angle(targetsDirection, enemyManager.transform.forward);

          if (viewableAngle > enemyManager.minimumDetectionAngle && viewableAngle < enemyManager.maximumDetectionAngle)
          {
            enemyManager.currentTarget = CharacterStatsManager;
            isSleeping = false;
            enemyAnimatorManager.PlayTargetAnimation(wakeAnimation, true);
          }
        }
      }

      #endregion

      #region Handle State Change
      if (enemyManager.currentTarget != null)
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
