using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SG
{
  public class PursueTargetState : State
  {
   public CombatStanceState combatStanceState;
    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
    {

      if (enemyManager.isPreformingAction) return this;

       enemyManager.distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, transform.position);

      if (enemyManager.distanceFromTarget > enemyManager.maximumAttackRange)
      {
        enemyAnimatorManager.anim.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
      }

      HandleRotateTowardsTarget(enemyManager);
      enemyManager.navMeshAgent.transform.localPosition = Vector3.zero;
      enemyManager.navMeshAgent.transform.localRotation = Quaternion.identity;

      if (enemyManager.distanceFromTarget <= enemyManager.maximumAttackRange)
      {
        return combatStanceState;
      }
      else
      {
        return this;
      }
    }


    private void HandleRotateTowardsTarget(EnemyManager enemyManager)
    {
      // Rotate Manually
      if (enemyManager.isPreformingAction)
      {
        Vector3 direction = enemyManager.currentTarget.transform.position - transform.position;
        direction.y = 0;
        direction.Normalize();

        if (direction == Vector3.zero)
        {
          direction = transform.forward;
        }

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemyManager.rotationSpeed / Time.deltaTime);
      }
      // Rotate with pathfinding(navmesh)
      else
      {
        Vector3 relativeDirection = transform.InverseTransformDirection(enemyManager.navMeshAgent.desiredVelocity);
        Vector3 targetVelocity = enemyManager.enemyRigidbody.velocity;

        enemyManager.navMeshAgent.enabled = true;
        enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
        enemyManager.enemyRigidbody.velocity = targetVelocity;
        enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation, enemyManager.navMeshAgent.transform.rotation, enemyManager.rotationSpeed / Time.deltaTime);
      }
    }
  }
}