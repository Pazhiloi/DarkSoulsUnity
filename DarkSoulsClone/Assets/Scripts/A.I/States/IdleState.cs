using UnityEngine;
namespace MR
{
  public class IdleState : State
  {
    public PursueTargetState pursueTargetState;
    public LayerMask detectionLayer;
    public override State Tick(EnemyManager enemy)
    {
      #region Handle Enemy Target Detection
      Collider[] colliders = Physics.OverlapSphere(transform.position, enemy.detectionRadius, detectionLayer);
      for (int i = 0; i < colliders.Length; i++)
      {
        CharacterStatsManager characterStatsManager = colliders[i].transform.GetComponent<CharacterStatsManager>();

        if (characterStatsManager != null)
        {
          if (characterStatsManager.teamIDNumber != enemy.enemyStatsManager.teamIDNumber)
          {
            Vector3 targetDirection = characterStatsManager.transform.position - transform.position;
            float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

            if (viewableAngle > enemy.minimumDetectionAngle && viewableAngle < enemy.maximumDetectionAngle)
            {
              enemy.currentTarget = characterStatsManager;
            }
          } 
        }
      }
      #endregion

      #region Handle Switch State

      if (enemy.currentTarget != null)
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
