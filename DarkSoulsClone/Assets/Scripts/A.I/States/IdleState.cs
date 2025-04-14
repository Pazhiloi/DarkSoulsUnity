using UnityEngine;
namespace MR
{
  public class IdleState : State
  {
    public PursueTargetState pursueTargetState;
    public LayerMask detectionLayer;
    public override State Tick(EnemyManager enemyManager, EnemyStatsManager enemyStatsManager, EnemyAnimatorManager enemyAnimatorManager)
    {
      #region Handle Enemy Target Detection
      Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius, detectionLayer);
      for (int i = 0; i < colliders.Length; i++)
      {
        CharacterStatsManager characterStatsManager = colliders[i].transform.GetComponent<CharacterStatsManager>();

        if (characterStatsManager != null)
        {
          if (characterStatsManager.teamIDNumber != enemyStatsManager.teamIDNumber)
          {
            Vector3 targetDirection = characterStatsManager.transform.position - transform.position;
            float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

            if (viewableAngle > enemyManager.minimumDetectionAngle && viewableAngle < enemyManager.maximumDetectionAngle)
            {
              enemyManager.currentTarget = characterStatsManager;
            }
          } 
        }
      }
      #endregion

      #region Handle Switch State

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
